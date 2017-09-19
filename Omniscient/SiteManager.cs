using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;

using Omniscient.Instruments;

namespace Omniscient
{
    class SiteManager
    {
        private List<Site> sites;

        public SiteManager()
        {
            sites = new List<Site>();
        }

        public ReturnCode LoadFromXML(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            foreach (XmlNode siteNode in doc.DocumentElement.ChildNodes)
            {
                Site newSite = new Site(siteNode.Attributes["name"]?.InnerText);
                foreach (XmlNode facilityNode in siteNode.ChildNodes)
                {
                    Facility newFacility = new Facility(facilityNode.Attributes["name"]?.InnerText);
                    foreach (XmlNode systemNode in facilityNode.ChildNodes)
                    {
                        DetectionSystem newSystem = new DetectionSystem(systemNode.Attributes["name"]?.InnerText);
                        foreach (XmlNode instrumentNode in systemNode.ChildNodes)
                        {
                            switch(instrumentNode.Attributes["type"]?.InnerText)
                            {
                                case "ISR":
                                    ISRInstrument newInstrument = new ISRInstrument(instrumentNode.Attributes["name"]?.InnerText);
                                    newInstrument.SetDataFolder(instrumentNode.Attributes["directory"]?.InnerText);
                                    newSystem.AddInstrument(newInstrument);
                                    break;
                                default:
                                    break;
                            }
                            // MessageBox.Show(instrumentNode.Attributes["name"]?.InnerText);
                        }
                        newFacility.AddSystem(newSystem);
                    }
                    newSite.AddFacility(newFacility);
                }
                sites.Add(newSite);
            }
            return ReturnCode.SUCCESS;
        }

        public List<Site> GetSites() { return sites; }
    }
}
