using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient.Instruments
{
    public class Channel
    {
        public enum ChannelType { COUNT_RATE, GAMMA_SPECTRUM, VIDEO};

        private Instrument instrument;
        private string name;
        private ChannelType channelType;
        private List<DateTime> timeStamps;
        private List<double> values;
        private int lastIndex;

        public Channel(string newName, Instrument parent, ChannelType newType)
        {
            name = newName;
            instrument = parent;
            channelType = newType;
            timeStamps = new List<DateTime>();
            values = new List<double>();
            lastIndex = -1;
        }

        public void AddDataPoint(DateTime time, double value)
        {
            timeStamps.Add(time);
            values.Add(value);
        }

        public void ClearData()
        {
            timeStamps = new List<DateTime>();
            values = new List<double>();
            lastIndex = -1;
        }

        public void Sort()
        {
            DateTime[] stampArray = timeStamps.ToArray();
            double[] valueArray = values.ToArray();

            Array.Sort(stampArray, valueArray);

            timeStamps = stampArray.ToList();
            values = valueArray.ToList();
        }

        public string GetName() { return name; }
        public ChannelType GetChannelType() { return channelType; }
        public List<DateTime> GetTimeStamps() { return timeStamps; }
        public List<double> GetValues() { return values; }
        public Instrument GetInstrument() { return instrument; }

        
    }
}
