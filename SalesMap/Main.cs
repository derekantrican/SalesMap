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
using System.Configuration;
using System.Xml.Linq;

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
            this.Text = this.Text + " (" + Properties.Settings.Default.Version + ")"; //Change the name of the window to include the current version
            Common.Log("------------ STARTING SALESMAP (" + Properties.Settings.Default.Version + ") ------------");

            checkFirstRun();
            checkForUpdate();
            compareFiles();

            //File.WriteAllText(@"C:\Users\" + Environment.UserName + @"\log.txt", ""); //Clear the log

            readFiles();
        }

        private void checkForUpdate()
        {
            if (Properties.Settings.Default.AutoCheckUpdate)
            {
                string GitVersion = Common.checkGitHub();
                string thisVersion = Properties.Settings.Default.Version;

                if (GitVersion != thisVersion)
                {
                    Common.Log("Prompted for new update. Current: " + thisVersion + "  Online: " + GitVersion);

                    if (MessageBox.Show("A new version is available!\n\nThe current version is " + GitVersion + " and you are running " + thisVersion +
                                        "\n\nDo you want to update to the new version?",
                                        "New Update Available!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                    {
                        Common.Log("User selected \"Yes\" for the new update");
                        Update(GitVersion);
                    }
                    else
                    {
                        Common.Log("User selected \"No\" for the new update");
                    }
                }
            }
        }

        private void SalesMapSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
            Common.Log("++++++++++++ CLOSING SALESMAP ++++++++++++");

            if (Properties.Settings.Default.SendLog)
            {
                SendStatistics();

                string user = Environment.UserName;
                string logPath = @"C:\Users\" + user + @"\log.txt";

                if (File.Exists(logPath))
                {
                    ProcessStartInfo Info = new ProcessStartInfo();
                    Info.UseShellExecute = false;
                    Info.RedirectStandardOutput = true;
                    Info.Arguments = @"/C copy ""C:\Users\" + Environment.UserName + @"\log.txt"" ""\\sigmatek.net\Documents\Employees\Derek_Antrican\SalesMap\Common.Log Files\" + Environment.UserName + " log.txt\" /y";
                    Info.WindowStyle = ProcessWindowStyle.Hidden;
                    Info.CreateNoWindow = true;
                    Info.FileName = "cmd.exe";
                    Process infoProcess = Process.Start(Info);
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
                Common.Log("Attempted to get the online databases and failed to get html");
                return;
            }

            //Remove the extra character that comes at the end of these strings and replace "&amp;" with "&"
            regionTextOnline = regionTextOnline.TrimEnd(regionTextOnline[regionTextOnline.Length - 1]);
            regionTextOnline = regionTextOnline.Replace("&amp;", "&");
            salesTextOnline = salesTextOnline.TrimEnd(salesTextOnline[salesTextOnline.Length - 1]);
            salesTextOnline = salesTextOnline.Replace("&amp;", "&");

            //Compare the raw text of files by checking files without special characters
            if (Common.RemoveSpecial(Properties.Settings.Default.Regions) != Common.RemoveSpecial(regionTextOnline) && regionTextOnline != "")
            {
                Common.Log("Internal Regions is not the same as online. Updating...");
                Common.Log("DIFF: " + Common.Diff(Common.RemoveSpecial(Properties.Settings.Default.Regions), Common.RemoveSpecial(regionTextOnline)), false);
                Properties.Settings.Default.Regions = regionTextOnline;
            }

            if (Common.RemoveSpecial(Properties.Settings.Default.SalesReps) != Common.RemoveSpecial(salesTextOnline) && salesTextOnline != "")
            {
                Common.Log("Internal SalesReps is not the same as online. Updating...");
                Common.Log("DIFF: " + Common.Diff(Common.RemoveSpecial(Properties.Settings.Default.SalesReps), Common.RemoveSpecial(salesTextOnline)), false);
                Properties.Settings.Default.SalesReps = salesTextOnline;
            }

            url = "https://github.com/derekantrican/SalesMap/wiki/International-Databases";
            html = "";

            string internationalRegionTextOnline = "";
            string internationalSalesTextOnline = "";

            try
            {
                html = client.DownloadString(url);

                internationalRegionTextOnline = html.Substring(html.IndexOf("<pre><code>") + 11).Split('<')[0];
                internationalSalesTextOnline = html.Substring(html.LastIndexOf("<pre><code>") + 11).Split('<')[0];
            }
            catch
            {
                Common.Log("Attempted to get the online databases and failed to get html");
                return;
            }

            //Remove the extra character that comes at the end of these strings and replace "&amp;" with "&"
            internationalRegionTextOnline = internationalRegionTextOnline.TrimEnd(internationalRegionTextOnline[internationalRegionTextOnline.Length - 1]);
            internationalRegionTextOnline = internationalRegionTextOnline.Replace("&amp;", "&");
            internationalSalesTextOnline = internationalSalesTextOnline.TrimEnd(internationalSalesTextOnline[internationalSalesTextOnline.Length - 1]);
            internationalSalesTextOnline = internationalSalesTextOnline.Replace("&amp;", "&");

            //Compare the raw text of files by checking files without special characters
            if (Common.RemoveSpecial(Properties.Settings.Default.InternationalRegions) != Common.RemoveSpecial(internationalRegionTextOnline) && internationalRegionTextOnline != "")
            {
                Common.Log("Internal InternationalRegions is not the same as online. Updating...");
                Common.Log("DIFF: " + Common.Diff(Common.RemoveSpecial(Properties.Settings.Default.InternationalRegions), Common.RemoveSpecial(internationalRegionTextOnline)), false);
                Properties.Settings.Default.InternationalRegions = internationalRegionTextOnline;
            }

            if (Common.RemoveSpecial(Properties.Settings.Default.InternationalReps) != Common.RemoveSpecial(internationalSalesTextOnline) && internationalSalesTextOnline != "")
            {
                Common.Log("Internal InternationalSalesReps is not the same as online. Updating...");
                Common.Log("DIFF: " + Common.Diff(Common.RemoveSpecial(Properties.Settings.Default.InternationalReps), Common.RemoveSpecial(internationalSalesTextOnline)), false);
                Properties.Settings.Default.InternationalReps = internationalSalesTextOnline;
            }

            Properties.Settings.Default.Save();
        }

        public void readFiles()
        {
            string regionsSource = "";
            string repsSource = "";

            if (Properties.Settings.Default.UseInternational)
            {
                regionsSource = Properties.Settings.Default.InternationalRegions;
                repsSource = Properties.Settings.Default.InternationalReps;
            }
            else
            {
                regionsSource = Properties.Settings.Default.Regions;
                repsSource = Properties.Settings.Default.SalesReps;
            }

            if (regionsSource == "" || repsSource == "")
            {
                Common.Log("Error setting comboBox values (UseInternational: " + Properties.Settings.Default.UseInternational + ")");
                Environment.Exit(1);
            }

            using (StringReader reader = new StringReader(regionsSource))
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
                        Common.Log("Failed to read from Regions.txt");
                    }

                    line = reader.ReadLine();
                }
            }

            using (StringReader reader = new StringReader(repsSource))
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
                        Common.Log("Failed to read from SalesReps.txt");
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
                Common.Log("Selecting state: " + comboBoxState.Text);

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

            showPicture("Regions/" + comboBoxState.SelectedItem.ToString().Replace(" ", "%20") + ".jpg");

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
                Common.Log("Selecting rep: " + comboBoxRepresentative.Text);

                comboBoxState.SelectedIndex = 0;
                labelRegionResult.Text = "Region: ";
            }
            else if (comboBoxRepresentative.SelectedItem == null || comboBoxState.SelectedItem == null)
            {
                return;
            }

            showPicture("SalesReps/" + comboBoxRepresentative.SelectedItem.ToString().Replace(" ", "%20") + ".jpg");

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
            Common.Log("Opening config");
            Settings config = new Settings();
            config.Show();
        }

        private void pictureBoxMap_Click(object sender, EventArgs e)
        {
            Common.Log("Opening PDF map");
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
            XMLFunctions.parseReps();
            return;

            Common.Log("Opening Google Maps");

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
            Common.Log("Composing an OffSMR email with state: " + comboBoxState.Text + " & rep: " + comboBoxRepresentative.Text);

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

            cc = Common.RemoveSpecial(cc); //Remove any extraneous characters

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
                    if (RegionNamesArray[i] == comboBoxState.SelectedItem.ToString() && RegionAreaArray[i] != "")
                    {
                        area = "RSM:" + RegionAreaArray[i];
                        break;
                    }
                }

                for (var i = 0; i < SalesRepPositionArray.Length; i++)
                {
                    if (SalesRepPositionArray[i].IndexOf(area) >= 0 && area != "")
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
                    Common.Log("Could not find an RSM for the selection: " + comboBoxRepresentative.Text);
                }
                else if(comboBoxState.SelectedItem.ToString() != "")
                {
                    Common.Log("Could not find an RSM for the selection: " + comboBoxState.Text);
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
                Common.Log("Failed to create email with cc: " + cc + " & subject: " + subject + " & exception: " + eX.Message);
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
            string copy = Common.RemoveSpecial(temp.Substring(temp.IndexOf(": ") + 2));

            try
            {
                Clipboard.SetText(copy);
                labelContactResult.Text = "Contact: COPIED!";
                Common.Log("Clicked first Sales Rep email and set clipboard to \"" + copy + "\"");
            }
            catch
            {
                Common.Log("Attempted to set the clipboard text and failed");
                labelContactResult.Text = "Contact: FAILED TO COPY...TRY AGAIN";
            }

            Application.DoEvents();
            Thread.Sleep(750);
            labelContactResult.Text = temp;
        }

        private void labelPhoneResult_Click(object sender, EventArgs e)
        {
            string temp = labelPhoneResult.Text;
            string copy = Common.RemoveSpecial(temp);

            try
            {
                Clipboard.SetText(copy);
                labelPhoneResult.Text = "COPIED!";
                Common.Log("Clicked first Sales Rep phone and set clipboard to \"" + copy + "\"");
            }
            catch
            {
                Common.Log("Attempted to set the clipboard text and failed");
                labelPhoneResult.Text = "FAILED TO COPY...TRY AGAIN";
            }

            Application.DoEvents();
            Thread.Sleep(750);
            labelPhoneResult.Text = temp;
        }

        private void labelContactResult2_Click(object sender, EventArgs e)
        {
            string temp = labelContactResult2.Text;
            string copy = Common.RemoveSpecial(temp.Substring(temp.IndexOf(": ") + 2));

            try
            {
                Clipboard.SetText(copy);
                labelContactResult2.Text = "Contact: COPIED!";
                Common.Log("Clicked second Sales Rep email and set clipboard to \"" + copy + "\"");
            }
            catch
            {
                Common.Log("Attempted to set the clipboard text and failed");
                labelContactResult2.Text = "Contact: FAILED TO COPY...TRY AGAIN";
            }

            Application.DoEvents();
            Thread.Sleep(750);
            labelContactResult2.Text = temp;
        }

        private void labelPhoneResult2_Click(object sender, EventArgs e)
        {
            string temp = labelPhoneResult2.Text;
            string copy = Common.RemoveSpecial(temp);

            try
            {
                Clipboard.SetText(copy);
                labelPhoneResult2.Text = "COPIED!";
                Common.Log("Clicked second Sales Rep phone and set clipboard to \"" + copy + "\"");
            }
            catch
            {
                Common.Log("Attempted to set the clipboard text and failed");
                labelPhoneResult2.Text = "FAILED TO COPY...TRY AGAIN";
            }

            Application.DoEvents();
            Thread.Sleep(750);
            labelPhoneResult2.Text = temp;
        }

        [STAThread]
        private void showPicture(string name)
        {
            string url = "http://info.sigmatek.net/downloads/SalesMap/" + name;

            try
            {
                labelNoImage.Hide();
                pictureBox1.Show();
                pictureBox1.Load(url);
            }
            catch
            {
                pictureBox1.Hide();
                labelNoImage.Show();
            }
        }

        private void Update(string version)
        {
            Updater updater = new Updater(version);
            updater.ShowDialog();
        }

        private void SendStatistics()
        {
            string statisticsPath = @"\\sigmatek.net\Documents\Employees\Derek_Antrican\SalesMap\Common.Log Files\usage statistics.txt";
            bool found = false;

            if (File.Exists(statisticsPath))
            {
                string[] contents = File.ReadAllLines(statisticsPath);
                List<string> contentsList = contents.OfType<string>().ToList();

                foreach (string s in contentsList)
                {
                    Console.WriteLine(s);

                    if (s.Split(',').First() == Environment.UserName)
                    {
                        contentsList[contentsList.IndexOf(s)] = Environment.UserName + "," + Properties.Settings.Default.Version + "," + TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
                        Common.Log("Added this session's info to the stats table");
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    contentsList.Add(Environment.UserName + "," + Properties.Settings.Default.Version + "," + TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time")));
                    Common.Log("Could not find a previous session in the stats table, so created one");
                }

                File.WriteAllLines(statisticsPath, contentsList);
            }
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
                    Common.Log("Key does not exist. Creating Key...");
                    try
                    {
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SalesMap");
                        key.SetValue("FirstRun", Properties.Settings.Default.Version);
                    }
                    catch
                    {
                        Common.Log("Could not create and set key (key does not exist).");
                        return;
                    }
                }

                //Force the user to set up their signature
                if (Common.RemoveSpecial(Properties.Settings.Default.OffSMRSignature) == Common.RemoveSpecial(Properties.Settings.Default.OffSMRSignatureDefault))
                {
                    MessageBox.Show("Please set up your signature in the settings!\n\n(Change \"YOUR_NAME\" and \"Application Engineer\" to be your name and title)");

                    Common.Log("Opening config so the user can set their signature");
                    Settings config = new Settings();
                    config.ShowDialog();
                }

                if (key.GetValue("FirstRun").ToString() != Properties.Settings.Default.Version)
                {
                    key.SetValue("FirstRun", Properties.Settings.Default.Version);
                    Common.Log("First time running version " + Properties.Settings.Default.Version + " of this program.");
                    Common.Log("This was the last time this will run");
                }

                if (key.GetValue("Updating") != null && Convert.ToBoolean(key.GetValue("Updating")) == true)
                {
                    try
                    {
                        File.Delete(Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\") + 1) + "SalesMap-old.exe");
                        Common.Log("Deleted the old executable");
                    }
                    catch (Exception ex)
                    {
                        Common.Log("Could not delete the old executable: " + ex.Message);
                    }

                    key.SetValue("Updating", false);
                    Common.Log("Set \"Updating\" key to false");
                }

                //Delete old files (if they exist)

                if (File.Exists(offSMRPath))
                {
                    File.Delete(offSMRPath);
                    Common.Log("Deleted OffSMR.txt from the last version of this program");
                }

                if (File.Exists(regionPath))
                {
                    File.Delete(regionPath);
                    Common.Log("Deleted the Regions.txt from the last version of this program");
                }

                if (File.Exists(salesPath))
                {
                    File.Delete(salesPath);
                    Common.Log("Deleted the SalesReps.txt from the last version of this program");
                }

                try
                {
                    key.SetValue("FirstRun", Properties.Settings.Default.Version);
                }
                catch
                {
                    Common.Log("Could not set value at end of checkFirstRun");
                }

                key.Close();
            }
            catch (Exception ex)
            {
                Common.Log("Problems with running checkFirstRun. Error: " + ex.Message);
            }
        }
    }
}
