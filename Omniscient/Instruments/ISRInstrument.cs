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

        public ISRInstrument(DetectionSystem parent, string name, uint id) : base(parent, name, id)
        {
            InstrumentType = "ISR";
            FileExtension = FILE_EXTENSION;
            filePrefix = "";
            isrParser = new ISRParser();

            numChannels = NUM_CHANNELS;
            channels = new Channel[numChannels];
            channels[TOTALS1] = new Channel(Name+"-Totals-1", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[TOTALS2] = new Channel(Name + "-Totals-2", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[TOTALS3] = new Channel(Name + "-Totals-3", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[REALS_PLUS_ACC] = new Channel(Name + "-Real+Acc", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[ACC] = new Channel(Name + "-Acc", this, Channel.ChannelType.COUNT_RATE, 0);
        }

        public override DateTime GetFileDate(string file)
        {
            if (isrParser.ParseHeader(file) == ReturnCode.SUCCESS)
            {
                return isrParser.GetDate();
            }
            return DateTime.MinValue;
        }

        public override ReturnCode IngestFile(ChannelCompartment compartment, string fileName)
        {
            ReturnCode returnCode = isrParser.ParseFile(fileName);
            DataFile dataFile = new DataFile(fileName, isrParser.GetDate());
            int numRecords = isrParser.GetNumRecords();
            DateTime time = DateTime.MinValue;
            for (int r = 0; r < numRecords; ++r)
            {
                time = isrParser.ISRTimeToDateTime(isrParser.GetRecord(r).time);
                channels[TOTALS1].AddDataPoint(compartment, time, isrParser.GetRecord(r).totals1, dataFile);
                channels[TOTALS2].AddDataPoint(compartment, time, isrParser.GetRecord(r).totals2, dataFile);
                channels[TOTALS3].AddDataPoint(compartment, time, isrParser.GetRecord(r).totals3, dataFile);
                channels[REALS_PLUS_ACC].AddDataPoint(compartment, time, isrParser.GetRecord(r).realsPlusAccidentals, dataFile);
                channels[ACC].AddDataPoint(compartment, time, isrParser.GetRecord(r).accidentals, dataFile);
            }
            dataFile.DataEnd = time;

            isrParser = new ISRParser();

            return ReturnCode.SUCCESS;
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

        public override Instrument FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            ISRInstrument instrument = new ISRInstrument(parent, newName, id);
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
