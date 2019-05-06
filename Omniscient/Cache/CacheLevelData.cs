﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public enum CacheLevel { Base, FiveSecond, OneMinute, TenMinute, TwoHour}
    public enum CacheDataType { Double, MMAC }
    public class ChannelCacheLevelData
    {
        public CacheLevel Level { get; private set; }
        public CacheDataType DataType { get; private set; }
        public Instrument Instrument { get; private set; }
        public DateTimeRange Range { get; private set; }

        public List<DateTime> TimeStamps { get; private set; }
        public List<TimeSpan> Durations { get; private set; }
        public List<double> Values { get; private set; }
        public List<DataFile> Files { get; private set; }

        public double[][] ChannelMinData { get; private set; }
        public double[][] ChannelMaxData { get; private set; }
        public double[][] ChannelAveData { get; private set; }
        public int[][] ChannelCountData { get; private set; }

        public void ImportFromChannel(Channel channel, ChannelCompartment compartment)
        {
            TimeStamps = channel.GetTimeStamps(compartment);
            Durations = channel.GetDurations(compartment);
            Values = channel.GetValues(compartment);
            Files = channel.GetFiles(compartment);
        }

        public void ExportToChannel(Channel channel, ChannelCompartment compartment)
        {
            if (Durations.Count == 0)
            {
                for (int i = 0; i < TimeStamps.Count; i++)
                {
                    channel.AddDataPoint(compartment, TimeStamps[i], Values[i], Files[i]);
                }
            }
            else
            {
                for (int i = 0; i < TimeStamps.Count; i++)
                {
                    channel.AddDataPoint(compartment, TimeStamps[i], Values[i], Durations[i], Files[i]);
                }
            }
        }
    }
}
