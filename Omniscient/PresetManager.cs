using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Omniscient.Instruments;
using Omniscient.Events;

namespace Omniscient
{
    public class PresetManager
    {
        const int N_CHARTS = 4;
        List<Preset> presets;
        SiteManager siteMan;
        string xmlFile;

        public PresetManager(string newXMLfile, SiteManager newSiteMan)
        {
            xmlFile = newXMLfile;
            presets = new List<Preset>();
            siteMan = newSiteMan;
        }

        public ReturnCode Save()
        {
            return WriteToXML(xmlFile);
        }

        public ReturnCode Reload()
        {
            return LoadFromXML(xmlFile);
        }

        public ReturnCode LoadFromXML(string fileName)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                presets.Clear();
                foreach (XmlNode presetNode in doc.DocumentElement.ChildNodes)
                {
                    Preset newPreset = new Preset(presetNode.Attributes["name"]?.InnerText);
                    foreach (XmlNode siteNode in presetNode.ChildNodes)
                    {
                        Site site = siteMan.GetSites().Single(s => s.GetName() == siteNode.Attributes["name"]?.InnerText);
                        foreach (XmlNode facilityNode in siteNode.ChildNodes)
                        {
                            Facility fac = site.GetFacilities().Single(s => s.GetName() == facilityNode.Attributes["name"]?.InnerText);
                            foreach (XmlNode systemNode in facilityNode.ChildNodes)
                            {
                                DetectionSystem sys = fac.GetSystems().Single(s => s.GetName() == systemNode.Attributes["name"]?.InnerText);
                                foreach (XmlNode instrumentNode in systemNode.ChildNodes)
                                {
                                    if (instrumentNode.Name == "Instrument")
                                    {
                                        Instrument inst = sys.GetInstruments().Single(i => i.GetName() == instrumentNode.Attributes["name"]?.InnerText);
                                        if (instrumentNode.Attributes["checked"] != null)
                                        {
                                            if (instrumentNode.Attributes["checked"].InnerText == "true")
                                            {
                                                newPreset.GetActiveInstruments().Add(inst);
                                                foreach (XmlNode chanNode in instrumentNode.ChildNodes)
                                                {
                                                    Channel chan = inst.GetChannels().Single(c => c.GetName() == chanNode.Attributes["name"]?.InnerText);
                                                    foreach (XmlNode checkNode in chanNode.ChildNodes)
                                                    {
                                                        if (checkNode.Attributes["checked"] != null)
                                                        {
                                                            if (checkNode.Attributes["checked"].InnerText == "true")
                                                            {
                                                                newPreset.AddChannel(chan, int.Parse(checkNode.Attributes["chart"].InnerText));
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (instrumentNode.Name == "EventGenerator")
                                    {
                                        XmlNode eventGenNode = instrumentNode;
                                        EventGenerator eventGenerator = sys.GetEventGenerators().Single(e => e.GetName() == eventGenNode.Attributes["name"]?.InnerText);
                                        if (eventGenNode.Attributes["checked"].InnerText == "true")
                                        {
                                            newPreset.GetActiveEventGenerators().Add(eventGenerator);
                                        }
                                    }
                                    else
                                        return ReturnCode.CORRUPTED_FILE;
                                }
                            }
                        }
                    }
                    presets.Add(newPreset);
                }
            }
            catch
            {
                return ReturnCode.FAIL;
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
            xmlWriter.WriteStartElement("Presets");
            foreach (Preset preset in presets)
            {
                xmlWriter.WriteStartElement("Preset");
                xmlWriter.WriteAttributeString("name", preset.GetName());
                foreach (Site site in siteMan.GetSites())
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
                                if (preset.GetActiveInstruments().Contains(inst))
                                {
                                    xmlWriter.WriteAttributeString("checked", "true");
                                    foreach (Channel ch in inst.GetChannels())
                                    {
                                        xmlWriter.WriteStartElement("Channel");
                                        xmlWriter.WriteAttributeString("name", ch.GetName());
                                        for (int chart = 0; chart<N_CHARTS; chart++)
                                        {
                                            if (preset.GetActiveChannels()[chart].Contains(ch))
                                            {
                                                xmlWriter.WriteStartElement("CheckBox");
                                                xmlWriter.WriteAttributeString("chart", chart.ToString());
                                                xmlWriter.WriteAttributeString("checked", "true");
                                                xmlWriter.WriteEndElement();
                                            }
                                        }
                                        xmlWriter.WriteEndElement();
                                    }
                                }
                                xmlWriter.WriteEndElement();
                            }
                            foreach (EventGenerator eventGenerator in sys.GetEventGenerators())
                            {
                                if (preset.GetActiveEventGenerators().Contains(eventGenerator))
                                {
                                    xmlWriter.WriteStartElement("EventGenerator");
                                    xmlWriter.WriteAttributeString("name", eventGenerator.GetName());
                                    xmlWriter.WriteAttributeString("checked", "true");
                                    xmlWriter.WriteEndElement();
                                }
                            }
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

        public List<Preset> GetPresets() { return presets; }
    }
}
