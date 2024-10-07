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
    public class JSONInstrument : Instrument
    {
        //todo: make dynamic
        private const string FILE_EXTENSION = "json";
        private const int NUM_CHANNELS = 12;

        private const int FRONT_RIGHT_COUNTS_30B = 0;
        private const int FRONT_LEFT_COUNTS_30B = 1;
        private const int BACK_RIGHT_COUNTS_30B = 2;
        private const int BACK_LEFT_COUNTS_30B = 3;
        private const int GROSS_WEIGHT_30B = 4;
        private const int NET_WEIGHT_30B = 5;

        private const int FRONT_RIGHT_COUNTS_48Y = 6;
        private const int FRONT_LEFT_COUNTS_48Y = 7;
        private const int BACK_RIGHT_COUNTS_48Y = 8;
        private const int BACK_LEFT_COUNTS_48Y = 9;
        private const int GROSS_WEIGHT_48Y = 10;
        private const int NET_WEIGHT_48Y = 11;

        JSONParser jsonParser;

        private int _numberOfAttributes;
        public int NumberOfAttributes
        {
            get { return _numberOfAttributes; }
            set
            {

            _numberOfAttributes = value;
                if (jsonParser != null) jsonParser.NumberOfAttributes = value; 
            }
        }
        

        

        public JSONInstrument(DetectionSystem parent, string name, uint id) : base(parent, name, id)
        {
            InstrumentType = "JSON";
            FileExtension = FILE_EXTENSION;
            NumberOfAttributes = 0;
            filePrefix = "";
            fileSuffix = "";
            jsonParser = new JSONParser();

            numChannels = NUM_CHANNELS;
            channels = new Channel[numChannels];
            channels[FRONT_RIGHT_COUNTS_30B] = new Channel(Name+"-Front-Right_Counts-30b", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[FRONT_LEFT_COUNTS_30B] = new Channel(Name + "-Front-Left-Counts-30b", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[BACK_RIGHT_COUNTS_30B] = new Channel(Name + "-Back-Right-Counts-30b", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[BACK_LEFT_COUNTS_30B] = new Channel(Name + "-Back-Left-Counts-30b", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[GROSS_WEIGHT_30B] = new Channel(Name + "-Gross-Weight-30b", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[NET_WEIGHT_30B] = new Channel(Name + "-Net-Weight-30b", this, Channel.ChannelType.COUNT_RATE, 0);

            channels[FRONT_RIGHT_COUNTS_48Y] = new Channel(Name + "-Front-Right_Counts-48y", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[FRONT_LEFT_COUNTS_48Y] = new Channel(Name + "-Front-Left-Counts-48y", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[BACK_RIGHT_COUNTS_48Y] = new Channel(Name + "-Back-Right-Counts-48y", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[BACK_LEFT_COUNTS_48Y] = new Channel(Name + "-Back-Left-Counts-48y", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[GROSS_WEIGHT_48Y] = new Channel(Name + "-Gross-Weight-48y", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[NET_WEIGHT_48Y] = new Channel(Name + "-Net-Weight-48y", this, Channel.ChannelType.COUNT_RATE, 0);
        }

        public override DateTime GetFileDate(string file)
        {
            //todo: make work for json
            if (jsonParser.ParseHeader(file) == ReturnCode.SUCCESS)
            {
                return jsonParser.GetDate();
            }
            return DateTime.MinValue;
        }

        public override ReturnCode IngestFile(ChannelCompartment compartment, string fileName)
        {
            ReturnCode returnCode = jsonParser.ParseFile(fileName);
            
            DataFile dataFile = new DataFile(fileName, jsonParser.GetDate());
            int numRecords = jsonParser.GetNumRecords();
            DateTime time = DateTime.MinValue;
            DateTime[] times = new DateTime[numRecords];
            double[] T1 = new double[numRecords];
            double[] T2 = new double[numRecords];
            double[] T3 = new double[numRecords];
            double[] RA = new double[numRecords];
            double[] A = new double[numRecords];
            DataFile[] dataFiles = new DataFile[numRecords];
            for (int r = 0; r < numRecords; ++r) dataFiles[r] = dataFile;
            JSONRecord record;
            for (int r = 0; r < numRecords; ++r)
            {
                record = jsonParser.GetRecord(r);
                time = times[r] = jsonParser.JSONTimeToDateTime(record.time);
                //todo
                T1[r] = record.totals1;
                T2[r] = record.totals2;
                T3[r] = record.totals3;
                RA[r] = record.realsPlusAccidentals;
                A[r] = record.accidentals;
            }
            //todo
            //fix
            //channels[TOTALS1].AddDataPoints(compartment, times, T1, dataFiles);
            //channels[TOTALS2].AddDataPoints(compartment, times, T2, dataFiles);
            //channels[TOTALS3].AddDataPoints(compartment, times, T3, dataFiles);
            //channels[REALS_PLUS_ACC].AddDataPoints(compartment, times, RA, dataFiles);
            //channels[ACC].AddDataPoints(compartment, times, A, dataFiles);

            channels[FRONT_RIGHT_COUNTS_30B].AddDataPoints(compartment, times, T1, dataFiles);
            channels[FRONT_LEFT_COUNTS_30B] = new Channel(Name + "-Front-Left-Counts-30b", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[BACK_RIGHT_COUNTS_30B] = new Channel(Name + "-Back-Right-Counts-30b", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[BACK_LEFT_COUNTS_30B] = new Channel(Name + "-Back-Left-Counts-30b", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[GROSS_WEIGHT_30B] = new Channel(Name + "-Gross-Weight-30b", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[NET_WEIGHT_30B] = new Channel(Name + "-Net-Weight-30b", this, Channel.ChannelType.COUNT_RATE, 0);

            channels[FRONT_RIGHT_COUNTS_48Y] = new Channel(Name + "-Front-Right_Counts-48y", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[FRONT_LEFT_COUNTS_48Y] = new Channel(Name + "-Front-Left-Counts-48y", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[BACK_RIGHT_COUNTS_48Y] = new Channel(Name + "-Back-Right-Counts-48y", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[BACK_LEFT_COUNTS_48Y] = new Channel(Name + "-Back-Left-Counts-48y", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[GROSS_WEIGHT_48Y] = new Channel(Name + "-Gross-Weight-48y", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[NET_WEIGHT_48Y] = new Channel(Name + "-Net-Weight-48y", this, Channel.ChannelType.COUNT_RATE, 0);

            dataFile.DataEnd = time;

            jsonParser = new JSONParser();

            return ReturnCode.SUCCESS;
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = GetStandardInstrumentParameters();
            parameters.Add(new EnumParameter("File Extension")
            {
                Value = FileExtension,
                ValidValues = {"json"}
            });
            parameters.Add(new IntParameter("Number of Attributes")
            {
                Value = NumberOfAttributes.ToString()

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

    public class JSONInstrumentHookup : InstrumentHookup
    {
        public JSONInstrumentHookup()
        {
            TemplateParameters.Add(new ParameterTemplate("File Extension", ParameterType.Enum)
            {
                ValidValues = {"json"}
            });
            TemplateParameters.Add(new ParameterTemplate("Number of Attributes", ParameterType.Enum)
            {
                ValidValues = { "json" }
            });
        }

        public override string Type { get { return "JSON"; } }

        public override Instrument FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            string fileExtension = "json";
            int nAttributes = 0;
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "File Extension":
                        fileExtension = param.Value;
                        break;
                    case "Number of Attributes":
                        //todo: im not sure
                        //nAttributes = ((IntParameter)param).ToInt();
                        break;
                }
            }

            JSONInstrument instrument = new JSONInstrument(parent, newName, id);
            instrument.FileExtension = fileExtension;
            instrument.NumberOfAttributes = nAttributes;
            Instrument.ApplyStandardInstrumentParameters(instrument, parameters);

            return instrument;
        }
    }
}
