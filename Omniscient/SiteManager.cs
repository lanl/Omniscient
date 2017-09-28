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
    public class SiteManager
    {
        private List<Site> sites;
        private string xmlFile;

        public SiteManager(string newXMLFile)
        {
            sites = new List<Site>();
            xmlFile = newXMLFile;
        }

        public ReturnCode Reload()
        {
            return LoadFromXML(xmlFile);
        }

        public ReturnCode Save()
        {
            return WriteToXML(xmlFile);
        }

        public ReturnCode LoadFromXML(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            sites.Clear();
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
                            Instrument newInstrument;
                            switch (instrumentNode.Attributes["type"]?.InnerText)
                            {
                                case "ISR":
                                    newInstrument = new ISRInstrument(instrumentNode.Attributes["name"]?.InnerText);
                                    break;
                                case "GRAND":
                                    newInstrument = new GRANDInstrument(instrumentNode.Attributes["name"]?.InnerText);
                                    break;
                                case "MCA":
                                    newInstrument = new MCAInstrument(instrumentNode.Attributes["name"]?.InnerText);
                                    break;
                                default:
                                    newInstrument = null;
                                    break;
                            }
                            if (!newInstrument.Equals(null))
                            {
                                if(instrumentNode.Attributes["file_prefix"] != null)
                                {
                                    newInstrument.SetFilePrefix(instrumentNode.Attributes["file_prefix"].InnerText);
                                }
                                newInstrument.SetDataFolder(instrumentNode.Attributes["directory"]?.InnerText);
                                newSystem.AddInstrument(newInstrument);
                            }
                        }
                        newFacility.AddSystem(newSystem);
                    }
                    newSite.AddFacility(newFacility);
                }
                sites.Add(newSite);
            }
            return ReturnCode.SUCCESS;
        }

        public ReturnCode WriteToXML(string fileName)
        {

            XmlWriter xmlWriter = XmlWriter.Create(fileName, new XmlWriterSettings()
            {
                Indent = true,
            });
            
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("SiteManager");
            foreach (Site site in sites)
            {
                xmlWriter.WriteStartElement("Site");
                xmlWriter.WriteAttributeString("name", site.GetName());
                foreach (Facility fac in site.GetFacilities())
                {
                    xmlWriter.WriteStartElement("Facility");
                    xmlWriter.WriteAttributeString("name", fac.GetName());
                    foreach (DetectionSystem sys in fac.GetSystems())
                    {
                        xmlWriter.WriteStartElement("System");
                        xmlWriter.WriteAttributeString("name", sys.GetName());
                        foreach (Instrument inst in sys.GetInstruments())
                        {
                            xmlWriter.WriteStartElement("Instrument");
                            xmlWriter.WriteAttributeString("name", inst.GetName());
                            xmlWriter.WriteAttributeString("file_prefix", inst.GetFilePrefix());
                            xmlWriter.WriteAttributeString("type", inst.GetInstrumentType());
                            xmlWriter.WriteAttributeString("directory", inst.GetDataFolder());
                            xmlWriter.WriteEndElement();
                        }
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return ReturnCode.SUCCESS;
        }

        public List<Site> GetSites() { return sites; }
    }
}
