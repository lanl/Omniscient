﻿// This software is open source software available under the BSD-3 license.
// 
// Copyright (c) 2018, Los Alamos National Security, LLC
// All rights reserved.
// 
// Copyright 2018. Los Alamos National Security, LLC. This software was produced under U.S. Government contract DE-AC52-06NA25396 for Los Alamos National Laboratory (LANL), which is operated by Los Alamos National Security, LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  NEITHER THE GOVERNMENT NOR LOS ALAMOS NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.  If software is modified to produce derivative works, such modified software should be clearly marked, so as not to confuse it with the version available from LANL.
// 
// Additionally, redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 1.       Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
// 2.       Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 
// 3.       Neither the name of Los Alamos National Security, LLC, Los Alamos National Laboratory, LANL, the U.S. Government, nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 
//  
// THIS SOFTWARE IS PROVIDED BY LOS ALAMOS NATIONAL SECURITY, LLC AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL LOS ALAMOS NATIONAL SECURITY, LLC OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;
using System.IO;

namespace Omniscient
{
    public class SiteManager : Persister
    {
        public override string Species { get { return "Site Manager"; } }

        private List<Site> sites;
        private string xmlFile;
        string omniscient_version;

        public SiteManager(string newXMLFile, string version) : base(null, "", 0)
        {
            sites = new List<Site>();
            xmlFile = newXMLFile;
            omniscient_version = version;
        }

        public override void ToXML(XmlWriter xmlWriter)
        {
            throw new NotImplementedException();
        }

        public bool ContainsName(string name)
        {
            foreach(Site site in sites)
            {
                if (site.Name == name) return true;
                foreach(Facility fac in site.GetFacilities())
                {
                    if (fac.Name == name) return true;
                    foreach(DetectionSystem sys in fac.GetSystems())
                    {
                        if (sys.Name == name) return true;
                        foreach(Instrument inst in sys.GetInstruments())
                        {
                            if (inst.Name == name) return true;
                            foreach(Channel chan in inst.GetChannels())
                                if (chan.Name == name) return true;
                        }
                        foreach (EventGenerator eventGenerator in sys.GetEventGenerators())
                        {
                            if (eventGenerator.Name == name) return true;
                            foreach (Action action in eventGenerator.GetActions())
                                if (action.Name == name) return true;
                        }
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

        public ReturnCode WriteBlank()
        {
            XmlWriter xmlWriter = XmlWriter.Create(xmlFile, new XmlWriterSettings()
            {
                Indent = true,
            });

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("SiteManager");
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return ReturnCode.SUCCESS;
        }

        public ReturnCode LoadFromXML(string fileName)
        {
            if (!File.Exists(fileName)) return ReturnCode.FILE_DOESNT_EXIST;
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            Persister.TakenIDs.Clear();
            sites.Clear();

            if(doc.DocumentElement.Attributes["Omniscient_Version"] == null)
            {
                MessageBox.Show("Warning: SiteManager.xml was made by an older version of Omniscient.");
            }

            foreach (XmlNode siteNode in doc.DocumentElement.ChildNodes)
            {
                if (siteNode.Name != "Site") return ReturnCode.CORRUPTED_FILE;
                Site newSite = Site.FromXML(siteNode, this);
                foreach (XmlNode facilityNode in siteNode.ChildNodes)
                {
                    if (facilityNode.Name != "Facility") return ReturnCode.CORRUPTED_FILE;
                    Facility newFacility = Facility.FromXML(facilityNode, newSite);
                    foreach (XmlNode systemNode in facilityNode.ChildNodes)
                    {
                        if (systemNode.Name != "System") return ReturnCode.CORRUPTED_FILE;
                        DetectionSystem newSystem = DetectionSystem.FromXML(systemNode, newFacility);
                        foreach (XmlNode instrumentNode in systemNode.ChildNodes)
                        {
                            if (instrumentNode.Name == "Instrument")
                            {
                                Instrument newInstrument = Instrument.FromXML(instrumentNode, newSystem);
                                if (!newInstrument.Equals(null))
                                {
                                    int channelCount = 0;
                                    Channel[] channels = newInstrument.GetStandardChannels();
                                    foreach (XmlNode chanNode in instrumentNode.ChildNodes)
                                    {
                                        if (chanNode.Name == "Channel")
                                        {
                                            if (channelCount >= channels.Length) return ReturnCode.CORRUPTED_FILE;
                                            channels[channelCount].ApplyXML(chanNode);
                                            channelCount++;
                                        }
                                        else if (chanNode.Name == "VirtualChannel")
                                        {
                                            try
                                            {
                                                if (chanNode.Attributes["type"]?.InnerText != "ROI")
                                                {
                                                    VirtualChannel chan = VirtualChannel.FromXML(chanNode, newInstrument);
                                                }
                                                else
                                                {
                                                    ROIChannel chan = new ROIChannel(chanNode.Attributes["name"]?.InnerText,
                                                                                            (MCAInstrument)newInstrument, Channel.ChannelType.DURATION_VALUE,
                                                                                            uint.Parse(siteNode.Attributes["ID"]?.InnerText, System.Globalization.NumberStyles.HexNumber));
                                                    ROI roi = chan.GetROI();
                                                    roi.SetROIStart(double.Parse(chanNode.Attributes["roi_start"]?.InnerText));
                                                    roi.SetROIEnd(double.Parse(chanNode.Attributes["roi_end"]?.InnerText));
                                                    roi.SetBG1Start(double.Parse(chanNode.Attributes["bg1_start"]?.InnerText));
                                                    roi.SetBG1End(double.Parse(chanNode.Attributes["bg1_end"]?.InnerText));
                                                    roi.SetBG2Start(double.Parse(chanNode.Attributes["bg2_start"]?.InnerText));
                                                    roi.SetBG2End(double.Parse(chanNode.Attributes["bg2_end"]?.InnerText));
                                                    switch (chanNode.Attributes["bg_type"]?.InnerText)
                                                    {
                                                        case "None":
                                                            roi.SetBGType(ROI.BG_Type.NONE);
                                                            break;
                                                        case "Flat":
                                                            roi.SetBGType(ROI.BG_Type.FLAT);
                                                            break;
                                                        case "Linear":
                                                            roi.SetBGType(ROI.BG_Type.LINEAR);
                                                            break;
                                                        default:
                                                            return ReturnCode.CORRUPTED_FILE;
                                                    }
                                                }
                                            }
                                            catch { return ReturnCode.CORRUPTED_FILE; }
                                        }
                                        else return ReturnCode.CORRUPTED_FILE;
                                    }
                                }
                            }
                            else if (instrumentNode.Name == "EventGenerator")
                            {
                                XmlNode eventNode = instrumentNode;     // Correct some shoddy nomenclature...
                                EventGenerator eg = EventGenerator.FromXML(eventNode, newSystem);
                                if (eg == null) return ReturnCode.CORRUPTED_FILE;
                                foreach (XmlNode actionNode in eventNode.ChildNodes)
                                {
                                    if (actionNode.Name != "Action") return ReturnCode.CORRUPTED_FILE;
                                    Action action = Action.FromXML(actionNode, eg);
                                }
                            }
                            else
                            {
                                return ReturnCode.CORRUPTED_FILE;
                            }
                        }
                    }
                }
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
            xmlWriter.WriteAttributeString("Omniscient_Version", omniscient_version);
            foreach (Site site in sites)
            {
                site.ToXML(xmlWriter);
            }
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return ReturnCode.SUCCESS;
        }
        public List<Site> GetSites() { return sites; }
    }
}
