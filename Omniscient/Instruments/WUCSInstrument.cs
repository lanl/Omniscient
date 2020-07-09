﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    class WUCSInstrument : Instrument
    {
        private const string FILE_EXTENSION = "txt";
        private const int NUM_CHANNELS = 10;
        private const int MAIN_VOLTAGE_A = 0;
        private const int BATTERY_TEMP_A = 1;
        private const int MODULE_TEMP_A = 2;
        private const int BATTERY_VOLTAGE_A = 3;
        private const int CHARGER_VOLTAGE_A = 4;
        private const int MAIN_VOLTAGE_B = 5;
        private const int BATTERY_TEMP_B = 6;
        private const int MODULE_TEMP_B = 7;
        private const int BATTERY_VOLTAGE_B = 8;
        private const int CHARGER_VOLTAGE_B = 9;

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
            channels[MAIN_VOLTAGE_A] = new Channel(Name + "-Main_V_A", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[BATTERY_TEMP_A] = new Channel(Name + "-Battery_Temp_A", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[MODULE_TEMP_A] = new Channel(Name + "-Module_Temp_A", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[BATTERY_VOLTAGE_A] = new Channel(Name + "-Battery_V_A", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[CHARGER_VOLTAGE_A] = new Channel(Name + "-Charger_V_A", this, Channel.ChannelType.COUNT_RATE, 0);
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

            DateTime time = DateTime.MinValue;
            for (int r = 0; r < numRecords; ++r)
            {
                time = records[r].time;
                channels[MAIN_VOLTAGE_A].AddDataPoint(compartment, time, records[r].mainVoltageA, dataFile);
                channels[BATTERY_TEMP_A].AddDataPoint(compartment, time, records[r].batteryTempA, dataFile);
                channels[MODULE_TEMP_A].AddDataPoint(compartment, time, records[r].moduleTempA, dataFile);
                channels[BATTERY_VOLTAGE_A].AddDataPoint(compartment, time, records[r].batteryVoltageA, dataFile);
                channels[CHARGER_VOLTAGE_A].AddDataPoint(compartment, time, records[r].chargerVoltageA, dataFile);
                channels[MAIN_VOLTAGE_B].AddDataPoint(compartment, time, records[r].mainVoltageB, dataFile);
                channels[BATTERY_TEMP_B].AddDataPoint(compartment, time, records[r].batteryTempB, dataFile);
                channels[MODULE_TEMP_B].AddDataPoint(compartment, time, records[r].moduleTempB, dataFile);
                channels[BATTERY_VOLTAGE_B].AddDataPoint(compartment, time, records[r].batteryVoltageB, dataFile);
                channels[CHARGER_VOLTAGE_B].AddDataPoint(compartment, time, records[r].chargerVoltageB, dataFile);
            }
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