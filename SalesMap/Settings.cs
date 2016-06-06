using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Microsoft.Win32;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SalesMap
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();

            textBoxEditSubject.Text = Properties.Settings.Default.OffSMRSubject;
            textBoxMapLocation.Text = Properties.Settings.Default.MapFileLocation;
            checkBoxAutoUpdates.Checked = Properties.Settings.Default.AutoCheckUpdate;
            checkBoxSendLog.Checked = Properties.Settings.Default.SendLog;
            textBoxEdit.Text = Properties.Settings.Default.OffSMRBody;
            richTextBoxSignature.Text = Properties.Settings.Default.OffSMRSignature;

            //If the user's Off SMR Signature is the same as the default, show them where to set up a new one
            if (removeSpecial(Properties.Settings.Default.OffSMRSignature) == removeSpecial(Properties.Settings.Default.OffSMRSignatureDefault))
            {
                tabControlOffSMREmail.SelectTab(1);
                richTextBoxSignature.BackColor = Color.LightCoral;
            }
            else
            {
                richTextBoxSignature.BackColor = Color.White;
            }
        }

        private void linkLabelGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/derekantrican/SalesMap/wiki");
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Log("Saving settings");

            Properties.Settings.Default.OffSMRSubject = textBoxEditSubject.Text;
            Properties.Settings.Default.MapFileLocation = textBoxMapLocation.Text;
            Properties.Settings.Default.AutoCheckUpdate = checkBoxAutoUpdates.Checked;
            Properties.Settings.Default.SendLog = checkBoxSendLog.Checked;
            Properties.Settings.Default.OffSMRBody = textBoxEdit.Text;
            Properties.Settings.Default.OffSMRSignature = richTextBoxSignature.Text;
            Properties.Settings.Default.Save();

            if (Form.ModifierKeys == Keys.Control)
            {
                if (MessageBox.Show("This will factory reset this program! \n\nAre you sure?", "Factory Reset",MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Log("Factory reset!");

                    try
                    {
                        RegistryKey key = Registry.CurrentUser.OpenSubKey("SalesMap", true);
                        if (key != null)
                            key.DeleteSubKey("SalesMap");//Reset the key value

                        string settingsPath = @"C:\Users\" + Environment.UserName + @"\AppData\Local\SalesMap";
                        DirectoryInfo di = new DirectoryInfo(settingsPath);
                        foreach (DirectoryInfo dir in di.GetDirectories())
                            dir.Delete(true);
                    }
                    catch (Exception ex)
                    {
                        Log("Problems encountered during a factory reset: " + ex.Message);
                        MessageBox.Show("Could not factory reset. Please contact the developer");
                        return;
                    }

                    ProcessStartInfo Info = new ProcessStartInfo();
                    Info.Arguments = "/C ping 127.0.0.1 -n 2 && \"" + Application.ExecutablePath + "\"";
                    Info.WindowStyle = ProcessWindowStyle.Hidden;
                    Info.CreateNoWindow = true;
                    Info.FileName = "cmd.exe";
                    Process.Start(Info);
                    Application.Exit();
                }
            }

            this.Close();
        }

        private void linkLabelUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
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
                MessageBox.Show("Failed to check for a new update. Are you connected to the internet?");
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
                                    "\n\nDo you want to update to the new version?",
                                    "New Update Available!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    Log("User selected \"Yes\" for the new update");
                    Update(GitVersion);
                }
                else
                {
                    Log("User selected \"No\" for the new update");
                }
            }
            else
            {
                MessageBox.Show("Congrats, you have the most current version! You are running version v5.5");
            }
        }

        private void Update(string version)
        {
            Updater updater = new Updater(version);
            updater.ShowDialog();
        }

        private void buttonVariables_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You can use the following variables when defining the Off SMR Email (which will get replaced with the appropriate information" +
                            " when the email is composed):\n\n" +
                            "   - \"{SALESREPNAME}\" ... which will get replaced with the rep's name\n" +
                            "   - \"{SALESREPEMAIL}\" ... which will get replaced with the rep's email\n" +
                            "   - \"{SALESREPPHONE}\" ... which will get replaced with the rep's phone #", "Off SMR EMail Variables");
        }

        private void pictureBoxPreview_Click(object sender, EventArgs e)
        {
            string subject = textBoxEditSubject.Text;
            string body = replaceVariables(textBoxEdit.Text + richTextBoxSignature.Text, "Mr. SalesRep", "mr.salesrep@sigmanest.com", "123-456-7890");

            ThreadPool.QueueUserWorkItem(composeOutlook, new object[] { "mr.salesrep@sigmanest.com", subject, body });
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

        private string removeSpecial(string input)
        {
            input = input.Replace("\r", "").Replace("\n", "").Replace(" ", "").Replace("\t", "");

            return input;
        }

        private void Log(string itemToLog)
        {
            //Add a check for a "on/off" for the log in settings?
            DateTime date = DateTime.UtcNow;
            string logPath = @"C:\Users\" + Environment.UserName + @"\log.txt";

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
    }
}
