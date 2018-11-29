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
    class GapEG : EventGenerator
    {
        TimeSpan interval;
        Channel channel;

        public GapEG(EventWatcher parent, string newName, Channel newChannel, double newInterval) : base(parent, newName)
        {
            eventGeneratorType = "Gap";
            channel = newChannel;
            interval = TimeSpan.FromSeconds(newInterval);
        }

        public override List<Event> GenerateEvents(DateTime start, DateTime end)
        {
            List<DateTime> times = channel.GetTimeStamps();
            events = new List<Event>();
            Event eve = new Event(this);        // Really shouldn't need to make an event here but visual studio freaks out without it

            for (int i = 1; i < times.Count; i++)
            {
                if(times[i] - times[i-1] > interval)
                {
                    eve = new Event(this);
                    eve.SetStartTime(times[i-1]);
                    eve.SetEndTime(times[i]);
                    eve.SetMaxTime(times[i - 1]);
                    eve.SetMaxValue(0);
                    eve.SetComment(channel.GetName() + " has a gap.");
                    events.Add(eve);
                }
            }
            return events;
        }

        public override string GetName()
        {
            return name;
        }

        public override void SetName(string newName)
        {
            name = newName;
        }

        public void SetChannel(Channel newChannel) { channel = newChannel; }
        public Channel GetChannel() { return channel; }
        public void SetInterval(TimeSpan newInterval) { interval = newInterval; }
        public TimeSpan GetInterval() { return interval; }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new SystemChannelParameter("Channel", (DetectionSystem)eventWatcher){ Value = channel.GetName() },
                new TimeSpanParameter("Interval") { Value = interval.TotalSeconds.ToString() }
            };
            return parameters;
        }
    }

    public class GapEGHookup : EventGeneratorHookup
    {
        public override EventGenerator FromParameters(EventWatcher parent, string newName, List<Parameter> parameters)
        {
            Channel channel = null;
            double interval = 0;

            foreach(Parameter param in parameters)
            {
                switch(param.Name)
                {
                    case "Channel":
                        channel = ((SystemChannelParameter)param).ToChannel();
                        break;
                    case "Interval":
                        interval = ((TimeSpanParameter)param).ToTimeSpan().TotalSeconds;
                        break;
                }
            }
            return new GapEG(parent, newName, channel, interval);
        }

        public override string Type { get { return "Gap"; } }

        public GapEGHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
            {
                new ParameterTemplate("Channel", ParameterType.SystemChannel),
                new ParameterTemplate("Interval", ParameterType.TimeSpan)
            };
        }
    }
}
