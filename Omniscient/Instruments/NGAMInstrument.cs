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
    class NGAMInstrument : Instrument
    {
        private const string FILE_EXTENSION = "VBF";

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

        string[] vbfFiles;
        DateTime[] vbfDates;

        public NGAMInstrument(string newName) : base(newName)
        {
            instrumentType = "NGAM";
            FileExtension = FILE_EXTENSION;
            filePrefix = "";
            vbfFiles = new string[0];
            vbfDates = new DateTime[0];
            vbfParser = new VBFParser();

            numChannels = NUM_CHANNELS;
            channels = new Channel[numChannels];
            channels[data0] = new Channel(name + "-data0", this, Channel.ChannelType.COUNT_RATE);
            channels[data1] = new Channel(name + "-data1", this, Channel.ChannelType.COUNT_RATE);
            channels[data2] = new Channel(name + "-data2", this, Channel.ChannelType.COUNT_RATE);
            channels[data3] = new Channel(name + "-data3", this, Channel.ChannelType.COUNT_RATE);
            channels[data4] = new Channel(name + "-data4", this, Channel.ChannelType.COUNT_RATE);
            channels[data5] = new Channel(name + "-data5", this, Channel.ChannelType.COUNT_RATE);
            channels[data6] = new Channel(name + "-data6", this, Channel.ChannelType.COUNT_RATE);
            channels[data7] = new Channel(name + "-data7", this, Channel.ChannelType.COUNT_RATE);
        }

        public override void SetName(string newName)
        {
            name = newName;
            channels[data0].SetName(name + "-data0");
            channels[data1].SetName(name + "-data1");
            channels[data2].SetName(name + "-data2");
            channels[data3].SetName(name + "-data3");
            channels[data4].SetName(name + "-data4");
            channels[data5].SetName(name + "-data5");
            channels[data6].SetName(name + "-data6");
            channels[data7].SetName(name + "-data7");
        }

        public override void ScanDataFolder()
        {
            List<string> vbfFileList = new List<string>();
            List<DateTime> vbfDateList = new List<DateTime>();

            string[] filesInDirectory = Directory.GetFiles(dataFolder);
            foreach (string file in filesInDirectory)
            {
                string fileAbrev = file.Substring(file.LastIndexOf('\\') + 1);
                if (fileAbrev.Substring(fileAbrev.Length - 4).ToLower() == ".vbf" && fileAbrev.ToLower().StartsWith(filePrefix.ToLower()))
                {
                    if (vbfParser.ParseHeader(file) == ReturnCode.SUCCESS)
                    {
                        vbfFileList.Add(file);
                        vbfDateList.Add(vbfParser.GetDate());
                    }
                    else
                    {
                        // Something should really go here...
                    }
                }
            }

            vbfFiles = vbfFileList.ToArray();
            vbfDates = vbfDateList.ToArray();

            Array.Sort(vbfDates, vbfFiles);
        }

        public override void LoadData(DateTime startDate, DateTime endDate)
        {
            ReturnCode returnCode = ReturnCode.SUCCESS;

            int startIndex = Array.FindIndex(vbfDates.ToArray(), x => x >= startDate);
            int endIndex = Array.FindIndex(vbfDates.ToArray(), x => x >= endDate);

            if (startIndex == -1) return;
            if (endIndex == -1) endIndex = (vbfDates.Length) - 1;

            DateTime time;
            DataFile dataFile;
            for (int i = startIndex; i <= endIndex; ++i)
            {
                returnCode = vbfParser.ParseFile(vbfFiles[i]);
                dataFile = new DataFile(vbfFiles[i]);
                int numRecords = vbfParser.GetNumRecords();
                for (int r = 0; r < numRecords; ++r)
                {
                    time = vbfParser.VBFTimeToDateTime(vbfParser.GetRecord(r).time);
                    channels[data0].AddDataPoint(time, vbfParser.GetRecord(r).data[0], dataFile);
                    channels[data1].AddDataPoint(time, vbfParser.GetRecord(r).data[1], dataFile);
                    channels[data2].AddDataPoint(time, vbfParser.GetRecord(r).data[2], dataFile);
                    channels[data3].AddDataPoint(time, vbfParser.GetRecord(r).data[3], dataFile);
                    channels[data4].AddDataPoint(time, vbfParser.GetRecord(r).data[4], dataFile);
                    channels[data5].AddDataPoint(time, vbfParser.GetRecord(r).data[5], dataFile);
                    channels[data6].AddDataPoint(time, vbfParser.GetRecord(r).data[6], dataFile);
                    channels[data7].AddDataPoint(time, vbfParser.GetRecord(r).data[7], dataFile);
                }
            }
            channels[data0].Sort();
            channels[data1].Sort();
            channels[data2].Sort();
            channels[data3].Sort();
            channels[data4].Sort();
            channels[data5].Sort();
            channels[data6].Sort();
            channels[data7].Sort();
            LoadVirtualChannels();
        }

        public override void ClearData()
        {
            foreach (Channel ch in channels)
            {
                ch.ClearData();
            }
        }
    }
}
