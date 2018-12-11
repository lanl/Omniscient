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

        public int NumberOfHeaders
        {
            get;
            set;
        }

        CSVParser csvParser;

        string[] csvFiles;
        DateTime[] csvDates;

        public CSVInstrument(string newName, int nChannels) : base(newName)
        {
            InstrumentType = "CSV";
            numChannels = nChannels;
            FileExtension = FILE_EXTENSION;
            filePrefix = "";
            NumberOfHeaders = 0;

            csvFiles = new string[0];
            csvDates = new DateTime[0];
            csvParser = new CSVParser();
            csvParser.NumberOfColumns = numChannels + 1;

            channels = new Channel[numChannels];
            for (int i = 0; i < numChannels; i++)
                channels[i] = new Channel(name + "-" + i.ToString(), this, Channel.ChannelType.COUNT_RATE);
        }

        public ReturnCode SetNumberOfChannels(int nChannels)
        {
            if (nChannels < 1) return ReturnCode.BAD_INPUT;
            numChannels = nChannels;
            channels = new Channel[numChannels];
            for (int i = 0; i < numChannels; i++)
                channels[i] = new Channel(name + "-" + i.ToString(), this, Channel.ChannelType.COUNT_RATE);
            csvParser.NumberOfColumns = numChannels + 1;
            return ReturnCode.SUCCESS;
        }

        public override void ClearData()
        {
            foreach (Channel ch in channels)
            {
                ch.ClearData();
            }
        }

        public override void LoadData(DateTime startDate, DateTime endDate)
        {
            csvParser.NumberOfHeaders = NumberOfHeaders;
            ReturnCode returnCode = ReturnCode.SUCCESS;

            int startIndex = Array.FindIndex(csvDates.ToArray(), x => x >= startDate);
            int endIndex = Array.FindIndex(csvDates.ToArray(), x => x >= endDate);

            if (endIndex == -1) endIndex = (csvDates.Length) - 1;
            if (endIndex == -1) startIndex = 0;

            DateTime time;
            DataFile dataFile;
            for (int i = startIndex; i <= endIndex; ++i)
            {
                returnCode = csvParser.ParseFile(csvFiles[i]);
                dataFile = new DataFile(csvFiles[i]);
                int numRecords = csvParser.GetNumRecords();
                for (int r = 0; r < numRecords; ++r)
                {
                    time = csvParser.TimeStamps[r];
                    for (int c = 0; c < numChannels; c++)
                    {
                        channels[c].AddDataPoint(time, csvParser.Data[r, c], dataFile);
                    }
                }
            }
            for (int c = 0; c < numChannels; c++)
            {
                channels[c].Sort();
            }
            LoadVirtualChannels();
        }

        public override void ScanDataFolder()
        {
            if (string.IsNullOrEmpty(dataFolder)) return;
            csvParser.NumberOfHeaders = NumberOfHeaders;
            List<string> csvFileList = new List<string>();
            List<DateTime> csvDateList = new List<DateTime>();

            string[] filesInDirectory = Directory.GetFiles(dataFolder);
            foreach (string file in filesInDirectory)
            {
                string fileAbrev = file.Substring(file.LastIndexOf('\\') + 1);
                if (fileAbrev.Substring(fileAbrev.Length - 4).ToLower() == ("." + FileExtension) && fileAbrev.ToLower().StartsWith(filePrefix.ToLower()))
                {
                    if (csvParser.ParseFirstEntry(file) == ReturnCode.SUCCESS)
                    {
                        csvFileList.Add(file);
                        csvDateList.Add(csvParser.TimeStamps[0]);
                    }
                    else
                    {
                        // Something should really go here...
                    }
                }
            }

            csvFiles = csvFileList.ToArray();
            csvDates = csvDateList.ToArray();
            Array.Sort(csvDates, csvFiles);
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

        public override Instrument FromParameters(string newName, List<Parameter> parameters)
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
            CSVInstrument instrument = new CSVInstrument(newName, nChannels);
            instrument.NumberOfHeaders = nHeaders;
            Instrument.ApplyStandardInstrumentParameters(instrument, parameters);
            return instrument;
        }
    }
}
