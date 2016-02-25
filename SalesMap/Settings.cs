using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
            buttonRegions.Enabled = false;
            buttonSalesReps.Enabled = true;

            var resourceRegions = "SalesMap.Regions.txt";
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream fileStream = assembly.GetManifestResourceStream(resourceRegions))
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string Regions = "";

                while (!reader.EndOfStream)
                {
                    Regions += reader.ReadLine();
                    Regions += Environment.NewLine;
                }
                Console.WriteLine(Regions);
                textBoxEdit.Text = Regions;
            }
        }

        private void buttonSalesReps_Click(object sender, EventArgs e)
        {
            buttonSalesReps.Enabled = false;
            buttonRegions.Enabled = true;

            var resourceRegions = "SalesMap.SalesReps.txt";
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream fileStream = assembly.GetManifestResourceStream(resourceRegions))
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string SalesReps = "";

                while (!reader.EndOfStream)
                {
                    SalesReps += reader.ReadLine();
                    SalesReps += Environment.NewLine;
                }
                Console.WriteLine(SalesReps);
                textBoxEdit.Text = SalesReps;
            }
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
