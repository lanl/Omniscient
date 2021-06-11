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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Omniscient
{
    public enum ChannelCompartment { View=0, Process=1, Cache=2};
    public class Channel : Persister
    {
        private const int N_COMPARTMENTS = 3;

        public override string Species { get { return "Channel"; } }

        public enum ChannelType { COUNT_RATE, DURATION_VALUE, GAMMA_SPECTRUM, VIDEO};

        protected Instrument instrument;
        protected ChannelType channelType;
        protected List<DateTime>[] timeStamps;
        protected List<TimeSpan>[] durations;
        protected List<double>[] values;
        protected List<DataFile>[] files;

        /// <summary>
        /// A hidden Channel does not appear in the MainWindow ChannelPanel 
        /// but can still be used as a source of data for VirtualChannels.
        /// </summary>
        public bool Hidden { get; set; }

        public Channel(string newName, Instrument parent, ChannelType newType, uint id) : base(parent, newName, id)
        {
            instrument = parent;


            channelType = newType;
            timeStamps = new List<DateTime>[N_COMPARTMENTS];
            durations = new List<TimeSpan>[N_COMPARTMENTS];
            values = new List<double>[N_COMPARTMENTS];
            files = new List<DataFile>[N_COMPARTMENTS];
            for (int c = 0; c<N_COMPARTMENTS; c++)
            {
                timeStamps[c] = new List<DateTime>();
                durations[c] = new List<TimeSpan>();
                values[c] = new List<double>();
                files[c] = new List<DataFile>();
            }

            Hidden = false;
        }

        public void AddDataPoint(ChannelCompartment compartment, DateTime time, double value, DataFile file)
        {
            timeStamps[(int)compartment].Add(time);
            values[(int)compartment].Add(value);
            files[(int)compartment].Add(file);

            List<DateTime> times = timeStamps[(int)compartment];
            if (times.Count() == 0 || times[0] < time) // Add to end
            {
                timeStamps[(int)compartment].Add(time);
                values[(int)compartment].Add(value);
                files[(int)compartment].Add(file);
            }
            else if (times[0] >= time) // Add to start
            {
                times.Insert(0, time);
                values[(int)compartment].Insert(0, value);
                files[(int)compartment].Insert(0, file);
            }
            else // Add to middle
            {
                int min = 1;
                int max = times.Count() - 2;
                int mid;
                while (min < max)
                {
                    mid = (min + max) / 2;
                    if (times[mid] < time) min = mid;
                    else max = mid;
                    if (max == min + 1) break;
                }
                times.Insert(max, time);
                values[(int)compartment].Insert(max, value);
                files[(int)compartment].Insert(max, file);
            }
        }

        public void AddDataPoints(ChannelCompartment compartment, IEnumerable<DateTime> timeE, IEnumerable<double> valueE, IEnumerable<DataFile> fileE)
        {
            if (timeE.Count() == 0) return;
            List<DateTime> times = timeStamps[(int)compartment];
            DateTime firstT = timeE.First();
            if (times.Count() == 0 || times[0] < firstT) // Add to end
            {
                times.AddRange(timeE);
                values[(int)compartment].AddRange(valueE);
                files[(int)compartment].AddRange(fileE);
            }
            else if (times[0] > firstT) // Add to start
            {
                times.InsertRange(0, timeE);
                values[(int)compartment].InsertRange(0, valueE);
                files[(int)compartment].InsertRange(0, fileE);
            }
            else // Add to middle
            {
                int min = 1;
                int max = times.Count() - 2;
                int mid;
                while (min < max)
                {
                    mid = (min + max) / 2;
                    if (times[mid] < firstT) min = mid;
                    else max = mid;
                    if (max == min + 1) break;
                }
                times.InsertRange(max, timeE);
                values[(int)compartment].InsertRange(max, valueE);
                files[(int)compartment].InsertRange(max, fileE);
            }
        }

        public void AddDataPoint(ChannelCompartment compartment, DateTime time, double value, TimeSpan duration, DataFile file)
        {
            timeStamps[(int)compartment].Add(time);
            values[(int)compartment].Add(value);
            durations[(int)compartment].Add(duration);
            files[(int)compartment].Add(file);

            List<DateTime> times = timeStamps[(int)compartment];
            if (times.Count() == 0 || times[0] < time) // Add to end
            {
                timeStamps[(int)compartment].Add(time);
                values[(int)compartment].Add(value);
                durations[(int)compartment].Add(duration);
                files[(int)compartment].Add(file);
            }
            else if (times[0] > time) // Add to start
            {
                times.Insert(0, time);
                values[(int)compartment].Insert(0, value);
                durations[(int)compartment].Insert(0, duration);
                files[(int)compartment].Insert(0, file);
            }
            else // Add to middle
            {
                int min = 1;
                int max = times.Count() - 2;
                int mid;
                while (min < max)
                {
                    mid = (min + max) / 2;
                    if (times[mid] < time) min = mid;
                    else max = mid;
                    if (max == min + 1) break;
                }
                times.Insert(max, time);
                values[(int)compartment].Insert(max, value);
                durations[(int)compartment].Insert(max, duration);
                files[(int)compartment].Insert(max, file);
            }
        }

        public void AddDataPoints(ChannelCompartment compartment, IEnumerable<DateTime> timeE, IEnumerable<double> valueE, IEnumerable<TimeSpan> durationsE, IEnumerable<DataFile> fileE)
        {
            if (timeE.Count() == 0) return;
            List<DateTime> times = timeStamps[(int)compartment];
            DateTime firstT = timeE.First();
            if (times.Count() == 0 || times[0] < firstT) // Add to end
            {
                times.AddRange(timeE);
                values[(int)compartment].AddRange(valueE);
                durations[(int)compartment].AddRange(durationsE);
                files[(int)compartment].AddRange(fileE);
            }
            else if (times[0] > firstT) // Add to start
            {
                times.InsertRange(0, timeE);
                values[(int)compartment].InsertRange(0, valueE);
                durations[(int)compartment].InsertRange(0, durationsE);
                files[(int)compartment].InsertRange(0, fileE);
            }
            else // Add to middle
            {
                int min = 1;
                int max = times.Count() - 2;
                int mid;
                while (min < max)
                {
                    mid = (min + max) / 2;
                    if (times[mid] < firstT) min = mid;
                    else max = mid;
                    if (max == min + 1) break;
                }
                times.InsertRange(max, timeE);
                values[(int)compartment].InsertRange(max, valueE);
                durations[(int)compartment].InsertRange(max, durationsE);
                files[(int)compartment].InsertRange(max, fileE);
            }
        }

        public void ClearData(ChannelCompartment compartment)
        {
            timeStamps[(int)compartment] = new List<DateTime>();
            values[(int)compartment] = new List<double>();
            files[(int)compartment] = new List<DataFile>();
            if (channelType == ChannelType.DURATION_VALUE)
                durations[(int)compartment] = new List<TimeSpan>();
        }

        public void Sort(ChannelCompartment compartment)
        {
            DateTime[] stampArray = timeStamps[(int)compartment].ToArray();
            double[] valueArray = values[(int)compartment].ToArray();
            DataFile[] fileArray = files[(int)compartment].ToArray();

            if (channelType == ChannelType.DURATION_VALUE)
            {
                TimeSpan[] durationArray = durations[(int)compartment].ToArray();
                Array.Sort(stampArray.ToArray(), durationArray);
                durations[(int)compartment] = durationArray.ToList();
            }
            Array.Sort(stampArray.ToArray(), fileArray);
            Array.Sort(stampArray, valueArray);

            timeStamps[(int)compartment] = stampArray.ToList();
            values[(int)compartment] = valueArray.ToList();
            files[(int)compartment] = fileArray.ToList();
        }

        public List<DataFile> GetFiles(ChannelCompartment compartment, DateTime start, DateTime end)
        {
            List<DataFile> outFiles = new List<DataFile>();
            for(int i=0; i<files.Count(); i++)
            {
                if(timeStamps[(int)compartment][i] >= start && timeStamps[(int)compartment][i] <= end)
                {
                    if (!outFiles.Contains(files[(int)compartment][i])) outFiles.Add(files[(int)compartment][i]);
                }
            }
            return outFiles;
        }
        
        public ChannelType GetChannelType() { return channelType; }
        public List<DateTime> GetTimeStamps(ChannelCompartment compartment) { return timeStamps[(int)compartment]; }
        public List<TimeSpan> GetDurations(ChannelCompartment compartment) { return durations[(int)compartment]; }
        public List<double> GetValues(ChannelCompartment compartment) { return values[(int)compartment]; }
        public virtual List<DataFile> GetFiles(ChannelCompartment compartment) { return files[(int)compartment]; }
        public Instrument GetInstrument() { return instrument; }

        public double GetAverage(ChannelCompartment compartment, DateTime start, DateTime end)
        {
            int count = timeStamps[(int)compartment].Count;
            int startIndex = FindFirstIndexAfter(compartment, start);
            int endIndex = FindFirstIndexAfter(compartment, end);
            if (startIndex >= count || endIndex == 0 || startIndex>=endIndex) return double.NaN;

            double sum = 0;
            for (int i=startIndex; i<endIndex; ++i)
            {
                sum += values[(int)compartment][i];
            }
            return sum / (endIndex - startIndex);
        }

        public double GetStandardDeviation(ChannelCompartment compartment, DateTime start, DateTime end)
        {
            int count = timeStamps[(int)compartment].Count;
            int startIndex = FindFirstIndexAfter(compartment, start);
            int endIndex = FindFirstIndexAfter(compartment, end);
            if (startIndex >= count || endIndex == 0 || startIndex >= endIndex) return double.NaN;

            double average = GetAverage(compartment, start, end);
            double sumSquareDeviations = 0;
            for (int i=startIndex; i<endIndex; i++)
            {
                sumSquareDeviations += (values[(int)compartment][i] - average) * (values[(int)compartment][i] - average);
            }

            return Math.Sqrt(sumSquareDeviations / (endIndex - startIndex));
        }

        public double GetMax(ChannelCompartment compartment, DateTime start, DateTime end)
        {
            int count = timeStamps[(int)compartment].Count;
            int startIndex = FindFirstIndexAfter(compartment, start);
            int endIndex = FindFirstIndexAfter(compartment, end);
            if (startIndex >= count || endIndex == 0 || startIndex >= endIndex) return double.NaN;

            double max = double.MinValue;
            for (int i=startIndex; i<endIndex; i++)
            {
                if (values[(int)compartment][i] > max) max = values[(int)compartment][i];
            }
            return max;
        }

        public double GetMin(ChannelCompartment compartment, DateTime start, DateTime end)
        {
            int count = timeStamps[(int)compartment].Count;
            int startIndex = FindFirstIndexAfter(compartment, start);
            int endIndex = FindFirstIndexAfter(compartment, end);
            if (startIndex >= count || endIndex == 0 || startIndex >= endIndex) return double.NaN;

            double min = double.MaxValue;
            for (int i = startIndex; i < endIndex; i++)
            {
                if (values[(int)compartment][i] < min) min = values[(int)compartment][i];
            }
            return min;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Binary search FTW</remarks>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public int FindFirstIndexAfter(ChannelCompartment compartment, DateTime dateTime)
        {
            int count = timeStamps[(int)compartment].Count;
            if (count == 0 || timeStamps[(int)compartment][0] > dateTime) return 0;
            if (dateTime > timeStamps[(int)compartment][count - 1]) return count;

            int lastSmallerIndex = 0;
            int lastBiggerIndex = count - 1;
            int index = count / 2;
            int nextIndex;
            while (true)
            {
                if (timeStamps[(int)compartment][index] > dateTime)
                {
                    // Go down
                    if (timeStamps[(int)compartment][index - 1] < dateTime) return index;
                    nextIndex = (lastSmallerIndex + index) / 2;
                    lastBiggerIndex = index;
                    index = nextIndex;
                }
                else
                {
                    // Go up
                    if (timeStamps[(int)compartment][index + 1] > dateTime) return index + 1;
                    nextIndex = (lastBiggerIndex + index) / 2;
                    lastSmallerIndex = index;
                    index = nextIndex;
                }
            }
        }

        public List<Parameter> GetParameters()
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new BoolParameter("Hidden", Hidden)
            };
            return parameters;
        }

        public void ApplyParameters(List<Parameter> parameters)
        {
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Hidden":
                        Hidden = ((BoolParameter) param).ToBool();
                        break;
                }
            }
        }

        public static List<ParameterTemplate> TemplateParameters { get; private set; } = new List<ParameterTemplate>()
        {
            new ParameterTemplate("Hidden", ParameterType.Bool)
        };

        public override void ToXML(XmlWriter xmlWriter)
        {
            StartToXML(xmlWriter);
            List<Parameter> parameters = GetParameters();
            foreach (Parameter param in parameters)
            {
                xmlWriter.WriteAttributeString(param.Name.Replace(' ', '_'), param.Value);
            }
            xmlWriter.WriteEndElement();
        }

        public void ApplyXML(XmlNode node)
        {
            string name;
            uint id;
            Persister.StartFromXML(node, out name, out id);
            Name = name;
            if(id>0) ID = id;
            List<Parameter> parameters = Parameter.FromXML(node, TemplateParameters);
            ApplyParameters(parameters);
        }

        public override bool SetIndex(int index)
        {
            throw new InvalidOperationException("Standard channels have fixed indices!");
        }
    }
}
