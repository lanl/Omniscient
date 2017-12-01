using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class Channel
    {
        public enum ChannelType { COUNT_RATE, DURATION_VALUE, GAMMA_SPECTRUM, VIDEO};

        protected Instrument instrument;
        protected string name;
        protected ChannelType channelType;
        protected List<DateTime> timeStamps;
        protected List<TimeSpan> durations;
        protected List<double> values;
        protected List<string> files;

        public Channel(string newName, Instrument parent, ChannelType newType)
        {
            name = newName;
            instrument = parent;
            channelType = newType;
            timeStamps = new List<DateTime>();
            durations = new List<TimeSpan>();
            values = new List<double>();
            files = new List<string>();
        }

        public void AddDataPoint(DateTime time, double value, string file)
        {
            timeStamps.Add(time);
            values.Add(value);
            files.Add(file);
        }

        public void AddDataPoint(DateTime time, double value, TimeSpan duration, string file)
        {
            timeStamps.Add(time);
            values.Add(value);
            durations.Add(duration);
            files.Add(file);
        }

        public void ClearData()
        {
            timeStamps = new List<DateTime>();
            values = new List<double>();
            files = new List<string>();
            if (channelType == ChannelType.DURATION_VALUE)
                durations = new List<TimeSpan>();
        }

        public void Sort()
        {
            DateTime[] stampArray = timeStamps.ToArray();
            double[] valueArray = values.ToArray();
            string[] fileArray = files.ToArray();

            if (channelType == ChannelType.DURATION_VALUE)
            {
                TimeSpan[] durationArray = durations.ToArray();
                Array.Sort(stampArray.ToArray(), durationArray);
                durations = durationArray.ToList();
            }
            Array.Sort(stampArray.ToArray(), fileArray);
            Array.Sort(stampArray, valueArray);

            timeStamps = stampArray.ToList();
            values = valueArray.ToList();
            files = fileArray.ToList();
        }

        public void SetName(string newName)
        {
            name = newName;
        }

        public string GetName() { return name; }
        public ChannelType GetChannelType() { return channelType; }
        public List<DateTime> GetTimeStamps() { return timeStamps; }
        public List<TimeSpan> GetDurations() { return durations; }
        public List<double> GetValues() { return values; }
        public List<string> GetFiles() { return files; }
        public Instrument GetInstrument() { return instrument; }
    }
}
