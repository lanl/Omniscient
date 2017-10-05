using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Omniscient.Parsers;

namespace Omniscient.Instruments
{
    public class ISRInstrument : Instrument
    {
        private const int NUM_CHANNELS = 5;
        private const int TOTALS1 = 0;
        private const int TOTALS2 = 1;
        private const int TOTALS3 = 2;
        private const int REALS_PLUS_ACC = 3;
        private const int ACC = 4;

        ISRParser isrParser;

        string[] isrFiles;
        DateTime[] isrDates;

        public ISRInstrument(string newName) : base(newName)
        {
            instrumentType = "ISR";
            filePrefix = "";
            isrFiles = new string[0];
            isrDates = new DateTime[0];
            isrParser = new ISRParser();

            numChannels = NUM_CHANNELS;
            channels = new Channel[numChannels];
            channels[TOTALS1] = new Channel(name+"-Totals-1", this, Channel.ChannelType.COUNT_RATE);
            channels[TOTALS2] = new Channel(name + "-Totals-2", this, Channel.ChannelType.COUNT_RATE);
            channels[TOTALS3] = new Channel(name + "-Totals-3", this, Channel.ChannelType.COUNT_RATE);
            channels[REALS_PLUS_ACC] = new Channel(name + "-Real+Acc", this, Channel.ChannelType.COUNT_RATE);
            channels[ACC] = new Channel(name + "-Acc", this, Channel.ChannelType.COUNT_RATE);
        }

        public override void SetName(string newName)
        {
            name = newName;
            channels[TOTALS1].SetName(name + "-Totals-1");
            channels[TOTALS2].SetName(name + "-Totals-2");
            channels[TOTALS3].SetName(name + "-Totals-3");
            channels[REALS_PLUS_ACC].SetName(name + "-Real+Acc");
            channels[ACC].SetName(name + "-Acc");
        }

        public override void ScanDataFolder()
        {
            List<string> isrFileList = new List<string>();
            List<DateTime> isrDateList = new List<DateTime>();

            string[] filesInDirectory = Directory.GetFiles(dataFolder);
            foreach(string file in filesInDirectory)
            {
                string fileAbrev = file.Substring(file.LastIndexOf('\\') + 1);
                if (fileAbrev.Substring(fileAbrev.Length - 4).ToLower() == ".isr" && fileAbrev.ToLower().StartsWith(filePrefix.ToLower()))
                {
                    if (isrParser.ParseHeader(file) == ReturnCode.SUCCESS)
                    {
                        isrFileList.Add(file);
                        isrDateList.Add(isrParser.GetDate());
                    }
                    else
                    {
                        // Something should really go here...
                    }
                }
            }

            isrFiles = isrFileList.ToArray();
            isrDates = isrDateList.ToArray();

            Array.Sort(isrDates, isrFiles);
        }

        public override void LoadData(DateTime startDate, DateTime endDate)
        {
            ReturnCode returnCode = ReturnCode.SUCCESS;

            int startIndex = Array.FindIndex(isrDates.ToArray(), x => x >= startDate);
            int endIndex = Array.FindIndex(isrDates.ToArray(), x => x >= endDate);

            if (endIndex == -1) endIndex = (isrDates.Length) - 1;

            DateTime time;

            for (int i=startIndex; i<=endIndex; ++i)
            {
                returnCode = isrParser.ParseFile(isrFiles[i]);
                int numRecords = isrParser.GetNumRecords();
                for (int r=0; r < numRecords; ++r)
                {
                    time = isrParser.ISRTimeToDateTime(isrParser.GetRecord(r).time);
                    channels[TOTALS1].AddDataPoint(time, isrParser.GetRecord(r).totals1);
                    channels[TOTALS2].AddDataPoint(time, isrParser.GetRecord(r).totals2);
                    channels[TOTALS3].AddDataPoint(time, isrParser.GetRecord(r).totals3);
                    channels[REALS_PLUS_ACC].AddDataPoint(time, isrParser.GetRecord(r).realsPlusAccidentals);
                    channels[ACC].AddDataPoint(time, isrParser.GetRecord(r).accidentals);
                }
            }
            channels[TOTALS1].Sort();
            channels[TOTALS2].Sort();
            channels[TOTALS3].Sort();
            channels[REALS_PLUS_ACC].Sort();
            channels[ACC].Sort();
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
