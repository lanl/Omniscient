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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Omniscient
{
    public class LDAQInstrument : Instrument
    {
        private const string FILE_EXTENSION = "json";
        private const int NUM_CHANNELS = 6;

        private const int FRONT_RIGHT_COUNTS = 0;
        private const int FRONT_LEFT_COUNTS = 1;
        private const int BACK_RIGHT_COUNTS = 2;
        private const int BACK_LEFT_COUNTS = 3;

        private const int GROSS_WEIGHT = 4;
        private const int NET_WEIGHT = 5;



        LDAQParser isrParser;

        public LDAQInstrument(DetectionSystem parent, string name, uint id) : base(parent, name, id)
        {
            InstrumentType = "LDAQ";
            FileExtension = FILE_EXTENSION;
            filePrefix = "";
            fileSuffix = "";
            ldaqParser = new LDAQParser();

            numChannels = NUM_CHANNELS;  //?
            channels = new Channel[numChannels];
            //todo
            //fix channeltypes in Channels.cs
            channels[FRONT_RIGHT_COUNTS] = new Channel(Name + "-Front-Right-Counts", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[FRONT_LEFT_COUNTS] = new Channel(Name + "-Front-Left-Counts", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[BACK_RIGHT_COUNTS] = new Channel(Name + "-Back-Right-Counts", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[BACK_LEFT_COUNTS] = new Channel(Name + "-Backing-Left-Counts", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[GROSS_WEIGHT] = new Channel(Name + "-Gross-Weight", this, Channel.ChannelType.WEIGHT, 0);
            channels[NET_WEIGHT] = new Channel(Name + "-Net-Weight", this, Channel.ChannelType.WEIGHT, 0);

        }

        public override DateTime GetFileDate(string file)
        {
            if (ldaqParser.ParseHeader(file) == ReturnCode.SUCCESS)
            {
                return ldaqParser.GetDate();
            }
            return DateTime.MinValue;
        }

        public override ReturnCode IngestFile(ChannelCompartment compartment, string fileName)
        {
            ReturnCode returnCode = ldaqParser.ParseFile(fileName);
            DataFile dataFile = new DataFile(fileName, ldaqParser.GetDate());
            int numRecords = ldaqParser.GetNumRecords();
            DateTime time = DateTime.MinValue;
            DateTime[] times = new DateTime[numRecords];
            double[] T1 = new double[numRecords];
            double[] T2 = new double[numRecords];
            double[] T3 = new double[numRecords];
            double[] RA = new double[numRecords];
            double[] A = new double[numRecords];
            DataFile[] dataFiles = new DataFile[numRecords];
            for (int r = 0; r < numRecords; ++r) dataFiles[r] = dataFile;
            LDAQRecord record;
            for (int r = 0; r < numRecords; ++r)
            {
                record = ldaqParser.GetRecord(r);
                time = times[r] = ldaqParser.LDAQTimeToDateTime(record.time);
                T1[r] = record.totals1;
                T2[r] = record.totals2;
                T3[r] = record.totals3;
                RA[r] = record.realsPlusAccidentals;
                A[r] = record.accidentals;
            }
            channels[TOTALS1].AddDataPoints(compartment, times, T1, dataFiles);
            channels[TOTALS2].AddDataPoints(compartment, times, T2, dataFiles);
            channels[TOTALS3].AddDataPoints(compartment, times, T3, dataFiles);
            channels[REALS_PLUS_ACC].AddDataPoints(compartment, times, RA, dataFiles);
            channels[ACC].AddDataPoints(compartment, times, A, dataFiles);
            dataFile.DataEnd = time;

            ldaqParser = new LDAQParser();

            return ReturnCode.SUCCESS;
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = GetStandardInstrumentParameters();
            parameters.Add(new EnumParameter("File Extension")
            {
                Value = FileExtension,
                ValidValues = { "ldaq", "jsr", "hmr" }
            });
            return parameters;
        }
        public override void ApplyParameters(List<Parameter> parameters)
        {
            ApplyStandardInstrumentParameters(this, parameters);
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "File Extension":
                        FileExtension = param.Value;
                        break;
                }
            }
        }
    }

    public class LDAQInstrumentHookup : InstrumentHookup
    {
        public LDAQInstrumentHookup()
        {
            TemplateParameters.Add(new ParameterTemplate("File Extension", ParameterType.Enum)
            {
                ValidValues = { "ldaq", "jsr", "hmr" }
            });
        }

        public override string Type { get { return "LDAQ"; } }

        public override Instrument FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            LDAQInstrument instrument = new LDAQInstrument(parent, newName, id);
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
