using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient.Instruments
{
    public class Channel
    {
        public enum ChannelType { COUNT_RATE, DURATION_VALUE, GAMMA_SPECTRUM, VIDEO};

        private Instrument instrument;
        private string name;
        private ChannelType channelType;
        private List<DateTime> timeStamps;
        private List<TimeSpan> durations;
        private List<double> values;

        public Channel(string newName, Instrument parent, ChannelType newType)
        {
            name = newName;
            instrument = parent;
            channelType = newType;
            timeStamps = new List<DateTime>();
            durations = new List<TimeSpan>();
            values = new List<double>();
        }

        public void AddDataPoint(DateTime time, double value)
        {
            timeStamps.Add(time);
            values.Add(value);
        }

        public void AddDataPoint(DateTime time, double value, TimeSpan duration)
        {
            timeStamps.Add(time);
            values.Add(value);
            durations.Add(duration);
        }

        public void ClearData()
        {
            timeStamps = new List<DateTime>();
            values = new List<double>();
            if (channelType == ChannelType.DURATION_VALUE)
                durations = new List<TimeSpan>();
        }

        public void Sort()
        {
            DateTime[] stampArray = timeStamps.ToArray();
            double[] valueArray = values.ToArray();

            if (channelType == ChannelType.DURATION_VALUE)
            {
                TimeSpan[] durationArray = durations.ToArray();
                Array.Sort(stampArray.ToArray(), durationArray);
                durations = durationArray.ToList();
            }

            Array.Sort(stampArray, valueArray);

            timeStamps = stampArray.ToList();
            values = valueArray.ToList();
        }

        public string GetName() { return name; }
        public ChannelType GetChannelType() { return channelType; }
        public List<DateTime> GetTimeStamps() { return timeStamps; }
        public List<TimeSpan> GetDurations() { return durations; }
        public List<double> GetValues() { return values; }
        public Instrument GetInstrument() { return instrument; }
    }
}
