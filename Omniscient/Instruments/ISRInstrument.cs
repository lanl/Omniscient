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
using System.IO;

namespace Omniscient
{
    public class ISRInstrument : Instrument
    {
        private const string FILE_EXTENSION = "isr";
        private const int NUM_CHANNELS = 5;
        private const int TOTALS1 = 0;
        private const int TOTALS2 = 1;
        private const int TOTALS3 = 2;
        private const int REALS_PLUS_ACC = 3;
        private const int ACC = 4;

        ISRParser isrParser;

        string[] isrFiles;
        DateTime[] isrDates;

        public ISRInstrument(string newName) : base(newName)
        {
            InstrumentType = "ISR";
            FileExtension = FILE_EXTENSION;
            filePrefix = "";
            isrFiles = new string[0];
            isrDates = new DateTime[0];
            isrParser = new ISRParser();

            numChannels = NUM_CHANNELS;
            channels = new Channel[numChannels];
            channels[TOTALS1] = new Channel(name+"-Totals-1", this, Channel.ChannelType.COUNT_RATE);
            channels[TOTALS2] = new Channel(name + "-Totals-2", this, Channel.ChannelType.COUNT_RATE);
            channels[TOTALS3] = new Channel(name + "-Totals-3", this, Channel.ChannelType.COUNT_RATE);
            channels[REALS_PLUS_ACC] = new Channel(name + "-Real+Acc", this, Channel.ChannelType.COUNT_RATE);
            channels[ACC] = new Channel(name + "-Acc", this, Channel.ChannelType.COUNT_RATE);
        }

        public override void SetName(string newName)
        {
            name = newName;
            channels[TOTALS1].SetName(name + "-Totals-1");
            channels[TOTALS2].SetName(name + "-Totals-2");
            channels[TOTALS3].SetName(name + "-Totals-3");
            channels[REALS_PLUS_ACC].SetName(name + "-Real+Acc");
            channels[ACC].SetName(name + "-Acc");
        }

        public override void ScanDataFolder()
        {
            List<string> isrFileList = new List<string>();
            List<DateTime> isrDateList = new List<DateTime>();

            string[] filesInDirectory = Directory.GetFiles(dataFolder);
            foreach(string file in filesInDirectory)
            {
                string fileAbrev = file.Substring(file.LastIndexOf('\\') + 1);
                if (fileAbrev.Substring(fileAbrev.Length - 4).ToLower() == ("." + FileExtension) && fileAbrev.ToLower().StartsWith(filePrefix.ToLower()))
                {
                    if (isrParser.ParseHeader(file) == ReturnCode.SUCCESS)
                    {
                        isrFileList.Add(file);
                        isrDateList.Add(isrParser.GetDate());
                    }
                    else
                    {
                        // Something should really go here...
                    }
                }
            }

            isrFiles = isrFileList.ToArray();
            isrDates = isrDateList.ToArray();

            Array.Sort(isrDates, isrFiles);
        }

        public override void LoadData(DateTime startDate, DateTime endDate)
        {
            ReturnCode returnCode = ReturnCode.SUCCESS;

            int startIndex = Array.FindIndex(isrDates.ToArray(), x => x >= startDate);
            int endIndex = Array.FindIndex(isrDates.ToArray(), x => x >= endDate);

            if (endIndex == -1) endIndex = (isrDates.Length) - 1;

            DateTime time;
            DataFile dataFile;
            for (int i=startIndex; i<=endIndex; ++i)
            {
                returnCode = isrParser.ParseFile(isrFiles[i]);
                dataFile = new DataFile(isrFiles[i]);
                int numRecords = isrParser.GetNumRecords();
                for (int r=0; r < numRecords; ++r)
                {
                    time = isrParser.ISRTimeToDateTime(isrParser.GetRecord(r).time);
                    channels[TOTALS1].AddDataPoint(time, isrParser.GetRecord(r).totals1, dataFile);
                    channels[TOTALS2].AddDataPoint(time, isrParser.GetRecord(r).totals2, dataFile);
                    channels[TOTALS3].AddDataPoint(time, isrParser.GetRecord(r).totals3, dataFile);
                    channels[REALS_PLUS_ACC].AddDataPoint(time, isrParser.GetRecord(r).realsPlusAccidentals, dataFile);
                    channels[ACC].AddDataPoint(time, isrParser.GetRecord(r).accidentals, dataFile);
                }
            }
            channels[TOTALS1].Sort();
            channels[TOTALS2].Sort();
            channels[TOTALS3].Sort();
            channels[REALS_PLUS_ACC].Sort();
            channels[ACC].Sort();
            LoadVirtualChannels();
        }

        public override void ClearData()
        {
            foreach (Channel ch in channels)
            {
                ch.ClearData();
            }
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = GetStandardInstrumentParameters();
            parameters.Add(new EnumParameter("File Extension")
            {
                Value = FileExtension,
                ValidValues = {"isr", "jsr", "hmr"}
            });
            return parameters;
        }
    }

    public class ISRInstrumentHookup : InstrumentHookup
    {
        public ISRInstrumentHookup()
        {
            TemplateParameters.Add(new ParameterTemplate("File Extension", ParameterType.Enum)
            {
                ValidValues = {"isr", "jsr", "hmr"}
            });
        }

        public override string Type { get { return "ISR"; } }

        public override Instrument FromParameters(string newName, List<Parameter> parameters)
        {
            ISRInstrument instrument = new ISRInstrument(newName);
            Instrument.ApplyStandardInstrumentParameters(instrument, parameters);
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "File Extension":
                        instrument.FileExtension = param.Value;
                        break;
                }
            }
            return instrument;
        }
    }
}
