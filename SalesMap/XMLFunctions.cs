using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static SalesMap.Common;

namespace SalesMap
{
    public partial class XMLFunctions
    {
        public static List<Region> RegionList = new List<Region>();
        public static List<SalesRep> SalesRepList = new List<SalesRep>();
        public static List<SalesRep> SIMadmins = new List<SalesRep>();

        public enum Database
        {
            Reps,
            Regions
        }

        public static object readSetting(string settingName, Type defaultType = null, object defaultValue = null)
        {
            if (!File.Exists(Path.Combine(Common.UserSettingsPath, "Settings.xml")))
                DownloadDefaultXml();

            XDocument document = XDocument.Load(Path.Combine(Common.UserSettingsPath, "Settings.xml"));
            var setting = document.Descendants("Setting").Where(x => x.Attribute("name").Value.Equals(settingName)).SingleOrDefault();

            if (setting != null)
                return XMLConverter(setting);
            else
            {
                Log("The setting \"" + settingName + "\" does not exist");

                if (defaultType == null || defaultValue == null)
                {
                    Log("A defaultType/defaultValue was not specified");
                    return null;
                }

                setting = new XElement("Setting",
                          new XAttribute("name", settingName),
                          new XAttribute("type", defaultType.ToString()),
                          defaultValue);

                document.Element("Settings").Add(setting);
                document.Save(Path.Combine(Common.UserSettingsPath, "Settings.xml"));
                return defaultValue;

            }
        }

        public static void saveSetting(string settingName, object value)
        {
            if (!File.Exists(Path.Combine(Common.UserSettingsPath, "Settings.xml")))
                DownloadDefaultXml();
            XDocument document = XDocument.Load(Path.Combine(Common.UserSettingsPath, "Settings.xml"));
            var setting = document.Descendants("Setting").Where(x => x.Attribute("name").Value.Equals(settingName)).SingleOrDefault();

            if (setting == null)
            {
                setting = new XElement("Setting",
                          new XAttribute("name", settingName),
                          new XAttribute("type", value.GetType()),
                          value);
                document.Element("Settings").Add(setting);
            }
            else
            {
                if (setting.Attribute("type").Value != value.GetType().ToString())
                {
                    Log("Trying to save setting \"" + settingName + "\" but the saved type is " + setting.Attribute("type").Value + " and trying to save type " + value.GetType().ToString());
                    return;
                }

                setting.Value = value.ToString();
            }

            document.Element("Settings").Attribute("updated").Value = DateTime.Now.ToString("%M/%d/yyyy HH:mm");
            document.Save(Path.Combine(Common.UserSettingsPath, "Settings.xml"));
        }

        static void DownloadDefaultXml()
        {
            WebClient client = new WebClient();
            client.DownloadFile(InfoSiteBase + "Settings.xml", Path.Combine(Common.UserSettingsPath, "Settings.xml"));

            Log("Downloaded the default Settings.xml from online");
        }

        public static DateTime getLastXmlOnlineUpdated(Database database, bool useInternational = false)
        {
            XDocument document = XDocument.Parse(downloadXML(database, useInternational));
            DateTime updated;
            if (database == Database.Reps)
                updated = DateTime.Parse(document.Element("SalesReps").Attribute("updated").Value);
            else
                updated = DateTime.Parse(document.Element("Regions").Attribute("updated").Value);

            return updated;
        }

        //"Local XML" functionality is partially introduced for version 6.0+, but will not be implemented unless deemed necessary

        //public static DateTime? getLastXmlLocalUpdated(Database database)
        //{
        //    string desiredXml = database == Database.Reps ? "SalesReps.xml" : "Regions.xml";

        //    if (database == Database.Regions && !File.Exists(Path.Combine(UserSettingsPath, desiredXml)))
        //    {
        //        DownloadXMLToDisk(database);
        //        return null;
        //    }
        //    else if (database == Database.Reps && !File.Exists(Path.Combine(UserSettingsPath, desiredXml)))
        //    {
        //        DownloadXMLToDisk(database);
        //        return null;
        //    }

        //    XDocument document = XDocument.Load(Path.Combine(UserSettingsPath, desiredXml));
        //    DateTime updated;
        //    if (database == Database.Reps)
        //        updated = DateTime.Parse(document.Element("SalesReps").Attribute("updated").Value);
        //    else
        //        updated = DateTime.Parse(document.Element("Regions").Attribute("updated").Value);

