using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Omniscient.Instruments
{
    public abstract class Instrument
    {
        protected string name;
        protected string instrumentType;
        protected string dataFolder;
        protected string filePrefix;

        protected int numChannels;
        protected Channel[] channels;
        protected List<VirtualChannel> virtualChannels;

        public Instrument(string newName)
        {
            name = newName;
            virtualChannels = new List<VirtualChannel>();
        }

        public abstract void SetName(string newName);
        public abstract void ScanDataFolder();
        public abstract void LoadData(DateTime startDate, DateTime endDate);
        public abstract void ClearData();

        public void SetDataFolder(string newDataFolder)
        {
            dataFolder = newDataFolder;
            ScanDataFolder();
        }

        public void SetFilePrefix(string newPrefix)
        {
            filePrefix = newPrefix;
        }

        public string GetName() { return name; }
        public string GetInstrumentType() { return instrumentType; }
        public string GetDataFolder() { return dataFolder; }
        public string GetFilePrefix() { return filePrefix; }
        public int GetNumChannels() { return numChannels; }
        public Channel[] GetChannels()
        {
            Channel[] result = new Channel[channels.Length + virtualChannels.Count];
            for(int i = 0; i< result.Length;i++)
            {
                if (i < channels.Length)
                    result[i] = channels[i];
                else
                    result[i] = virtualChannels[i - channels.Length];
            }
            return channels;
        }
        //public Channel GetChannel(int chanNum)
        //{

        //    return channels[chanNum];
        //}
        public List<VirtualChannel> GetVirtualChannels() { return virtualChannels; }
    }
}
