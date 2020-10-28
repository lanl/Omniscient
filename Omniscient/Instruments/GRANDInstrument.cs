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
    class GRANDInstrument : Instrument
    {
        private const string FILE_EXTENSION = "bid";

        private const int NUM_CHANNELS = 8;
        private const int chACountRate = 0;
        private const int chBCountRate = 1;
        private const int chCCountRate = 2;
        private const int gamInGamCh1 = 3;
        private const int gamCh1Sigma = 4;
        private const int gamInGamCh2 = 5;
        private const int gamCh2Sigma = 6;
        private const int statCh = 7;

        BIDParser bidParser;

        public bool HideFlags { get; set; }

        public GRANDInstrument(DetectionSystem parent, string name, uint id) : base(parent, name, id)
        {
            InstrumentType = "GRAND";
            FileExtension = FILE_EXTENSION;
            filePrefix = "";
            fileSuffix = "";
            FileExtension = "bid";
            HideFlags = false;
            bidParser = new BIDParser();

            numChannels = NUM_CHANNELS;
            channels = new Channel[numChannels];
            channels[chACountRate] = new Channel(Name + "-Neutrons-A", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[chBCountRate] = new Channel(Name + "-Neutrons-B", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[chCCountRate] = new Channel(Name + "-Neutrons-C", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[gamInGamCh1] = new Channel(Name + "-Gammas-1", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[gamCh1Sigma] = new Channel(Name + "-Gammas-1-Sigma", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[gamInGamCh2] = new Channel(Name + "-Gammas-2", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[gamCh2Sigma] = new Channel(Name + "-Gammas-2-Sigma", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[statCh] = new Channel(Name + "-Status", this, Channel.ChannelType.COUNT_RATE, 0);

        }

        public override DateTime GetFileDate(string file)
        {
            if (bidParser.ParseHeader(file) == ReturnCode.SUCCESS)
            {
                return bidParser.GetDate();
            }
            return DateTime.MinValue;
        }

        public override ReturnCode IngestFile(ChannelCompartment compartment, string fileName)
        {
            ReturnCode returnCode = bidParser.ParseFile(fileName);
            if (returnCode != ReturnCode.SUCCESS) return returnCode;

            DataFile dataFile = new DataFile(fileName, bidParser.GetDate());
            int numRecords = bidParser.GetNumRecords();
            DateTime time = DateTime.MinValue;
            DateTime[] times = new DateTime[numRecords];
            double[] A = new double[numRecords];
            double[] B = new double[numRecords];
            double[] C = new double[numRecords];
            double[] G1 = new double[numRecords];
            double[] G1s = new double[numRecords];
            double[] G2 = new double[numRecords];
            double[] G2s = new double[numRecords];
            double[] stat = new double[numRecords];

            BIDRecord record;
            DataFile[] dataFiles = new DataFile[numRecords];
            for (int r = 0; r < numRecords; ++r) dataFiles[r] = dataFile;
            if (HideFlags)
            { 
                float lastGam1 = 994000;
                float lastGam1Sigma = 9940;
                float lastGam2 = 994000;
                float lastGam2Sigma = 9940;
                for (int r = 0; r < numRecords; ++r)
                {
                    record = bidParser.GetRecord(r);
                    time = times[r] = bidParser.BIDTimeToDateTime(record.time);
                    A[r] = record.chACountRate;
                    B[r] = record.chACountRate;
                    C[r] = record.chACountRate;
                    
                    if (record.gamInGamCh1 > 990000 && record.gamCh1Sigma > 9900)
                    {
                        G1[r] = lastGam1;
                        G1s[r] = lastGam1Sigma;
                    }
                    else
                    {
                        G1[r] = record.gamInGamCh1;
                        G1s[r] = record.gamCh1Sigma;
                        lastGam1 = record.gamInGamCh1;
                        lastGam1Sigma = record.gamCh1Sigma;
                    }
                    if (record.gamInGamCh2 > 990000 && record.gamCh2Sigma > 9900)
                    {
                        G2[r] = lastGam2;
                        G2s[r] = lastGam2Sigma;
                    }
                    else
                    {
                        G2[r] = record.gamInGamCh2;
                        G2s[r] = record.gamCh2Sigma;
                        lastGam2 = record.gamInGamCh2;
                        lastGam2Sigma = record.gamCh2Sigma;
                    }
                    stat[r] = record.status;
                }
            }
            else
            {
                for (int r = 0; r < numRecords; ++r)
                {
                    record = bidParser.GetRecord(r);
                    time = times[r] = bidParser.BIDTimeToDateTime(record.time);
                    A[r] = record.chACountRate;
                    B[r] = record.chACountRate;
                    C[r] = record.chACountRate;
                    G1[r] = record.gamInGamCh1;
                    G1s[r] = record.gamCh1Sigma;
                    G2[r] = record.gamInGamCh2;
                    G2s[r] = record.gamCh2Sigma;
                    stat[r] = record.status;
                }
            }
            channels[chACountRate].AddDataPoints(compartment, times, A, dataFiles);
            channels[chBCountRate].AddDataPoints(compartment, times, B, dataFiles);
            channels[chCCountRate].AddDataPoints(compartment, times, C, dataFiles);
            channels[gamInGamCh1].AddDataPoints(compartment, times, G1, dataFiles);
            channels[gamCh1Sigma].AddDataPoints(compartment, times, G1s, dataFiles);
            channels[gamInGamCh2].AddDataPoints(compartment, times, G2, dataFiles);
            channels[gamCh2Sigma].AddDataPoints(compartment, times, G2s, dataFiles);
            channels[statCh].AddDataPoints(compartment, times, stat, dataFiles);

            dataFile.DataEnd = time;
            bidParser = new BIDParser();
            return ReturnCode.SUCCESS;
        }

        public override ReturnCode AutoIngestFile(ChannelCompartment compartment, string fileName)
        {
            if (fileName.Substring(fileName.Length - 4).ToLower() != ".bid") return ReturnCode.BAD_INPUT;
            return base.AutoIngestFile(compartment, fileName);
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = GetStandardInstrumentParameters();
            parameters.Add(new BoolParameter("Hide Flags")
            {
                Value = HideFlags ? BoolParameter.True : BoolParameter.False
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
                    case "Hide Flags":
                        HideFlags = (param as BoolParameter).ToBool();
                        break;
                }
            }
        }
    }

    public class GRANDInstrumentHookup : InstrumentHookup
    {
        public GRANDInstrumentHookup() 
        {
            TemplateParameters.Add(new ParameterTemplate("Hide Flags", ParameterType.Bool));
        }

        public override string Type { get { return "GRAND"; } }

        public override Instrument FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            GRANDInstrument instrument = new GRANDInstrument(parent, newName, id);
            Instrument.ApplyStandardInstrumentParameters(instrument, parameters);
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Hide Flags":
                        instrument.HideFlags = (param as BoolParameter).ToBool();
                        break;
                }
            }
            return instrument;
        }
    }
}