        //    return updated;
        //}

        //public static void DownloadXMLToDisk(Database database)
        //{
        //    string databaseSelection = database == Database.Regions ? "Regions.xml" : "SalesReps.xml";
        //    string downloadURL = InfoSiteBase + databaseSelection;

        //    WebClient webClient = new WebClient();
        //    webClient.DownloadFileAsync(new Uri(downloadURL), Path.Combine(UserSettingsPath, databaseSelection));
        //}

        public static string DownloadZipCSV()
        {
            string zipCodes = InfoSiteBase + "Settings/Zipcodes.csv";
            string html = "";

            WebClient client = new WebClient();
            html = client.DownloadString(zipCodes);

            return html;
        }

        private static string downloadXML(Database database, bool useInternational)
        {
            string regionsURL = !useInternational ? InfoSiteBase + "Settings/Regions.xml" : InfoSiteBase + "Settings/Regions_International.xml";
            string salesRepURL = !useInternational ? InfoSiteBase + "Settings/SalesReps.xml" : InfoSiteBase + "Settings/SalesReps_International.xml";
            string html = "";

            WebClient client = new WebClient();

            if (database == Database.Regions)
                html = client.DownloadString(regionsURL);
            else if (database == Database.Reps)
                html = client.DownloadString(salesRepURL);

            return html;
        }

        public static void parseRegions(bool useInternational = false)
        {
            XDocument document = XDocument.Parse(downloadXML(Database.Regions, useInternational));
            XElement parent = document.Element("Regions");

            if (RegionList != new List<Region>())
                RegionList = new List<Region>();

            Region blankRegion = new Region();
            RegionList.Add(blankRegion);

            foreach (XElement element in parent.Elements("Region"))
            {
                Region region = new Region();

                try
                {
                    region.Name = element.Element("Name").Value;
                    region.Abbreviation = element.Element("Abbreviation").Value;
                    region.Area = element.Element("Area").Value;
                    region.Picture = element.Element("Picture").Value;
                }
                catch
                {
                    Common.Log("Could not read region " + region.Name + " from xml");
                }

                RegionList.Add(region);
            }

            UpdateComboBoxes.Invoke();
        }

        public static void parseReps(bool useInternational = false)
        {
            XDocument document = XDocument.Parse(downloadXML(Database.Reps, useInternational));
            XElement parent = document.Element("SalesReps");

            if (SalesRepList != new List<SalesRep>())
                SalesRepList = new List<SalesRep>();

            SalesRep blankRep = new SalesRep();
            blankRep.Name = new Name();
            blankRep.Responsibilities = new List<string>();
            blankRep.CC = new List<string>();
            SalesRepList.Add(blankRep);

            foreach (XElement element in parent.Elements("Rep"))
            {
                SalesRep rep = new SalesRep();

                try
                {
                    Name name = new Name();
                    name.First = element.Element("Name").Element("First").Value;
                    name.Last = element.Element("Name").Element("Last").Value;
                    rep.Name = name;
                    rep.Email = element.Element("Email").Value;
                    try { rep.Phone = element.Element("Phone").Value; } catch { rep.Phone = ""; }
                    try { rep.SkypeIdentity = element.Element("SkypeIdentity").Value; } catch { rep.SkypeIdentity = ""; }
                
                    List<string> responsibilities = new List<string>();
                    foreach (XElement responsibility in element.Element("Responsibilities").Elements("Region"))
                    {
                        responsibilities.Add(responsibility.Value);
                    }

                    rep.Responsibilities = responsibilities.Count > 0 ? responsibilities : null;

                    List<string> CC = new List<string>();
                    foreach (XElement cc in element.Element("CC").Elements("Area"))
                    {
                        CC.Add(cc.Value);
                    }

                    rep.CC = CC.Count > 0 ? CC : null;

                    List<string> SIMS = new List<string>();
                    try
                    {
                        foreach (XElement sims in element.Element("SIMS").Elements("Area"))
                            SIMS.Add(sims.Value);

                        rep.SIMS = SIMS;
                    }
                    catch
                    {
                        rep.SIMS = null;
                    }                    

                    rep.Picture = element.Element("Picture").Value;

                    if (rep.SIMS != null && rep.SIMS.Count > 0)
                    {
                        SIMadmins.Add(rep);
                        continue;
                    }

                    SalesRepList.Add(rep);
                }
                catch
                {
                    Common.Log("Could not read rep " + rep.Name.First + " " + rep.Name.Last + " from xml");
                }
            }

            //Make list sorted by first name by default
            var result = XMLFunctions.SalesRepList.ToList();
            result.RemoveAt(0);
            result = result.OrderBy(p => p.Name.First).ToList();
            result.Insert(0, XMLFunctions.SalesRepList[0]);
            SalesRepList = result;

            UpdateComboBoxes.Invoke();
        }

