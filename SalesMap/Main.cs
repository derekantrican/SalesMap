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
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace SalesMap
{
    public partial class SalesMapSearch : Form
    {
        public string CommandLineSelect { get; set; }
        public SalesMapSearch()
        {
            InitializeComponent();

            Common.CheckPaths();

            if (File.Exists(Path.Combine(Common.UserSettingsPath, "log.txt")) && File.ReadLines(Path.Combine(Common.UserSettingsPath, "log.txt")).Count() > 10000)
            {
                File.WriteAllText(Path.Combine(Common.UserSettingsPath, "log.txt"), ""); //Clear the log
                Common.Log("Cleared the log as it was longer than 10,000 lines");
            }

            this.Text = this.Text + " (" + Common.ThisVersion + ")"; //Change the name of the window to include the current version
            Common.Log("------------ STARTING SALESMAP (" + Common.ThisVersion + ") ------------");
            
            if (!Common.IsOnline)
            {
                Common.Log("No internet connection");
                MessageBox messageBox = new MessageBox("Not connected to the internet...", "You are not connected to the internet. SalesMap " + Common.ThisVersion + " requires an internet connection",
                    "Cancel", Common.MessageBoxResult.Cancel, true, "Retry", Common.MessageBoxResult.Retry);
                messageBox.ShowDialog();
                
                if (Common.DialogResult == Common.MessageBoxResult.Retry && !Common.IsOnline)
                {
                    MessageBox messageBox2 = new MessageBox("Not connected to the internet...", "...still can't find an internet connection. \n\nThe program will now exit.", "OK", Common.MessageBoxResult.OK);
                    messageBox2.ShowDialog();
                    Common.Log("Retried, but still no internet connection");
                    Environment.Exit(1);
                }
                else if (Common.DialogResult == Common.MessageBoxResult.Cancel)
                {
                    Environment.Exit(1);
                }
            }

            XMLFunctions.UpdateComboBoxes += populateComboBoxes;

            checkFirstRun();

            if ((bool)XMLFunctions.readSetting("AutoCheckForUpdates", typeof(bool), true))
                Common.CheckForUpdate();

            compareFiles();

            if (!(bool)XMLFunctions.readSetting("UseInternational", typeof(bool), false))
            {
                new Thread(() => XMLFunctions.parseRegions(false)).Start();
                new Thread(() => XMLFunctions.parseReps(false)).Start();
            }
            else
            {
                new Thread(() => XMLFunctions.parseRegions(true)).Start();
                new Thread(() => XMLFunctions.parseReps(true)).Start();
            }
        }

        private void SelectRegionFromCommandLine(string region)
        {
            string modifiedQuery = Regex.Replace(region, "[^a-zA-Z]", "").ToLower();
            //Common.Region foundRegion = null;
            //List<Common.Region> copyOfRegionsList = XMLFunctions.RegionList.ToList();
            //foreach (Common.Region r in copyOfRegionsList)
            //{
            //    if (r.Name != null && Regex.Replace(r.Name, "[^a-zA-Z]", "").ToLower() == modifiedQuery)
            //    {
            //        foundRegion = r;
            //        break;
            //    }
            //}
            Common.Region foundRegion = XMLFunctions.RegionList.Where(p => p.Name != null && Regex.Replace(p.Name, "[^a-zA-Z]", "").ToLower() == modifiedQuery).FirstOrDefault();



            comboBoxState.SelectedItem = foundRegion;
            comboBoxState_SelectedIndexChanged(null, null);
        }

        private void SalesMapSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
            XMLFunctions.saveSetting("MainWindowLocation", this.Location);

            Common.Log("++++++++++++ CLOSING SALESMAP ++++++++++++");

            if ((bool)XMLFunctions.readSetting("SendLogToDeveloper", typeof(bool), true))
            {
                string logPath = Path.Combine(Common.UserSettingsPath, "log.txt");

                if (File.Exists(logPath))
                {
                    ProcessStartInfo Info = new ProcessStartInfo();
                    Info.Arguments = "/C copy \"" + logPath + @""" ""\\sigmatek.net\Documents\Employees\Derek_Antrican\SalesMap\Log Files\" + Environment.UserName + " log.txt\" /y";
                    Info.WindowStyle = ProcessWindowStyle.Hidden;
                    Info.FileName = "cmd.exe";
                    Process infoProcess = Process.Start(Info);
                }

            }

            if ((bool)XMLFunctions.readSetting("SendStatisticsToDeveloper", typeof(bool), true))
            {
                string statisticsPath = Path.Combine(Common.UserSettingsPath, "stats.txt");

                if (File.Exists(statisticsPath))
                {
                    ProcessStartInfo Info = new ProcessStartInfo();
                    Info.Arguments = "/C copy \"" + statisticsPath + @""" ""\\sigmatek.net\Documents\Employees\Derek_Antrican\SalesMap\Statistics\" + Environment.UserName + ".txt\" /y";
                    Info.WindowStyle = ProcessWindowStyle.Hidden;
                    Info.FileName = "cmd.exe";
                    Process infoProcess = Process.Start(Info);
                }
            }
        }

        public void compareFiles()
        {
            //"Local XML" functionality is partially introduced for version 6.0+, but will not be implemented unless deemed necessary

            //XMLFunctions.getLastXmlOnlineUpdated(XMLFunctions.Database.Regions);
            //XMLFunctions.getLastXmlOnlineUpdated(XMLFunctions.Database.Reps);

            //if (XMLFunctions.getLastXmlLocalUpdated(XMLFunctions.Database.Regions) != null && 
            //    XMLFunctions.getLastXmlLocalUpdated(XMLFunctions.Database.Regions) < XMLFunctions.getLastXmlOnlineUpdated(XMLFunctions.Database.Regions))
            //{

                //Common.Log("Updated Regions.xml from " + XMLFunctions.getLastXmlLocalUpdated(XMLFunctions.Database.Regions).ToString() + " to " + XMLFunctions.getLastXmlOnlineUpdated(XMLFunctions.Database.Regions).ToString());
            //}

            //if (XMLFunctions.getLastXmlLocalUpdated(XMLFunctions.Database.Reps) != null &&
            //    XMLFunctions.getLastXmlLocalUpdated(XMLFunctions.Database.Reps) < XMLFunctions.getLastXmlOnlineUpdated(XMLFunctions.Database.Reps))
            //{

                //Common.Log("Updated SalesReps.xml from " + XMLFunctions.getLastXmlLocalUpdated(XMLFunctions.Database.Reps).ToString() + " to " + XMLFunctions.getLastXmlOnlineUpdated(XMLFunctions.Database.Reps).ToString());
            //}
        }

        public void populateComboBoxes()
        {
            if (!this.IsHandleCreated)
                return;

            //Set the comboBoxes
            this.Invoke((MethodInvoker)delegate 
            {
                comboBoxState.DataSource = XMLFunctions.RegionList;
                comboBoxState.DisplayMember = "DisplayName";
                comboBoxRepresentative.DataSource = XMLFunctions.SalesRepList;
                comboBoxRepresentative.DisplayMember = "DisplayName";
            });

            //Clear the result labels on startup
            this.Invoke((MethodInvoker)delegate 
            {
                labelPhoneResult.Text = "";
                labelRepResult2.Text = "";
                labelContactResult2.Text = "";
                labelPhoneResult2.Text = "";

                comboBoxState.Enabled = true;
                comboBoxRepresentative.Enabled = true;
            });

            if (!string.IsNullOrWhiteSpace(CommandLineSelect))
            {
                this.Invoke((MethodInvoker)delegate
                {
                    SelectRegionFromCommandLine(CommandLineSelect);
                });
            }
        }

        private void comboBoxState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isNullOrEmpty(comboBoxState) && comboBoxRepresentative.Items.Count > 0)
            {
                Common.Log("Selecting state: " + (comboBoxState.SelectedItem as Common.Region).DisplayName);

                comboBoxRepresentative.SelectedIndex = 0;
                labelRepResult.Text = "Sales Rep: ";
                labelContactResult.Text = "Contact: ";
                labelPhoneResult.Text = "";

                labelRegionResult.Text = "Region: " + (comboBoxState.SelectedItem as Common.Region).Name;
            }
            else if (isNullOrEmpty(comboBoxState) || isNullOrEmpty(comboBoxRepresentative))
            {
                labelRepResult.Text = "Sales Rep: ";
                labelContactResult.Text = "Contact: ";
                labelPhoneResult.Text = "";
                labelRepResult2.Text = "";
                labelContactResult2.Text = "";
                labelPhoneResult2.Text = "";
                labelSIMadmin.Text = "";
                labelRegionResult.Text = "Region: ";
                showPicture("");

                Common.SelectedItem = null;

                return;
            }

            Common.Stat();

            Common.SelectedItem = comboBoxState.SelectedItem as Common.Region;

            string pictureLocation = (comboBoxState.SelectedItem as Common.Region).Picture;
            new Thread(() => showPicture("Regions/" + pictureLocation)).Start();

            int found = 0;

            string search = (comboBoxState.SelectedItem as Common.Region).Name;
            Console.WriteLine("\"" + search + "\"");

            if (search == "")
                return;

            foreach (Common.SalesRep rep in XMLFunctions.SalesRepList)
            {
                if (rep.Responsibilities != null && rep.Responsibilities.Find(p => p == search) != null)
                {
                    found++;

                    if (found == 1)
                    {
                        labelRepResult.Text = "Sales Rep: " + rep.DisplayName;
                        labelContactResult.Text = "Contact: " + rep.Email;
                        labelPhoneResult.Text = rep.Phone;
                    }

                    if (found > 1)
                    {
                        labelRepResult2.Text = "2nd Sales Rep: " + rep.DisplayName;
                        labelContactResult2.Text = "Contact: " + rep.Email;
                        labelPhoneResult2.Text = rep.Phone;
                    }
                }
            }

            if (found == 0)
            {
                foreach(Common.SalesRep rep in XMLFunctions.SalesRepList)
                {
                    if (rep.CC != null && rep.CC.Contains((comboBoxState.SelectedItem as Common.Region).Area))
                    {
                        labelRepResult.Text = "Sales Rep: " + rep.DisplayName + " (RSM)";
                        labelContactResult.Text = "Contact: " + rep.Email;
                        labelPhoneResult.Text = rep.Phone;
                        break;
                    }
                }
            }

            if (found < 2)
            {
                labelRepResult2.Text = "";
                labelContactResult2.Text = "";
                labelPhoneResult2.Text = "";
            }

            //Get the SIM admin
            labelSIMadmin.Text = "SIM admin: ";
            foreach (Common.SalesRep rep in XMLFunctions.SIMadmins)
            {
                if (rep.SIMS != null && (rep.SIMS.Contains((comboBoxState.SelectedItem as Common.Region).Area) || rep.SIMS.Contains("ALL")))
                    labelSIMadmin.Text += labelSIMadmin.Text == "SIM admin: " ? rep.DisplayName : ", " + rep.DisplayName;
            }
        }

        private void comboBoxRepresentative_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isNullOrEmpty(comboBoxRepresentative) && comboBoxRepresentative.Items.Count > 0)
            {
                Common.Log("Selecting rep: " + (comboBoxRepresentative.SelectedItem as Common.SalesRep).DisplayName);

                comboBoxState.SelectedIndex = 0;
                labelRegionResult.Text = "Region: ";

                labelSIMadmin.Text = "";
            }
            else if (isNullOrEmpty(comboBoxRepresentative) || isNullOrEmpty(comboBoxState))
            {
                labelRepResult.Text = "Sales Rep: ";
                labelContactResult.Text = "Contact: ";
                labelPhoneResult.Text = "";
                labelRepResult2.Text = "";
                labelContactResult2.Text = "";
                labelPhoneResult2.Text = "";
                labelSIMadmin.Text = "";
                labelRegionResult.Text = "Region: ";
                showPicture("");

                Common.SelectedItem = null;

                return;
            }

            Common.Stat();

            Common.SelectedItem = comboBoxRepresentative.SelectedItem as Common.SalesRep;

            string pictureLocation = (comboBoxRepresentative.SelectedItem as Common.SalesRep).Picture;
            new Thread(() => showPicture("SalesReps/" + pictureLocation)).Start();

            labelRepResult.Text = "Sales Rep: " + (comboBoxRepresentative.SelectedItem as Common.SalesRep).DisplayName;
            labelContactResult.Text = "Contact: " + (comboBoxRepresentative.SelectedItem as Common.SalesRep).Email;
            labelPhoneResult.Text = (comboBoxRepresentative.SelectedItem as Common.SalesRep).Phone;
            labelRegionResult.Text = "Region: ";

            if ((comboBoxRepresentative.SelectedItem as Common.SalesRep).Responsibilities != null)
            {
                foreach (string region in (comboBoxRepresentative.SelectedItem as Common.SalesRep).Responsibilities)
                {
                    Common.Region relatedRegion = XMLFunctions.RegionList.Where(p => p.Name == region).SingleOrDefault();

                    string abbreviation = relatedRegion != null && relatedRegion.Abbreviation != null ? relatedRegion.Abbreviation : region;

                    labelRegionResult.Text += abbreviation;

                    if (region != (comboBoxRepresentative.SelectedItem as Common.SalesRep).Responsibilities.Last())
                        labelRegionResult.Text += ", ";
                }
            }


            labelRepResult2.Text = "";
            labelContactResult2.Text = "";
            labelPhoneResult2.Text = "";
        }

        private void pictureBoxSettings_Click(object sender, EventArgs e)
        {
            Common.Stat();

            Common.Log("Opening config");
            Settings config = new Settings();
            config.ShowDialog();
        }

        private void pictureBoxMap_Click(object sender, EventArgs e)
        {
            Common.Stat();

            Common.Log("Opening PDF map");
            string path = "http://info.sigmatek.net/documents/Sales/US&CanadianTerritoriesMap_PDF.pdf";

            if (Common.NetworkFileExists(new Uri(path), 250))
                Process.Start(path);
            else
            {
                MessageBox messageBox = new MessageBox("Invalid Path", "The path " + path + " is invalid.", "OK", Common.MessageBoxResult.OK);
                messageBox.ShowDialog();
            }
        }

        private void pictureBoxOnlineMaps_Click(object sender, EventArgs e)
        {
            Common.Stat();

            Common.Log("Opening Google Maps");

            if (isNullOrEmpty(comboBoxState) || (comboBoxState.SelectedItem as Common.Region).Name == "")
            {
                Process.Start("https://www.google.com/maps/@38.9165981,-96.6887,5z");
            }
            else
            {
                string state = (comboBoxState.SelectedItem as Common.Region).Name;

                switch (state)
                {
                    case "Alberta":
                    case "British Columba":
                    case "Manitoba":
                    case "New Brunswick":
                    case "Newfoundland and Labrador":
                    case "Nova Scotia":
                    case "Northwest Territories":
                    case "Nunavut":
                    case "Ontario":
                    case "Prince Edward Island":
                    case "Quebec":
                    case "Saskatchewan":
                    case "Yukon":
                        state += ",Canada";
                        break;
                }

                Process.Start("https://www.google.com/maps/place/" + state);
            }
        }

        private void sortRegions_Click(object sender, EventArgs e)
        {
            Common.Stat();

            MenuItem arrangeByDefault = new MenuItem();
            arrangeByDefault.Text = "Default Sorting";
            arrangeByDefault.Click += ArrangeRegionsByDefault;

            MenuItem arrangeByAbbrev = new MenuItem();
            arrangeByAbbrev.Text = "Sort by Abbreviation";
            arrangeByAbbrev.Click += ArrangeByAbbrev;

            MenuItem arrangeByName = new MenuItem();
            arrangeByName.Text = "Sort by Name";
            arrangeByName.Click += ArrangeByName;

            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(arrangeByDefault);
            contextMenu.MenuItems.Add(arrangeByAbbrev);
            contextMenu.MenuItems.Add(arrangeByName);
            contextMenu.Show((sender as PictureBox), new Point(20, 20));
        }

        private void ArrangeRegionsByDefault(object sender, EventArgs e)
        {
            comboBoxState.DataSource = XMLFunctions.RegionList;
        }

        private void ArrangeByAbbrev(object sender, EventArgs e)
        {
            var result = XMLFunctions.RegionList.ToList();
            result.RemoveAt(0);
            result.RemoveAt(0);
            result = result.OrderBy(p => p.Abbreviation).ToList();
            result.Insert(0, XMLFunctions.RegionList[1]);
            result.Insert(0, XMLFunctions.RegionList[0]);
            comboBoxState.DataSource = result;
        }

        private void ArrangeByName(object sender, EventArgs e)
        {
            var result = XMLFunctions.RegionList.ToList();
            result.RemoveAt(0);
            result.RemoveAt(0);
            result = result.OrderBy(p => p.Name).ToList();
            result.Insert(0, XMLFunctions.RegionList[1]);
            result.Insert(0, XMLFunctions.RegionList[0]);
            comboBoxState.DataSource = result;
        }

        private void sortReps_Click(object sender, EventArgs e)
        {
            Common.Stat();

            MenuItem arrangeByDefault = new MenuItem();
            arrangeByDefault.Text = "Default Sorting";
            arrangeByDefault.Click += ArrangeRepsByDefault;

            MenuItem arrangeByFirst = new MenuItem();
            arrangeByFirst.Text = "Sort by First Name";
            arrangeByFirst.Click += ArrangeByFirstName;

            MenuItem arrangeByLast = new MenuItem();
            arrangeByLast.Text = "Sort by Last Name";
            arrangeByLast.Click += ArrangeByLastName;

            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(arrangeByDefault);
            contextMenu.MenuItems.Add(arrangeByFirst);
            contextMenu.MenuItems.Add(arrangeByLast);
            contextMenu.Show((sender as PictureBox), new Point(20,20));
        }

        private void ArrangeRepsByDefault(object sender, EventArgs e)
        {
            comboBoxRepresentative.DataSource = XMLFunctions.SalesRepList;
        }

        private void ArrangeByFirstName(object sender, EventArgs e)
        {
            var result = XMLFunctions.SalesRepList.ToList();
            result.RemoveAt(0);
            result = result.OrderBy(p => p.Name.First).ToList();
            result.Insert(0, XMLFunctions.SalesRepList[0]);
            comboBoxRepresentative.DataSource = result;
        }

        private void ArrangeByLastName(object sender, EventArgs e)
        {
            var result = XMLFunctions.SalesRepList.ToList();
            result.RemoveAt(0);
            result = result.OrderBy(p => p.Name.Last).ToList();
            result.Insert(0, XMLFunctions.SalesRepList[0]);
            comboBoxRepresentative.DataSource = result;
        }

        private void pictureBoxEmail_Click(object sender, EventArgs e)
        {
            Common.Stat();


            if (Common.SelectedItem == null)
            {
                MessageBox messageBox = new MessageBox("No Rep/Region selected", "Please choose a Region or Sales Rep from the dropdowns", "OK", Common.MessageBoxResult.OK);
                messageBox.ShowDialog();
                return;
            }

            MenuItem offSMREmail = new MenuItem();
            offSMREmail.Text = "Off SMR Email";
            offSMREmail.Click += OffSMREmail_Click;

            MenuItem gracePeriodEmail = new MenuItem();
            gracePeriodEmail.Text = "Grace Period Email";
            gracePeriodEmail.Click += GracePeriodEmail_Click;

            MenuItem repEmail = new MenuItem();
            repEmail.Text = "Email to Rep";
            repEmail.Click += repEmail_Click;

            MenuItem SIMEmail = new MenuItem();
            SIMEmail.Text = "Email to SIM admin";
            SIMEmail.Click += SIMEmail_Click;

            ContextMenu contextMenu = new ContextMenu();

            if (Common.SelectedItem is Common.Region)
            {
                contextMenu.MenuItems.Add(offSMREmail);
                contextMenu.MenuItems.Add(gracePeriodEmail);
                contextMenu.MenuItems.Add(SIMEmail);
            }

            if (Common.SelectedItem is Common.SalesRep)
                contextMenu.MenuItems.Add(repEmail);

            contextMenu.Show((sender as PictureBox), new Point(20, 20));
        }

        private void OffSMREmail_Click(object sender, EventArgs e)
        {
            Common.Log("Composing an Off SMR email with state: " + comboBoxState.Text + " & rep: " + comboBoxRepresentative.Text);

            SignatureCheck();

            Common.SalesRep rep = getRep(comboBoxState.SelectedItem as Common.Region);
            string companyName = "";

            CompanyPrompt companyPrompt = new CompanyPrompt();
            companyPrompt.CloseWindow += () =>
            {
                companyName = companyPrompt.CompanyName;
                companyPrompt.Dispose();
            };
            companyPrompt.ShowDialog();

            string rsr = rep.Email;
            string cc = getCC(comboBoxState.SelectedItem as Common.Region);

            if (cc.Contains(rsr))
                cc = cc.Replace(rsr, "");

            cc = rsr + ";" + cc;
            string subject = (string)XMLFunctions.readSetting("OffSMRSubject", typeof(string), "SigmaNEST Subscription Membership Renewal");
            string body = (string)XMLFunctions.readSetting("OffSMRBody", typeof(string)) + (string)XMLFunctions.readSetting("OffSMRSignature", typeof(string), Properties.Settings.Default.OffSMRSignatureDefault);

            subject = replaceVariables(subject, rep.DisplayName, rep.Email, rep.Phone);
            body = replaceVariables(body, rep.DisplayName, rep.Email, rep.Phone);

            subject += " | " + companyName; //Add the company name to the end of the subject

            ThreadPool.QueueUserWorkItem(composeOutlook, new object[] { "", cc, subject, body });
        }

        private void GracePeriodEmail_Click(object sender, EventArgs e)
        {
            Common.Log("Composing a Grace Period email with state: " + comboBoxState.Text + " & rep: " + comboBoxRepresentative.Text);

            SignatureCheck();

            Common.SalesRep rep = getRep(comboBoxState.SelectedItem as Common.Region);
            string companyName = "";

            CompanyPrompt companyPrompt = new CompanyPrompt();
            companyPrompt.CloseWindow += () =>
            {
                companyName = companyPrompt.CompanyName;
                companyPrompt.Dispose();
            };
            companyPrompt.ShowDialog();

            string rsr = rep.Email;
            string cc = getCC(comboBoxState.SelectedItem as Common.Region) + getCC(new Common.Region() { Area = "Grace"});

            if (cc.Contains(rsr))
                cc.Replace(rsr, "");

            cc = rsr + ";" + cc;
            string subject = (string)XMLFunctions.readSetting("GracePeriodSubject", typeof(string), "SigmaNEST Subscription Membership Expiring Soon");
            string body = (string)XMLFunctions.readSetting("GracePeriodBody") + (string)XMLFunctions.readSetting("OffSMRSignature", typeof(string), Properties.Settings.Default.OffSMRSignatureDefault);

            subject = replaceVariables(subject, rep.DisplayName, rep.Email, rep.Phone);
            body = replaceVariables(body, rep.DisplayName, rep.Email, rep.Phone);

            subject += " | " + companyName; //Add the company name to the end of the subject

            ThreadPool.QueueUserWorkItem(composeOutlook, new object[] { "", cc, subject, body });
        }

        private void repEmail_Click(object sender, EventArgs e)
        {
            Common.Log("Composing a blank email to rep with state: " + comboBoxState.Text + " & rep: " + comboBoxRepresentative.Text);

            SignatureCheck();

            string to = (comboBoxRepresentative.SelectedItem as Common.SalesRep).Email;
            string body = "<br><br>" + (string)XMLFunctions.readSetting("OffSMRSignature", typeof(string), Properties.Settings.Default.OffSMRSignatureDefault);
            ThreadPool.QueueUserWorkItem(composeOutlook, new object[] { to, "", "", body });
        }

        private void SIMEmail_Click(object sender, EventArgs e)
        {
            Common.Log("Composing a blank email to SIM admin with state: " + comboBoxState.Text + " & rep: " + comboBoxRepresentative.Text);
            string to = "";

            SignatureCheck();

            foreach (Common.SalesRep rep in XMLFunctions.SIMadmins)
            {
                if (rep.SIMS.Contains((comboBoxState.SelectedItem as Common.Region).Area))
                    to += rep.Email + ";";
            }

            if (string.IsNullOrEmpty(to))
                return;

            string body = "<br><br>" + (string)XMLFunctions.readSetting("OffSMRSignature", typeof(string), Properties.Settings.Default.OffSMRSignatureDefault);
            ThreadPool.QueueUserWorkItem(composeOutlook, new object[] { to, "", "", body });
        }

        private Common.SalesRep getRep(Common.Region region)
        {
            Common.SalesRep rep = null;
            foreach (Common.SalesRep representative in XMLFunctions.SalesRepList)
            {
                if (representative.Responsibilities != null && representative.Responsibilities.Contains(region.Name))
                {
                    if (rep == null)
                        rep = representative;
                    else
                    {
                        Common.Log("Prompting to select a \"North-South\" Rep");
                        //Choose a rep with the "North-South" dialog
                        DialogResult res = new DialogResult();
                        North_South frm = new North_South(rep.DisplayName, representative.DisplayName);
                        res = frm.ShowDialog();

                        if (res == DialogResult.Yes) //"Yes" means "North rep"
                            continue; //aka rep = rep;
                        else if (res == DialogResult.No) //"No" means "South rep"
                            rep = representative;
                        else //User closed out the dialog box
                            return null;

                        Common.Log("User selected " + rep.DisplayName);
                    }
                }
            }

            if (rep == null) //No reps, so look for an RSM
            {
                foreach (Common.SalesRep rsm in XMLFunctions.SalesRepList)
                {
                    if (rsm.CC != null && rsm.CC.Contains(region.Area))
                        rep = rsm;
                }
            }

            if (rep == null)
            {
                MessageBox messageBox = new MessageBox("No Reps or RSMs for selected Region", "The selected region has no representatives or RSMs", "OK", Common.MessageBoxResult.OK);
                messageBox.ShowDialog();
            }

            return rep;
        }

        private string getCC(Common.Region region)
        {
            string result = "";

            foreach (Common.SalesRep rep in XMLFunctions.SalesRepList)
            {
                if (rep.CC != null && rep.CC.Contains(region.Area))
                    result += rep.Email + ";";
            }

            return result;
        }

        private void composeOutlook(object parameters)
        {
            object[] array = parameters as object[];

            string to = Convert.ToString(array[0]);
            string cc = Convert.ToString(array[1]);
            string subject = Convert.ToString(array[2]);
            string body = Convert.ToString(array[3]);


            try
            {
                Outlook.Application outlookApp = new Outlook.Application();
                Outlook.MailItem mailItem = (Outlook.MailItem)outlookApp.CreateItem(Outlook.OlItemType.olMailItem);

                mailItem.To = to;
                mailItem.CC = cc;
                mailItem.Subject = subject;
                mailItem.HTMLBody = body;
                mailItem.Display(true);
            }
            catch (Exception eX)
            {
                MessageBox messageBox = new MessageBox("Email Failed", "Failed to create the email. (Exception: " + eX.Message + ")\n\n Please try again", "OK", Common.MessageBoxResult.OK);
                messageBox.ShowDialog();
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

        private void pictureBoxSkype_Click(object sender, EventArgs e)
        {
            Common.Stat();

            if (Common.SelectedItem == null)
            {
                MessageBox messageBox = new MessageBox("No Rep/Region selected", "Please choose a Region or Sales Rep from the dropdowns", "OK", Common.MessageBoxResult.OK);
                messageBox.ShowDialog();
                return;
            }

            MenuItem beta = new MenuItem();
            beta.Text = "[BETA]";
            beta.Enabled = false;

            MenuItem messageRSR = new MenuItem();
            messageRSR.Text = "Message RSR(s)";
            messageRSR.Click += MessageRSR_Click;

            MenuItem messageRSMs = new MenuItem();
            messageRSMs.Text = "Message RSM(s)";
            messageRSMs.Click += MessageRSMs_Click;

            MenuItem messageSIMadmin = new MenuItem();
            messageSIMadmin.Text = "Message SIM Admin(s)";
            messageSIMadmin.Click += MessageSIMadmin_Click;

            MenuItem message = new MenuItem();
            message.Text = "Send Skype Message";
            message.Click += Message_Click;

            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(beta);

            if (Common.SelectedItem is Common.Region)
            {
                contextMenu.MenuItems.Add(messageRSR);
                contextMenu.MenuItems.Add(messageRSMs);
                contextMenu.MenuItems.Add(messageSIMadmin);
            }
            else if (Common.SelectedItem is Common.SalesRep)
            {
                contextMenu.MenuItems.Add(message);
            }

            contextMenu.Show((sender as PictureBox), new Point(20, 20));
        }

        private void Message_Click(object sender, EventArgs e)
        {
            Common.Log("Composing Skype message");

            Common.SalesRep rep = comboBoxRepresentative.SelectedItem as Common.SalesRep;

            if (!string.IsNullOrEmpty(rep.SkypeIdentity))
                StartSkypeMessage("<sip:" + rep.SkypeIdentity + ">");
        }

        private void MessageRSR_Click(object sender, EventArgs e)
        {
            Common.Log("Composing Skype message to RSR(s)");

            Common.SalesRep rep = getRep(comboBoxState.SelectedItem as Common.Region);

            if (!string.IsNullOrEmpty(rep.SkypeIdentity))
                StartSkypeMessage("<sip:" + rep.SkypeIdentity + ">");
        }

        private void MessageRSMs_Click(object sender, EventArgs e)
        {
            Common.Log("Composing Skype message to RSM(s)");

            string result = "";
            Common.Region region = comboBoxState.SelectedItem as Common.Region;
            foreach (Common.SalesRep rep in XMLFunctions.SalesRepList)
            {
                if (rep.CC != null && rep.CC.Contains(region.Area) && !string.IsNullOrEmpty(rep.SkypeIdentity))
                    result += "<sip:" + rep.SkypeIdentity + ">";
            }

            if (!string.IsNullOrEmpty(result))
                StartSkypeMessage(result);
        }

        private void MessageSIMadmin_Click(object sender, EventArgs e)
        {
            Common.Log("Composing Skype message to SIM admin");

            string result = "";
            Common.Region region = comboBoxState.SelectedItem as Common.Region;
            foreach (Common.SalesRep rep in XMLFunctions.SIMadmins)
            {
                if (rep.SIMS != null && (rep.SIMS.Contains((comboBoxState.SelectedItem as Common.Region).Area) || rep.SIMS.Contains("ALL")) && !string.IsNullOrEmpty(rep.SkypeIdentity))
                    result += "<sip:" + rep.SkypeIdentity + ">";
            }

            if (!string.IsNullOrEmpty(result))
                StartSkypeMessage(result);
        }

        private void StartSkypeMessage(string arguments)
        {
            Common.Log("Composing a Skype message to " + arguments);

            ProcessStartInfo Info = new ProcessStartInfo();
            Info.Arguments = "/C start im:\"" + arguments + "\"";
            Info.WindowStyle = ProcessWindowStyle.Hidden;
            Info.FileName = "cmd.exe";
            Process infoProcess = Process.Start(Info);
        }

        private void labelContactResult_Click(object sender, EventArgs e)
        {
            Common.Stat();

            string temp = labelContactResult.Text;
            string copy = Common.RemoveSpecial(temp.Substring(temp.IndexOf(": ") + 2));

            try
            {
                Clipboard.SetText(copy);
                labelContactResult.Text = "Contact: COPIED!";
                Common.Log("Clicked first Sales Rep email and set clipboard to \"" + copy + "\"");
            }
            catch(Exception ex)
            {
                Common.Log("Attempted to set the clipboard text and failed (Exception: " + ex.Message + ")");
                labelContactResult.Text = "Contact: FAILED TO COPY...TRY AGAIN";
            }

            Application.DoEvents();
            Thread.Sleep(750);
            labelContactResult.Text = temp;
        }

        private void labelPhoneResult_Click(object sender, EventArgs e)
        {
            Common.Stat();

            string temp = labelPhoneResult.Text;
            string copy = Common.RemoveSpecial(temp);

            try
            {
                Clipboard.SetText(copy);
                labelPhoneResult.Text = "COPIED!";
                Common.Log("Clicked first Sales Rep phone and set clipboard to \"" + copy + "\"");
            }
            catch(Exception ex)
            {
                Common.Log("Attempted to set the clipboard text and failed (Exception: " + ex.Message + ")");
                labelPhoneResult.Text = "FAILED TO COPY...TRY AGAIN";
            }

            Application.DoEvents();
            Thread.Sleep(750);
            labelPhoneResult.Text = temp;
        }

        private void labelContactResult2_Click(object sender, EventArgs e)
        {
            Common.Stat();

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
            Common.Stat();

            Console.WriteLine(sender.GetType() + " | " + sender.ToString());

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

        private void showPicture(string name)
        {
            string url = "http://info.sigmatek.net/downloads/SalesMap/" + name;
            this.Invoke((MethodInvoker)delegate
            {
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
            });
        }

        private bool isNullOrEmpty(ComboBox comboBox)
        {
            bool result = false;

            if (comboBox.SelectedItem == null)
                result = true;
            else if (comboBox.Text == "")
                result = true;
            else if (comboBox.SelectedItem is Common.SalesRep && string.IsNullOrEmpty((comboBox.SelectedItem as Common.SalesRep).DisplayName))
                result = true;
            else if (comboBox.SelectedItem is Common.Region && string.IsNullOrEmpty((comboBox.SelectedItem as Common.Region).DisplayName))
                result = true;

            return result;
        }

        private void SignatureCheck()
        {
            //Force the user to set up their signature
            if (Common.RemoveSpecial((string)XMLFunctions.readSetting("OffSMRSignature", typeof(string), Properties.Settings.Default.OffSMRSignatureDefault)) == Common.RemoveSpecial(Properties.Settings.Default.OffSMRSignatureDefault))
            {
                MessageBox messageBox = new MessageBox("Signature not set", "Please set up your signature in the settings!\n\n(Change \"YOUR_NAME\" and \"Application Engineer\" to be your name and title)",
                                                        "OK", Common.MessageBoxResult.OK);
                messageBox.ShowDialog();

                Common.Log("Opening config so the user can set their signature");
                Settings config = new Settings();
                config.ShowDialog();
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
                        key.SetValue("FirstRun", Common.ThisVersion);

                        if ((bool)XMLFunctions.readSetting("ShowAboutOnStartup", typeof(bool), true))
                        {
                            About about = new About();
                            about.ShowDialog();
                        }
                    }
                    catch
                    {
                        Common.Log("Could not create and set key (key does not exist).");
                        return;
                    }
                }

                if (key.GetValue("FirstRun").ToString() != Common.ThisVersion)
                {
                    Common.Log("Key does not match current version. Key: " + key.GetValue("FirstRun").ToString() + " Version: " + Common.ThisVersion);
                    
                    key.SetValue("FirstRun", Common.ThisVersion);
                    if ((bool)XMLFunctions.readSetting("ShowAboutOnStartup", typeof(bool), true))
                    {
                        About about = new About();
                        about.ShowDialog();
                    }

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

                XMLFunctions.ReadOldSettings();

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

                key.SetValue("Exe location", Application.ExecutablePath);

                key.Close();
            }
            catch (Exception ex)
            {
                Common.Log("Problems with running checkFirstRun. Error: " + ex.Message);
            }
        }

        private void SalesMapSearch_Load(object sender, EventArgs e)
        {
            Point startupPoint = (System.Drawing.Point)XMLFunctions.readSetting("MainWindowLocation", typeof(System.Drawing.Point), new Point(0,0));

            if (!startupPoint.IsEmpty)
            {
                foreach (Screen s in Screen.AllScreens)
                {
                    if (s.Bounds.Contains(startupPoint))
                    {
                        this.Top = startupPoint.Y;
                        this.Left = startupPoint.X;

                        return;
                    }
                }
            }

            Screen screen = Screen.FromPoint(new Point(Cursor.Position.X, Cursor.Position.Y));
            this.Top = screen.Bounds.Y + (screen.Bounds.Height / 10);
            this.Left = screen.Bounds.X + (screen.Bounds.Width / 10);
        }
    }
}
