// This software is open source software available under the BSD-3 license.
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
                        foreach (EventGenerator eventGenerator in sys.GetEventGenerators())
                        {
                            if (eventGenerator.GetName() == name) return true;
                            foreach (Action action in eventGenerator.GetActions())
                                if (action.GetName() == name) return true;
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
                                    case "NGAM":
                                        newInstrument = new NGAMInstrument(instrumentNode.Attributes["name"]?.InnerText);
                                        break;
                                    case "CSV":
                                        newInstrument = new CSVInstrument(instrumentNode.Attributes["name"]?.InnerText,
                                                                           int.Parse(instrumentNode.Attributes["nChannels"]?.InnerText));
                                        ((CSVInstrument)newInstrument).NumberOfHeaders = int.Parse(instrumentNode.Attributes["nHeaders"]?.InnerText);
                                        break;
                                    case "Declaration":
                                        newInstrument = new DeclarationInstrument(instrumentNode.Attributes["name"]?.InnerText);
                                        break;
                                    default:
                                        return ReturnCode.CORRUPTED_FILE;
                                }
                                if (!newInstrument.Equals(null))
                                {
                                    if (instrumentNode.Attributes["file_extension"] != null)
                                    {
                                        newInstrument.FileExtension = instrumentNode.Attributes["file_extension"].InnerText;
                                    }
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
                                            if (chanNode.Attributes["type"]?.InnerText != "ROI")
                                            {
                                                VirtualChannel chan = new VirtualChannel(chanNode.Attributes["name"]?.InnerText,
                                                                                        newInstrument, chanA.GetChannelType());
                                                chan.SetChannelA(chanA);
                                                switch (chanNode.Attributes["type"]?.InnerText)
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
                                                    case "CONVOLVE":
                                                        chan.SetVirtualChannelType(VirtualChannel.VirtualChannelType.CONVOLVE);
                                                        break;
                                                    case "LOCAL_MAX":
                                                        chan.SetVirtualChannelType(VirtualChannel.VirtualChannelType.LOCAL_MAX);
                                                        break;
                                                    case "LOCAL_MIN":
                                                        chan.SetVirtualChannelType(VirtualChannel.VirtualChannelType.LOCAL_MIN);
                                                        break;
                                                    default:
                                                        return ReturnCode.CORRUPTED_FILE;
                                                }
                                                switch (chan.GetVirtualChannelType())
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
                                                    case VirtualChannel.VirtualChannelType.LOCAL_MAX:
                                                    case VirtualChannel.VirtualChannelType.LOCAL_MIN:
                                                        chan.SetConstant(double.Parse(chanNode.Attributes["constant"]?.InnerText));
                                                        break;
                                                    case VirtualChannel.VirtualChannelType.DELAY:
                                                        chan.SetDelay(TimeSpan.FromSeconds(double.Parse(chanNode.Attributes["delay"]?.InnerText)));
                                                        break;
                                                    case VirtualChannel.VirtualChannelType.CONVOLVE:
                                                        chan.SetDataFileName(chanNode.Attributes["data_file"]?.InnerText);
                                                        break;
                                                    default:
                                                        return ReturnCode.CORRUPTED_FILE;
                                                }
                                                newInstrument.GetVirtualChannels().Add(chan);
                                            }
                                            else
                                            {
                                                ROIChannel chan = new ROIChannel(chanNode.Attributes["name"]?.InnerText,
                                                                                        (MCAInstrument)newInstrument, Channel.ChannelType.DURATION_VALUE);
                                                ROI roi = chan.GetROI();
                                                roi.SetROIStart(double.Parse(chanNode.Attributes["roi_start"]?.InnerText));
                                                roi.SetROIEnd(double.Parse(chanNode.Attributes["roi_end"]?.InnerText));
                                                roi.SetBG1Start(double.Parse(chanNode.Attributes["bg1_start"]?.InnerText));
                                                roi.SetBG1End(double.Parse(chanNode.Attributes["bg1_end"]?.InnerText));
                                                roi.SetBG2Start(double.Parse(chanNode.Attributes["bg2_start"]?.InnerText));
                                                roi.SetBG2End(double.Parse(chanNode.Attributes["bg2_end"]?.InnerText));
                                                switch(chanNode.Attributes["bg_type"]?.InnerText)
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
                                                newInstrument.GetVirtualChannels().Add(chan);
                                            }
                                        }
                                        catch { return ReturnCode.CORRUPTED_FILE; }
                                    }
                                    newSystem.AddInstrument(newInstrument);
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
                                    Action action;
                                    switch(actionNode.Attributes["type"]?.InnerText)
                                    {
                                        case "Analysis":
                                            AnalysisAction analysisAction = new AnalysisAction(eg, actionNode.Attributes["name"]?.InnerText);
                                            analysisAction.GetAnalysis().SetCommand(actionNode.Attributes["command"]?.InnerText);
                                            foreach (Instrument inst in newSystem.GetInstruments())
                                            {
                                                foreach (Channel ch in inst.GetChannels())
                                                {
                                                    if (ch.GetName() == actionNode.Attributes["channel"]?.InnerText)
                                                        analysisAction.AddChannel(ch);
                                                }
                                            }
                                            analysisAction.SetCompiledFileName(actionNode.Attributes["compiled_file"]?.InnerText);
                                            analysisAction.GetAnalysis().SetResultsFile(actionNode.Attributes["result_file"]?.InnerText);
                                            switch(actionNode.Attributes["result_parser"]?.InnerText)
                                            {
                                                case "FRAM-Pu":
                                                    analysisAction.GetAnalysis().SetResultParser(new FRAMPlutoniumResultParser());
                                                    break;
                                                case "FRAM-U":
                                                    analysisAction.GetAnalysis().SetResultParser(new FRAMUraniumResultParser());
                                                    break;
                                                default:
                                                    return ReturnCode.CORRUPTED_FILE;
                                            }
                                            foreach (XmlNode dataCompilerNode in actionNode.ChildNodes)
                                            {
                                                if (dataCompilerNode.Name != "DataCompiler") return ReturnCode.CORRUPTED_FILE;
                                                switch (dataCompilerNode.Attributes["type"]?.InnerText)
                                                {
                                                    case "SpectrumCompiler":
                                                        SpectrumCompiler spectrumCompiler = new SpectrumCompiler("");
                                                        switch(dataCompilerNode.Attributes["parser"]?.InnerText)
                                                        {
                                                            case "CHN":
                                                                spectrumCompiler.SetSpectrumParser(new CHNParser());
                                                                break;
                                                            default:
                                                                return ReturnCode.CORRUPTED_FILE;
                                                        }
                                                        switch(dataCompilerNode.Attributes["writer"]?.InnerText)
                                                        {
                                                            case "CHN":
                                                                spectrumCompiler.SetSpectrumWriter(new CHNWriter());
                                                                break;
                                                            default:
                                                                return ReturnCode.CORRUPTED_FILE;
                                                        }
                                                        analysisAction.GetDataCompilers().Add(spectrumCompiler);
                                                        break;
                                                    case "FileListCompiler":
                                                        analysisAction.GetDataCompilers().Add(new FileListCompiler(""));
                                                        break;
                                                    default:
                                                        return ReturnCode.CORRUPTED_FILE;
                                                }
                                            }
                                            action = analysisAction;
                                            break;
                                        case "Command":
                                            action = new CommandAction(eg, actionNode.Attributes["name"]?.InnerText);
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
                            xmlWriter.WriteAttributeString("file_extension", inst.FileExtension);
                            xmlWriter.WriteAttributeString("file_prefix", inst.GetFilePrefix());
                            xmlWriter.WriteAttributeString("type", inst.GetInstrumentType());
                            xmlWriter.WriteAttributeString("directory", inst.GetDataFolder());
                            if (inst is CSVInstrument)
                            {
                                xmlWriter.WriteAttributeString("nHeaders", ((CSVInstrument)inst).NumberOfHeaders.ToString());
                                xmlWriter.WriteAttributeString("nChannels", inst.GetNumChannels().ToString());
                            }
                            foreach (VirtualChannel chan in inst.GetVirtualChannels())
                            {
                                xmlWriter.WriteStartElement("VirtualChannel");
                                xmlWriter.WriteAttributeString("name", chan.GetName());
                                xmlWriter.WriteAttributeString("type", chan.GetVirtualChannelType().ToString());
                                switch (chan.GetVirtualChannelType())
                                {
                                    case VirtualChannel.VirtualChannelType.RATIO:
                                    case VirtualChannel.VirtualChannelType.SUM:
                                    case VirtualChannel.VirtualChannelType.DIFFERENCE:
                                        xmlWriter.WriteAttributeString("channel_A", chan.GetChannelA().GetName());
                                        xmlWriter.WriteAttributeString("channel_B", chan.GetChannelB().GetName());
                                        break;
                                    case VirtualChannel.VirtualChannelType.ADD_CONST:
                                    case VirtualChannel.VirtualChannelType.SCALE:
                                    case VirtualChannel.VirtualChannelType.LOCAL_MAX:
                                    case VirtualChannel.VirtualChannelType.LOCAL_MIN:
                                        xmlWriter.WriteAttributeString("channel_A", chan.GetChannelA().GetName());
                                        xmlWriter.WriteAttributeString("constant", chan.GetConstant().ToString());
                                        break;
                                    case VirtualChannel.VirtualChannelType.DELAY:
                                        xmlWriter.WriteAttributeString("channel_A", chan.GetChannelA().GetName());
                                        xmlWriter.WriteAttributeString("delay", chan.GetDelay().TotalSeconds.ToString());
                                        break;
                                    case VirtualChannel.VirtualChannelType.CONVOLVE:
                                        xmlWriter.WriteAttributeString("channel_A", chan.GetChannelA().GetName());
                                        xmlWriter.WriteAttributeString("data_file", chan.GetDataFileName());
                                        break;
                                    case VirtualChannel.VirtualChannelType.ROI:
                                        xmlWriter.WriteAttributeString("roi_start", ((ROIChannel)chan).GetROI().GetROIStart().ToString());
                                        xmlWriter.WriteAttributeString("roi_end", ((ROIChannel)chan).GetROI().GetROIEnd().ToString());
                                        xmlWriter.WriteAttributeString("bg1_start", ((ROIChannel)chan).GetROI().GetBG1Start().ToString());
                                        xmlWriter.WriteAttributeString("bg1_end", ((ROIChannel)chan).GetROI().GetBG1End().ToString());
                                        xmlWriter.WriteAttributeString("bg2_start", ((ROIChannel)chan).GetROI().GetBG2Start().ToString());
                                        xmlWriter.WriteAttributeString("bg2_end", ((ROIChannel)chan).GetROI().GetBG2End().ToString());
                                        xmlWriter.WriteAttributeString("bg_type", (ROI.BGTypeToString(((ROIChannel)chan).GetROI().GetBGType())));
                                        break;
                                }
                                xmlWriter.WriteEndElement();
                            }
                            xmlWriter.WriteEndElement();
                        }
                        foreach (EventGenerator eg in sys.GetEventGenerators())
                        {
                            EventGenerator.ToXML(xmlWriter, eg);
                            foreach(Action action in eg.GetActions())
                            {
                                xmlWriter.WriteStartElement("Action");
                                xmlWriter.WriteAttributeString("name", action.GetName());
                                xmlWriter.WriteAttributeString("type", action.GetActionType());
                                if (action is AnalysisAction)
                                {
                                    xmlWriter.WriteAttributeString("command", ((AnalysisAction)action).GetAnalysis().GetCommand());
                                    xmlWriter.WriteAttributeString("channel", ((AnalysisAction)action).GetChannels()[0].GetName());
                                    xmlWriter.WriteAttributeString("compiled_file", ((AnalysisAction)action).GetCompiledFileName());
                                    xmlWriter.WriteAttributeString("result_file", ((AnalysisAction)action).GetAnalysis().GetResultsFile());
                                    if(((AnalysisAction)action).GetAnalysis().GetResultParser() is FRAMPlutoniumResultParser)
                                    {
                                        xmlWriter.WriteAttributeString("result_parser", "FRAM-Pu");
                                    }
                                    else if (((AnalysisAction)action).GetAnalysis().GetResultParser() is FRAMUraniumResultParser)
                                    {
                                        xmlWriter.WriteAttributeString("result_parser", "FRAM-U");
                                    }
                                    foreach (DataCompiler dataCompiler in ((AnalysisAction)action).GetDataCompilers())
                                    {
                                        xmlWriter.WriteStartElement("DataCompiler");
                                        if(dataCompiler is SpectrumCompiler)
                                        {
                                            xmlWriter.WriteAttributeString("type", "SpectrumCompiler");
                                            xmlWriter.WriteAttributeString("parser", ((SpectrumCompiler)dataCompiler).GetSpectrumParser().GetParserType());
                                            xmlWriter.WriteAttributeString("writer", ((SpectrumCompiler)dataCompiler).GetSpectrumWriter().GetWriterType());
                                        }
                                        else if(dataCompiler is FileListCompiler)
                                        {
                                            xmlWriter.WriteAttributeString("type", "FileListCompiler");
                                        }
                                        xmlWriter.WriteEndElement();
                                    }
                                }
                                else if (action is CommandAction)
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
