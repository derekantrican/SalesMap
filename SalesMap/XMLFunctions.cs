using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static SalesMap.Common;

namespace SalesMap
{
    public partial class XMLFunctions
    {
        public static List<Region> RegionList = new List<Region>();
        public static List<SalesRep> SalesRepList = new List<SalesRep>();

        private enum Database
        {
            Reps,
            Regions
        }

        [STAThread]
        private static string downloadXML(Database database)
        {
            string regionsURL = "http://info.sigmatek.net/downloads/SalesMap/Settings/Regions.xml";
            string salesRepURL = "http://info.sigmatek.net/downloads/SalesMap/Settings/SalesReps.xml";
            string html = "";

            WebClient client = new WebClient();

            if (database == Database.Regions)
                html = client.DownloadString(regionsURL);
            else if (database == Database.Reps)
                html = client.DownloadString(salesRepURL);

            return html;
        }

        [STAThread]
        public static void parseRegions()
        {
            XDocument document = XDocument.Parse(downloadXML(Database.Regions));
            XElement parent = document.Element("Regions");

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
        }

        [STAThread]
        public static void parseReps()
        {
            XDocument document = XDocument.Parse(downloadXML(Database.Reps));
            XElement parent = document.Element("SalesReps");

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
                    rep.Phone = element.Element("Phone").Value;
                
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

                    rep.Picture = element.Element("Picture").Value;
                }
                catch
                {
                    Common.Log("Could not read rep " + rep.Name.First + " " + rep.Name.Last + " from xml");
                }

                SalesRepList.Add(rep);
            }
        }
    }
}