        public static object XMLConverter(XElement XMLElement)
        {
            string type = XMLElement.Attribute("type").Value;
            if (type == "System.Boolean")
                return Convert.ToBoolean(XMLElement.Value);
            else if (type == "System.Drawing.Point")
            {
                System.Drawing.Point p = new System.Drawing.Point();
                var pointString = Regex.Replace(XMLElement.Value, @"[\{\}a-zA-Z=]", "").Split(',');

                Console.WriteLine(pointString.Where(o => o == "").FirstOrDefault());

                if (pointString.Count() == 2 && pointString.Where(o => o == "").FirstOrDefault() == null)
                {
                    p.X = int.Parse(pointString[0]);
                    p.Y = int.Parse(pointString[1]);
                }

                return p;
            }
            else
                return XMLElement.Value;
        }

        public static void ReadOldSettings()
        {
            if (!Directory.Exists(UserSettingsPath) && (new DirectoryInfo(UserSettingsPath)).GetDirectories().Count() > 0)
                return;

            if (!File.Exists(Path.Combine(Common.UserSettingsPath, "Settings.xml")))
                DownloadDefaultXml();

            bool settingCopied = false;
            DirectoryInfo UserSettingsFolder = new DirectoryInfo(UserSettingsPath);
            DirectoryInfo oldSettingsFolder = null;
            foreach (DirectoryInfo di in UserSettingsFolder.GetDirectories())
            {
                if (oldSettingsFolder == null || di.LastAccessTime > oldSettingsFolder.LastAccessTime)
                    oldSettingsFolder = di;
            }

            if (oldSettingsFolder == null)
                return;

            while (oldSettingsFolder.GetDirectories().Count() > 0)
            {
                oldSettingsFolder = oldSettingsFolder.GetDirectories().First();
            }

            XDocument document = XDocument.Load(oldSettingsFolder.GetFiles()[0].FullName);
            XElement OffSMRSignature = document.Descendants("setting").Where(x => x.Attribute("name").Value.Equals("OffSMRSignature")).SingleOrDefault();
            if (OffSMRSignature != null)
            {
                saveSetting("OffSMRSignature", OffSMRSignature.Value);
                Log("Copied over OffSMRSignature from the old settings");
                settingCopied = true;
            }

            XElement OffSMRBody = document.Descendants("setting").Where(x => x.Attribute("name").Value.Equals("OffSMRBody")).SingleOrDefault();
            if (OffSMRBody != null)
            {
                saveSetting("OffSMRBody", OffSMRBody.Value);
                Log("Copied over OffSMRBody from the old settings");
                settingCopied = true;
            }

            XElement OffSMRSubject = document.Descendants("setting").Where(x => x.Attribute("name").Value.Equals("OffSMRSubject")).SingleOrDefault();
            if (OffSMRSubject != null)
            {
                saveSetting("OffSMRSubject", OffSMRSubject.Value);
                Log("Copied over OffSMRSubject from the old settings");
                settingCopied = true;
            }

            XElement MapFileLocation = document.Descendants("setting").Where(x => x.Attribute("name").Value.Equals("MapFileLocation")).SingleOrDefault();
            if (MapFileLocation != null)
            {
                saveSetting("MapFileLocation", MapFileLocation.Value);
                Log("Copied over MapFileLocation from the old settings");
                settingCopied = true;
            }

            if (settingCopied)
            {
                Log("Deleting all the old settings folders");
                foreach (DirectoryInfo di in UserSettingsFolder.GetDirectories())
                    di.Delete(true);
            }
        }

        public delegate void UpdateComboBoxesDelegate();
        public static UpdateComboBoxesDelegate UpdateComboBoxes;
    }
}
