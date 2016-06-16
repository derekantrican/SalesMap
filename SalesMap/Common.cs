using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesMap
{
    public partial class Common
    {
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
                MessageBox.Show("Failed to check for a new update. Are you connected to the internet?");
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

            return versions.First();
        }

        public static string Diff(string s1, string s2)
        {
            if (s1 == s2)
                return null;

            if (s1.Length > s2.Length)
            {
                for (var i = 0; i < s1.Length; i++)
                {
                    if (s1[i] != s2[i])
                    {
                        if (i >= 10 && i + 10 < s1.Length)
                            return s1.Substring(i - 10, 21) + " ||| " + s2.Substring(i - 10, 21);
                        else if (i >= 10)
                            return s1.Substring(i - 10, 11) + " ||| " + s2.Substring(i - 10, 11);
                        else
                            return s1.Substring(0, 10) + " ||| " + s2.Substring(0, 11);
                    }

                }
            }
            else
            {
                for (var i = 0; i < s2.Length; i++)
                {
                    if (s1[i] != s2[i])
                    {
                        if (i >= 10 && i + 10 < s2.Length)
                            return s1.Substring(i - 10, 21) + " ||| " + s2.Substring(i - 10, 21);
                        else if (i >= 10)
                            return s1.Substring(i - 10, 11) + " ||| " + s2.Substring(i - 10, 11);
                        else
                            return s1.Substring(0, 10) + " ||| " + s2.Substring(0, 10);
                    }

                }
            }
            return null;
        }

        public static string RemoveSpecial(string input)
        {
            input = input.Replace("\r", "").Replace("\n", "").Replace(" ", "").Replace("\t", "");

            return input;
        }

        public static void Log(string itemToLog)
        {
            string logPath = @"C:\Users\" + Environment.UserName + @"\log.txt";
            DateTime date = DateTime.Now;
            TimeZone zone = TimeZone.CurrentTimeZone;

            File.AppendAllText(logPath, "[" + date + " " + abbreviate(zone.StandardName) + "] " + itemToLog + Environment.NewLine);
        }

        public static void Log(string itemToLog, bool addTimeStamp)
        {
            string logPath = @"C:\Users\" + Environment.UserName + @"\log.txt";

            if (addTimeStamp)
            {
                DateTime date = DateTime.Now;
                TimeZone zone = TimeZone.CurrentTimeZone;
                File.AppendAllText(logPath, "[" + date + " " + abbreviate(zone.StandardName) + "] " + itemToLog + Environment.NewLine);
            }
            else
                File.AppendAllText(logPath, itemToLog + Environment.NewLine);
        }

        private static string abbreviate(string longForm)
        {
            return new string(longForm.Where((c, i) => c != ' ' && (i == 0 || longForm[i - 1] == ' ')).ToArray());
        }
    }
}
