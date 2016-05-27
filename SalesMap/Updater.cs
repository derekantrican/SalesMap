using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesMap
{
    public partial class Updater : Form
    {
        public Updater(string versionToDownload)
        {
            InitializeComponent();

            string downloadURL = "https://github.com/derekantrican/SalesMap/releases/download/" + versionToDownload + "/SalesMap.exe";
            string progName = Application.ExecutablePath.Substring(Application.ExecutablePath.LastIndexOf("\\") + 1);
            string progLoc = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\") + 1);
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SalesMap", true);
            key.SetValue("Updating", true);


            if (File.Exists(progLoc + "SalesMap-old.exe"))
                File.Delete(progLoc + "SalesMap-old.exe");

            try
            {
                File.Move(progLoc + progName, progLoc + "SalesMap-old.exe");
            }
            catch (Exception ex)
            {
                Log("[UPDATER] Problem renaming the old executable: " + ex.Message, false);
            }

            Log("[UPDATER] Downloading new version... (" + versionToDownload + ")", false);

            WebClient webClient = new WebClient();
            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
            webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
            webClient.DownloadFileAsync(new Uri(downloadURL), progLoc + progName);
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            string progName = Application.ExecutablePath.Substring(Application.ExecutablePath.LastIndexOf("\\") + 1);
            string progLoc = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\") + 1);

            Log("[UPDATER] Download has completed....restarting", false);

            ProcessStartInfo Info = new ProcessStartInfo();
            Info.Arguments = "/C ping 127.0.0.1 -n 2 && \"" + progLoc + progName + "\"";
            Info.WindowStyle = ProcessWindowStyle.Hidden;
            Info.CreateNoWindow = true;
            Info.FileName = "cmd.exe";
            Process.Start(Info);
            Application.Exit();
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
    }
}
