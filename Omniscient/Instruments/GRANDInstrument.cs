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
    class GRANDInstrument : Instrument
    {
        private const string FILE_EXTENSION = "bid";

        private const int NUM_CHANNELS = 7;
        private const int chACountRate = 0;
        private const int chBCountRate = 1;
        private const int chCCountRate = 2;
        private const int gamInGamCh1 = 3;
        private const int gamCh1Sigma = 4;
        private const int gamInGamCh2 = 5;
        private const int gamCh2Sigma = 6;

        BIDParser bidParser;

        public GRANDInstrument(DetectionSystem parent, string name, uint id) : base(parent, name, id)
        {
            InstrumentType = "GRAND";
            FileExtension = FILE_EXTENSION;
            filePrefix = "";
            fileSuffix = "";
            FileExtension = "bid";
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
            DataFile dataFile = new DataFile(fileName, bidParser.GetDate());
            int numRecords = bidParser.GetNumRecords();
            DateTime time = DateTime.MinValue;
            for (int r = 0; r < numRecords; ++r)
            {
                time = bidParser.BIDTimeToDateTime(bidParser.GetRecord(r).time);
                channels[chACountRate].AddDataPoint(compartment, time, bidParser.GetRecord(r).chACountRate, dataFile);
                channels[chBCountRate].AddDataPoint(compartment, time, bidParser.GetRecord(r).chBCountRate, dataFile);
                channels[chCCountRate].AddDataPoint(compartment, time, bidParser.GetRecord(r).chCCountRate, dataFile);
                channels[gamInGamCh1].AddDataPoint(compartment, time, bidParser.GetRecord(r).gamInGamCh1, dataFile);
                channels[gamCh1Sigma].AddDataPoint(compartment, time, bidParser.GetRecord(r).gamCh1Sigma, dataFile);
                channels[gamInGamCh2].AddDataPoint(compartment, time, bidParser.GetRecord(r).gamInGamCh2, dataFile);
                channels[gamCh2Sigma].AddDataPoint(compartment, time, bidParser.GetRecord(r).gamCh2Sigma, dataFile);
            }

            dataFile.DataEnd = time;

            bidParser = new BIDParser();

            return ReturnCode.SUCCESS;
        }

        public override List<Parameter> GetParameters()
        {
            return GetStandardInstrumentParameters();
        }
        public override void ApplyParameters(List<Parameter> parameters)
        {
            ApplyStandardInstrumentParameters(this, parameters);
        }
    }

    public class GRANDInstrumentHookup : InstrumentHookup
    {
        public GRANDInstrumentHookup() { }

        public override string Type { get { return "GRAND"; } }

        public override Instrument FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            GRANDInstrument instrument = new GRANDInstrument(parent, newName, id);
            Instrument.ApplyStandardInstrumentParameters(instrument, parameters);
            return instrument;
        }
    }
}
