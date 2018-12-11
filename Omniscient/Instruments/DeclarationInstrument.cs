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
    public class DeclarationInstrument : Instrument
    {

        private const int NUM_CHANNELS = 1;
        private const int DECLARATION = 0;

        DECFile decParser;

        string[] decFiles;
        DateTime[] decDates;

        public DeclarationInstrument(string newName) : base(newName)
        {
            InstrumentType = "Declaration";
            filePrefix = "";
            decFiles = new string[0];
            decDates = new DateTime[0];
            decParser = new DECFile();

            numChannels = NUM_CHANNELS;
            channels = new Channel[numChannels];
            channels[DECLARATION] = new Channel(name + "-Declarations", this, Channel.ChannelType.DURATION_VALUE);
        }

        public override void ScanDataFolder()
        {
            if (string.IsNullOrEmpty(dataFolder)) return;
            List<string> decFileList = new List<string>();
            List<DateTime> decDateList = new List<DateTime>();

            string[] filesInDirectory = Directory.GetFiles(dataFolder);
            foreach (string file in filesInDirectory)
            {
                string fileAbrev = file.Substring(file.LastIndexOf('\\') + 1);
                if (fileAbrev.Substring(fileAbrev.Length - 4).ToLower() == ".dec" && fileAbrev.ToLower().StartsWith(filePrefix.ToLower()))
                {
                    if (decParser.ParseDeclarationFile(file) == ReturnCode.SUCCESS)
                    {
                        decFileList.Add(file);
                        decDateList.Add(decParser.FromTime);
                    }
                    else
                    {
                        // Something should really go here...
                    }
                }
            }

            decFiles = decFileList.ToArray();
            decDates = decDateList.ToArray();

            Array.Sort(decDates, decFiles);
        }

        public override void LoadData(DateTime startDate, DateTime endDate)
        {
            ReturnCode returnCode = ReturnCode.SUCCESS;

            int startIndex = Array.FindIndex(decDates.ToArray(), x => x >= startDate);
            int endIndex = Array.FindIndex(decDates.ToArray(), x => x >= endDate);

            if (endIndex == -1) endIndex = (decDates.Length) - 1;
            if (endIndex == -1) startIndex = 0;

            DateTime time;
            TimeSpan duration;
            if (startIndex >= 0)
            {
                for (int i = startIndex; i <= endIndex; ++i)
                {
                    returnCode = decParser.ParseDeclarationFile(decFiles[i]);
                    time = decParser.FromTime;
                    duration = decParser.ToTime - decParser.FromTime;

                    channels[DECLARATION].AddDataPoint(time, 1, duration, new DataFile(decFiles[i]));
                }
                channels[DECLARATION].Sort();
                LoadVirtualChannels();
            }
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

    public class DeclarationInstrumentHookup : InstrumentHookup
    {
        public DeclarationInstrumentHookup() { }

        public override string Type { get { return "Declaration"; } }

        public override Instrument FromParameters(string newName, List<Parameter> parameters)
        {
            DeclarationInstrument instrument = new DeclarationInstrument(newName);
            Instrument.ApplyStandardInstrumentParameters(instrument, parameters);
            return instrument;
        }
    }
}
