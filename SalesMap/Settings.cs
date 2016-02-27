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

            string regionPath = @"C:\Users\" + Environment.UserName + @"\Regions.txt";
            Stream fileStream;

            if (File.Exists(regionPath))
            {
                Console.WriteLine("Regions file exists");
                fileStream = File.Open(regionPath, FileMode.Open);
            }
            else
            {
                Console.WriteLine("Regions file does not exist");
                var resourceRegions = "SalesMap.Regions.txt";
                var assembly = Assembly.GetExecutingAssembly();

                fileStream = assembly.GetManifestResourceStream(resourceRegions);
            }

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

            string salesPath = @"C:\Users\" + Environment.UserName + @"\SalesReps.txt";
            Stream fileStream;

            if (File.Exists(salesPath))
            {
                Console.WriteLine("Sales file exists");
                fileStream = File.Open(salesPath, FileMode.Open);
            }
            else
            {
                Console.WriteLine("Sales file does not exist");
                var resourceSales = "SalesMap.SalesReps.txt";
                var assembly = Assembly.GetExecutingAssembly();

                fileStream = assembly.GetManifestResourceStream(resourceSales);
            }

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

            if (buttonRegions.Enabled == false) //We are editing Regions.txt
            {
                //WRITE TO REGIONS FILE
                string regionPath = @"C:\Users\" + Environment.UserName + @"\Regions.txt";

                if (File.Exists(regionPath))
                {
                    Console.WriteLine("Regions file exists");
                }
                else
                {
                    Console.WriteLine("Regions file does not exist");
                    using (var stream = File.Create(regionPath))
                    {
                        //Doing this "using bracket" so that IDisposable is implemented afterwards
                    }
                }

                File.WriteAllText(regionPath, textBoxEdit.Text);
            }
            else if(buttonSalesReps.Enabled == false) //We are editing SalesReps.txt
            {
                //WRITE TO SALES REP FILE
                string salesPath = @"C:\Users\" + Environment.UserName + @"\SalesReps.txt";

                if (File.Exists(salesPath))
                {
                    Console.WriteLine("Sales file exists");
                }
                else
                {
                    Console.WriteLine("Sales file does not exist");
                    using (var stream = File.Create(salesPath))
                    {
                        //Doing this "using bracket" so that IDisposable is implemented afterwards
                    }
                }

                File.WriteAllText(salesPath, textBoxEdit.Text);
            }

            this.Close();
        }
    }
}
