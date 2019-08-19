using System;
using System.Windows.Forms;

namespace SalesMap
{
    public partial class North_South : Form
    {
        public North_South(string buttonNorthText, string buttonSouthText)
        {
            InitializeComponent();

            buttonNorthRep.Text = buttonNorthText;
            buttonSouthRep.Text = buttonSouthText;

            buttonNorthRep.DialogResult = DialogResult.Yes;
            buttonSouthRep.DialogResult = DialogResult.No;
        }

        private void buttonNorthRep_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSouthRep_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
