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
        bool hasParent;

        public Settings(bool openingFromSalesMap = true)
        {
            InitializeComponent();

            hasParent = openingFromSalesMap;

            textBoxEditSubject.Text = (string)XMLFunctions.readSetting("OffSMRSubject");
            textBoxMapLocation.Text = (string)XMLFunctions.readSetting("MapFileLocation");
            checkBoxInternational.Checked = Properties.Settings.Default.UseInternational;
            checkBoxAutoUpdates.Checked = (bool)XMLFunctions.readSetting("AutoCheckForUpdates");
            checkBoxSendLog.Checked = (bool)XMLFunctions.readSetting("SendLogToDeveloper");
            textBoxEdit.Text = (string)XMLFunctions.readSetting("OffSMRBody");
            richTextBoxSignature.Text = (string)XMLFunctions.readSetting("OffSMRSignature");

            //If the user's Off SMR Signature is the same as the default, show them where to set up a new one
            if (Common.RemoveSpecial((string)XMLFunctions.readSetting("OffSMRSignature")) == Common.RemoveSpecial(Properties.Settings.Default.OffSMRSignatureDefault))
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
            Common.Log("Saving settings");

            XMLFunctions.saveSetting("OffSMRSubject", textBoxEditSubject.Text);
            XMLFunctions.saveSetting("MapFileLocation", textBoxMapLocation.Text);
            XMLFunctions.saveSetting("AutoCheckForUpdates", checkBoxAutoUpdates.Checked);
            XMLFunctions.saveSetting("SendLogToDeveloper", checkBoxSendLog.Checked);
            XMLFunctions.saveSetting("OffSMRBody", textBoxEdit.Text);
            XMLFunctions.saveSetting("OffSMRSignature", richTextBoxSignature.Text);

            if (Properties.Settings.Default.UseInternational != checkBoxInternational.Checked)
            {
                MessageBox.Show("The program will now restart...", "Restart Required", MessageBoxButtons.OK);
                Properties.Settings.Default.UseInternational = checkBoxInternational.Checked;
                Properties.Settings.Default.Save();

                ProcessStartInfo Info = new ProcessStartInfo();
                Info.Arguments = "/C ping 127.0.0.1 -n 2 && \"" + Application.ExecutablePath + "\"";
                Info.WindowStyle = ProcessWindowStyle.Hidden;
                Info.CreateNoWindow = true;
                Info.FileName = "cmd.exe";
                Process.Start(Info);
                Application.Exit();
            }


            if (Form.ModifierKeys == Keys.Control)
            {
                if (MessageBox.Show("This will factory reset this program! \n\nAre you sure?", "Factory Reset",MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Common.Log("Factory reset!");

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
                        Common.Log("Problems encountered during a factory reset: " + ex.Message);
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
            string GitVersion = Common.checkGitHub();
            string thisVersion = "v" + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

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

        private void Settings_Load(object sender, EventArgs e)
        {
            if (!hasParent)
            {
                Point startupPoint = (System.Drawing.Point)XMLFunctions.readSetting("MainWindowLocation");

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
}
