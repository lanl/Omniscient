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
    class WUCSInstrument : Instrument
    {
        private const string FILE_EXTENSION = "txt";
        private const int NUM_CHANNELS = 12;
        private const int STATUS_A = 0;
        private const int MAIN_VOLTAGE_A = 1;
        private const int BATTERY_TEMP_A = 2;
        private const int MODULE_TEMP_A = 3;
        private const int BATTERY_VOLTAGE_A = 4;
        private const int CHARGER_VOLTAGE_A = 5;
        private const int STATUS_B = 6;
        private const int MAIN_VOLTAGE_B = 7;
        private const int BATTERY_TEMP_B = 8;
        private const int MODULE_TEMP_B = 9;
        private const int BATTERY_VOLTAGE_B = 10;
        private const int CHARGER_VOLTAGE_B = 11;

        WUCSParser parser;

        public WUCSInstrument(DetectionSystem parent, string name, uint id) : base(parent, name, id)
        {
            InstrumentType = "WUCS";
            FileExtension = FILE_EXTENSION;
            filePrefix = "";
            fileSuffix = "";
            parser = new WUCSParser();

            numChannels = NUM_CHANNELS;
            channels = new Channel[numChannels];
            channels[STATUS_A] = new Channel(Name + "-Status_A", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[MAIN_VOLTAGE_A] = new Channel(Name + "-Main_V_A", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[BATTERY_TEMP_A] = new Channel(Name + "-Battery_Temp_A", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[MODULE_TEMP_A] = new Channel(Name + "-Module_Temp_A", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[BATTERY_VOLTAGE_A] = new Channel(Name + "-Battery_V_A", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[CHARGER_VOLTAGE_A] = new Channel(Name + "-Charger_V_A", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[STATUS_B] = new Channel(Name + "-Status_B", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[MAIN_VOLTAGE_B] = new Channel(Name + "-Main_V_B", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[BATTERY_TEMP_B] = new Channel(Name + "-Battery_Temp_B", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[MODULE_TEMP_B] = new Channel(Name + "-Module_Temp_B", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[BATTERY_VOLTAGE_B] = new Channel(Name + "-Battery_V_B", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[CHARGER_VOLTAGE_B] = new Channel(Name + "-Charger_V_B", this, Channel.ChannelType.COUNT_RATE, 0);
        }

        public override DateTime GetFileDate(string file)
        {
            if (parser.ParseOneRecord(file) == ReturnCode.SUCCESS && parser.Records.Count > 0)
            {
                return parser.Records[0].time;
            }
            return DateTime.MinValue;
        }

        public override ReturnCode IngestFile(ChannelCompartment compartment, string fileName)
        {
            ReturnCode returnCode = parser.ParseFile(fileName);
            List<WUCSRecord> records = parser.Records;
            int numRecords = records.Count;
            if (returnCode != ReturnCode.SUCCESS || numRecords < 1) return returnCode;
            DataFile dataFile = new DataFile(fileName, records[0].time);
            DataFile[] dataFiles = new DataFile[numRecords];
            for (int r = 0; r < numRecords; ++r) dataFiles[r] = dataFile;
            DateTime[] times = new DateTime[numRecords];
            double[] statA = new double[numRecords];
            double[] MVA = new double[numRecords];
            double[] BTA = new double[numRecords];
            double[] MTA = new double[numRecords];
            double[] BVA = new double[numRecords];
            double[] CVA = new double[numRecords];
            double[] statB = new double[numRecords];
            double[] MVB = new double[numRecords];
            double[] BTB = new double[numRecords];
            double[] MTB = new double[numRecords];
            double[] BVB = new double[numRecords];
            double[] CVB = new double[numRecords];
            DateTime time = DateTime.MinValue;
            WUCSRecord record;
            for (int r = 0; r < numRecords; ++r)
            {
                record = records[r];
                time = times[r] = record.time;
                statA[r] = record.statusA;
                MVA[r] = record.mainVoltageA;
                BTA[r] = record.batteryTempA;
                MTA[r] = record.moduleTempA;
                BVA[r] = record.batteryVoltageA;
                CVA[r] = record.chargerVoltageA;
                statB[r] = record.statusB;
                MVB[r] = record.mainVoltageB;
                BTB[r] = record.batteryTempB;
                MTB[r] = record.moduleTempB;
                BVB[r] = record.batteryVoltageB;
                CVB[r] = record.chargerVoltageB;
            }
            channels[STATUS_A].AddDataPoints(compartment, times, statA, dataFiles);
            channels[MAIN_VOLTAGE_A].AddDataPoints(compartment, times, MVA, dataFiles);
            channels[BATTERY_TEMP_A].AddDataPoints(compartment, times, BTA, dataFiles);
            channels[MODULE_TEMP_A].AddDataPoints(compartment, times, MTA, dataFiles);
            channels[BATTERY_VOLTAGE_A].AddDataPoints(compartment, times, BVA, dataFiles);
            channels[CHARGER_VOLTAGE_A].AddDataPoints(compartment, times, CVA, dataFiles);
            channels[STATUS_B].AddDataPoints(compartment, times, statB, dataFiles);
            channels[MAIN_VOLTAGE_B].AddDataPoints(compartment, times, MVB, dataFiles);
            channels[BATTERY_TEMP_B].AddDataPoints(compartment, times, BTB, dataFiles);
            channels[MODULE_TEMP_B].AddDataPoints(compartment, times, MTB, dataFiles);
            channels[BATTERY_VOLTAGE_B].AddDataPoints(compartment, times, BVB, dataFiles);
            channels[CHARGER_VOLTAGE_B].AddDataPoints(compartment, times, CVB, dataFiles);
            
            dataFile.DataEnd = time;
            parser = new WUCSParser();
            return ReturnCode.SUCCESS;
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = GetStandardInstrumentParameters();
            return parameters;
        }

        public override void ApplyParameters(List<Parameter> parameters)
        {
            ApplyStandardInstrumentParameters(this, parameters);
        }
    }

    public class WUCSInstrumentHookup : InstrumentHookup
    {
        public WUCSInstrumentHookup()
        {
        }

        public override string Type { get { return "WUCS"; } }

        public override Instrument FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            WUCSInstrument instrument = new WUCSInstrument(parent, newName, id);
            Instrument.ApplyStandardInstrumentParameters(instrument, parameters);
            return instrument;
        }
    }
}
