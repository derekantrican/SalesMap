using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Microsoft.Win32;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace SalesMap
{
    public partial class SalesMapSearch : Form
    {
        List<String> RegionNames = new List<String>();
        List<String> RegionParts = new List<String>();
        List<String> RegionArea = new List<String>();

        List<String> SalesRepNames = new List<String>();
        List<String> SalesRepEmails = new List<String>();
        List<String> SalesRepPhones = new List<String>();
        List<String> SalesRepRegions = new List<String>();
        List<String> SalesRepPosition = new List<String>();

        public SalesMapSearch()
        {
            InitializeComponent();
            Log("------------ STARTING SALESMAP (" + Properties.Settings.Default.Version + ") ------------");

            checkFirstRun();
            checkForUpdate();

            //File.WriteAllText(@"C:\Users\" + Environment.UserName + @"\log.txt", ""); //Clear the log

            readFiles();
        }

        private void checkForUpdate()
        {
            if (Properties.Settings.Default.AutoCheckUpdate)
            {
                compareFiles();

                WebClient client = new WebClient();
                string url = "https://github.com/derekantrican/SalesMap/tags";
                string html = "";
                try
                {
                    html = client.DownloadString(url);
                }
                catch
                {
                    Log("Attempted to check for new version and failed to get html");
                    return;
                }

                string nextUrl = "";

                List<string> versions = new List<string>();
                string version = "";
                while (html.IndexOf("<span class=\"disabled\">Next</span>") < 0)
                {
                    while (html.IndexOf("<span class=\"tag-name\">v") > -1)
                    {
                        html = html.Substring(html.IndexOf("<span class=\"tag-name\">v") + 23);
                        version = html.Split('<')[0];

                        if (version != "" && version.IndexOf("beta") < 0)
                            versions.Add(html.Split('<')[0]);
                    }

                    nextUrl = html.Substring(html.IndexOf("<span class=\"disabled\">Previous</span>") + 47).Split('\"')[0];
                    html = client.DownloadString(nextUrl);
                }

                //Run this while loop again to get the last page
                while (html.IndexOf("<span class=\"tag-name\">v") > -1)
                {
                    html = html.Substring(html.IndexOf("<span class=\"tag-name\">v") + 23);
                    version = html.Split('<')[0];

                    if (version != "" && version.IndexOf("beta") < 0)
                        versions.Add(html.Split('<')[0]);
                }

                string GitVersion = versions.First();
                string thisVersion = Properties.Settings.Default.Version;

                if (GitVersion != thisVersion)
                {
                    Log("Prompted for new update. Current: " + thisVersion + "  Online: " + GitVersion);

                    if (MessageBox.Show("A new version is available!\n\nThe current version is " + GitVersion + " and you are running " + thisVersion +
                                        "\n\nGo to " + url + " to download the new version?",
                                        "New Update Available!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(url);
                        Log("User selected \"Yes\" for the new update");
                    }
                    else
                    {
                        Log("User selected \"No\" for the new update");
                    }
                }
            }
        }

        private void SalesMapSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log("++++++++++++ CLOSING SALESMAP ++++++++++++");

            if (Properties.Settings.Default.SendLog)
            {
                string user = Environment.UserName;
                string logPath = @"C:\Users\" + user + @"\log.txt";
                string newLogPath = @"\\sigmatek.net\Documents\Employees\Derek_Antrican\SalesMap\Log Files\" + user + " log.txt";

                if (File.Exists(logPath))
                {
                    try
                    {
                        ProcessStartInfo Info = new ProcessStartInfo();
                        Info.UseShellExecute = false;
                        Info.RedirectStandardOutput = true;
                        Info.Arguments = @"/C copy ""C:\Users\" + Environment.UserName + @"\log.txt"" ""\\sigmatek.net\Documents\Employees\Derek_Antrican\SalesMap\Log Files\" + Environment.UserName + " log.txt\" /y";
                        Info.WindowStyle = ProcessWindowStyle.Hidden;
                        Info.CreateNoWindow = true;
                        Info.FileName = "cmd.exe";
                        Process infoProcess = Process.Start(Info);
                        string output = infoProcess.StandardOutput.ReadToEnd();

                        if (output != "")
                            Log("Copying log file. Result: \"" + removeSpecial(output) + "\"");
                    }
                    catch
                    {
                        Log("Could not copy log file");
                    }
                }
            }
        }

        public void compareFiles()
        {
            WebClient client = new WebClient();
            string url = "https://github.com/derekantrican/SalesMap/wiki/Most-current-%22databases%22";
            string html = "";

            string regionTextOnline = "";
            string salesTextOnline = "";

            try
            {
                html = client.DownloadString(url);

                regionTextOnline = html.Substring(html.IndexOf("<pre><code>") + 11).Split('<')[0];
                salesTextOnline = html.Substring(html.LastIndexOf("<pre><code>") + 11).Split('<')[0];
            }
            catch
            {
                Log("Attempted to get the online databases and failed to get html");
                return;
            }

            //Remove the extra character that comes at the end of these strings and replace "&amp;" with "&"
            regionTextOnline = regionTextOnline.TrimEnd(regionTextOnline[regionTextOnline.Length - 1]);
            regionTextOnline = regionTextOnline.Replace("&amp;", "&");
            salesTextOnline = salesTextOnline.TrimEnd(salesTextOnline[salesTextOnline.Length - 1]);
            salesTextOnline = salesTextOnline.Replace("&amp;", "&");

            //Compare the raw text of files by checking files without special characters
            if (removeSpecial(Properties.Settings.Default.Regions) != removeSpecial(regionTextOnline) && regionTextOnline != "")
            {
                Log("Internal regions is not the same as online. Updating...");
                Log("DIFF: " + diff(removeSpecial(Properties.Settings.Default.Regions), removeSpecial(regionTextOnline)), false);
                Properties.Settings.Default.Regions = regionTextOnline;
            }

            if (removeSpecial(Properties.Settings.Default.SalesReps) != removeSpecial(salesTextOnline) && salesTextOnline != "")
            {
                Log("Internal SalesReps is not the same as online. Updating...");
                Log("DIFF: " + diff(removeSpecial(Properties.Settings.Default.SalesReps), removeSpecial(salesTextOnline)), false);
                Properties.Settings.Default.SalesReps = salesTextOnline;
            }

            Properties.Settings.Default.Save();
        }

        public void readFiles()
        {
            using (StringReader reader = new StringReader(Properties.Settings.Default.Regions))
            {
                string line = reader.ReadLine();

                while (line != null)
                {
                    string[] items = line.Split(',');

                    try
                    {
                        RegionNames.Add(items[0]);
                        RegionParts.Add(items[1]);
                        RegionArea.Add(items[2]);
                    }
                    catch
                    {
                        Log("Failed to read from Regions.txt");
                    }

                    line = reader.ReadLine();
                }
            }

            using (StringReader reader = new StringReader(Properties.Settings.Default.SalesReps))
            {
                string line = reader.ReadLine();

                while (line != null)
                {
                    string[] items = line.Split(',');

                    try
                    {
                        SalesRepNames.Add(items[0]);
                        SalesRepEmails.Add(items[1]);
                        SalesRepPhones.Add(items[2]);
                        SalesRepRegions.Add(items[3]);
                        SalesRepPosition.Add(items[4]);
                    }
                    catch
                    {
                        Log("Failed to read from SalesReps.txt");
                    }

                    line = reader.ReadLine();
                }
            }

            //Set the comboBoxes
            comboBoxState.DataSource = RegionNames;
            comboBoxState.Refresh();
            comboBoxRepresentative.DataSource = SalesRepNames;

            //Clear the result labels on startup
            labelPhoneResult.Text = "";
            labelRepResult2.Text = "";
            labelContactResult2.Text = "";
            labelPhoneResult2.Text = "";
        }

        private void comboBoxState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxState.SelectedItem.ToString() != "")
            {
                Log("Selecting state: " + comboBoxState.Text);

                comboBoxRepresentative.SelectedIndex = 0;
                labelRepResult.Text = "Sales Rep: ";
                labelContactResult.Text = "Contact: ";
                labelPhoneResult.Text = "";

                labelRegionResult.Text = "Region: " + comboBoxState.SelectedItem.ToString();
            }
            else if (comboBoxState.SelectedItem == null || comboBoxRepresentative.SelectedItem == null)
            {
                return;
            }

            ResourceManager rm = new ResourceManager("SalesMap.Properties.Resources", Assembly.GetExecutingAssembly());
            pictureBox1.Image = rm.GetObject(comboBoxState.SelectedItem.ToString().Replace(')', '_').Replace('(', '_').Replace(' ', '_')) as Image;

            if (pictureBox1.Image == null && comboBoxState.SelectedIndex != 0)
            {
                labelNoImage.Text = "No Image Available";
            }
            else
            {
                labelNoImage.Text = "";
            }

            rm.ReleaseAllResources();

            string[] SalesRegions = SalesRepRegions.ToArray();
            string[] SalesNames = SalesRepNames.ToArray();
            string[] SalesEmails = SalesRepEmails.ToArray();
            string[] SalesPhones = SalesRepPhones.ToArray();
            string[] RegionPart = RegionParts.ToArray();

            int found = 0;

            string search = comboBoxState.SelectedItem.ToString().Split('(').Last().Split(')').First();
            Console.WriteLine("\"" + search + "\"");

            if (search == "")
            {
                labelRepResult.Text = "Sales Rep: ";
                labelContactResult.Text = "Contact: ";
                labelPhoneResult.Text = "";
                labelRepResult2.Text = "";
                labelContactResult2.Text = "";
                labelPhoneResult2.Text = "";
                return;
            }

            for (int i = 0; i < SalesRegions.Length; i++)
            {
                Console.WriteLine(SalesRegions[i]);
                if (SalesRegions[i].IndexOf(search) >= 0)
                {
                    found++;
                    Console.WriteLine("found: " + found);
                    if (found == 1)
                    {
                        labelRepResult.Text = "Sales Rep: " + SalesNames[i];
                        labelContactResult.Text = "Contact: " + SalesEmails[i];
                        labelPhoneResult.Text = SalesPhones[i];
                    }

                    if (found > 1)
                    {
                        labelRepResult2.Text = "2nd Sales Rep: " + SalesNames[i];
                        labelContactResult2.Text = "Contact: " + SalesEmails[i];
                        labelPhoneResult2.Text = SalesPhones[i];
                    }
                }
            }

            if (found < 2)
            {
                labelRepResult2.Text = "";
                labelContactResult2.Text = "";
                labelPhoneResult2.Text = "";
            }
        }

        private void comboBoxRepresentative_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxRepresentative.SelectedItem.ToString() != "")
            {
                Log("Selecting rep: " + comboBoxRepresentative.Text);

                comboBoxState.SelectedIndex = 0;
                labelRegionResult.Text = "Region: ";
            }
            else if (comboBoxRepresentative.SelectedItem == null || comboBoxState.SelectedItem == null)
            {
                return;
            }

            ResourceManager rm = new ResourceManager("SalesMap.Properties.Resources", Assembly.GetExecutingAssembly());
            pictureBox1.Image = rm.GetObject(comboBoxRepresentative.SelectedItem.ToString().Replace(' ', '_')) as Image;

            if (pictureBox1.Image == null && comboBoxRepresentative.SelectedIndex != 0)
            {
                labelNoImage.Text = "No Image Available";
            }
            else
            {
                labelNoImage.Text = "";
            }

            rm.ReleaseAllResources();



            labelRepResult.Text = "Sales Rep: " + comboBoxRepresentative.SelectedItem.ToString();
            labelContactResult.Text = "Contact: " + SalesRepEmails[comboBoxRepresentative.SelectedIndex];
            labelPhoneResult.Text = SalesRepPhones[comboBoxRepresentative.SelectedIndex];
            labelRegionResult.Text = "Region: " + SalesRepRegions[comboBoxRepresentative.SelectedIndex].Replace(":", ", ");

            labelRepResult2.Text = "";
            labelContactResult2.Text = "";
            labelPhoneResult2.Text = "";
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Log("Opening config");
            Settings config = new Settings();
            config.Show();
        }

        private void pictureBoxMap_Click(object sender, EventArgs e)
        {
            Log("Opening PDF map");
            string path = Properties.Settings.Default.MapFileLocation;

            try
            {
                System.Diagnostics.Process.Start(@path);
            }
            catch
            {
                MessageBox.Show("The path " + path + " is invalid.");
            }
        }

        private void pictureBoxOnlineMaps_Click(object sender, EventArgs e)
        {

            Update("v5.0");
            Log("Opening Google Maps");

            if (comboBoxState.SelectedItem.ToString() == "")
            {
                System.Diagnostics.Process.Start("https://www.google.com/maps/@38.9165981,-96.6887,5z");
            }
            else
            {
                string state = comboBoxState.SelectedItem.ToString().Split(')').Last().Replace(" ", "+");
                System.Diagnostics.Process.Start("https://www.google.com/maps/place/" + state);
            }
        }

        private void pictureBoxOffSMR_Click(object sender, EventArgs e)
        {
            Log("Composing an OffSMR email with state: " + comboBoxState.Text + " & rep: " + comboBoxRepresentative.Text);

            string rep = "";
            string cc = "";
            string phone = "";
            string subject = Properties.Settings.Default.OffSMRSubject;
            string body = Properties.Settings.Default.OffSMRBody + Properties.Settings.Default.OffSMRSignature;

            if (labelRepResult.Text == "Sales Rep: ")
            {
                MessageBox.Show("Please choose a Region or Sales Rep from the dropdowns");
                return;
            }
            else if (labelRepResult.Text != "" && labelRepResult2.Text == "")
            {
                rep = labelRepResult.Text;
                rep = rep.Substring(rep.IndexOf(": ") + 2);
                cc = labelContactResult.Text.Split(' ').ElementAt(1);
                phone = labelPhoneResult.Text;
            }
            else if (labelRepResult.Text != "" && labelRepResult2.Text != "")
            {
                //Choose a rep with the "North-South" dialog
                DialogResult res = new DialogResult();
                string firstRep = labelRepResult.Text.Split(':').Last();
                string secondRep = labelRepResult2.Text.Split(':').Last();

                North_South frm = new North_South(firstRep, secondRep);
                res = frm.ShowDialog();

                if (res == DialogResult.Yes) //"Yes" means "North rep"
                {
                    rep = labelRepResult.Text;
                    rep = rep.Substring(rep.IndexOf(": ") + 2);
                    cc = labelContactResult.Text.Split(' ').ElementAt(1);
                    phone = labelPhoneResult.Text;
                }
                else if (res == DialogResult.No) //"No" means "South rep"
                {
                    rep = labelRepResult2.Text;
                    rep = rep.Substring(rep.IndexOf(": ") + 2);
                    cc = labelContactResult2.Text.Split(' ').ElementAt(1);
                    phone = labelPhoneResult2.Text;
                }
                else //User closed out the dialog box
                {
                    return;
                }
            }

            cc = removeSpecial(cc); //Remove any extraneous characters

            string[] RegionNamesArray = RegionNames.ToArray();
            string[] RegionAreaArray = RegionArea.ToArray();
            string[] SalesRepNamesArray = SalesRepNames.ToArray();
            string[] SalesRepEmailArray = SalesRepEmails.ToArray();
            string[] SalesRepPositionArray = SalesRepPosition.ToArray();
            string area = "";
            string rsm = "";

            //Find the RSM
            if (comboBoxState.SelectedItem.ToString() != "")
            {
                for (var i = 0; i < RegionNamesArray.Length; i++)
                {
                    if (RegionNamesArray[i] == comboBoxState.SelectedItem.ToString())
                    {
                        area = "RSM:" + RegionAreaArray[i];
                        break;
                    }
                }

                for (var i = 0; i < SalesRepPositionArray.Length; i++)
                {
                    if (SalesRepPositionArray[i].IndexOf(area) >= 0)
                    {
                        rsm = SalesRepEmailArray[i];
                        break;
                    }
                }
            }
            else if (comboBoxRepresentative.SelectedItem.ToString() != "")
            {
                for (var i = 0; i < SalesRepNamesArray.Length; i++)
                {
                    if (SalesRepNamesArray[i].IndexOf(comboBoxRepresentative.SelectedItem.ToString()) >= 0)
                    {
                        area = "RSM:" + SalesRepPositionArray[i].Split(':')[1];
                        break;
                    }
                }

                for (var i = 0; i < SalesRepPositionArray.Length; i++)
                {
                    if (SalesRepPositionArray[i].IndexOf(area) >= 0)
                    {
                        rsm = SalesRepEmailArray[i];
                        break;
                    }
                }
            }

            if (rsm == cc) //If the rsr IS the rsm
            {
                cc = rsm;
            }
            else if (rsm != "") //If the rsr IS NOT the rsm
            {
                cc += ";" + rsm;
            }
            else if (rsm == "" && area != "") //If there is no rsm
            {
                if (comboBoxRepresentative.SelectedItem.ToString() != "")
                {
                    Log("Could not find an RSM for the selection: " + comboBoxRepresentative.Text);
                }
                else if(comboBoxState.SelectedItem.ToString() != "")
                {
                    Log("Could not find an RSM for the selection: " + comboBoxState.Text);
                }
            }

            subject = replaceVariables(subject, rep, cc.Split(';')[0], phone);
            body = replaceVariables(body, rep, cc.Split(';')[0], phone);

            ThreadPool.QueueUserWorkItem(composeOutlook, new object[] {cc, subject, body});
        }

        private void composeOutlook(object parameters)
        {
            object[] array = parameters as object[];

            string cc = Convert.ToString(array[0]);
            string subject = Convert.ToString(array[1]);
            string body = Convert.ToString(array[2]);


            try
            {
                Outlook.Application outlookApp = new Outlook.Application();
                Outlook.MailItem mailItem = (Outlook.MailItem)outlookApp.CreateItem(Outlook.OlItemType.olMailItem);

                mailItem.CC = cc;
                mailItem.Subject = subject;
                mailItem.HTMLBody = body;
                mailItem.Display(true);
            }
            catch (Exception eX)
            {
                MessageBox.Show("Failed to create the email. (Exception: " + eX.Message + "\n\n Please try again");
                Log("Failed to create email with cc: " + cc + " & subject: " + subject + " & exception: " + eX.Message);
            }
        }

        private string replaceVariables(string raw, string repName, string repEmail, string repPhone)
        {
            string rawReplaced = raw;
            rawReplaced = rawReplaced.Replace("{SALESREPNAME}", repName);
            rawReplaced = rawReplaced.Replace("{SALESREPEMAIL}", repEmail);
            rawReplaced = rawReplaced.Replace("{SALESREPPHONE}", repPhone);
            return rawReplaced;
        }

        private void labelContactResult_Click(object sender, EventArgs e)
        {        
            string temp = labelContactResult.Text;
            string copy = removeSpecial(temp.Substring(temp.IndexOf(": ") + 2));

            try
            {
                Clipboard.SetText(copy);
                labelContactResult.Text = "Contact: COPIED!";
            }
            catch
            {
                Log("Attempted to set the clipboard text and failed");
                labelContactResult.Text = "Contact: FAILED TO COPY...TRY AGAIN";
            }

            Application.DoEvents();
            Thread.Sleep(750);
            labelContactResult.Text = temp;
        }

        private void labelPhoneResult_Click(object sender, EventArgs e)
        {
            string temp = labelPhoneResult.Text;
            string copy = removeSpecial(temp);

            try
            {
                Clipboard.SetText(copy);
                labelPhoneResult.Text = "COPIED!";
            }
            catch
            {
                Log("Attempted to set the clipboard text and failed");
                labelPhoneResult.Text = "FAILED TO COPY...TRY AGAIN";
            }

            Application.DoEvents();
            Thread.Sleep(750);
            labelPhoneResult.Text = temp;
        }

        private void labelContactResult2_Click(object sender, EventArgs e)
        {
            string temp = labelContactResult2.Text;
            string copy = removeSpecial(temp.Substring(temp.IndexOf(": ") + 2));

            try
            {
                Clipboard.SetText(copy);
                labelContactResult2.Text = "Contact: COPIED!";
            }
            catch
            {
                Log("Attempted to set the clipboard text and failed");
                labelContactResult2.Text = "Contact: FAILED TO COPY...TRY AGAIN";
            }

            Application.DoEvents();
            Thread.Sleep(750);
            labelContactResult2.Text = temp;
        }

        private void labelPhoneResult2_Click(object sender, EventArgs e)
        {
            string temp = labelPhoneResult2.Text;
            string copy = removeSpecial(temp);

            try
            {
                Clipboard.SetText(copy);
                labelPhoneResult2.Text = "COPIED!";
            }
            catch
            {
                Log("Attempted to set the clipboard text and failed");
                labelPhoneResult2.Text = "FAILED TO COPY...TRY AGAIN";
            }

            Application.DoEvents();
            Thread.Sleep(750);
            labelPhoneResult2.Text = temp;
        }

        private string diff(string s1, string s2)
        {
            if (s1 == s2)
                return null;
            
            if (s1.Length > s2.Length)
            {
                for (var i = 0; i < s1.Length; i++)
                {
                    if (s1[i] != s2[i])
                    {
                        if (i >= 10 && i + 10 < s1.Length)
                            return s1.Substring(i - 10, 21) + " ||| " + s2.Substring(i - 10, 21);
                        else if (i >= 10)
                            return s1.Substring(i - 10, 11) + " ||| " + s2.Substring(i - 10, 11);
                        else
                            return s1.Substring(0, 10) + " ||| " + s2.Substring(0, 11);
                    }
                        
                }
            }
            else
            {
                for (var i = 0; i < s2.Length; i++)
                {
                    if (s1[i] != s2[i])
                    {
                        if (i >= 10 && i + 10 < s2.Length)
                            return s1.Substring(i - 10, 21) + " ||| " + s2.Substring(i - 10, 21);
                        else if (i >= 10)
                            return s1.Substring(i - 10, 11) + " ||| " + s2.Substring(i - 10, 11);
                        else
                            return s1.Substring(0, 10) + " ||| " + s2.Substring(0, 10);
                    }

                }
            }
            return null;
        }

        private void Update(string version)
        {
            Updater updater = new Updater(version);
            updater.ShowDialog();
        }

        private string removeSpecial(string input)
        {
            input = input.Replace("\r", "").Replace("\n", "").Replace(" ", "").Replace("\t", "");

            return input;
        }

        private void Log(string itemToLog)
        {
            string logPath = @"C:\Users\" + Environment.UserName + @"\log.txt";
            DateTime date = DateTime.UtcNow;
            File.AppendAllText(logPath, "[" + date + " UTC] " + itemToLog + Environment.NewLine);
        }

        private void Log(string itemToLog, bool addTimeStamp)
        {
            string logPath = @"C:\Users\" + Environment.UserName + @"\log.txt";

            if (addTimeStamp)
            {
                DateTime date = DateTime.UtcNow;
                File.AppendAllText(logPath, "[" + date + " UTC] " + itemToLog + Environment.NewLine);
            }
            else
                File.AppendAllText(logPath, itemToLog + Environment.NewLine);
        }

        private void checkFirstRun()
        {
            try //ToDo: Eventually, remove this (if we're never hitting it)
            {
                string regionPath = @"C:\Users\" + Environment.UserName + @"\Regions.txt";
                string salesPath = @"C:\Users\" + Environment.UserName + @"\SalesReps.txt";
                string offSMRPath = @"C:\Users\" + Environment.UserName + @"\OffSMR.txt";
                string settingsPath = @"C:\Users\" + Environment.UserName + @"\AppData\Local\SalesMap";
                RegistryKey key = Registry.CurrentUser.OpenSubKey("SalesMap", true);

                if (key == null || key.GetValue("FirstRun") == null)
                {
                    Log("Key does not exist. Creating Key...");
                    try
                    {
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SalesMap");
                        key.SetValue("FirstRun", Properties.Settings.Default.Version);
                    }
                    catch
                    {
                        Log("Could not create and set key (key does not exist).");
                        return;
                    }

                    try
                    {
                        DirectoryInfo di = new DirectoryInfo(settingsPath);
                        foreach (DirectoryInfo dir in di.GetDirectories())
                            dir.Delete(true);

                        Log("Cleared out the old settings and AppData folder");

                        ProcessStartInfo Info = new ProcessStartInfo();
                        Info.Arguments = "/C ping 127.0.0.1 -n 2 && \"" + Application.ExecutablePath + "\"";
                        Info.WindowStyle = ProcessWindowStyle.Hidden;
                        Info.CreateNoWindow = true;
                        Info.FileName = "cmd.exe";
                        Process.Start(Info);
                        Application.Exit();
                    }
                    catch
                    {
                        Log("Could not clear out old settings folder");
                    }
                }

                //Force the user to set up their signature
                if (removeSpecial(Properties.Settings.Default.OffSMRSignature) == removeSpecial(Properties.Settings.Default.OffSMRSignatureDefault))
                {
                    MessageBox.Show("Please set up your signature in the settings!\n\n(Change \"YOUR_NAME\" and \"Application Engineer\" to be your name and title)");

                    Log("Opening config so the user can set their signature");
                    Settings config = new Settings();
                    config.ShowDialog();
                }

                if (key.GetValue("FirstRun").ToString() != Properties.Settings.Default.Version)
                {
                    key.SetValue("FirstRun", Properties.Settings.Default.Version);
                    Log("First time running version " + Properties.Settings.Default.Version + " of this program. Last version: " + key.GetValue("FirstRun").ToString());
                    Log("This was the last time this will run");
                }

                if (key.GetValue("Updating") != null && Convert.ToBoolean(key.GetValue("Updating")) == true)
                {
                    try
                    {
                        File.Delete(Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\") + 1) + "SalesMap-old.exe");
                        Log("Deleted the old executable");
                    }
                    catch (Exception ex)
                    {
                        Log("Could not delete the old executable: " + ex.Message);
                    }

                    key.SetValue("Updating", false);
                    Log("Set \"Updating\" key to false");
                }

                //Delete old files (if they exist)

                if (File.Exists(offSMRPath))
                {
                    File.Delete(offSMRPath);
                    Log("Deleted OffSMR.txt from version " + key.GetValue("FirstRun").ToString() + " of this program");
                }

                if (File.Exists(regionPath))
                {
                    File.Delete(regionPath);
                    Log("Deleted the Regions.txt from version " + key.GetValue("FirstRun").ToString() + " of this program");
                }

                if (File.Exists(salesPath))
                {
                    File.Delete(salesPath);
                    Log("Deleted the SalesReps.txt from version " + key.GetValue("FirstRun").ToString() + " of this program");
                }

                try
                {
                    key.SetValue("FirstRun", Properties.Settings.Default.Version);
                }
                catch
                {
                    Log("Could not set value at end of checkFirstRun");
                }

                key.Close();
            }
            catch (Exception ex)
            {
                Log("Problems with running checkFirstRun. Error: " + ex.Message);
            }
        }
    }
}
