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

namespace Omniscient
{
    public abstract class Instrument
    {
        public static readonly InstrumentHookup[] Hookups = new InstrumentHookup[]
        {
            new CSVInstrumentHookup(),
            new GRANDInstrumentHookup(),
            new ISRInstrumentHookup(),
            new MCAInstrumentHookup(),
            new NGAMInstrumentHookup()
        };

        protected string name;
        public string InstrumentType { get; protected set; }
        protected string dataFolder;
        protected string filePrefix;

        protected int numChannels;
        protected Channel[] channels;
        protected List<VirtualChannel> virtualChannels;

        protected string _fileExtension;
        public virtual string FileExtension
        {
            get { return _fileExtension; }
            set
            {
                _fileExtension = value;
                ScanDataFolder();
            }
        }

        public bool IncludeSubDirectories { get; set; }
        public Instrument(string newName)
        {
            name = newName;
            virtualChannels = new List<VirtualChannel>();
        }

        public void LoadVirtualChannels()
        {
            foreach (VirtualChannel chan in virtualChannels)
                chan.CalculateValues();
        }

        public void SetName(string newName)
        {
            foreach (Channel channel in GetStandardChannels())
            {
                channel.SetName(channel.GetName().Replace(name, newName));
            }
            name = newName;
        }

        public abstract void ScanDataFolder();
        public abstract void LoadData(DateTime startDate, DateTime endDate);
        //public abstract ReturnCode IngestFile(string fileName);
        public abstract void ClearData();

        public void SetDataFolder(string newDataFolder)
        {
            dataFolder = newDataFolder;
            if (dataFolder!="") ScanDataFolder();
        }

        public void SetFilePrefix(string newPrefix)
        {
            filePrefix = newPrefix;
        }

        public string GetName() { return name; }
        public string GetInstrumentType() { return InstrumentType; }
        public string GetDataFolder() { return dataFolder; }
        public string GetFilePrefix() { return filePrefix; }
        public int GetNumChannels() { return numChannels; }
        public Channel[] GetStandardChannels()
        {
            return channels;
        }
        public Channel[] GetChannels()
        {
            Channel[] result = new Channel[channels.Length + virtualChannels.Count];
            for(int i = 0; i< result.Length;i++)
            {
                if (i < channels.Length)
                    result[i] = channels[i];
                else
                    result[i] = virtualChannels[i - channels.Length];
            }
            return result;
        }
        
        public List<VirtualChannel> GetVirtualChannels() { return virtualChannels; }

        public abstract List<Parameter> GetParameters();

        protected List<Parameter> GetStandardInstrumentParameters()
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new StringParameter("File Prefix") { Value = filePrefix },
                new DirectoryParameter("Data Directory"){ Value = dataFolder },
                new BoolParameter("Include Subdirectories") {Value = IncludeSubDirectories ? 
                                                                BoolParameter.True : BoolParameter.False}
            };
            return parameters;
        }

        public abstract void ApplyParameters(List<Parameter> parameters);
        public static void ApplyStandardInstrumentParameters(Instrument instrument, List<Parameter> parameters)
        {
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "File Prefix":
                        instrument.filePrefix = param.Value;
                        break;
                    case "Data Directory":
                        instrument.SetDataFolder(param.Value);
                        break;
                    case "Include Subdirectories":
                        instrument.IncludeSubDirectories = ((BoolParameter)param).ToBool();
                        break;
                }
            }
        }

        public static InstrumentHookup GetHookup(string type)
        {
            foreach (InstrumentHookup hookup in Hookups)
            {
                if (hookup.Type == type)
                {
                    return hookup;
                }
            }
            return null;
        }

        public static Instrument FromXML(XmlNode node, DetectionSystem system)
        {
            string name = node.Attributes["Name"]?.InnerText;
            InstrumentHookup hookup = GetHookup(node.Attributes["Type"]?.InnerText);
            List<Parameter> parameters = Parameter.FromXML(node, hookup.TemplateParameters, system);
            return hookup?.FromParameters(name, parameters);
        }

        public static void ToXML(XmlWriter xmlWriter, Instrument instrument)
        {
            xmlWriter.WriteStartElement("Instrument");
            xmlWriter.WriteAttributeString("Name", instrument.GetName());
            xmlWriter.WriteAttributeString("Type", instrument.GetInstrumentType());
            List<Parameter> parameters = instrument.GetParameters();
            foreach (Parameter param in parameters)
            {
                xmlWriter.WriteAttributeString(param.Name.Replace(' ', '_'), param.Value);
            }
        }
    }

    public abstract class InstrumentHookup
    {
        public abstract Instrument FromParameters(string newName, List<Parameter> parameters);
        public abstract string Type { get; }
        public List<ParameterTemplate> TemplateParameters { get; set; } = new List<ParameterTemplate>()
        {
            new ParameterTemplate("File Prefix", ParameterType.String),
            new ParameterTemplate("Data Directory", ParameterType.Directory),
            new ParameterTemplate("Include Subdirectories", ParameterType.Bool)
        };
    }
}
