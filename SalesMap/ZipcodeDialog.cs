using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SalesMap
{
    public partial class ZipcodeDialog : Form
    {
        string zipCodes;
        public ZipcodeDialog()
        {
            Common.Log("Opening Zipcode selector");
            InitializeComponent();
            zipCodes = XMLFunctions.DownloadZipCSV();
        }

        public delegate void SalesRepSelectDelegate(string region, string rep);
        public event SalesRepSelectDelegate SalesRepSelect;

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonSearch_Click(null, null);
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            Common.Log("Searching with zipcode \"" + textBox1.Text + "\"");

            string[] result = GetRepNameForZip(textBox1.Text.Substring(0, 3));
            if (/*string.IsNullOrEmpty(result[0]) ||*/ string.IsNullOrEmpty(result[1]))
            {
                Common.Log("No results for zipcode \"" + textBox1.Text + "\"");
                labelError.Visible = true;
                return;
            }

            Common.Log("Found results: " + result[0] + " " + result[1]);

            SalesRepSelect?.Invoke(result[0], result[1]);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private string[] GetRepNameForZip(string zip3Digit)
        {
            List<string> zipList = zipCodes.Split('\n').ToList();
            string zipLine = zipList.Find(p => p.Split(',')[0] == zip3Digit);

            if (string.IsNullOrEmpty(zipLine))
                return null;

            string region = zipLine.Split(',')[1];
            string rep = zipLine.Split(',')[2];

            return new string[] { region, rep };
        }
    }
}
