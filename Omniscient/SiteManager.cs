using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;

using Omniscient.Instruments;
using Omniscient.Events;

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
                if (siteNode.Name != "Site") return ReturnCode.CORRUPTED_FILE;
                Site newSite = new Site(siteNode.Attributes["name"]?.InnerText);
                foreach (XmlNode facilityNode in siteNode.ChildNodes)
                {
                    if (facilityNode.Name != "Facility") return ReturnCode.CORRUPTED_FILE;
                    Facility newFacility = new Facility(facilityNode.Attributes["name"]?.InnerText);
                    foreach (XmlNode systemNode in facilityNode.ChildNodes)
                    {
                        if (systemNode.Name != "System") return ReturnCode.CORRUPTED_FILE;
                        DetectionSystem newSystem = new DetectionSystem(systemNode.Attributes["name"]?.InnerText);
                        foreach (XmlNode instrumentNode in systemNode.ChildNodes)
                        {
                            if (instrumentNode.Name == "Instrument")
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
                                        return ReturnCode.CORRUPTED_FILE;
                                        break;
                                }
                                if (!newInstrument.Equals(null))
                                {
                                    if (instrumentNode.Attributes["file_prefix"] != null)
                                    {
                                        newInstrument.SetFilePrefix(instrumentNode.Attributes["file_prefix"].InnerText);
                                    }
                                    newInstrument.SetDataFolder(instrumentNode.Attributes["directory"]?.InnerText);
                                    newSystem.AddInstrument(newInstrument);
                                }
                            }
                            else if (instrumentNode.Name == "EventGenerator")
                            {
                                XmlNode eventNode = instrumentNode;     // Correct some shoddy nomenclature...
                                Channel channel = null;
                                EventGenerator eg;
                                foreach (Instrument inst in newSystem.GetInstruments())
                                {
                                    foreach(Channel ch in inst.GetChannels())
                                    {
                                        if (ch.GetName() == eventNode.Attributes["channel"]?.InnerText)
                                            channel = ch;
                                    }
                                }
                                if (channel is null) return ReturnCode.CORRUPTED_FILE;
                                try
                                {
                                    eg = new ThresholdEG(eventNode.Attributes["name"]?.InnerText, channel, double.Parse(instrumentNode.Attributes["threshold"]?.InnerText));
                                }
                                catch
                                {
                                    return ReturnCode.CORRUPTED_FILE;
                                }
                                newSystem.GetEventGenerators().Add(eg);
                            }
                            else
                            {
                                return ReturnCode.CORRUPTED_FILE;
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
                        foreach (EventGenerator eg in sys.GetEventGenerators())
                        {
                            xmlWriter.WriteStartElement("EventGenerator");
                            xmlWriter.WriteAttributeString("name", eg.GetName());
                            xmlWriter.WriteAttributeString("channel", ((ThresholdEG)eg).GetChannel().GetName());    // Not friendly for non-threshold event generators
                            xmlWriter.WriteAttributeString("threshold", ((ThresholdEG)eg).GetThreshold().ToString());    // Not friendly for non-threshold event generators
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
