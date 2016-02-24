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
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();

            textBoxMapLocation.Text = Properties.Settings.Default.MapFileLocation;
        }

        private void buttonEditRegions_Click(object sender, EventArgs e)
        {
            
        }

        private void linkLabelGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/derekantrican/SalesMap/issues");
        }

        private void buttonRegions_Click(object sender, EventArgs e)
        {
            buttonSalesReps.Enabled = false;
            //textBoxEdit.Text = READ FROM REGIONS FILE
        }

        private void buttonSalesReps_Click(object sender, EventArgs e)
        {
            buttonRegions.Enabled = false;
            //textBoxEdit.Text = READ FROM SALES REP FILE
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.MapFileLocation = textBoxMapLocation.Text;
            Properties.Settings.Default.Save();

            if(buttonRegions.Enabled == true)
            {
                //WRITE TO REGIONS FILE
            }
            else if(buttonSalesReps.Enabled == true)
            {
                //WRITE TO SALES REP FILE
            }
        }
    }
}
