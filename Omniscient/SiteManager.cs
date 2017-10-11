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

        public bool ContainsName(string name)
        {
            foreach(Site site in sites)
            {
                if (site.GetName() == name) return true;
                foreach(Facility fac in site.GetFacilities())
                {
                    if (fac.GetName() == name) return true;
                    foreach(DetectionSystem sys in fac.GetSystems())
                    {
                        if (sys.GetName() == name) return true;
                        foreach(Instrument inst in sys.GetInstruments())
                        {
                            if (inst.GetName() == name) return true;
                            foreach(Channel chan in inst.GetChannels())
                                if (chan.GetName() == name) return true;
                        }
                        foreach(EventGenerator eventGenerator in sys.GetEventGenerators())
                            if (eventGenerator.GetName() == name) return true;
                    }
                }
            }
            return false;
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
                                    foreach(XmlNode chanNode in instrumentNode.ChildNodes)
                                    {
                                        try
                                        {
                                            Channel chanA = null;
                                            foreach (Channel ch in newInstrument.GetChannels())
                                            {
                                                if (ch.GetName() == chanNode.Attributes["channel_A"]?.InnerText)
                                                    chanA = ch;
                                            }
                                            VirtualChannel chan = new VirtualChannel(chanNode.Attributes["name"]?.InnerText,
                                                                                    newInstrument, chanA.GetChannelType());
                                            chan.SetChannelA(chanA);
                                            switch(chanNode.Attributes["type"]?.InnerText)
                                            {
                                                case "RATIO":
                                                    chan.SetVirtualChannelType(VirtualChannel.VirtualChannelType.RATIO);
                                                    break;
                                                case "SUM":
                                                    chan.SetVirtualChannelType(VirtualChannel.VirtualChannelType.SUM);
                                                    break;
                                                case "DIFFERENCE":
                                                    chan.SetVirtualChannelType(VirtualChannel.VirtualChannelType.DIFFERENCE);
                                                    break;
                                                case "ADD_CONST":
                                                    chan.SetVirtualChannelType(VirtualChannel.VirtualChannelType.ADD_CONST);
                                                    break;
                                                case "SCALE":
                                                    chan.SetVirtualChannelType(VirtualChannel.VirtualChannelType.SCALE);
                                                    break;
                                                case "DELAY":
                                                    chan.SetVirtualChannelType(VirtualChannel.VirtualChannelType.DELAY);
                                                    break;
                                                default:
                                                    return ReturnCode.CORRUPTED_FILE;
                                            }
                                            switch(chan.GetVirtualChannelType())
                                            {
                                                case VirtualChannel.VirtualChannelType.RATIO:
                                                case VirtualChannel.VirtualChannelType.SUM:
                                                case VirtualChannel.VirtualChannelType.DIFFERENCE:
                                                    Channel chanB = null;
                                                    foreach (Channel ch in newInstrument.GetChannels())
                                                    {
                                                        if (ch.GetName() == chanNode.Attributes["channel_B"]?.InnerText)
                                                            chanB = ch;
                                                    }
                                                    chan.SetChannelB(chanB);
                                                    break;
                                                case VirtualChannel.VirtualChannelType.ADD_CONST:
                                                case VirtualChannel.VirtualChannelType.SCALE:
                                                    chan.SetConstant(double.Parse(chanNode.Attributes["constant"]?.InnerText));
                                                    break;
                                                case VirtualChannel.VirtualChannelType.DELAY:
                                                    chan.SetDelay(TimeSpan.FromSeconds(double.Parse(chanNode.Attributes["delay"]?.InnerText)));
                                                    break;
                                                default:
                                                    return ReturnCode.CORRUPTED_FILE;
                                            }
                                            newInstrument.GetVirtualChannels().Add(chan);
                                        }
                                        catch { return ReturnCode.CORRUPTED_FILE; }
                                    }
                                    newSystem.AddInstrument(newInstrument);
                                }
                            }
                            else if (instrumentNode.Name == "EventGenerator")
                            {
                                XmlNode eventNode = instrumentNode;     // Correct some shoddy nomenclature...
                                EventGenerator eg;
                                switch (eventNode.Attributes["type"]?.InnerText)
                                {
                                    case "Threshold":
                                        Channel channel = null;
                                        foreach (Instrument inst in newSystem.GetInstruments())
                                        {
                                            foreach (Channel ch in inst.GetChannels())
                                            {
                                                if (ch.GetName() == eventNode.Attributes["channel"]?.InnerText)
                                                    channel = ch;
                                            }
                                        }
                                        if (channel is null) return ReturnCode.CORRUPTED_FILE;
                                        try
                                        {
                                            eg = new ThresholdEG(eventNode.Attributes["name"]?.InnerText, channel, double.Parse(instrumentNode.Attributes["threshold"]?.InnerText));
                                            if (eventNode.Attributes["debounce_time"] != null)
                                                ((ThresholdEG)eg).SetDebounceTime(TimeSpan.FromSeconds(double.Parse(eventNode.Attributes["debounce_time"]?.InnerText)));
                                        }
                                        catch
                                        {
                                            return ReturnCode.CORRUPTED_FILE;
                                        }
                                        break;
                                    case "Coincidence":
                                        try
                                        {
                                            eg = new CoincidenceEG(eventNode.Attributes["name"]?.InnerText);
                                            CoincidenceEG coinkEG = (CoincidenceEG)eg;
                                            switch (eventNode.Attributes["coincidence_type"]?.InnerText)
                                            {
                                                case "A_THEN_B":
                                                    coinkEG.SetCoincidenceType(CoincidenceEG.CoincidenceType.A_THEN_B);
                                                    break;
                                                case "B_THEN_A":
                                                    coinkEG.SetCoincidenceType(CoincidenceEG.CoincidenceType.B_THEN_A);
                                                    break;
                                                case "EITHER_ORDER":
                                                    coinkEG.SetCoincidenceType(CoincidenceEG.CoincidenceType.EITHER_ORDER);
                                                    break;
                                                default:
                                                    return ReturnCode.CORRUPTED_FILE;
                                            }
                                            switch (eventNode.Attributes["timing_type"]?.InnerText)
                                            {
                                                case "START_TO_START":
                                                    coinkEG.SetTimingType(CoincidenceEG.TimingType.START_TO_START);
                                                    break;
                                                case "START_TO_END":
                                                    coinkEG.SetTimingType(CoincidenceEG.TimingType.START_TO_END);
                                                    break;
                                                case "END_TO_START":
                                                    coinkEG.SetTimingType(CoincidenceEG.TimingType.END_TO_START);
                                                    break;
                                                case "END_TO_END":
                                                    coinkEG.SetTimingType(CoincidenceEG.TimingType.END_TO_END);
                                                    break;
                                                case "MAX_TO_MAX":
                                                    coinkEG.SetTimingType(CoincidenceEG.TimingType.MAX_TO_MAX);
                                                    break;
                                                default:
                                                    return ReturnCode.CORRUPTED_FILE;
                                            }
                                            foreach(EventGenerator watchedEG in newSystem.GetEventGenerators())
                                            {
                                                if (watchedEG.GetName() == eventNode.Attributes["event_generator_A"]?.InnerText)
                                                    coinkEG.SetEventGeneratorA(watchedEG);
                                                if (watchedEG.GetName() == eventNode.Attributes["event_generator_B"]?.InnerText)
                                                    coinkEG.SetEventGeneratorB(watchedEG);
                                            }
                                            coinkEG.SetWindow(TimeSpan.FromSeconds(double.Parse(eventNode.Attributes["window"]?.InnerText)));
                                            coinkEG.SetMinDifference(TimeSpan.FromSeconds(double.Parse(eventNode.Attributes["min_difference"]?.InnerText)));
                                        }
                                        catch
                                        {
                                            return ReturnCode.CORRUPTED_FILE;
                                        }
                                        break;
                                    default:
                                        return ReturnCode.CORRUPTED_FILE;
                                }
                                foreach (XmlNode actionNode in eventNode.ChildNodes)
                                {
                                    if (actionNode.Name != "Action") return ReturnCode.CORRUPTED_FILE;
                                    Events.Action action;
                                    switch(actionNode.Attributes["type"]?.InnerText)
                                    {
                                        case "command":
                                            action = new CommandAction(actionNode.Attributes["name"]?.InnerText);
                                            ((CommandAction)action).SetCommand(actionNode.Attributes["command"]?.InnerText);
                                            break;
                                        default:
                                            return ReturnCode.CORRUPTED_FILE;
                                    }
                                    eg.GetActions().Add(action);
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
                            foreach(VirtualChannel chan in inst.GetVirtualChannels())
                            {
                                xmlWriter.WriteStartElement("VirtualChannel");
                                xmlWriter.WriteAttributeString("name", chan.GetName());
                                xmlWriter.WriteAttributeString("type", chan.GetVirtualChannelType().ToString());
                                xmlWriter.WriteAttributeString("channel_A", chan.GetChannelA().GetName());
                                switch (chan.GetVirtualChannelType())
                                {
                                    case VirtualChannel.VirtualChannelType.RATIO:
                                    case VirtualChannel.VirtualChannelType.SUM:
                                    case VirtualChannel.VirtualChannelType.DIFFERENCE:
                                        xmlWriter.WriteAttributeString("channel_B", chan.GetChannelB().GetName());
                                        break;
                                    case VirtualChannel.VirtualChannelType.ADD_CONST:
                                    case VirtualChannel.VirtualChannelType.SCALE:
                                        xmlWriter.WriteAttributeString("constant", chan.GetConstant().ToString());
                                        break;
                                    case VirtualChannel.VirtualChannelType.DELAY:
                                        xmlWriter.WriteAttributeString("delay", chan.GetDelay().TotalSeconds.ToString());
                                        break;
                                }
                                xmlWriter.WriteEndElement();
                            }
                            xmlWriter.WriteEndElement();
                        }
                        foreach (EventGenerator eg in sys.GetEventGenerators())
                        {
                            xmlWriter.WriteStartElement("EventGenerator");
                            xmlWriter.WriteAttributeString("name", eg.GetName());
                            xmlWriter.WriteAttributeString("type", eg.GetEventGeneratorType());
                            if (eg is ThresholdEG)
                            {
                                xmlWriter.WriteAttributeString("channel", ((ThresholdEG)eg).GetChannel().GetName());    
                                xmlWriter.WriteAttributeString("threshold", ((ThresholdEG)eg).GetThreshold().ToString());
                                xmlWriter.WriteAttributeString("debounce_time", ((ThresholdEG)eg).GetDebounceTime().TotalSeconds.ToString());
                            }
                            else if(eg is CoincidenceEG)
                            {
                                CoincidenceEG coinkEg = (CoincidenceEG)eg;
                                switch (coinkEg.GetCoincidenceType())
                                {
                                    case CoincidenceEG.CoincidenceType.A_THEN_B:
                                        xmlWriter.WriteAttributeString("coincidence_type", "A_THEN_B");
                                        break;
                                    case CoincidenceEG.CoincidenceType.B_THEN_A:
                                        xmlWriter.WriteAttributeString("coincidence_type", "B_THEN_A");
                                        break;
                                    case CoincidenceEG.CoincidenceType.EITHER_ORDER:
                                        xmlWriter.WriteAttributeString("coincidence_type", "EITHER_ORDER");
                                        break;
                                }
                                switch (coinkEg.GetTimingType())
                                {
                                    case CoincidenceEG.TimingType.START_TO_START:
                                        xmlWriter.WriteAttributeString("timing_type", "START_TO_START");
                                        break;
                                    case CoincidenceEG.TimingType.START_TO_END:
                                        xmlWriter.WriteAttributeString("timing_type", "START_TO_END");
                                        break;
                                    case CoincidenceEG.TimingType.END_TO_START:
                                        xmlWriter.WriteAttributeString("timing_type", "END_TO_START");
                                        break;
                                    case CoincidenceEG.TimingType.END_TO_END:
                                        xmlWriter.WriteAttributeString("timing_type", "END_TO_END");
                                        break;
                                    case CoincidenceEG.TimingType.MAX_TO_MAX:
                                        xmlWriter.WriteAttributeString("timing_type", "MAX_TO_MAX");
                                        break;
                                }
                                xmlWriter.WriteAttributeString("event_generator_A", coinkEg.GetEventGeneratorA().GetName());
                                xmlWriter.WriteAttributeString("event_generator_B", coinkEg.GetEventGeneratorA().GetName());
                                xmlWriter.WriteAttributeString("window", coinkEg.GetWindow().TotalSeconds.ToString());
                                xmlWriter.WriteAttributeString("min_difference", coinkEg.GetMinDifference().TotalSeconds.ToString());
                            }
                            foreach(Events.Action action in eg.GetActions())
                            {
                                xmlWriter.WriteStartElement("Action");
                                xmlWriter.WriteAttributeString("name", action.GetName());
                                xmlWriter.WriteAttributeString("type", action.GetActionType());
                                if (action is CommandAction)
                                {
                                    xmlWriter.WriteAttributeString("command", ((CommandAction)action).GetCommand());
                                }
                                xmlWriter.WriteEndElement();
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
        public List<Site> GetSites() { return sites; }
    }
}
