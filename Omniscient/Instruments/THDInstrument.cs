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
        private const int data0 = 0;
        private const int data1 = 1;
        private const int data2 = 2;

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
            channels[data0] = new Channel(Name + "-0", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[data1] = new Channel(Name + "-1", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[data2] = new Channel(Name + "-2", this, Channel.ChannelType.COUNT_RATE, 0);
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
            DataFile dataFile = new DataFile(fileName, thdParser.Date);
            int numRecords = thdParser.Records.Count;
            DateTime time = DateTime.MinValue;
            for (int r = 0; r < numRecords; ++r)
            {
                time = thdParser.Records[r].time;
                channels[data0].AddDataPoint(compartment, time, thdParser.Records[r].data0, dataFile);
                channels[data1].AddDataPoint(compartment, time, thdParser.Records[r].data1, dataFile);
                channels[data2].AddDataPoint(compartment, time, thdParser.Records[r].data2, dataFile);
            }

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
