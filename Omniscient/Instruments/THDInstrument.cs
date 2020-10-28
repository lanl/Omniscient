/*
This source code is Free Open Source Software. It is provided
with NO WARRANTY expressed or implied to the extent permitted by law.

This source code is distributed under the New BSD license:

================================================================================

   Copyright (c) 2020, International Atomic Energy Agency (IAEA), IAEA.org

   All rights reserved.

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice,
      this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice,
      this list of conditions and the following disclaimer in the documentation
      and/or other materials provided with the distribution.
    * Neither the name of IAEA nor the names of its contributors
      may be used to endorse or promote products derived from this software
      without specific prior written permission.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
   "AS IS"AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
   A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
   LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
   NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class THDInstrument : Instrument
    {
        private const string FILE_EXTENSION = "dat";

        private const int NUM_CHANNELS = 3;
        private const int TEMP = 0;
        private const int HUMIDITY = 1;
        private const int DEW = 2;

        THDParser thdParser;

        public THDInstrument(DetectionSystem parent, string name, uint id) : base(parent, name, id)
        {
            InstrumentType = "THD";
            FileExtension = FILE_EXTENSION;
            filePrefix = "";
            fileSuffix = "_THD";
            FileExtension = "txt";
            thdParser = new THDParser();

            numChannels = NUM_CHANNELS;
            channels = new Channel[numChannels];
            channels[TEMP] = new Channel(Name + "-Temperature", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[HUMIDITY] = new Channel(Name + "-Humidity", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[DEW] = new Channel(Name + "-Dew_Point", this, Channel.ChannelType.COUNT_RATE, 0);
        }

        public override DateTime GetFileDate(string file)
        {
            if (thdParser.ParseFirstRecord(file) == ReturnCode.SUCCESS)
            {
                return thdParser.Date;
            }
            return DateTime.MinValue;
        }

        public override ReturnCode IngestFile(ChannelCompartment compartment, string fileName)
        {
            ReturnCode returnCode = thdParser.ParseFile(fileName);
            if (returnCode != ReturnCode.SUCCESS) return returnCode;

            DataFile dataFile = new DataFile(fileName, thdParser.Date);
            int numRecords = thdParser.Records.Count;
            DateTime[] times = new DateTime[numRecords];
            double[] d0 = new double[numRecords];
            double[] d1 = new double[numRecords];
            double[] d2 = new double[numRecords];
            DataFile[] dataFiles = new DataFile[numRecords];
            for (int r = 0; r < numRecords; ++r) dataFiles[r] = dataFile;
            THDRecord record;
            DateTime time = DateTime.MinValue;
            for (int r = 0; r < numRecords; ++r)
            {
                record = thdParser.Records[r];
                time = times[r] = record.time;
                d0[r] = record.data0;
                d1[r] = record.data1;
                d2[r] = record.data2;
            }
            channels[TEMP].AddDataPoints(compartment, times, d0, dataFiles);
            channels[HUMIDITY].AddDataPoints(compartment, times, d1, dataFiles);
            channels[DEW].AddDataPoints(compartment, times, d2, dataFiles);

            dataFile.DataEnd = time;
            thdParser = new THDParser();
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

    public class THDInstrumentHookup : InstrumentHookup
    {
        public THDInstrumentHookup() { }

        public override string Type { get { return "THD"; } }

        public override Instrument FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            THDInstrument instrument = new THDInstrument(parent, newName, id);
            Instrument.ApplyStandardInstrumentParameters(instrument, parameters);
            return instrument;
        }
    }
}
