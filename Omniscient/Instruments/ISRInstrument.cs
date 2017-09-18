using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Omniscient.Parsers;

namespace Omniscient.Instruments
{
    class ISRInstrument : Instrument
    {
        private const int NUM_CHANNELS = 3;
        private const int TOTALS1 = 0;
        private const int TOTALS2 = 1;
        private const int TOTALS3 = 2;

        ISRParser isrParser;

        string[] isrFiles;
        DateTime[] isrDates;

        public ISRInstrument(string newName)
        {
            name = newName;
            instrumentType = "ISR";
            isrFiles = new string[0];
            isrDates = new DateTime[0];
            isrParser = new ISRParser();

            numChannels = NUM_CHANNELS;
            channels = new Channel[numChannels];
            channels[TOTALS1] = new Channel("Totals-1", Channel.ChannelType.COUNT_RATE);
            channels[TOTALS2] = new Channel("Totals-2", Channel.ChannelType.COUNT_RATE);
            channels[TOTALS3] = new Channel("Totals-3", Channel.ChannelType.COUNT_RATE);
        }

        public override void ScanDataFolder()
        {
            List<string> isrFileList = new List<string>();
            List<DateTime> isrDateList = new List<DateTime>();

            string[] filesInDirectory = Directory.GetFiles(dataFolder);
            foreach(string file in filesInDirectory)
            {
                if (file.Substring(file.Length - 4).ToLower() == ".isr")
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

            for (int i=startIndex; i<=endIndex; ++i)
            {
                returnCode = isrParser.ParseFile(isrFiles[i]);
                int numRecords = isrParser.GetNumRecords();
                for (int r=0; r < numRecords; ++r)
                {
                    channels[TOTALS1].AddDataPoint(isrParser.ISRTimeToDateTime(isrParser.GetRecord(r).time), isrParser.GetRecord(r).totals1);
                    channels[TOTALS2].AddDataPoint(isrParser.ISRTimeToDateTime(isrParser.GetRecord(r).time), isrParser.GetRecord(r).totals2);
                    channels[TOTALS3].AddDataPoint(isrParser.ISRTimeToDateTime(isrParser.GetRecord(r).time), isrParser.GetRecord(r).totals3);
                }
            }
        }


    }
}
