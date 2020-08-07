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
    /// <summary>
    /// Generates events during which the channel value is at or above the threshold.
    /// </summary>
    public class ThresholdEG : EventGenerator
    {
        Channel channel;
        double threshold;
        TimeSpan debounceTime;

        public ThresholdEG(DetectionSystem parent, string newName, Channel newChannel, double newThreshold, TimeSpan newDebounceTime, uint id) : base(parent, newName, id)
        {
            eventGeneratorType = "Threshold";
            channel = newChannel;
            threshold = newThreshold;
            debounceTime = newDebounceTime;
        }

        public ThresholdEG(DetectionSystem parent, string newName, Channel newChannel, double newThreshold, uint id) : base(parent, newName, id)
        {
            eventGeneratorType = "Threshold";
            channel = newChannel;
            threshold = newThreshold;
            debounceTime = TimeSpan.FromTicks(0);
        }

        public override List<Event> GenerateEvents(DateTime start, DateTime end)
        {
            channel.GetInstrument().LoadData(ChannelCompartment.Process, start, end);
            List<DateTime> times = channel.GetTimeStamps(ChannelCompartment.Process);
            List<double> vals = channel.GetValues(ChannelCompartment.Process);
            List<TimeSpan> durations = null;
            if(channel.GetChannelType() == Channel.ChannelType.DURATION_VALUE)
                durations = channel.GetDurations(ChannelCompartment.Process);
            events = new List<Event>();
            Event eve = new Event(this);        // Really shouldn't need to make an event here but visual studio freaks out without it
            DateTime maxTime = new DateTime();
            double maxValue = 0;
            double runningSum = 0;
            int count = 0;

            bool inEvent = false;
            bool onTheDrop = false;
            DateTime lastDrop = new DateTime();
            for (int i=0; i<times.Count; i++)
            {
                if (!inEvent)
                {
                    // New event
                    if (vals[i] >= threshold)
                    {
                        eve = new Event(this);
                        eve.StartTime = times[i];
                        eve.Comment = channel.Name + " is above threshold.";
                        maxValue = vals[i];
                        runningSum = vals[i];
                        count = 1;
                        maxTime = times[i];
                        inEvent = true;
                        onTheDrop = false;
                    }
                }
                else
                {
                    // Event ended and new one created by exceeding debounce time between data points
                    TimeSpan deltaT;
                    if (durations != null)
                        deltaT = times[i] - (times[i - 1] + durations[i - 1]);
                    else
                        deltaT = times[i] - times[i - 1];
                    if (deltaT >= debounceTime)
                    {
                        if (durations != null)
                            eve.EndTime = times[i - 1] + durations[i - 1];
                        else
                            eve.EndTime = times[i - 1];
                        eve.MaxValue = maxValue;
                        eve.MaxTime = maxTime;
                        eve.MeanValue = runningSum / count;
                        events.Add(eve);
                        inEvent = false;
                        onTheDrop = false;

                        if (vals[i] >= threshold)
                        {
                            eve = new Event(this);
                            eve.StartTime = times[i];
                            eve.Comment = channel.Name + " is above threshold.";
                            maxValue = vals[i];
                            maxTime = times[i];
                            runningSum = vals[i];
                            count = 1;
                            inEvent = true;
                            onTheDrop = false;
                        }
                    }
                    else if (vals[i] < threshold)
                    {
                        if (!onTheDrop)
                        {
                            if (durations != null)
                                lastDrop = times[i] + durations[i];
                            else
                                lastDrop = times[i];
                            onTheDrop = true;
                        }

                        // Event ended by dropping below threshold
                        if (times[i] - lastDrop >= debounceTime)
                        {
                            eve.EndTime = lastDrop;
                            eve.MaxValue = maxValue;
                            eve.MaxTime = maxTime;
                            eve.MeanValue = runningSum / count;
                            events.Add(eve);
                            inEvent = false;
                            onTheDrop = false;
                        }
                    }
                    else
                    {
                        // In event
                        if (vals[i] > maxValue)
                        {
                            maxValue = vals[i];
                            maxTime = times[i];
                            runningSum += vals[i];
                            count++;
                        }
                        onTheDrop = false;
                    }
                }
            }
            if (inEvent)
            {
                eve.EndTime = times[times.Count - 1];
                eve.MaxValue = maxValue;
                eve.MaxTime = maxTime;
                eve.MeanValue = runningSum / count;
                events.Add(eve);
                inEvent = false;
            }
            return events;
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new SystemChannelParameter("Channel", (DetectionSystem)eventWatcher){ Value = channel.Name },
                new DoubleParameter("Threshold") { Value = threshold.ToString() },
                new TimeSpanParameter("Debounce Time") { Value = debounceTime.TotalSeconds.ToString() }
            };
            return parameters;
        }
    }

    public class ThresholdEGHookup : EventGeneratorHookup
    {
        public ThresholdEGHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
            {
                new ParameterTemplate("Channel", ParameterType.SystemChannel),
                new ParameterTemplate("Threshold", ParameterType.Double),
                new ParameterTemplate("Debounce Time", ParameterType.TimeSpan)
            };
        }

        public override string Type { get { return "Threshold"; } }

        public override EventGenerator FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            Channel channel = null;
            double threshold = 0;
            TimeSpan debounceTime = TimeSpan.FromTicks(0);
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Channel":
                        channel = ((SystemChannelParameter)param).ToChannel();
                        break;
                    case "Threshold":
                        threshold = ((DoubleParameter)param).ToDouble();
                        break;
                    case "Debounce Time":
                        debounceTime = ((TimeSpanParameter)param).ToTimeSpan();
                        break;
                }
            }
            return new ThresholdEG(parent, newName, channel, threshold, debounceTime, id);
        }
    }
}
