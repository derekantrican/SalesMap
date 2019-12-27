using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SalesMap
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();

            this.labelVersion.Text = "Version: " + Assembly.GetExecutingAssembly().GetName().Version;

            WebClient client = new WebClient();
            string url = "https://github.com/derekantrican/SalesMap/releases/tag/" + Common.ThisVersion;
            string html = "";
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                html = client.DownloadString(url);
                html = html.Substring(html.IndexOf("<div class=\"markdown-body\">") + ("< div class=\"markdown-body\">").Length);
                html = html.Substring(0, html.IndexOf("</div>"));
                richTextBoxChangelog.Text = Regex.Replace(html, "<.*?>", string.Empty);
            }
            catch
            {
                Common.Log("Attempted to get the changelog for version " + Common.ThisVersion + " and failed...");
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabelFeedback_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Feedback feedback = new Feedback();
            feedback.ShowDialog();
        }

        private void linkLabelUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Common.CheckForUpdate(true);
        }

        private void richTextBoxChangelog_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void linkLabelEmail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("mailto:Trent.Patterson@sigmanest.com&Subject=SalesMap%20Feedback");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Common.Log("Composing a Skype message to developer");

            ProcessStartInfo Info = new ProcessStartInfo();
            Info.Arguments = "/C start im:\"<sip:Trent.Patterson@sigmatek.net>\"";
            Info.WindowStyle = ProcessWindowStyle.Hidden;
            Info.FileName = "cmd.exe";
            Process infoProcess = Process.Start(Info);
        }

        private void linkLabelTrelloBoard_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://trello.com/b/mvRhnwaF/salesmap");
        }
    }
}
