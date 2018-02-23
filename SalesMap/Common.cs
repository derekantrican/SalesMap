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
        public static object SelectedItem;

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
                    Stream stream = client.OpenRead(InfoSiteBase);
                    return true;
                }
                catch (WebException ex)
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

        public static void CheckForUpdate(bool notifyIfIsCurrent = false)
        {
            WebClient client = new WebClient();
            string url = "http://info.sigmatek.net/downloads/SalesMap/Update.txt";
            string html = "";
            try
            {
                html = client.DownloadString(url);
            }
            catch (Exception ex)
            {
                Log("Attempted to check for new version and failed to get html");

                if (!IsOnline)
                {
                    MessageBox messageBox = new MessageBox("Check for Update failed", "Failed to check for a new update. Are you connected to the internet?", "OK", MessageBoxResult.OK);
                    messageBox.ShowDialog();
                }

                Log("Exception: " + ex.Message);
                return;
            }

            List<string> updateInfo = new List<string>(html.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));

            double latestVersion = updateInfo.Where(p => p.IndexOf("Current") >= 0).FirstOrDefault() != null ? Convert.ToDouble(updateInfo.Where(p => p.IndexOf("Current") >= 0).FirstOrDefault().Split(':')[1]) : double.NaN;
            string updateURL = ""; //= updateInfo.Where(p => p.IndexOf("Location") >= 0).FirstOrDefault() != null ? updateInfo.Where(p => p.IndexOf("Location") >= 0).FirstOrDefault().Split(':')[1] : string.Empty;
            if (updateInfo.Where(p => p.IndexOf("Location") >= 0).FirstOrDefault() != null)
            {
                updateURL = updateInfo.Where(p => p.IndexOf("Location") >= 0).FirstOrDefault();
                updateURL = updateURL.Substring(updateURL.IndexOf(':') + 1);
            }

            if (latestVersion > Convert.ToDouble(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion))
            {
                Log("Prompted for new update. Current: " + ThisVersion + "  Online: v" + latestVersion);

                MessageBox messageBox = new MessageBox("New Update Available!", "A new version is available!\n\nThe current version is v" + latestVersion + " and you are running " + ThisVersion +
                                    "\n\nDo you want to update to the new version?", "No", MessageBoxResult.No, true, "Yes", MessageBoxResult.Yes);
                messageBox.ShowDialog();

                if (DialogResult == MessageBoxResult.Yes)
                {
                    try
                    {
                        Log("User selected \"Yes\" for the new update");
                        Updater updater = new Updater(updateURL);
                        updater.ShowDialog();
                    }
                    catch(Exception ex)
                    {
                        Log("Error getting the new update: " + ex.Message);
                        Log("Attempted update URL: \"" + updateURL + "\"");
                        MessageBox errorMessage = new MessageBox("Update Error", "There was a problem getting the new update. Please go to https://github.com/derekantrican/SalesMap/releases and download it manually",
                                                    "Go to GitHub", MessageBoxResult.OK, true, "Continue with this version", MessageBoxResult.Cancel);
                        errorMessage.ShowDialog();

                        if (DialogResult == MessageBoxResult.OK)
                        {
                            Process.Start("https://github.com/derekantrican/SalesMap/releases");
                            Environment.Exit(1);
                        }
                    }
                }
                else
                {
                    Log("User selected \"No\" for the new update");
                }
            }
            else if (notifyIfIsCurrent)
            {
                MessageBox messageBox2 = new MessageBox("Most Current Version", "Congrats, you have the most current version! You are running version " + ThisVersion, "OK", MessageBoxResult.OK);
                messageBox2.ShowDialog();
            }
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
            if (!(bool)XMLFunctions.readSetting("SendStatisticsToDeveloper", typeof(bool), true))
                return;

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
