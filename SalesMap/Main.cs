using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesMap
{
    public partial class SalesMapSearch : Form
    {
        List<String> RegionNames = new List<String>();
        List<String> RegionParts = new List<String>();

        List<String> SalesRepNames = new List<String>();
        List<String> SalesRepEmails = new List<String>();
        List<String> SalesRepPhones = new List<String>();
        List<String> SalesRepRegions = new List<String>();

        public SalesMapSearch()
        {
            InitializeComponent();

            readFiles();
        }

        public void readFiles()
        {
            string regionPath = @"C:\Users\" + Environment.UserName + @"\Regions.txt";
            string salesPath = @"C:\Users\" + Environment.UserName + @"\SalesReps.txt";
            Stream fileStream;
            Stream fileStream1;

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
                var lines = new List<string[]>();
                int Row = 0;
                while (!reader.EndOfStream)
                {
                    string[] Line = reader.ReadLine().Split(',');
                    RegionNames.Add(Line[0]);
                    RegionParts.Add(Line[1]);
                    Row++;
                }
            }

            if (File.Exists(salesPath))
            {
                Console.WriteLine("SalesReps file exists");
                fileStream1 = File.Open(salesPath, FileMode.Open);
            }
            else
            {
                Console.WriteLine("SalesReps file does not exist");
                var resourceSales = "SalesMap.SalesReps.txt";
                var assembly1 = Assembly.GetExecutingAssembly();

                fileStream1 = assembly1.GetManifestResourceStream(resourceSales);
            }

            using (StreamReader reader1 = new StreamReader(fileStream1))
            {
                var lines = new List<string[]>();
                int Row = 0;
                while (!reader1.EndOfStream)
                {
                    string[] Line = reader1.ReadLine().Split(',');
                    SalesRepNames.Add(Line[0]);
                    SalesRepEmails.Add(Line[1]);
                    SalesRepPhones.Add(Line[2]);
                    SalesRepRegions.Add(Line[3]);
                    Row++;
                }
            }

            comboBoxState.DataSource = RegionNames;
            comboBoxState.Refresh();
            comboBoxRepresentative.DataSource = SalesRepNames;

            labelRepResult2.Text = "";
            labelContactResult2.Text = "";
            Console.WriteLine("READFILES RAN!");
        }

        private void comboBoxState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxState.SelectedItem.ToString() != "")
            {
                comboBoxRepresentative.SelectedIndex = 0;
                labelRepResult.Text = "Sales Rep: ";
                labelContactResult.Text = "Contact: ";

                labelRegionResult.Text = "Region: " + comboBoxState.SelectedItem.ToString();
            }
            else if (comboBoxState.SelectedItem == null || comboBoxRepresentative.SelectedItem == null)
            {
                return;
            }

            ResourceManager rm = new ResourceManager("SalesMap.Properties.Resources", Assembly.GetExecutingAssembly());
            pictureBox1.Image = rm.GetObject(comboBoxState.SelectedItem.ToString().Replace(')','_').Replace('(','_').Replace(' ', '_')) as Image;

            if (pictureBox1.Image == null && comboBoxState.SelectedIndex != 0)
            {
                labelNoImage.Text = "No Image Available";
            }
            else
            {
                labelNoImage.Text = "";
            }

            rm.ReleaseAllResources();

            string[] SalesRegions = SalesRepRegions.ToArray();
            string[] SalesNames = SalesRepNames.ToArray();
            string[] SalesEmails = SalesRepEmails.ToArray();
            string[] SalesPhones = SalesRepPhones.ToArray();
            string[] RegionPart = RegionParts.ToArray();

            int found = 0;

            string search = comboBoxState.SelectedItem.ToString().Split('(').Last().Split(')').First();
            Console.WriteLine("\"" + search + "\"");

            for (int i = 0; i < SalesRegions.Length; i++)
            {
                if (SalesRegions[i].IndexOf(search) >= 0)
                {
                    found++;

                    if (found == 1)
                    {
                        labelRepResult.Text = "Sales Rep: " + SalesNames[i];
                        labelContactResult.Text = "Contact: " + SalesEmails[i] + Environment.NewLine + "\t\t\t   " + SalesPhones[i];
                    }

                    if (RegionPart[i] == "0" && found > 1)
                    {
                        labelRepResult2.Text = "2nd Sales Rep: " + SalesNames[i];
                        labelContactResult2.Text = "Contact: " + SalesEmails[i] + Environment.NewLine + "\t\t\t   " + SalesPhones[i];
                    }
                }
            }

            if (found < 2)
            {
                labelRepResult2.Text = "";
                labelContactResult2.Text = "";
            }
        }

        private void comboBoxRepresentative_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxRepresentative.SelectedItem.ToString() != "")
            {
                comboBoxState.SelectedIndex = 0;
                labelRegionResult.Text = "Region: ";
            }
            else if (comboBoxRepresentative.SelectedItem == null || comboBoxState.SelectedItem == null)
            {
                return;
            }

            ResourceManager rm = new ResourceManager("SalesMap.Properties.Resources",Assembly.GetExecutingAssembly());
            pictureBox1.Image = rm.GetObject(comboBoxRepresentative.SelectedItem.ToString().Replace(' ', '_')) as Image;

            if(pictureBox1.Image == null && comboBoxRepresentative.SelectedIndex != 0)
            {
                labelNoImage.Text = "No Image Available";
            }
            else
            {
                labelNoImage.Text = "";
            }

            rm.ReleaseAllResources();



            labelRepResult.Text = "Sales Rep: " + comboBoxRepresentative.SelectedItem.ToString();
            labelContactResult.Text = "Contact: " + SalesRepEmails[comboBoxRepresentative.SelectedIndex] +
                                      Environment.NewLine + "\t\t\t   " + SalesRepPhones[comboBoxRepresentative.SelectedIndex];
            labelRegionResult.Text = "Region: " + SalesRepRegions[comboBoxRepresentative.SelectedIndex].Replace(":", ", ");

            labelRepResult2.Text = "";
            labelContactResult2.Text = "";
        }

        private void SalesMapSearch_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Settings config = new Settings();
            config.Show();

            //config.SettingsUpdated += readFiles;
        }

        private void pictureBoxMap_Click(object sender, EventArgs e)
        {
            string path = Properties.Settings.Default.MapFileLocation;

            try
            {
                System.Diagnostics.Process.Start(@path);
            }
            catch
            {
                MessageBox.Show("The path " + path + " is invalid.");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
