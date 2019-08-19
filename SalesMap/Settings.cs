using System;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using Outlook = Microsoft.Office.Interop.Outlook;
using Microsoft.Win32;
using System.Threading;
using System.Windows.Forms;

namespace SalesMap
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();

            checkBoxInternational.Checked = (bool)XMLFunctions.ReadSetting("UseInternational", typeof(bool), false);
            checkBoxAutoUpdates.Checked = (bool)XMLFunctions.ReadSetting("AutoCheckForUpdates", typeof(bool), true);
            checkBoxAboutOnStartup.Checked = (bool)XMLFunctions.ReadSetting("ShowAboutOnStartup", typeof(bool), true);
            checkBoxSendLog.Checked = (bool)XMLFunctions.ReadSetting("SendLogToDeveloper", typeof(bool), true);
            checkBoxSendStats.Checked = (bool)XMLFunctions.ReadSetting("SendStatisticsToDeveloper", typeof(bool), true);
            textBoxOffSMRBody.Text = (string)XMLFunctions.ReadSetting("OffSMRBody", typeof(string));
            textBoxOffSMRSubject.Text = (string)XMLFunctions.ReadSetting("OffSMRSubject", typeof(string), "SigmaNEST Subscription Membership Renewal");
            textBoxGracePeriodBody.Text = (string)XMLFunctions.ReadSetting("GracePeriodBody", typeof(string));
            textBoxGracePeriodSubject.Text = (string)XMLFunctions.ReadSetting("GracePeriodSubject", typeof(string), "SigmaNEST Subscription Membership Expiring Soon");
            richTextBoxSignature.Text = (string)XMLFunctions.ReadSetting("OffSMRSignature", typeof(string), Properties.Settings.Default.OffSMRSignatureDefault);
            checkBoxUseOutlookWeb.Checked = (bool)XMLFunctions.ReadSetting("UseOutlookWeb", typeof(bool), false);

            //If the user's Off SMR Signature is the same as the default, show them where to set up a new one
            if (Common.RemoveSpecial((string)XMLFunctions.ReadSetting("OffSMRSignature", typeof(bool), Properties.Settings.Default.OffSMRSignatureDefault)) == Common.RemoveSpecial(Properties.Settings.Default.OffSMRSignatureDefault))
            {
                tabControlOffSMREmail.SelectTab(2);
                richTextBoxSignature.BackColor = Color.LightCoral;
            }
            else
            {
                richTextBoxSignature.BackColor = Color.White;
            }
        }

        private void pictureBoxAbout_Click(object sender, EventArgs e)
        {
            Common.Stat();

            About about = new About();
            about.ShowDialog();
        }

        private void linkLabelFeedback_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Common.Stat();

            Feedback feedback = new Feedback();
            feedback.ShowDialog();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Common.Stat();

            Common.Log("Saving settings");

            XMLFunctions.SaveSetting("AutoCheckForUpdates", checkBoxAutoUpdates.Checked);
            XMLFunctions.SaveSetting("ShowAboutOnStartup", checkBoxAboutOnStartup.Checked);
            XMLFunctions.SaveSetting("SendLogToDeveloper", checkBoxSendLog.Checked);
            XMLFunctions.SaveSetting("SendStatisticsToDeveloper", checkBoxSendLog.Checked);
            XMLFunctions.SaveSetting("OffSMRSubject", textBoxOffSMRSubject.Text);
            XMLFunctions.SaveSetting("OffSMRBody", textBoxOffSMRBody.Text);
            XMLFunctions.SaveSetting("OffSMRSignature", richTextBoxSignature.Text);
            XMLFunctions.SaveSetting("GracePeriodBody", textBoxGracePeriodBody.Text);
            XMLFunctions.SaveSetting("GracePeriodSubject", textBoxGracePeriodSubject.Text);
            XMLFunctions.SaveSetting("UseOutlookWeb", checkBoxUseOutlookWeb.Checked);

            if (checkBoxInternational.Checked != (bool)XMLFunctions.ReadSetting("UseInternational", typeof(bool), false))
            {
                if (checkBoxInternational.Checked)
                {
                    XMLFunctions.ParseRegions(true);
                    XMLFunctions.ParseReps(true);
                }
                else
                {
                    XMLFunctions.ParseRegions(false);
                    XMLFunctions.ParseReps(false);
                }

                XMLFunctions.SaveSetting("UseInternational", checkBoxInternational.Checked);
            }


            if (ModifierKeys == Keys.Control)
            {
                MessageBox messageBox = new MessageBox("Factory Reset", "This will factory reset this program! \n\nAre you sure?", "No", Common.MessageBoxResult.No, true, "Yes", Common.MessageBoxResult.Yes);
                messageBox.ShowDialog();
                if (Common.DialogResult == Common.MessageBoxResult.Yes)
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
                        MessageBox messageBox2 = new MessageBox("Error during factory reset", "Could not factory reset. Please contact the developer", "OK", Common.MessageBoxResult.OK);
                        messageBox2.ShowDialog();
                        return;
                    }

                    ProcessStartInfo info = new ProcessStartInfo();
                    info.Arguments = "/C ping 127.0.0.1 -n 2 && \"" + Application.ExecutablePath + "\"";
                    info.WindowStyle = ProcessWindowStyle.Hidden;
                    info.CreateNoWindow = true;
                    info.FileName = "cmd.exe";
                    Process.Start(info);
                    Application.Exit();
                }
            }

            this.Close();
        }

        private void linkLabelUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Common.Stat();
            Common.CheckForUpdate(true);
        }

        private void buttonVariables_Click(object sender, EventArgs e)
        {
            Common.Stat();

            MessageBox messageBox = new MessageBox("Off SMR EMail Variables", "You can use the following variables when defining the Off SMR Email:\n\n" +
                                                    "   - \"{SALESREPNAME}\" ... the rep's name\n" +
                                                    "   - \"{SALESREPEMAIL}\" ... the rep's email\n" +
                                                    "   - \"{SALESREPPHONE}\" ... the rep's phone #", "OK", Common.MessageBoxResult.OK);
            messageBox.ShowDialog();
        }

        private void OffSMRPreview_Click(object sender, EventArgs e)
        {
            Common.Stat();

            string subject = textBoxOffSMRSubject.Text;
            string body = replaceVariables(textBoxOffSMRBody.Text + richTextBoxSignature.Text, "Mr. SalesRep", "mr.salesrep@sigmanest.com", "123-456-7890");

            ThreadPool.QueueUserWorkItem(composeOutlook, new object[] { "mr.salesrep@sigmanest.com", subject, body });
        }

        private void GracePeriodPreview_Click(object sender, EventArgs e)
        {
            Common.Stat();

            string subject = textBoxGracePeriodSubject.Text;
            string body = replaceVariables(textBoxGracePeriodBody.Text + richTextBoxSignature.Text, "Mr. SalesRep", "mr.salesrep@sigmanest.com", "123-456-7890");
            string cc = "";

            foreach (Common.SalesRep rep in XMLFunctions.SalesRepList)
            {
                if (rep.CC != null && rep.CC.Contains("Grace"))
                    cc += rep.Email + ";";
            }

            ThreadPool.QueueUserWorkItem(composeOutlook, new object[] { cc, subject, body });
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
            catch (Exception ex)
            {
                Common.Log("Failed to create test email with cc: " + cc + " & subject: " + subject + " & exception: " + ex.Message);
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
            Point startupPoint = (Point)XMLFunctions.ReadSetting("MainWindowLocation", typeof(Point), new Point(0,0));

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

        private void CheckBoxUseOutlookWeb_MouseClick(object sender, MouseEventArgs e)
        {
            if (checkBoxUseOutlookWeb.Checked)
            {
                MessageBox messageBox = new MessageBox("Set up Outlook Web signature",
                    "Make sure you have your signature set up in Outlook web as SalesMap can't add it to the message for you.\n\n" +
                    "Set it up at https://outlook.office.com/owa/?path=/options/mailsignatures/mode/popup",
                    "Ok", Common.MessageBoxResult.OK, true, "Take me there", Common.MessageBoxResult.Open);
                messageBox.ShowDialog();

                if (Common.DialogResult == Common.MessageBoxResult.Open)
                    Process.Start("https://outlook.office.com/owa/?path=/options/mailsignatures/mode/popup");
            }
        }
    }
}
