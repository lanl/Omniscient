// This software is open source software available under the BSD-3 license.
// 
// Copyright (c) 2020, International Atomic Energy Agency (IAEA), IAEA.org
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Omniscient
{
    public abstract class Instrument : Persister
    {
        public override string Species { get { return "Instrument"; } }

        public static readonly InstrumentHookup[] Hookups = new InstrumentHookup[]
        {
            new ATPMInstrumentHookup(),
            new CSVInstrumentHookup(),
            new DATAZInstrumentHookup(),
            new FileInstrumentHookup(),
            new GRANDInstrumentHookup(),
            new ImageInstrumentHookup(),
            new ISRInstrumentHookup(),
            new MCAInstrumentHookup(),
            new NGAMInstrumentHookup(),
            new SMMSInstrumentHookup(),
            new THDInstrumentHookup(),
            new WUCSInstrumentHookup()
        };
        public override string Name
        {
            get => base.Name;
            set
            {
                if (channels != null)
                {
                    foreach (Channel channel in GetStandardChannels())
                    {
                        channel.Name = channel.Name.Replace(_name, value);
                    }
                }
                base.Name = value;
            }
        }

        public bool FileMode { get; set; } = false;
        public string FileModeFile { get; set; } = "";

        public InstrumentCache Cache { get; private set; }
		
        public string InstrumentType { get; protected set; }
        protected string dataFolder;
        protected string filePrefix;
        protected string fileSuffix;

        protected string[] dataFileNames;
        protected DateTime[] dataFileTimes;

        protected int numChannels;
        protected Channel[] channels = null;
        protected List<VirtualChannel> virtualChannels;

        protected string _fileExtension;
        public virtual string FileExtension
        {
            get { return _fileExtension; }
            set
            {
                _fileExtension = value;
            }
        }

        public bool IncludeSubDirectories { get; set; }
        public Instrument(DetectionSystem parent, string name, uint id) : base(parent, name, id)
        {
            parent.GetInstruments().Add(this);
            virtualChannels = new List<VirtualChannel>();
            dataFileNames = new string[0];
            dataFileTimes = new DateTime[0];
            Cache = new InstrumentCache(this, AppDataDirectory);
        }

        public void LoadVirtualChannels(ChannelCompartment compartment)
        {
            foreach (VirtualChannel chan in virtualChannels)
                chan.CalculateValues(compartment);
        }

        public List<string> GetSubdirectories(string directory)
        {
            List<string> directories = new List<string>();
            foreach(string d in Directory.GetDirectories(directory))
            {
                directories.Add(d);
                directories.AddRange(GetSubdirectories(d));
            }
            return directories;
        }

        public void ScanDataFolder()
        {
            // File Mode
            if (FileMode)
            {
                if (File.Exists(FileModeFile))
                { 
                    dataFileNames = new string[] { FileModeFile };
                    dataFileTimes = new DateTime[] { GetFileDate(FileModeFile) };
                }
                else
                {
                    dataFileNames = new string[0];
                    dataFileTimes = new DateTime[0];
                }
                Cache.SetDataFiles(dataFileNames, dataFileTimes);
                return;
            }

            if (string.IsNullOrEmpty(dataFolder) || !Directory.Exists(dataFolder)) return;
            List<string> dataFileList = new List<string>();
            List<DateTime> dataFileDateList = new List<DateTime>();
            DateTime fileDate;

            List<string> directories = new List<string>();
            directories.Add(dataFolder);
            if(IncludeSubDirectories)
            {
                directories.AddRange(GetSubdirectories(dataFolder));
            }

            // Try using the file names to get dates
            if (NameConvensionScan(directories)) return;

            string[] filesInDirectory;
            foreach (string directory in directories)
            {
                filesInDirectory = Directory.GetFiles(directory);

                foreach (string file in filesInDirectory)
                {
                    string fileAbrev = file.Substring(file.LastIndexOf('\\') + 1);
                    if (fileAbrev.Length > (fileSuffix.Length + FileExtension.Length)
                        && fileAbrev.Substring(fileAbrev.Length - (FileExtension.Length + 1)).ToLower() == ("." + FileExtension) 
                        && fileAbrev.ToLower().StartsWith(filePrefix.ToLower())
                        && fileAbrev.Substring(fileAbrev.Length - (FileExtension.Length + 1 + fileSuffix.Length), fileSuffix.Length).ToLower() == fileSuffix.ToLower())
                    {
                        fileDate = GetFileDate(file);
                        if (fileDate > DateTime.MinValue)
                        {
                            dataFileList.Add(file);
                            dataFileDateList.Add(fileDate);
                        }
                        else
                        {
                            // Something should really go here...
                        }
                    }
                }
            }
            dataFileNames = dataFileList.ToArray();
            dataFileTimes = dataFileDateList.ToArray();

            Array.Sort(dataFileTimes, dataFileNames);
            Cache.SetDataFiles(dataFileNames, dataFileTimes);
        }

        private bool NameConvensionScan(List<string> directories)
        {
            const int MIN_FILE_NAME_LENGTH = 6 + 4; 

            List<string> dataFileList = new List<string>();
            List<DateTime> dataFileDateList = new List<DateTime>();
            string[] filesInDirectory;
            string fileAbrev;

            int nameDateOffset = 0;
            bool usesDateSpacers = true;

            foreach (string directory in directories)
            {
                // Only one directory - just check file naming
                filesInDirectory = Directory.GetFiles(directory);
                foreach (string file in filesInDirectory)
                {
                    fileAbrev = file.Substring(file.LastIndexOf('\\') + 1);
                    if (fileAbrev.Length > (fileSuffix.Length + FileExtension.Length)
                        && fileAbrev.Substring(fileAbrev.Length - (FileExtension.Length + 1)).ToLower() == ("." + FileExtension)
                        && fileAbrev.ToLower().StartsWith(filePrefix.ToLower())
                        && fileAbrev.Substring(fileAbrev.Length - (FileExtension.Length + 1 + fileSuffix.Length), fileSuffix.Length).ToLower() == fileSuffix.ToLower())
                    {
                        dataFileList.Add(file);
                    }
                }
            }
            foreach (string file in dataFileList)
            {
                fileAbrev = file.Substring(file.LastIndexOf('\\') + 1);
                if (fileAbrev.Length < MIN_FILE_NAME_LENGTH) return false;

                DateTime fileDate;

                // Try current setting for nameDateOffset
                fileDate = ParseDateFromFileName(file, nameDateOffset, usesDateSpacers);

                if (fileDate > DateTime.MinValue)
                {
                    dataFileDateList.Add(fileDate);
                }
                else
                {
                    if (!SearchForDateInFileName(file, out nameDateOffset, out usesDateSpacers)) return false;
                    fileDate = ParseDateFromFileName(file, nameDateOffset, usesDateSpacers);
                    dataFileDateList.Add(fileDate);
                }
            }

            dataFileNames = dataFileList.ToArray();
            dataFileTimes = dataFileDateList.ToArray();

            Array.Sort(dataFileTimes, dataFileNames);
            Cache.SetDataFiles(dataFileNames, dataFileTimes);
            return true;
        }

        private DateTime ParseDateFromFileName(string file, int nameDateOffset, bool usesDateSpacers)
        {
            string fileAbrev = file.Substring(file.LastIndexOf('\\') + 1);
            int year = 0;
            int month = 0;
            int day = 0;
            bool foundDay = false;

            if (fileAbrev.Length < nameDateOffset + 10) return DateTime.MinValue;

            if (int.TryParse(fileAbrev.Substring(nameDateOffset, 4), out year) && year > 1900 && year < 3000)
            {
                if (usesDateSpacers)
                {
                    if (int.TryParse(fileAbrev.Substring(nameDateOffset + 5, 2), out month) && month > 0 && month < 13)
                    {
                        if (int.TryParse(fileAbrev.Substring(nameDateOffset + 8, 2), out day) && day > 0 && day < 32)
                        {
                            foundDay = true;
                        }
                    }
                }
                else
                {
                    if (int.TryParse(fileAbrev.Substring(nameDateOffset + 4, 2), out month) && month > 0 && month < 13)
                    {
                        if (int.TryParse(fileAbrev.Substring(nameDateOffset + 6, 2), out day) && day > 0 && day < 32)
                        {
                            foundDay = true;
                        }
                    }
                }
            }

            if (foundDay) return new DateTime(year, month, day);
            else return DateTime.MinValue;
        }

        private bool SearchForDateInFileName(string file, out int offset, out bool usesSpacers)
        {
            string fileAbrev = file.Substring(file.LastIndexOf('\\') + 1);
            int year;
            int month;
            int day;

            usesSpacers = true;
            for (int off = 0; off < fileAbrev.Length - 10; off++)
            {
                if (int.TryParse(fileAbrev.Substring(off, 4), out year) && year > 1900 && year < 3000)
                {
                    if (int.TryParse(fileAbrev.Substring(off + 5, 2), out month) && month > 0 && month < 13)
                    {
                        if (int.TryParse(fileAbrev.Substring(off + 8, 2), out day) && day > 0 && day < 32)
                        {
                            offset = off;
                            return true;
                        }
                    }
                }
            }

            usesSpacers = false;
            for (int off = 0; off < fileAbrev.Length - 10; off++)
            {
                if (int.TryParse(fileAbrev.Substring(off, 4), out year) && year > 1900 && year < 3000)
                {
                    if (int.TryParse(fileAbrev.Substring(off + 4, 2), out month) && month > 0 && month < 13)
                    {
                        if (int.TryParse(fileAbrev.Substring(off + 6, 2), out day) && day > 0 && day < 32)
                        {
                            offset = off;
                            return true;
                        }
                    }
                }
            }

            offset = 0;
            return false;
        }

        public abstract DateTime GetFileDate(string file);

        public DateTimeRange GetLoadedRange(ChannelCompartment compartment)
        {
            DateTimeRange range = new DateTimeRange(DateTime.MinValue, DateTime.MinValue);
            if (channels.Length > 0)
            {
                List<DateTime> times = channels[0].GetTimeStamps(compartment);
                if(times.Count > 0)
                {
                    range.Start = times[0];
                    range.End = times[times.Count - 1];
                }
            }
            return range;
        }

        public void RefreshData()
        {
            ClearData(ChannelCompartment.View);
            ClearData(ChannelCompartment.Cache);
            ClearData(ChannelCompartment.Process);
            Cache.UnloadAllDays();
            ScanDataFolder();
        }

        public virtual void LoadData(ChannelCompartment compartment, DateTime startDate, DateTime endDate)
        {
            ReturnCode returnCode = ReturnCode.SUCCESS;

            ///////////////////////////////////////////////////////////////////
            if (compartment == ChannelCompartment.View)
            {
                // Only make a load request if the data is out of range
                DateTimeRange loadedRange = GetLoadedRange(compartment);
                if (startDate < loadedRange.Start || endDate > loadedRange.End)
                {
                    Cache.LoadDataIntoInstrument(ChannelCompartment.View, new DateTimeRange(startDate, endDate));
                }
                return;
            }
            ///////////////////////////////////////////////////////////////////

            int startIndex = Array.FindIndex(dataFileTimes.ToArray(), x => x >= startDate.Date);
            int endIndex = Array.FindIndex(dataFileTimes.ToArray(), x => x >= endDate.Date.AddDays(1));

            if (startIndex == -1) return;
            if (endIndex == -1) endIndex = (dataFileTimes.Length) - 1;
            if (endIndex == -1) startIndex = 0;

            ClearData(compartment);
            for (int i = startIndex; i <= endIndex; ++i)
            {
                returnCode = IngestFile(compartment, dataFileNames[i]);
                
            }
            for (int c = 0; c < numChannels; c++)
            {
                channels[c].Sort(compartment);
            }
            LoadVirtualChannels(compartment);
        }
        public abstract ReturnCode IngestFile(ChannelCompartment compartment, string fileName);
        
        /// <summary>
        /// Automatically configure instrument to ingest file, and ingest file, if possible
        /// </summary>
        /// <param name="compartment"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual ReturnCode AutoIngestFile(ChannelCompartment compartment, string fileName)
        {
            return IngestFile(compartment, fileName);
        }
        public void ClearData(ChannelCompartment compartment)
        {
            foreach (Channel ch in GetChannels())
            {
                ch.ClearData(compartment);
            }
        }

        public void SetDataFolder(string newDataFolder)
        {
            dataFolder = newDataFolder;
        }

        public void SetFilePrefix(string newPrefix)
        {
            filePrefix = newPrefix.ToLower();
        }

        public void SetFileSuffix(string newSuffix)
        {
            fileSuffix = newSuffix.ToLower();
        }

        public string GetInstrumentType() { return InstrumentType; }
        public string GetDataFolder() { return dataFolder; }
        public string GetFilePrefix() { return filePrefix; }
        public string GetFileSuffix() { return fileSuffix; }
        public string[] GetDataFileNames() { return dataFileNames; }
        public DateTime[] GetDataFileDates() { return dataFileTimes; }
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
                new StringParameter("File Suffix") { Value = fileSuffix },
                new DirectoryParameter("Data Directory"){ Value = dataFolder },
                new BoolParameter("Include Subdirectories") {Value = IncludeSubDirectories ? 
                                                                BoolParameter.True : BoolParameter.False},
                new BoolParameter("File Mode") {Value = FileMode ?
                                                                BoolParameter.True : BoolParameter.False,
                    Visible = false},
                new StringParameter("File Mode File"){ Value = FileModeFile, Visible = false}
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
                    case "File Suffix":
                        if (param.Value is null) instrument.fileSuffix = "";
                        else instrument.fileSuffix = param.Value;
                        break;
                    case "Data Directory":
                        instrument.SetDataFolder(param.Value);
                        break;
                    case "Include Subdirectories":
                        instrument.IncludeSubDirectories = ((BoolParameter)param).ToBool();
                        break;
                    case "File Mode":
                        instrument.FileMode = ((BoolParameter)param).ToBool();
                        break;
                    case "File Mode File":
                        instrument.FileModeFile = param.Value;
                        break;
                }
            }
            //instrument.ScanDataFolder();
        }

        public override bool SetIndex(int index)
        {
            base.SetIndex(index);
            (Parent as DetectionSystem).GetInstruments().Remove(this);
            (Parent as DetectionSystem).GetInstruments().Insert(index, this);
            return true;
        }
        public override void Delete()
        {
            base.Delete();
            (Parent as DetectionSystem).GetInstruments().Remove(this);
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
            string name;
            uint id;
            Persister.StartFromXML(node, out name, out id);
            InstrumentHookup hookup = GetHookup(node.Attributes["Type"]?.InnerText);
            List<Parameter> parameters = Parameter.FromXML(node, hookup.TemplateParameters, system);
            return hookup?.FromParameters(system, name, parameters, id);
        }

        public override void ToXML(XmlWriter xmlWriter)
        {
            StartToXML(xmlWriter);
            xmlWriter.WriteAttributeString("Type", GetInstrumentType());
            List<Parameter> parameters = GetParameters();

            foreach (Parameter param in parameters)
            {
                xmlWriter.WriteAttributeString(param.Name.Replace(' ', '_'), param.Value);
            }
            foreach (Channel chan in GetStandardChannels())
            {
                chan.ToXML(xmlWriter);
            }
            foreach (VirtualChannel chan in GetVirtualChannels())
            {
                chan.ToXML(xmlWriter);
            }
            xmlWriter.WriteEndElement();
        }
    }

    public abstract class InstrumentHookup
    {
        public abstract Instrument FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id);
        public abstract string Type { get; }
        public List<ParameterTemplate> TemplateParameters { get; set; } = new List<ParameterTemplate>()
        {
            new ParameterTemplate("File Prefix", ParameterType.String),
            new ParameterTemplate("File Suffix", ParameterType.String),
            new ParameterTemplate("Data Directory", ParameterType.Directory),
            new ParameterTemplate("Include Subdirectories", ParameterType.Bool),
            new ParameterTemplate("File Mode", ParameterType.Bool, false),
            new ParameterTemplate("File Mode File", ParameterType.String, false)
        };
    }
}
