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

        public ThresholdEG(EventWatcher parent, string newName, Channel newChannel, double newThreshold, TimeSpan newDebounceTime) : base(parent, newName)
        {
            eventGeneratorType = "Threshold";
            channel = newChannel;
            threshold = newThreshold;
            debounceTime = newDebounceTime;
        }

        public ThresholdEG(EventWatcher parent, string newName, Channel newChannel, double newThreshold) : base(parent, newName)
        {
            eventGeneratorType = "Threshold";
            channel = newChannel;
            threshold = newThreshold;
            debounceTime = TimeSpan.FromTicks(0);
        }

        public override List<Event> GenerateEvents(DateTime start, DateTime end)
        {
            List<DateTime> times = channel.GetTimeStamps();
            List<double> vals = channel.GetValues();
            List<TimeSpan> durations = null;
            if(channel.GetChannelType() == Channel.ChannelType.DURATION_VALUE)
                durations = channel.GetDurations();
            events = new List<Event>();
            Event eve = new Event(this);        // Really shouldn't need to make an event here but visual studio freaks out without it
            DateTime maxTime = new DateTime();
            double maxValue = 0;

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
                        eve.SetStartTime(times[i]);
                        eve.SetComment(channel.GetName() + " is above threshold.");
                        maxValue = vals[i];
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
                            eve.SetEndTime(times[i - 1] + durations[i - 1]);
                        else
                            eve.SetEndTime(times[i - 1]);
                        eve.SetMaxValue(maxValue);
                        eve.SetMaxTime(maxTime);
                        events.Add(eve);
                        inEvent = false;
                        onTheDrop = false;

                        if (vals[i] >= threshold)
                        {
                            eve = new Event(this);
                            eve.SetStartTime(times[i]);
                            eve.SetComment(channel.GetName() + " is above threshold.");
                            maxValue = vals[i];
                            maxTime = times[i];
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
                            eve.SetEndTime(lastDrop);
                            eve.SetMaxValue(maxValue);
                            eve.SetMaxTime(maxTime);
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
                        }
                        onTheDrop = false;
                    }
                }
            }
            if (inEvent)
            {
                eve.SetEndTime(times[times.Count - 1]);
                eve.SetMaxValue(maxValue);
                eve.SetMaxTime(maxTime);
                events.Add(eve);
                inEvent = false;
            }
            return events;
        }

        public void SetChannel(Channel newChannel) { channel = newChannel; }
        public void SetThreshold(double newThreshold) { threshold = newThreshold; }
        public void SetDebounceTime(TimeSpan newBounce) { debounceTime = newBounce; }

        public Channel GetChannel() { return channel; }
        public double GetThreshold() { return threshold; }
        public TimeSpan GetDebounceTime() { return debounceTime; }
        new public List<Event> GetEvents() { return events; }

        public override string GetName()
        {
            return name;
        }

        public override void SetName(string newName)
        {
            name = newName;
        }
    }

    public class ThresholdEGHookup : EventGeneratorHookup
    {
        public override string Type { get { return "Threshold"; } }

        public override EventGenerator GenerateFromXML(XmlNode eventNode, DetectionSystem system)
        {
            Channel channel = null;
            foreach (Instrument inst in system.GetInstruments())
            {
                foreach (Channel ch in inst.GetChannels())
                {
                    if (ch.GetName() == eventNode.Attributes["channel"]?.InnerText)
                        channel = ch;
                }
            }
            if (channel is null) return null;
            try
            {
                ThresholdEG eg = new ThresholdEG(system, eventNode.Attributes["name"]?.InnerText, channel, double.Parse(eventNode.Attributes["threshold"]?.InnerText));
                if (eventNode.Attributes["debounce_time"] != null)
                    eg.SetDebounceTime(TimeSpan.FromSeconds(double.Parse(eventNode.Attributes["debounce_time"]?.InnerText)));
                return eg;
            }
            catch
            {
                return null;
            }
        }
    }
}
