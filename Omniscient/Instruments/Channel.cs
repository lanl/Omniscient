// This software is open source software available under the BSD-3 license.
// 
// Copyright (c) 2018, Los Alamos National Security, LLC
// All rights reserved.
// 
// Copyright 2018. Los Alamos National Security, LLC. This software was produced under U.S. Government contract DE-AC52-06NA25396 for Los Alamos National Laboratory (LANL), which is operated by Los Alamos National Security, LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  NEITHER THE GOVERNMENT NOR LOS ALAMOS NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.  If software is modified to produce derivative works, such modified software should be clearly marked, so as not to confuse it with the version available from LANL.
// 
// Additionally, redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 1.       Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
// 2.       Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 
// 3.       Neither the name of Los Alamos National Security, LLC, Los Alamos National Laboratory, LANL, the U.S. Government, nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 
//  
// THIS SOFTWARE IS PROVIDED BY LOS ALAMOS NATIONAL SECURITY, LLC AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL LOS ALAMOS NATIONAL SECURITY, LLC OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
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

        public List<string> GetFiles(DateTime start, DateTime end)
        {
            List<string> outFiles = new List<string>();
            for(int i=0; i<files.Count(); i++)
            {
                if(timeStamps[i] >= start && timeStamps[i] <= end)
                {
                    if (!outFiles.Contains(files[i])) outFiles.Add(files[i]);
                }
            }
            return outFiles;
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
