using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Omniscient
{
    class GRANDInstrument : Instrument
    {
        private const string FILE_EXTENSION = "bid";

        private const int NUM_CHANNELS = 7;
        private const int chACountRate = 0;
        private const int chBCountRate = 1;
        private const int chCCountRate = 2;
        private const int gamInGamCh1 = 3;
        private const int gamCh1Sigma = 4;
        private const int gamInGamCh2 = 5;
        private const int gamCh2Sigma = 6;

        BIDParser bidParser;

        string[] bidFiles;
        DateTime[] bidDates;

        public GRANDInstrument(string newName) : base(newName)
        {
            instrumentType = "GRAND";
            FileExtension = FILE_EXTENSION;
            filePrefix = "";
            bidFiles = new string[0];
            bidDates = new DateTime[0];
            bidParser = new BIDParser();

            numChannels = NUM_CHANNELS;
            channels = new Channel[numChannels];
            channels[chACountRate] = new Channel(name + "-Neutrons-A", this, Channel.ChannelType.COUNT_RATE);
            channels[chBCountRate] = new Channel(name + "-Neutrons-B", this, Channel.ChannelType.COUNT_RATE);
            channels[chCCountRate] = new Channel(name + "-Neutrons-C", this, Channel.ChannelType.COUNT_RATE);
            channels[gamInGamCh1] = new Channel(name + "-Gammas-1", this, Channel.ChannelType.COUNT_RATE);
            channels[gamCh1Sigma] = new Channel(name + "-Gammas-1-Sigma", this, Channel.ChannelType.COUNT_RATE);
            channels[gamInGamCh2] = new Channel(name + "-Gammas-2", this, Channel.ChannelType.COUNT_RATE);
            channels[gamCh2Sigma] = new Channel(name + "-Gammas-2-Sigma", this, Channel.ChannelType.COUNT_RATE);
        }

        public override void SetName(string newName)
        {
            name = newName;
            channels[chACountRate].SetName(name + "-Neutrons-A");
            channels[chBCountRate].SetName(name + "-Neutrons-B");
            channels[chCCountRate].SetName(name + "-Neutrons-C");
            channels[gamInGamCh1].SetName(name + "-Gammas-1");
            channels[gamCh1Sigma].SetName(name + "-Gammas-1-Sigma");
            channels[gamInGamCh2].SetName(name + "-Gammas-1");
            channels[gamCh2Sigma].SetName(name + "-Gammas-1-Sigma");
        }

        public override void ScanDataFolder()
        {
            List<string> bidFileList = new List<string>();
            List<DateTime> bidDateList = new List<DateTime>();

            string[] filesInDirectory = Directory.GetFiles(dataFolder);
            foreach (string file in filesInDirectory)
            {
                string fileAbrev = file.Substring(file.LastIndexOf('\\')+1);
                if (fileAbrev.Substring(fileAbrev.Length - 4).ToLower() == ".bid" && fileAbrev.ToLower().StartsWith(filePrefix.ToLower()))
                {
                    if (bidParser.ParseHeader(file) == ReturnCode.SUCCESS)
                    {
                        bidFileList.Add(file);
                        bidDateList.Add(bidParser.GetDate());
                    }
                    else
                    {
                        // Something should really go here...
                    }
                }
            }

            bidFiles = bidFileList.ToArray();
            bidDates = bidDateList.ToArray();

            Array.Sort(bidDates, bidFiles);
        }

        public override void LoadData(DateTime startDate, DateTime endDate)
        {
            ReturnCode returnCode = ReturnCode.SUCCESS;

            int startIndex = Array.FindIndex(bidDates.ToArray(), x => x >= startDate);
            int endIndex = Array.FindIndex(bidDates.ToArray(), x => x >= endDate);

            if (startIndex == -1) return;
            if (endIndex == -1) endIndex = (bidDates.Length) - 1;

            DateTime time;

            for (int i = startIndex; i <= endIndex; ++i)
            {
                returnCode = bidParser.ParseFile(bidFiles[i]);
                int numRecords = bidParser.GetNumRecords();
                for (int r = 0; r < numRecords; ++r)
                {
                    time = bidParser.BIDTimeToDateTime(bidParser.GetRecord(r).time);
                    channels[chACountRate].AddDataPoint(time, bidParser.GetRecord(r).chACountRate, bidFiles[i]);
                    channels[chBCountRate].AddDataPoint(time, bidParser.GetRecord(r).chBCountRate, bidFiles[i]);
                    channels[chCCountRate].AddDataPoint(time, bidParser.GetRecord(r).chCCountRate, bidFiles[i]);
                    channels[gamInGamCh1].AddDataPoint(time, bidParser.GetRecord(r).gamInGamCh1, bidFiles[i]);
                    channels[gamCh1Sigma].AddDataPoint(time, bidParser.GetRecord(r).gamCh1Sigma, bidFiles[i]);
                    channels[gamInGamCh2].AddDataPoint(time, bidParser.GetRecord(r).gamInGamCh2, bidFiles[i]);
                    channels[gamCh2Sigma].AddDataPoint(time, bidParser.GetRecord(r).gamCh2Sigma, bidFiles[i]);
                }
            }
            channels[chACountRate].Sort();
            channels[chBCountRate].Sort();
            channels[chCCountRate].Sort();
            channels[gamInGamCh1].Sort();
            channels[gamCh1Sigma].Sort();
            channels[gamInGamCh2].Sort();
            channels[gamCh2Sigma].Sort();
            LoadVirtualChannels();
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
