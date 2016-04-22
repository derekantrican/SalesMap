using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
            checkBoxSignature.Checked = Properties.Settings.Default.SignatureWorkaround;
            checkBoxSendLog.Checked = Properties.Settings.Default.SendLog;

            editOffSMR();
        }

        private void linkLabelGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/derekantrican/SalesMap/wiki");
        }

        private void editOffSMR()
        {
            string offSMRPath = @"C:\Users\" + Environment.UserName + @"\OffSMR.txt";

            if (File.Exists(offSMRPath))
            {
                Console.WriteLine("Off SMR file exists");
                textBoxEdit.Text = File.ReadAllText(offSMRPath);
            }
            else
            {
                Console.WriteLine("Off SMR file does not exist");
                using (StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("SalesMap.OffSMR.txt")))
                {
                    textBoxEdit.Text = reader.ReadToEnd();
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Log("Saving settings");

            Properties.Settings.Default.OffSMRSubject = textBoxEditSubject.Text;
            Properties.Settings.Default.MapFileLocation = textBoxMapLocation.Text;
            Properties.Settings.Default.AutoCheckUpdate = checkBoxAutoUpdates.Checked;
            Properties.Settings.Default.SignatureWorkaround = checkBoxSignature.Checked;
            Properties.Settings.Default.SendLog = checkBoxSendLog.Checked;
            Properties.Settings.Default.Save();

            if (Form.ModifierKeys == Keys.Control)
            {
                if (MessageBox.Show("This will factory reset this program! \n\nAre you sure?", "Factory Reset",MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Log("Factory reset!");

                    RegistryKey key = Registry.CurrentUser.OpenSubKey("SalesMap");
                    key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SalesMap");
                    key.SetValue("FirstRun", ""); //Reset the key value

                    ProcessStartInfo Info = new ProcessStartInfo();
                    Info.Arguments = "/C ping 127.0.0.1 -n 2 && \"" + Application.ExecutablePath + "\"";
                    Info.WindowStyle = ProcessWindowStyle.Hidden;
                    Info.CreateNoWindow = true;
                    Info.FileName = "cmd.exe";
                    Process.Start(Info);
                    Application.Exit();
                }
            }

                //WRITE TO OFF SMR FILE
                string OffSMRPath = @"C:\Users\" + Environment.UserName + @"\OffSMR.txt";

                if (File.Exists(OffSMRPath))
                {
                    Console.WriteLine("Off SMR file exists");
                    File.WriteAllText(OffSMRPath, textBoxEdit.Text);
                }
                else
                {
                    Console.WriteLine("Off SMR file does not exist");

                    string OffSMRText = "";
                    using (StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("SalesMap.OffSMR.txt")))
                    {
                        OffSMRText = reader.ReadToEnd();
                    }

                    //If the file is unchanged, leave it alone
                    if (OffSMRText != textBoxEdit.Text)
                    {
                        Console.WriteLine("Creating file...");
                        using (var stream = File.Create(OffSMRPath))
                        {
                            //Doing this "using bracket" so that IDisposable is implemented afterwards
                        }

                        File.WriteAllText(OffSMRPath, textBoxEdit.Text);
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

        private void buttonVariables_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You can use the following variables when defining the Off SMR Email (which will get replaced with the appropriate information" +
                            " when the email is composed):\n\n" +
                            "   - \"{SALESREPNAME}\" ... which will get replaced with the rep's name\n" +
                            "   - \"{SALESREPEMAIL}\" ... which will get replaced with the rep's email\n" +
                            "   - \"{SALESREPPHONE}\" ... which will get replaced with the rep's phone #", "Off SMR EMail Variables");
        }

        private void linkLabelSignature_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://goo.gl/BkoRR2";
            Process.Start(url);
        }

        private void Log(string itemToLog)
        {
            //Add a check for a "on/off" for the log in settings?
            DateTime date = DateTime.UtcNow;
            string logPath = @"C:\Users\" + Environment.UserName + @"\log.txt";

            File.AppendAllText(logPath, "[" + date + "] " + itemToLog + Environment.NewLine);
        }
    }
}
