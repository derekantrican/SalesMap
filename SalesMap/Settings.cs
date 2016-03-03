using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    public partial class Settings : Form
    {
        public event Action SettingsUpdated;

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
            System.Diagnostics.Process.Start("https://github.com/derekantrican/SalesMap/wiki");
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

            bool restart = false;

            if (buttonRegions.Enabled == false || buttonSalesReps.Enabled == false)
            {
                if (MessageBox.Show("In order for changes to the Regions or Sales Rep files to take effect, you must restart the program. \n\nRestart now?",
                                    "Restart Required", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    restart = true;
                }
                else
                {
                    MessageBox.Show("Your changes will not be saved");
                    return;
                }
            }

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

            //SettingsUpdated?.Invoke();
            Console.WriteLine("UPDATED: " + SettingsUpdated);

            if (restart)
            {
                ProcessStartInfo Info = new ProcessStartInfo();
                Info.Arguments = "/C ping 127.0.0.1 -n 2 && \"" + Application.ExecutablePath + "\"";
                Info.WindowStyle = ProcessWindowStyle.Hidden;
                Info.CreateNoWindow = true;
                Info.FileName = "cmd.exe";
                Process.Start(Info);
                Application.Exit();
            }
            else
            {
                this.Close();
            }
        }

        private void linkLabelUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebClient client = new WebClient();
            string url = "https://github.com/derekantrican/SalesMap/releases";
            string html = "";
            try
            {
                html = client.DownloadString(url);
            }
            catch
            {
                MessageBox.Show("Connection problem....\n\nAre you connected to the internet?");
                return;
            }

            string GitVersion = html.Substring(html.IndexOf("<span class=\"css-truncate-target\">v") + 34).Split('<')[0];
            string thisVersion = Properties.Settings.Default.Version;

            if (GitVersion != thisVersion)
            {
                if (MessageBox.Show("A new version is available!\n\nThe current version is " + GitVersion + " and you are running " + thisVersion +
                                    "\n\nGo to " + url + " to download the new version?",
                                    "New Update Available!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(url);
                }
            }
            else
            {
                MessageBox.Show("Congrats! You have the most current version!\n\nVersion: " + thisVersion, "Current Version");
            }
        }
    }
}
