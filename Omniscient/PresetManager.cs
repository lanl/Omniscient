// This software is open source software available under the BSD-3 license.
// 
// Copyright (c) 2018, Triad National Security, LLC
// All rights reserved.
// 
// Copyright 2018. Triad National Security, LLC. This software was produced under U.S. Government contract 89233218CNA000001 for Los Alamos National Laboratory (LANL), which is operated by Triad National Security, LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  NEITHER THE GOVERNMENT NOR TRIAD NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.  If software is modified to produce derivative works, such modified software should be clearly marked, so as not to confuse it with the version available from LANL.
// 
// Additionally, redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 1.       Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
// 2.       Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 
// 3.       Neither the name of Triad National Security, LLC, Los Alamos National Laboratory, LANL, the U.S. Government, nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 
//  
// THIS SOFTWARE IS PROVIDED BY TRIAD NATIONAL SECURITY, LLC AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL TRIAD NATIONAL SECURITY, LLC OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

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

        public ReturnCode WriteBlank()
        {
            XmlWriter xmlWriter = XmlWriter.Create(xmlFile, new XmlWriterSettings()
            {
                Indent = true,
            });

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Presets");
            xmlWriter.WriteAttributeString("Omniscient_Version", OmniscientCore.VERSION);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return ReturnCode.SUCCESS;
        }

        public ReturnCode LoadFromXML(string fileName)
        {
            if (!File.Exists(fileName)) return ReturnCode.FILE_DOESNT_EXIST;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                presets.Clear();

                // Silently update presets if need be
                if (doc.DocumentElement.Attributes["Omniscient_Version"] == null)
                {
                    WriteBlank();
                    return ReturnCode.SUCCESS;
                }
                foreach (XmlNode presetNode in doc.DocumentElement.ChildNodes)
                {
                    Preset newPreset = new Preset(presetNode.Attributes["name"]?.InnerText);
                    foreach (XmlNode siteNode in presetNode.ChildNodes)
                    {
                        uint siteID = uint.Parse(siteNode.Attributes["ID"].InnerText, System.Globalization.NumberStyles.HexNumber);
                        Site site;
                        try { site = siteMan.GetSites().Single(s => s.ID == siteID); }
                        catch { continue; }
                        foreach (XmlNode facilityNode in siteNode.ChildNodes)
                        {
                            uint facID = uint.Parse(facilityNode.Attributes["ID"].InnerText, System.Globalization.NumberStyles.HexNumber);
                            Facility fac;
                            try { fac = site.GetFacilities().Single(s => s.ID == facID); }
                            catch { continue; }
                            foreach (XmlNode systemNode in facilityNode.ChildNodes)
                            {
                                uint sysID = uint.Parse(systemNode.Attributes["ID"].InnerText, System.Globalization.NumberStyles.HexNumber);
                                DetectionSystem sys;
                                try { sys = fac.GetSystems().Single(s => s.ID == sysID); }
                                catch { continue; }
                                foreach (XmlNode instrumentNode in systemNode.ChildNodes)
                                {
                                    if (instrumentNode.Name == "Instrument")
                                    {
                                        uint instID = uint.Parse(instrumentNode.Attributes["ID"].InnerText, System.Globalization.NumberStyles.HexNumber);
                                        Instrument inst;
                                        try { inst = sys.GetInstruments().Single(i => i.ID == instID); }
                                        catch { continue; }
                                        if (instrumentNode.Attributes["checked"] != null)
                                        {
                                            if (instrumentNode.Attributes["checked"].InnerText == "true")
                                            {
                                                newPreset.GetActiveInstruments().Add(inst);
                                                foreach (XmlNode chanNode in instrumentNode.ChildNodes)
                                                {
                                                    uint chanID = uint.Parse(chanNode.Attributes["ID"].InnerText, System.Globalization.NumberStyles.HexNumber);
                                                    try
                                                    { 
                                                        Channel chan = inst.GetChannels().Single(c => c.ID == chanID);
                                                        ChannelDisplayConfig config = ChannelDisplayConfig.FromXML(chanNode["ChannelDisplayConfig"]);
                                                        newPreset.AddChannel(chan, config);
                                                    }
                                                    catch { }
                                                }
                                            }
                                        }
                                    }
                                    else if (instrumentNode.Name == "EventGenerator")
                                    {
                                        XmlNode eventGenNode = instrumentNode;
                                        try
                                        {
                                            uint egID = uint.Parse(eventGenNode.Attributes["ID"].InnerText, System.Globalization.NumberStyles.HexNumber);
                                            EventGenerator eventGenerator = sys.GetEventGenerators().Single(e => e.ID == egID);
                                            if (eventGenNode.Attributes["checked"].InnerText == "true")
                                            {
                                                newPreset.GetActiveEventGenerators().Add(eventGenerator);
                                            }
                                        }
                                        catch { }
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
            xmlWriter.WriteAttributeString("Omniscient_Version", OmniscientCore.VERSION);
            foreach (Preset preset in presets)
            {
                xmlWriter.WriteStartElement("Preset");
                xmlWriter.WriteAttributeString("name", preset.Name);
                foreach (Site site in siteMan.GetSites())
                {
                    xmlWriter.WriteStartElement("Site");
                    xmlWriter.WriteAttributeString("name", site.Name);
                    xmlWriter.WriteAttributeString("ID", site.ID.ToString("X8"));
                    foreach (Facility fac in site.GetFacilities())
                    {
                        xmlWriter.WriteStartElement("Facility");
                        xmlWriter.WriteAttributeString("name", fac.Name);
                        xmlWriter.WriteAttributeString("ID", fac.ID.ToString("X8"));
                        foreach (DetectionSystem sys in fac.GetSystems())
                        {
                            xmlWriter.WriteStartElement("System");
                            xmlWriter.WriteAttributeString("name", sys.Name);
                            xmlWriter.WriteAttributeString("ID", sys.ID.ToString("X8"));
                            foreach (Instrument inst in sys.GetInstruments())
                            {
                                xmlWriter.WriteStartElement("Instrument");
                                xmlWriter.WriteAttributeString("name", inst.Name);
                                xmlWriter.WriteAttributeString("ID", inst.ID.ToString("X8"));
                                if (preset.GetActiveInstruments().Contains(inst))
                                {
                                    xmlWriter.WriteAttributeString("checked", "true");
                                    foreach (Tuple<Channel, ChannelDisplayConfig> channelPreset in preset.ChannelPresets)
                                    {
                                        Channel channel = channelPreset.Item1;
                                        ChannelDisplayConfig config = channelPreset.Item2;
                                        if (inst.GetChannels().Contains(channel))
                                        {
                                            xmlWriter.WriteStartElement("Channel");
                                            xmlWriter.WriteAttributeString("name", channel.Name);
                                            xmlWriter.WriteAttributeString("ID", channel.ID.ToString("X8"));
                                            config.ToXML(xmlWriter);
                                            xmlWriter.WriteEndElement();
                                        }
                                    }
                                }
                                xmlWriter.WriteEndElement();
                            }
                            foreach (EventGenerator eventGenerator in sys.GetEventGenerators())
                            {
                                if (preset.GetActiveEventGenerators().Contains(eventGenerator))
                                {
                                    xmlWriter.WriteStartElement("EventGenerator");
                                    xmlWriter.WriteAttributeString("name", eventGenerator.Name);
                                    xmlWriter.WriteAttributeString("ID", eventGenerator.ID.ToString("X8"));
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
