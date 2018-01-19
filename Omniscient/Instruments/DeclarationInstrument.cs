using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Omniscient
{
    public class DeclarationInstrument : Instrument
    {

        private const int NUM_CHANNELS = 1;
        private const int DECLARATION = 0;

        DECFile decParser;

        string[] decFiles;
        DateTime[] decDates;

        public DeclarationInstrument(string newName) : base(newName)
        {
            instrumentType = "Declaration";
            filePrefix = "";
            decFiles = new string[0];
            decDates = new DateTime[0];
            decParser = new DECFile();

            numChannels = NUM_CHANNELS;
            channels = new Channel[numChannels];
            channels[DECLARATION] = new Channel(name + "-Declarations", this, Channel.ChannelType.DURATION_VALUE);
        }

        public override void SetName(string newName)
        {
            name = newName;
            channels[DECLARATION].SetName(name + "-Declarations");
        }

        public override void ScanDataFolder()
        {
            List<string> decFileList = new List<string>();
            List<DateTime> decDateList = new List<DateTime>();

            string[] filesInDirectory = Directory.GetFiles(dataFolder);
            foreach (string file in filesInDirectory)
            {
                string fileAbrev = file.Substring(file.LastIndexOf('\\') + 1);
                if (fileAbrev.Substring(fileAbrev.Length - 4).ToLower() == ".dec" && fileAbrev.ToLower().StartsWith(filePrefix.ToLower()))
                {
                    if (decParser.ParseDeclarationFile(file) == ReturnCode.SUCCESS)
                    {
                        decFileList.Add(file);
                        decDateList.Add(decParser.FromTime);
                    }
                    else
                    {
                        // Something should really go here...
                    }
                }
            }
        }

        public override void LoadData(DateTime startDate, DateTime endDate)
        {
            ReturnCode returnCode = ReturnCode.SUCCESS;

            int startIndex = Array.FindIndex(decDates.ToArray(), x => x >= startDate);
            int endIndex = Array.FindIndex(decDates.ToArray(), x => x >= endDate);

            if (endIndex == -1) endIndex = (decDates.Length) - 1;

            DateTime time;
            TimeSpan duration;

            for (int i = startIndex; i <= endIndex; ++i)
            {
                returnCode = decParser.ParseDeclarationFile(decFiles[i]);
                time = decParser.FromTime;
                duration = decParser.ToTime - decParser.FromTime;

                channels[DECLARATION].AddDataPoint(time, 1, duration, decFiles[i]);
            }
            channels[DECLARATION].Sort();
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
