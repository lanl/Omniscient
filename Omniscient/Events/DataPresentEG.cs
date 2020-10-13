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
    public class DataPresentEG : EventGenerator
    {
        Channel channel;
        public DataPresentEG(DetectionSystem parent, string newName, Channel newChannel, uint id) : base(parent, newName, id)
        {
            eventGeneratorType = "Data Present";
            channel = newChannel;
        }

        public override List<Event> GenerateEvents(DateTime start, DateTime end)
        {
            channel.GetInstrument().LoadData(ChannelCompartment.Process, start, end);
            events = new List<Event>();
            List<DateTime> times = channel.GetTimeStamps(ChannelCompartment.Process);
            List<TimeSpan> durations = channel.GetDurations(ChannelCompartment.Process);
            List<double> vals = channel.GetValues(ChannelCompartment.Process);
            if (durations is null || durations.Count != times.Count) return events;

            // Fast forward to start time
            int startIndex = 0;
            while (startIndex < times.Count && times[startIndex] <= start) startIndex++;

            Event eve = new Event(this);
            for (int i = startIndex; i < times.Count; i++)
            {
                if (times[i] > end) break; // Exit loop at end time

                eve = new Event(this, times[i], times[i] + durations[i]);
                eve.MaxValue = vals[i];
                eve.MaxTime = times[i] + TimeSpan.FromTicks(durations[i].Ticks / 2);
                eve.Comment = "Data present in " + channel.Name;
                events.Add(eve);
            }
            return events;
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new SystemChannelParameter("Channel", (DetectionSystem)eventWatcher){ Value = channel.Name }
            };
            return parameters;
        }
    }

    public class DataPresentEGHookup : EventGeneratorHookup
    {
        public DataPresentEGHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
            {
                new ParameterTemplate("Channel", ParameterType.SystemChannel)
            };
        }

        public override string Type { get { return "Data Present"; } }

        public override EventGenerator FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            Channel channel = null;
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Channel":
                        channel = ((SystemChannelParameter)param).ToChannel();
                        break;
                }
            }
            return new DataPresentEG(parent, newName, channel, id);
        }
    }
}
