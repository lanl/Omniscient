using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Omniscient
{
    class ATPMInstrument : Instrument
    {
        private const string FILE_EXTENSION = "dat";

        private const int NUM_CHANNELS = 4;
        private const int chVolumFlow = 0;
        private const int chTempSupply = 1;
        private const int chTempReturn = 2;
        private const int chActualPow = 3;

        ATPMParser atpmParser;

        public ATPMInstrument(DetectionSystem parent, string name, uint id) : base(parent, name, id)
        {
            InstrumentType = "ATPM";
            FileExtension = FILE_EXTENSION;
            filePrefix = "";
            fileSuffix = "";
            FileExtension = "dat";
            atpmParser = new ATPMParser();

            numChannels = NUM_CHANNELS;
            channels = new Channel[numChannels];
            channels[chVolumFlow] = new Channel(Name + "-VolumFlow", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[chTempSupply] = new Channel(Name + "-TempSupply", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[chTempReturn] = new Channel(Name + "-TempReturn", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[chActualPow] = new Channel(Name + "-ActualPow", this, Channel.ChannelType.COUNT_RATE, 0);
        }

        public override DateTime GetFileDate(string file)
        {
            if (atpmParser.ParseHeader(file) == ReturnCode.SUCCESS)
            {
                return atpmParser.Date;
            }
            return DateTime.MinValue;
        }

        public override ReturnCode IngestFile(ChannelCompartment compartment, string fileName)
        {
            ReturnCode returnCode = atpmParser.ParseFile(fileName);
            if (returnCode != ReturnCode.SUCCESS) return returnCode;

            DataFile dataFile = new DataFile(fileName, atpmParser.Date);
            int numRecords = atpmParser.Records.Length;
            DateTime time = DateTime.MinValue;
            for (int r = 0; r < numRecords; ++r)
            {
                time = atpmParser.ATPMTimeToDateTime(atpmParser.Records[r].time);
                channels[chVolumFlow].AddDataPoint(compartment, time, atpmParser.Records[r].volumFlow, dataFile);
                channels[chTempSupply].AddDataPoint(compartment, time, atpmParser.Records[r].tempSupply, dataFile);
                channels[chTempReturn].AddDataPoint(compartment, time, atpmParser.Records[r].tempReturn, dataFile);
                channels[chActualPow].AddDataPoint(compartment, time, atpmParser.Records[r].actualPow, dataFile);
            }

            dataFile.DataEnd = time;

            atpmParser = new ATPMParser();

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

    public class ATPMInstrumentHookup : InstrumentHookup
    {
        public ATPMInstrumentHookup() { }

        public override string Type { get { return "ATPM"; } }

        public override Instrument FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            ATPMInstrument instrument = new ATPMInstrument(parent, newName, id);
            Instrument.ApplyStandardInstrumentParameters(instrument, parameters);
            return instrument;
        }
    }
}
