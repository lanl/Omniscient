using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public enum CacheLevel { Base, FiveSecond, OneMinute, TenMinute, TwoHour}
    public enum CacheDataType { Double, MMAC }
    public struct DateTimeRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
    public class CacheLevelData
    {
        public CacheLevel Level { get; private set; }
        public CacheDataType DataType { get; private set; }
        public Instrument Instrument { get; private set; }
        public DateTimeRange Range { get; private set; }
        public DateTime[] TimeStamps { get; private set; }
        public double[][] ChannelDoubleData { get; private set; }
        public double[][] ChannelMinData { get; private set; }
        public double[][] ChannelMaxData { get; private set; }
        public double[][] ChannelAveData { get; private set; }
        public int[][] ChannelCountData { get; private set; }
    }
}
