using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Omniscient.Instruments;

namespace Omniscient
{
    public class PresetManager
    {
        List<Preset> presets;
        SiteManager siteMan;

        public PresetManager(SiteManager newSiteMan)
        {
            presets = new List<Preset>();
            siteMan = newSiteMan;
        }

        public ReturnCode LoadFromXML(string fileName)
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
                        }
                    }
                }
                presets.Add(newPreset);
            }
            return ReturnCode.SUCCESS;
        }

        public List<Preset> GetPresets() { return presets; }
    }
}
