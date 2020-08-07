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
    class GapEG : EventGenerator
    {
        TimeSpan interval;
        Channel channel;

        public GapEG(DetectionSystem parent, string newName, Channel newChannel, double newInterval, uint id) : base(parent, newName, id)
        {
            eventGeneratorType = "Gap";
            channel = newChannel;
            interval = TimeSpan.FromSeconds(newInterval);
        }

        public override List<Event> GenerateEvents(DateTime start, DateTime end)
        {
            channel.GetInstrument().LoadData(ChannelCompartment.Process, start, end);
            List<DateTime> times = channel.GetTimeStamps(ChannelCompartment.Process);
            events = new List<Event>();
            Event eve = new Event(this);        // Really shouldn't need to make an event here but visual studio freaks out without it

            for (int i = 1; i < times.Count; i++)
            {
                if(times[i] - times[i-1] > interval)
                {
                    eve = new Event(this);
                    eve.StartTime = times[i-1];
                    eve.EndTime = times[i];
                    eve.MaxTime = times[i - 1];
                    eve.MaxValue = 0;
                    eve.MeanValue = 0;
                    eve.Comment = channel.Name + " has a gap.";
                    events.Add(eve);
                }
            }
            return events;
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new SystemChannelParameter("Channel", (DetectionSystem)eventWatcher){ Value = channel.Name },
                new TimeSpanParameter("Interval") { Value = interval.TotalSeconds.ToString() }
            };
            return parameters;
        }
    }

    public class GapEGHookup : EventGeneratorHookup
    {
        public override EventGenerator FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
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
            return new GapEG(parent, newName, channel, interval, id);
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
