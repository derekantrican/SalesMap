using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace SalesMap
{
    public partial class Feedback : Form
    {
        bool feature = false;
        bool bug = false;
        RichTextBox textBox = new RichTextBox();
        Button buttonOk = new Button();

        public Feedback()
        {
            InitializeComponent();
            textBox.Visible = false;
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            Process.Start("mailto:derek.antrican@sigmanest.com&Subject=SalesMap%20Feedback");
            this.Close();
        }

        private void buttonFeature_Click(object sender, EventArgs e)
        {
            bug = false;
            feature = true;
            buttonOk.Text = "Submit Feature";

            showSubmissionForm();
        }

        private void buttonBug_Click(object sender, EventArgs e)
        {
            feature = false;
            bug = true;
            buttonOk.Text = "Submit Bug";

            showSubmissionForm();
        }

        private void showSubmissionForm()
        {
            if (textBox.Visible == false)
            {
                this.Height += 100;
                textBox.ScrollBars = RichTextBoxScrollBars.Vertical;
                textBox.Size = new Size(157, 75);
                textBox.Location = new Point(12, 98);
                this.Controls.Add(textBox);
                buttonOk.Size = new Size(90, 23);
                buttonOk.Location = new Point(87, 178);
                this.Controls.Add(buttonOk);
                buttonOk.Click += ButtonOk_Click;
                textBox.Visible = true;
            }
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            string subject = textBox.Text;

            if (bug)
            {
                sendEmail(subject + " #Bug");

                MessageBox messageBug = new MessageBox("Added to Trello!", "Your bug report has been added to the SalesMap Trello board!", "Go to Trello", Common.MessageBoxResult.Yes, true, "OK", Common.MessageBoxResult.OK);
                messageBug.ShowDialog();

                if (Common.DialogResult == Common.MessageBoxResult.Yes)
                    Process.Start("https://trello.com/b/mvRhnwaF/salesmap");

                this.Close();
            }
            else if (feature)
            {
                sendEmail(subject + " #Feature");

                MessageBox messageFeature = new MessageBox("Added to Trello!", "Your feature request has been added to the SalesMap Trello board!", "Go to Trello", Common.MessageBoxResult.Yes, true, "OK", Common.MessageBoxResult.OK);
                messageFeature.ShowDialog();

                if (Common.DialogResult == Common.MessageBoxResult.Yes)
                    Process.Start("https://trello.com/b/mvRhnwaF/salesmap");

                this.Close();
            }
        }

        private void sendEmail(string subject)
        {
            try
            {
                Outlook.Application outlookApp = new Outlook.Application();
                Outlook.MailItem mailItem = (Outlook.MailItem)outlookApp.CreateItem(Outlook.OlItemType.olMailItem);
                mailItem.To = "derekantrican+jtqvwnmoer0jrzmbktqy@boards.trello.com";
                mailItem.BCC = "derek.antrican@sigmanest.com";
                mailItem.Body = "Submitted by " + Environment.UserName;
                mailItem.Subject = subject;
                mailItem.Send();
            }
            catch(Exception ex)
            {
                Common.Log("Exception when submitting a bug/feature: " + ex.Message);
            }
        }
    }
}
