using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class CSVInstrument : Instrument
    {
        private const string FILE_EXTENSION = "csv";

        private int _numberOfHeaders;
        public int NumberOfHeaders
        {
            get { return _numberOfHeaders; }
            set
            {
                _numberOfHeaders = value;
                if(csvParser != null) csvParser.NumberOfHeaders = value;
            }
        }

        CSVParser csvParser;

        public CSVInstrument(DetectionSystem parent, string newName, int nChannels, uint id) : base(parent, newName, id)
        {
            InstrumentType = "CSV";
            numChannels = nChannels;
            FileExtension = FILE_EXTENSION;
            filePrefix = "";

            csvParser = new CSVParser();
            csvParser.NumberOfColumns = numChannels + 1;
            NumberOfHeaders = 0;

            channels = new Channel[numChannels];
            for (int i = 0; i < numChannels; i++)
                channels[i] = new Channel(Name + "-" + i.ToString(), this, Channel.ChannelType.COUNT_RATE, 0);
        }

        public ReturnCode SetNumberOfChannels(int nChannels)
        {
            if (nChannels < 1) return ReturnCode.BAD_INPUT;
            Channel[] newChannels = new Channel[nChannels];

            // Put as many of the original channels back as can fit in the new array
            if (numChannels < nChannels)
            {
                for (int i=0; i<numChannels; ++i)
                {
                    newChannels[i] = channels[i];
                }
                for (int i=numChannels; i<nChannels; ++i)
                {
                    newChannels[i] = new Channel(Name + "-" + i.ToString(), this, Channel.ChannelType.COUNT_RATE, 0);
                }
            }
            else
            {
                for (int i = 0; i < nChannels; ++i)
                {
                    newChannels[i] = channels[i];
                }
            }
            numChannels = nChannels;
            channels = newChannels;
            csvParser.NumberOfColumns = numChannels + 1;
            return ReturnCode.SUCCESS;
        }


        public override void ClearData(ChannelCompartment compartment)
        {
            foreach (Channel ch in channels)
            {
                ch.ClearData(compartment);
            }
        }

        public override ReturnCode IngestFile(ChannelCompartment compartment, string fileName)
        {
            DateTime time = DateTime.MinValue;
            DataFile dataFile = new DataFile(fileName);
            dataFile.DataStart = GetFileDate(fileName);

            ReturnCode returnCode = csvParser.ParseFile(fileName);

            int numRecords = csvParser.GetNumRecords();
            for (int r = 0; r < numRecords; ++r)
            {
                time = csvParser.TimeStamps[r];
                for (int c = 0; c < numChannels; c++)
                {
                    channels[c].AddDataPoint(compartment, time, csvParser.Data[r, c], dataFile);
                }
            }

            dataFile.DataEnd = time;

            return ReturnCode.SUCCESS;
        }

        public override DateTime GetFileDate(string file)
        {
            if (csvParser.ParseFirstEntry(file) == ReturnCode.SUCCESS)
            {
                return csvParser.TimeStamps[0];
            }
            return DateTime.MinValue;
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = GetStandardInstrumentParameters();
            parameters.Add(new IntParameter("Headers") { Value = NumberOfHeaders.ToString() });
            parameters.Add(new IntParameter("Channels") { Value = numChannels.ToString() });
            return parameters;
        }

        public override void ApplyParameters(List<Parameter> parameters)
        {
            ApplyStandardInstrumentParameters(this, parameters);
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Headers":
                        NumberOfHeaders = ((IntParameter)param).ToInt();
                        break;
                    case "Channels":
                        SetNumberOfChannels(((IntParameter)param).ToInt());
                        break;
                }
            }
        }
    }

    public class CSVInstrumentHookup : InstrumentHookup
    {
        public CSVInstrumentHookup()
        {
            TemplateParameters.AddRange( new List<ParameterTemplate>() {
                new ParameterTemplate("Headers", ParameterType.Int),
                new ParameterTemplate("Channels", ParameterType.Int)
                });
        }

        public override string Type { get { return "CSV"; } }

        public override Instrument FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            int nHeaders = 0;
            int nChannels = 0;
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Headers":
                        nHeaders = ((IntParameter)param).ToInt();
                        break;
                    case "Channels":
                        nChannels = ((IntParameter)param).ToInt();
                        break;
                }
            }
            CSVInstrument instrument = new CSVInstrument(parent, newName, nChannels, id);
            instrument.NumberOfHeaders = nHeaders;
            Instrument.ApplyStandardInstrumentParameters(instrument, parameters);
            return instrument;
        }
    }
}
