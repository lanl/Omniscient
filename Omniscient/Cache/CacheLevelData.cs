// This software is open source software available under the BSD-3 license.
// 
// Copyright (c) 2018, Triad National Security, LLC
// All rights reserved.
// 
// Copyright 2018. Triad National Security, LLC. This software was produced under U.S. Government contract 89233218CNA000001 for Los Alamos National Laboratory (LANL), which is operated by Triad National Security, LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  NEITHER THE GOVERNMENT NOR TRIAD NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.  If software is modified to produce derivative works, such modified software should be clearly marked, so as not to confuse it with the version available from LANL.
// 
// Additionally, redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 1.       Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
// 2.       Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 
// 3.       Neither the name of Triad National Security, LLC, Los Alamos National Laboratory, LANL, the U.S. Government, nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 
//  
// THIS SOFTWARE IS PROVIDED BY TRIAD NATIONAL SECURITY, LLC AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL TRIAD NATIONAL SECURITY, LLC OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
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

        public ChannelCacheLevelData(CacheLevel level)
        {
            Level = level;
        }

        /// <summary>
        /// Returns the approximate memory usage of the ChannelCacheLevelData in bytes
        /// </summary>
        /// <returns></returns>
        public int GetMemoryUsage()
        {
            switch (Level)
            {
                case CacheLevel.Base:
                    return 24 * TimeStamps.Count + 8 * Durations.Count;
                default:
                    throw new NotImplementedException();
            }
        }

        public void ImportFromChannel(Channel channel, ChannelCompartment compartment)
        {
            TimeStamps = channel.GetTimeStamps(compartment);
            Durations = channel.GetDurations(compartment);
            Values = channel.GetValues(compartment);
            Files = channel.GetFiles(compartment);
        }

        public void ExportToChannel(Channel channel, ChannelCompartment compartment)
        {
            if (compartment == ChannelCompartment.View && channel.Hidden == true)
            {
                // No need to view hidden data
                return;
            }
            if (channel is VirtualChannel)
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
            else
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
}
