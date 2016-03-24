using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Log("------------ STARTING SALESMAP ------------");

            CheckFirstRun();

            //File.WriteAllText(@"C:\Users\" + Environment.UserName + @"\log.txt", ""); //Clear the log

            if (Properties.Settings.Default.AutoCheckUpdate)
            {
                compareFiles();

                WebClient client = new WebClient();
                string url = "https://github.com/derekantrican/SalesMap/releases";
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

                string GitVersion = html.Substring(html.IndexOf("<span class=\"css-truncate-target\">v") + 34).Split('<')[0];
                string thisVersion = Properties.Settings.Default.Version;

                if (GitVersion != thisVersion)
                {
                    if (MessageBox.Show("A new version is available!\n\nThe current version is " + GitVersion + " and you are running " + thisVersion +
                                        "\n\nGo to " + url + " to download the new version?",
                                        "New Update Available!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(url);
                    }
                }
            }

            readFiles();
        }

        public void compareFiles()
        {
            string regionPath = @"C:\Users\" + Environment.UserName + @"\Regions.txt";
            string salesPath = @"C:\Users\" + Environment.UserName + @"\SalesReps.txt";

            string regionText = "";
            string salesText = "";

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

            if (File.Exists(regionPath))
            {
                regionText = File.ReadAllText(regionPath);
            }
            else
            {
                using (StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("SalesMap.Regions.txt")))
                {
                    regionText = reader.ReadToEnd();
                }
            }

            if (File.Exists(salesPath))
            {
                salesText = File.ReadAllText(salesPath);
            }
            else
            {
                using (StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("SalesMap.SalesReps.txt")))
                {
                    salesText = reader.ReadToEnd();
                }
            }

            //Remove the extra character that comes at the end of these strings and replace "&amp;" with "&"
            regionTextOnline = regionTextOnline.TrimEnd(regionTextOnline[regionTextOnline.Length - 1]);
            regionTextOnline = regionTextOnline.Replace("&amp;", "&");
            salesTextOnline = salesTextOnline.TrimEnd(salesTextOnline[salesTextOnline.Length - 1]);
            salesTextOnline = salesTextOnline.Replace("&amp;", "&");

            //Compare the raw text of files by checking files without special characters
            regionText = removeSpecial(regionText);
            salesText = removeSpecial(salesText);
            regionTextOnline = removeSpecial(regionTextOnline);
            salesTextOnline = removeSpecial(salesTextOnline);

            if (regionText != regionTextOnline)
            {
                Log("Regions.txt is not the same as online. Updating...");
                File.WriteAllText(regionPath, regionTextOnline);
            }

            if (salesText != salesTextOnline)
            {
                Log("SalesReps.txt is not the same as online. Updating...");
                File.WriteAllText(salesPath, salesTextOnline);
            }
        }

        public void readFiles()
        {
            string regionPath = @"C:\Users\" + Environment.UserName + @"\Regions.txt";
            string salesPath = @"C:\Users\" + Environment.UserName + @"\SalesReps.txt";
            Stream fileStream;
            Stream fileStream1;

            if (File.Exists(regionPath))
            {
                Console.WriteLine("Regions file exists");
                fileStream = File.Open(regionPath, FileMode.Open);
            }
            else
            {
                Console.WriteLine("Regions file does not exist");
                var resourceRegions = "SalesMap.Regions.txt";
                var assembly = Assembly.GetExecutingAssembly();

                fileStream = assembly.GetManifestResourceStream(resourceRegions);
            }

            using (StreamReader reader = new StreamReader(fileStream))
            {
                var lines = new List<string[]>();
                int Row = 0;
                while (!reader.EndOfStream)
                {
                    string[] Line = reader.ReadLine().Split(',');
                    try
                    {
                        RegionNames.Add(Line[0]);
                        RegionParts.Add(Line[1]);
                        RegionArea.Add(Line[2]);
                        Row++;
                    }
                    catch
                    {
                        Log("Failed to read from Regions.txt");
                        MessageBox.Show("Cannot read from Regions.txt in C:\\Users\\" + Environment.UserName + "\\Regions.txt \n\nCheck to make sure you have the right amount of commas");
                        Environment.Exit(1);
                    }
                }
            }

            if (File.Exists(salesPath))
            {
                Console.WriteLine("SalesReps file exists");
                fileStream1 = File.Open(salesPath, FileMode.Open);
            }
            else
            {
                Console.WriteLine("SalesReps file does not exist");
                var resourceSales = "SalesMap.SalesReps.txt";
                var assembly1 = Assembly.GetExecutingAssembly();

                fileStream1 = assembly1.GetManifestResourceStream(resourceSales);
            }

            using (StreamReader reader1 = new StreamReader(fileStream1))
            {
                var lines = new List<string[]>();
                int Row = 0;
                while (!reader1.EndOfStream)
                {
                    string[] Line = reader1.ReadLine().Split(',');
                    try
                    {
                        SalesRepNames.Add(Line[0]);
                        SalesRepEmails.Add(Line[1]);
                        SalesRepPhones.Add(Line[2]);
                        SalesRepRegions.Add(Line[3]);
                        SalesRepPosition.Add(Line[4]);
                        Row++;
                    }
                    catch
                    {
                        Log("Failed to read from SalesReps.txt");
                        MessageBox.Show("Cannot read from SalesReps.txt in C:\\Users\\" + Environment.UserName + "\\SalesReps.txt \n\nCheck to make sure you have the right amount of commas");
                        Environment.Exit(2);
                    }
                }
            }

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
            labelContactResult.Text = "Contact: " + SalesRepEmails[comboBoxRepresentative.SelectedIndex] +
                                      Environment.NewLine + "\t\t\t   " + SalesRepPhones[comboBoxRepresentative.SelectedIndex];
            labelRegionResult.Text = "Region: " + SalesRepRegions[comboBoxRepresentative.SelectedIndex].Replace(":", ", ");

            labelRepResult2.Text = "";
            labelContactResult2.Text = "";
            labelPhoneResult2.Text = "";
        }

        private void SalesMapSearch_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Settings config = new Settings();
            config.Show();
        }

        private void pictureBoxMap_Click(object sender, EventArgs e)
        {
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxOnlineMaps_Click(object sender, EventArgs e)
        {
            if (comboBoxState.SelectedItem.ToString() == "")
            {
                MessageBox.Show("\"State/Province:\" must be defined");
            }
            else
            {
                string state = comboBoxState.SelectedItem.ToString().Split(')').Last().Replace(" ", "+");
                System.Diagnostics.Process.Start("https://www.google.com/maps/place/" + state);
            }
        }

        private void pictureBoxOffSMR_Click(object sender, EventArgs e)
        {
            string rep = "";
            string cc = "";
            string phone = "";
            string subject = "SigmaNEST Maintenance Subscription Renewal";
            string body = "";

            string offSMRPath = @"C:\Users\" + Environment.UserName + @"\OffSMR.txt";
            Stream fileStream;

            if (File.Exists(offSMRPath))
            {
                Console.WriteLine("Off SMR file exists");
                fileStream = File.Open(offSMRPath, FileMode.Open);
            }
            else
            {
                Console.WriteLine("Off SMR file does not exist");
                var resourceOffSMR = "SalesMap.OffSMR.txt";
                var assembly = Assembly.GetExecutingAssembly();

                fileStream = assembly.GetManifestResourceStream(resourceOffSMR);
            }

            using (StreamReader reader = new StreamReader(fileStream))
            {
                string offSMRBody = "";

                while (!reader.EndOfStream)
                {
                    offSMRBody += reader.ReadLine();
                    offSMRBody += Environment.NewLine;
                }
                Console.WriteLine(offSMRBody.Split('\n').Length);

                body = offSMRBody;
            }

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
                    Log("Could not find an RSM for the selection: " + comboBoxRepresentative.SelectedItem.ToString());
                }
                else if(comboBoxState.SelectedItem.ToString() != "")
                {
                    Log("Could not find an RSM for the selection: " + comboBoxState.SelectedItem.ToString());
                }
            }

            subject = mailtoFormat(subject, rep, cc, phone);
            body = mailtoFormat(body, rep, cc, phone);

            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "mailto:?cc=" + cc + "&subject=" + subject + "&body=" + body;
            proc.Start();
        }

        private string mailtoFormat(string raw, string repName, string repEmail, string repPhone)
        {
            string rawReplaced = raw;
            rawReplaced = rawReplaced.Replace(" ", "%20").Replace("\n", "%0D%0A");
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

        private string removeSpecial(string input)
        {
            input = input.Replace("\r", "").Replace("\n", "").Replace(" ", "").Replace("\t", "");

            return input;
        }

        private void Log(string itemToLog)
        {
            DateTime date = DateTime.UtcNow;
            string logPath = @"C:\Users\" + Environment.UserName + @"\log.txt";

            File.AppendAllText(logPath, "[" + date + "] " + itemToLog + Environment.NewLine);
        }

        private void CheckFirstRun()
        {
            string regionPath = @"C:\Users\" + Environment.UserName + @"\Regions.txt";
            string salesPath = @"C:\Users\" + Environment.UserName + @"\SalesReps.txt";

            if (Properties.Settings.Default.FirstRun)
            {
                Log("First time running this program");

                if (File.Exists(regionPath))
                {
                    File.Delete(regionPath);
                    Log("Deleted the Regions.txt from the last version of this program");
                }

                if (File.Exists(salesPath))
                {
                    File.Delete(salesPath);
                    Log("Deleted the SalesReps.txt from the last version of this program");
                }

                Properties.Settings.Default.FirstRun = false;
                Properties.Settings.Default.Save();
                Log("This was the last time this will run");
            }
        }
    }
}
