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
    class NGAMInstrument : Instrument
    {
        private const string FILE_EXTENSION = "vbf";

        private const int NUM_CHANNELS = 8;
        private const int data0 = 0;
        private const int data1 = 1;
        private const int data2 = 2;
        private const int data3 = 3;
        private const int data4 = 4;
        private const int data5 = 5;
        private const int data6 = 6;
        private const int data7 = 7;

        VBFParser vbfParser;

        public override string FileExtension
        {
            get { return _fileExtension; }
            set
            {
                if (value == "vbf")
                {
                    _fileExtension = value;
                }
                else if (value == "adc")
                {
                    _fileExtension = value;
                }
                else
                    throw new ArgumentException("File extension must be vbf or adc!");
                //ScanDataFolder();
            }
        }

        public NGAMInstrument(DetectionSystem parent, string name, uint id) : base(parent, name, id)
        {
            InstrumentType = "NGAM";
            FileExtension = FILE_EXTENSION;
            filePrefix = "";
            fileSuffix = "";
            vbfParser = new VBFParser();

            numChannels = NUM_CHANNELS;
            channels = new Channel[numChannels];
            channels[data0] = new Channel(Name + "-data0", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[data1] = new Channel(Name + "-data1", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[data2] = new Channel(Name + "-data2", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[data3] = new Channel(Name + "-data3", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[data4] = new Channel(Name + "-data4", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[data5] = new Channel(Name + "-data5", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[data6] = new Channel(Name + "-data6", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[data7] = new Channel(Name + "-data7", this, Channel.ChannelType.COUNT_RATE, 0);
        }

        public override DateTime GetFileDate(string file)
        {
            if (vbfParser.ParseHeader(file) == ReturnCode.SUCCESS)
            {
                return vbfParser.GetDate();
            }
            return DateTime.MinValue;
        }

        public override ReturnCode IngestFile(ChannelCompartment compartment, string fileName)
        {
            ReturnCode returnCode = vbfParser.ParseFile(fileName);
            DataFile dataFile = new DataFile(fileName, vbfParser.GetDate());
            DateTime time = DateTime.MinValue;
            int numRecords = vbfParser.GetNumRecords();
            DateTime[] times = new DateTime[numRecords];
            double[] d0 = new double[numRecords];
            double[] d1 = new double[numRecords];
            double[] d2 = new double[numRecords];
            double[] d3 = new double[numRecords];
            double[] d4 = new double[numRecords];
            double[] d5 = new double[numRecords];
            double[] d6 = new double[numRecords];
            double[] d7 = new double[numRecords];
            DataFile[] dataFiles = new DataFile[numRecords];
            for (int r = 0; r < numRecords; ++r) dataFiles[r] = dataFile;
            VBFRecord record;
            for (int r = 0; r < numRecords; ++r)
            {
                record = vbfParser.GetRecord(r);
                time = times[r] = vbfParser.VBFTimeToDateTime(record.time);
                d0[r] = record.data[0];
                d1[r] = record.data[1];
                d2[r] = record.data[2];
                d3[r] = record.data[3];
                d4[r] = record.data[4];
                d5[r] = record.data[5];
                d6[r] = record.data[6];
                d7[r] = record.data[7];
            }
            channels[data0].AddDataPoints(compartment, times, d0, dataFiles);
            channels[data1].AddDataPoints(compartment, times, d1, dataFiles);
            channels[data2].AddDataPoints(compartment, times, d2, dataFiles);
            channels[data3].AddDataPoints(compartment, times, d3, dataFiles);
            channels[data4].AddDataPoints(compartment, times, d4, dataFiles);
            channels[data5].AddDataPoints(compartment, times, d5, dataFiles);
            channels[data6].AddDataPoints(compartment, times, d6, dataFiles);
            channels[data7].AddDataPoints(compartment, times, d7, dataFiles);

            dataFile.DataEnd = time;
            vbfParser = new VBFParser();
            return ReturnCode.SUCCESS;
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = GetStandardInstrumentParameters();
            parameters.Add(new EnumParameter("File Extension")
            {
                Value = FileExtension,
                ValidValues = { "vbf", "adc" }
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
                        if (!(param.Value is null)) FileExtension = param.Value;
                        break;
                }
            }
        }
    }

    public class NGAMInstrumentHookup : InstrumentHookup
    {
        public NGAMInstrumentHookup() 
        {
            TemplateParameters.Add(new ParameterTemplate("File Extension", ParameterType.Enum)
            {
                ValidValues = { "vbf", "adc" }
            });
        }

        public override string Type { get { return "NGAM"; } }

        public override Instrument FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            NGAMInstrument instrument = new NGAMInstrument(parent, newName, id);
            Instrument.ApplyStandardInstrumentParameters(instrument, parameters);
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "File Extension":
                        if (!(param.Value is null)) instrument.FileExtension = param.Value;
                        break;
                }
            }
            return instrument;
        }
    }
}
