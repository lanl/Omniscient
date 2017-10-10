using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Omniscient.Parsers;

namespace Omniscient.Instruments
{
    class MCAInstrument : Instrument
    {
        private const int NUM_CHANNELS = 1;
        private const int COUNT_RATE = 0;

        CHNParser chnParser;

        string[] chnFiles;
        DateTime[] chnDates;

        public MCAInstrument(string newName) : base(newName)
        {
            instrumentType = "MCA";
            filePrefix = "";
            chnFiles = new string[0];
            chnDates = new DateTime[0];
            chnParser = new CHNParser();

            numChannels = NUM_CHANNELS;
            channels = new Channel[numChannels];
            channels[COUNT_RATE] = new Channel(name + "-Count_Rate", this, Channel.ChannelType.DURATION_VALUE);
        }

        public override void SetName(string newName)
        {
            name = newName;
            channels[COUNT_RATE].SetName(name + "-Count_Rate");
        }

        public override void ScanDataFolder()
        {
            List<string> chnFileList = new List<string>();
            List<DateTime> chnDateList = new List<DateTime>();

            string[] filesInDirectory = Directory.GetFiles(dataFolder);
            foreach (string file in filesInDirectory)
            {
                string fileAbrev = file.Substring(file.LastIndexOf('\\') + 1);
                if (fileAbrev.Substring(fileAbrev.Length - 4).ToLower() == ".chn" && fileAbrev.ToLower().StartsWith(filePrefix.ToLower()))
                {
                    if (chnParser.ParseFile(file) == ReturnCode.SUCCESS)
                    {
                        chnFileList.Add(file);
                        chnDateList.Add(chnParser.GetStartDateTime());
                    }
                    else
                    {
                        // Something should really go here...
                    }
                }
            }

            chnFiles = chnFileList.ToArray();
            chnDates = chnDateList.ToArray();

            Array.Sort(chnDates, chnFiles);
        }

        public override void LoadData(DateTime startDate, DateTime endDate)
        {
            ReturnCode returnCode = ReturnCode.SUCCESS;

            int startIndex = Array.FindIndex(chnDates.ToArray(), x => x >= startDate);
            int endIndex = Array.FindIndex(chnDates.ToArray(), x => x >= endDate);

            if (endIndex == -1) endIndex = (chnDates.Length) - 1;

            DateTime time;
            TimeSpan duration;

            for (int i = startIndex; i <= endIndex; ++i)
            {
                returnCode = chnParser.ParseFile(chnFiles[i]);
                time = chnParser.GetStartDateTime();
                duration = TimeSpan.FromSeconds(chnParser.GetRealTime());

                int counts = 0;
                for (int ch=0; ch<chnParser.GetNumChannels();ch++)
                {
                    counts += chnParser.GetCounts()[ch];
                }
                channels[COUNT_RATE].AddDataPoint(time, counts/chnParser.GetLiveTime(), duration); 
            }
            channels[COUNT_RATE].Sort();
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
