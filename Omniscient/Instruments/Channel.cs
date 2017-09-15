using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient.Instruments
{
    class Channel
    {
        public enum ChannelType { COUNT_RATE, GAMMA_SPECTRUM, VIDEO};

        private string name;
        private ChannelType channelType;
        private List<DateTime> timeStamps;
        private List<double> values;

        public Channel(string name, ChannelType newType)
        {
            channelType = newType;
            timeStamps = new List<DateTime>();
            values = new List<double>();
        }

        public void AddDataPoint(DateTime time, double value)
        {
            timeStamps.Add(time);
            values.Add(value);
        }

        public string GetName() { return name; }
        public ChannelType GetChannelType() { return channelType; }
        public List<DateTime> GetTimeStamps() { return timeStamps; }
        public List<double> GetValues() { return values; }
    }
}
