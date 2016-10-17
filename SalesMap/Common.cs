using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesMap
{
    public partial class Common
    {
        public static string UserSettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SalesMap");
        public static string InfoSiteBase = "http://info.sigmatek.net/downloads/SalesMap/";
        public static string ThisVersion = "v" + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
        public static MessageBoxResult DialogResult = MessageBoxResult.Cancel;
        public enum MessageBoxResult
        {
            Cancel,
            OK,
            Yes,
            No,
            Retry
        }

        public static bool IsOnline
        {
            get
            {
                try
                {
                    WebClient client = new WebClient();
                    Stream stream = client.OpenRead("http://www.github.com");
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public class Name
        {
            public string First { get; set; }
            public string Last { get; set; }
        }
        public class Region
        {
            public string Name { get; set; }
            public string Abbreviation { get; set; }
            public string Area { get; set; }
            public string Picture { get; set; }
            public string DisplayName
            {
                get
                {
                    if (Abbreviation != "" && Abbreviation != null && Abbreviation != "Corporate Accounts")
                        return "(" + Abbreviation + ") " + Name;
                    else if (Name != "" && Name != null)
                        return Name;
                    else
                        return null;

                }
            }
        }

        public class SalesRep
        {
            public Name Name { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string SkypeIdentity { get; set; }
            public List<string> Responsibilities { get; set; }
            public List<string> CC { get; set; }
            public List<string> SIMS { get; set; }
            public string Picture { get; set; }
            public string DisplayName
            {
                get
                {
                    if (Name.First != null)
                        return Name.First + " " + Name.Last;
                    else
                        return null;
                }
            }
        }

        public static string checkGitHub()
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
                Common.Log("Attempted to check for new version and failed to get html");
                MessageBox messageBox = new MessageBox("Check for Update failed", "Failed to check for a new update. Are you connected to the internet?", "OK", MessageBoxResult.OK);
                messageBox.ShowDialog();
                return null;
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

                nextUrl = html.Substring(html.IndexOf("https://github.com/derekantrican/SalesMap/tags?after="));
                nextUrl = nextUrl.Split('\"')[0];
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

            versions.Sort();
            versions.Reverse();

            return versions.First();
        }

        public static string RemoveSpecial(string input)
        {
            input = input.Replace("\r", "").Replace("\n", "").Replace(" ", "").Replace("\t", "");

            return input;
        }

        public static void Log(string itemToLog)
        {
            string logPath = Path.Combine(UserSettingsPath, "log.txt");
            DateTime date = DateTime.Now;
            TimeZone zone = TimeZone.CurrentTimeZone;

            File.AppendAllText(logPath, "[" + date + " " + abbreviate(zone.StandardName) + "] " + itemToLog + Environment.NewLine);
        }

        public static void Log(string itemToLog, bool addTimeStamp)
        {
            string logPath = Path.Combine(UserSettingsPath, "log.txt");

            if (addTimeStamp)
            {
                DateTime date = DateTime.Now;
                TimeZone zone = TimeZone.CurrentTimeZone;
                File.AppendAllText(logPath, "[" + date + " " + abbreviate(zone.StandardName) + "] " + itemToLog + Environment.NewLine);
            }
            else
                File.AppendAllText(logPath, itemToLog + Environment.NewLine);
        }

        public static void Stat(string message = "", [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            string statisticsPath = Path.Combine(UserSettingsPath, "stats.txt");
            string copyPath = @"\\sigmatek.net\Documents\Employees\Derek_Antrican\SalesMap\Statistics\" + Environment.UserName + ".txt";

            if (!File.Exists(statisticsPath))
            {
                var statisticsFile = File.Create(statisticsPath);
                statisticsFile.Close();
            }

            File.AppendAllText(statisticsPath, "<" + memberName + ">" + message + Environment.NewLine);
        }

        public static bool NetworkFileExists(Uri uri, int timeout)
        {
            var task = new Task<bool>(() =>
            {
                var fi = new FileInfo(uri.LocalPath);
                return fi.Exists;
            });
            task.Start();
            return task.Wait(timeout) && task.Result;
        }

        public static void CheckPaths()
        {
            if (!Directory.Exists(UserSettingsPath))
                Directory.CreateDirectory(UserSettingsPath);
        }

        private static string abbreviate(string longForm)
        {
            return new string(longForm.Where((c, i) => c != ' ' && (i == 0 || longForm[i - 1] == ' ')).ToArray());
        }
    }
}
