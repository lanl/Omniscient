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
            instrumentType = "CSV";
            numChannels = nChannels;
            FileExtension = FILE_EXTENSION;
            filePrefix = "";
            NumberOfHeaders = 0;

            csvFiles = new string[0];
            csvDates = new DateTime[0];
            csvParser = new CSVParser();
            csvParser.NumberOfColumns = numChannels + 1;

            channels = new Channel[numChannels];
            for (int i=0; i<numChannels; i++)
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
                    for (int c = 0; c<numChannels; c++)
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

        public override void SetName(string newName)
        {
            name = newName;
            for (int i = 0; i < numChannels; i++)
                channels[i].SetName(name + "-" + i.ToString());
        }
    }
}
