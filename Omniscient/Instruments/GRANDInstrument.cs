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

        string[] bidFiles;
        DateTime[] bidDates;

        public GRANDInstrument(string newName) : base(newName)
        {
            InstrumentType = "GRAND";
            FileExtension = FILE_EXTENSION;
            filePrefix = "";
            bidFiles = new string[0];
            bidDates = new DateTime[0];
            bidParser = new BIDParser();

            numChannels = NUM_CHANNELS;
            channels = new Channel[numChannels];
            channels[chACountRate] = new Channel(name + "-Neutrons-A", this, Channel.ChannelType.COUNT_RATE);
            channels[chBCountRate] = new Channel(name + "-Neutrons-B", this, Channel.ChannelType.COUNT_RATE);
            channels[chCCountRate] = new Channel(name + "-Neutrons-C", this, Channel.ChannelType.COUNT_RATE);
            channels[gamInGamCh1] = new Channel(name + "-Gammas-1", this, Channel.ChannelType.COUNT_RATE);
            channels[gamCh1Sigma] = new Channel(name + "-Gammas-1-Sigma", this, Channel.ChannelType.COUNT_RATE);
            channels[gamInGamCh2] = new Channel(name + "-Gammas-2", this, Channel.ChannelType.COUNT_RATE);
            channels[gamCh2Sigma] = new Channel(name + "-Gammas-2-Sigma", this, Channel.ChannelType.COUNT_RATE);
        }

        public override void SetName(string newName)
        {
            name = newName;
            channels[chACountRate].SetName(name + "-Neutrons-A");
            channels[chBCountRate].SetName(name + "-Neutrons-B");
            channels[chCCountRate].SetName(name + "-Neutrons-C");
            channels[gamInGamCh1].SetName(name + "-Gammas-1");
            channels[gamCh1Sigma].SetName(name + "-Gammas-1-Sigma");
            channels[gamInGamCh2].SetName(name + "-Gammas-1");
            channels[gamCh2Sigma].SetName(name + "-Gammas-1-Sigma");
        }

        public override void ScanDataFolder()
        {
            if (string.IsNullOrEmpty(dataFolder)) return;
            List<string> bidFileList = new List<string>();
            List<DateTime> bidDateList = new List<DateTime>();

            string[] filesInDirectory = Directory.GetFiles(dataFolder);
            foreach (string file in filesInDirectory)
            {
                string fileAbrev = file.Substring(file.LastIndexOf('\\')+1);
                if (fileAbrev.Substring(fileAbrev.Length - 4).ToLower() == ".bid" && fileAbrev.ToLower().StartsWith(filePrefix.ToLower()))
                {
                    if (bidParser.ParseHeader(file) == ReturnCode.SUCCESS)
                    {
                        bidFileList.Add(file);
                        bidDateList.Add(bidParser.GetDate());
                    }
                    else
                    {
                        // Something should really go here...
                    }
                }
            }

            bidFiles = bidFileList.ToArray();
            bidDates = bidDateList.ToArray();

            Array.Sort(bidDates, bidFiles);
        }

        public override void LoadData(DateTime startDate, DateTime endDate)
        {
            ReturnCode returnCode = ReturnCode.SUCCESS;

            int startIndex = Array.FindIndex(bidDates.ToArray(), x => x >= startDate);
            int endIndex = Array.FindIndex(bidDates.ToArray(), x => x >= endDate);

            if (startIndex == -1) return;
            if (endIndex == -1) endIndex = (bidDates.Length) - 1;

            DateTime time;
            DataFile dataFile;
            for (int i = startIndex; i <= endIndex; ++i)
            {
                returnCode = bidParser.ParseFile(bidFiles[i]);
                dataFile = new DataFile(bidFiles[i]);
                int numRecords = bidParser.GetNumRecords();
                for (int r = 0; r < numRecords; ++r)
                {
                    time = bidParser.BIDTimeToDateTime(bidParser.GetRecord(r).time);
                    channels[chACountRate].AddDataPoint(time, bidParser.GetRecord(r).chACountRate, dataFile);
                    channels[chBCountRate].AddDataPoint(time, bidParser.GetRecord(r).chBCountRate, dataFile);
                    channels[chCCountRate].AddDataPoint(time, bidParser.GetRecord(r).chCCountRate, dataFile);
                    channels[gamInGamCh1].AddDataPoint(time, bidParser.GetRecord(r).gamInGamCh1, dataFile);
                    channels[gamCh1Sigma].AddDataPoint(time, bidParser.GetRecord(r).gamCh1Sigma, dataFile);
                    channels[gamInGamCh2].AddDataPoint(time, bidParser.GetRecord(r).gamInGamCh2, dataFile);
                    channels[gamCh2Sigma].AddDataPoint(time, bidParser.GetRecord(r).gamCh2Sigma, dataFile);
                }
            }
            channels[chACountRate].Sort();
            channels[chBCountRate].Sort();
            channels[chCCountRate].Sort();
            channels[gamInGamCh1].Sort();
            channels[gamCh1Sigma].Sort();
            channels[gamInGamCh2].Sort();
            channels[gamCh2Sigma].Sort();
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

        public override Instrument FromParameters(string newName, List<Parameter> parameters)
        {
            GRANDInstrument instrument = new GRANDInstrument(newName);
            Instrument.ApplyStandardInstrumentParameters(instrument, parameters);
            return instrument;
        }
    }
}
