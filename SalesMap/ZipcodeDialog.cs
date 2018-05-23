using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesMap
{
    public partial class ZipcodeDialog : Form
    {
        string zipCodes;
        public ZipcodeDialog()
        {
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
            string[] result = GetRepNameForZip(textBox1.Text.Substring(0, 3));
            if (/*string.IsNullOrEmpty(result[0]) ||*/ string.IsNullOrEmpty(result[1]))
            {
                labelError.Visible = true;
                return;
            }

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
