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
    /// Generates events when events it watches for occur in coincidence.
    /// </summary>
    public class CoincidenceEG : EventGenerator
    {
        public enum CoincidenceType { A_THEN_B, B_THEN_A, EITHER_ORDER }
        public enum TimingType { START_TO_START, START_TO_END, END_TO_START, END_TO_END, MAX_TO_MAX}

        EventGenerator eventGeneratorA;
        EventGenerator eventGeneratorB;

        CoincidenceType coincidenceType;
        TimingType timingType;

        TimeSpan window;
        TimeSpan minDifference;

        public CoincidenceEG(DetectionSystem parent, string newName, uint id) : base(parent, newName, id)
        {
            eventGeneratorType = "Coincidence";
            coincidenceType = CoincidenceType.A_THEN_B;
            timingType = TimingType.START_TO_END;
            eventGeneratorA = null;
            eventGeneratorB = null;
            window = TimeSpan.FromTicks(0);
            minDifference = TimeSpan.FromTicks(0);
        }

        public CoincidenceEG(DetectionSystem parent, string newName, CoincidenceType newCoincidenceType, TimingType newTimingType, uint id) : base(parent, newName, id)
        {
            eventGeneratorType = "Coincidence";
            coincidenceType = newCoincidenceType;
            timingType = newTimingType;
            eventGeneratorA = null;
            eventGeneratorB = null;
            window = TimeSpan.FromTicks(0);
            minDifference = TimeSpan.FromTicks(0);
        }

        public override List<Event> GenerateEvents(DateTime start, DateTime end)
        {
            events = new List<Event>();

            List<Event> eventsA;
            List<Event> eventsB;

            if (coincidenceType == CoincidenceType.B_THEN_A)
            {
                eventsA = eventGeneratorB.GetEvents();
                eventsB = eventGeneratorA.GetEvents();
            }
            else
            {
                eventsA = eventGeneratorA.GetEvents();
                eventsB = eventGeneratorB.GetEvents();
            }

            if (eventsA.Count == 0 || eventsB.Count == 0) return events;

            int aIndex = 0;
            int bIndex = 0;

            DateTime aTime = new DateTime();
            DateTime bTime = new DateTime();
            TimeSpan delta;

            while(aIndex < eventsA.Count && bIndex < eventsB.Count)
            {
                switch (timingType)
                {
                    case TimingType.START_TO_START:
                        aTime = eventsA[aIndex].StartTime;
                        bTime = eventsB[bIndex].StartTime;
                        break;
                    case TimingType.START_TO_END:
                        aTime = eventsA[aIndex].StartTime;
                        bTime = eventsB[bIndex].EndTime;
                        break;
                    case TimingType.END_TO_START:
                        aTime = eventsA[aIndex].EndTime;
                        bTime = eventsB[bIndex].StartTime;
                        break;
                    case TimingType.END_TO_END:
                        aTime = eventsA[aIndex].EndTime;
                        bTime = eventsB[bIndex].EndTime;
                        break;
                    case TimingType.MAX_TO_MAX:
                        aTime = eventsA[aIndex].MaxTime;
                        bTime = eventsB[bIndex].MaxTime;
                        break;
                }
                delta = bTime - aTime;
                if (coincidenceType == CoincidenceType.EITHER_ORDER) delta = delta.Duration();
                if(delta <= window && delta >= minDifference)   // Coincidence event has occured
                {
                    Event eve = new Event(this);
                    if (aTime < bTime)
                    {
                        eve.StartTime = eventsA[aIndex].StartTime;
                        eve.EndTime = eventsB[bIndex].EndTime;
                    }
                    else
                    {
                        eve.StartTime = eventsB[bIndex].StartTime;
                        eve.EndTime = eventsA[aIndex].EndTime;
                    }
                    eve.Comment = "Coincidence!";
                    eve.MaxTime = eve.StartTime;
                    eve.MaxValue = 0;
                    eve.MeanValue = 0;
                    events.Add(eve);
                    aIndex++;
                    bIndex++;
                }
                else if(aTime < bTime)
                {
                    aIndex++;
                }
                else
                {
                    bIndex++;
                }
            }
            return events;
        }

        public void SetEventGeneratorA(EventGenerator newEg) { eventGeneratorA = newEg; }
        public void SetEventGeneratorB(EventGenerator newEg) {eventGeneratorB = newEg; }
        public void SetWindow(TimeSpan newWindow) { window = newWindow; }
        public void SetMinDifference(TimeSpan newDifference) { minDifference = newDifference; }

        public override List<Parameter> GetParameters()
        {
            string coincidenceTypeStr = "";
            switch (coincidenceType)
            {
                case CoincidenceType.A_THEN_B:
                    coincidenceTypeStr = "A Then B";
                    break;
                case CoincidenceType.B_THEN_A:
                    coincidenceTypeStr = "B Then A";
                    break;
                case CoincidenceType.EITHER_ORDER:
                    coincidenceTypeStr = "Either Order";
                    break;
            }
            string timingTypeStr = "";
            switch(timingType)
            {
                case TimingType.START_TO_START:
                    timingTypeStr = "Start to Start";
                    break;
                case TimingType.START_TO_END:
                    timingTypeStr = "Start to End";
                    break;
                case TimingType.END_TO_START:
                    timingTypeStr = "End to Start";
                    break;
                case TimingType.END_TO_END:
                    timingTypeStr = "End to End";
                    break;
                case TimingType.MAX_TO_MAX:
                    timingTypeStr = "Max to Max";
                    break;
            }
            List<Parameter> parameters = new List<Parameter>()
            {
                new EnumParameter("Coincidence Type") {Value = coincidenceTypeStr, ValidValues = new List<string>()
                {
                    "A Then B", "B Then A", "Either Order"
                } },
                new EnumParameter("Timing Type") {Value = timingTypeStr, ValidValues = new List<string>()
                {
                    "Start to Start", "Start to End", "End to Start", "End to End", "Max to Max"
                } },
                new SystemEventGeneratorParameter("Event Generator A", (DetectionSystem)eventWatcher){ Value = eventGeneratorA.Name },
                new SystemEventGeneratorParameter("Event Generator B", (DetectionSystem)eventWatcher){ Value = eventGeneratorB.Name },
                new TimeSpanParameter("Window") { Value = window.TotalSeconds.ToString() },
                new TimeSpanParameter("Min Difference") { Value = minDifference.TotalSeconds.ToString() }
            };
            return parameters;
        }
    }

    public class CoincidenceEGHookup : EventGeneratorHookup
    {
        public CoincidenceEGHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
            {
                new ParameterTemplate("Coincidence Type", ParameterType.Enum, new List<string>()
                {
                    "A Then B", "B Then A", "Either Order"
                }),
                new ParameterTemplate("Timing Type", ParameterType.Enum, new List<string>()
                {
                    "Start to Start", "Start to End", "End to Start", "End to End", "Max to Max"
                }),
                new ParameterTemplate("Event Generator A", ParameterType.SystemEventGenerator),
                new ParameterTemplate("Event Generator B", ParameterType.SystemEventGenerator),
                new ParameterTemplate("Window", ParameterType.TimeSpan),
                new ParameterTemplate("Min Difference", ParameterType.TimeSpan)
            };
        }

        public override EventGenerator FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            CoincidenceEG.CoincidenceType coincidenceType = CoincidenceEG.CoincidenceType.A_THEN_B;
            CoincidenceEG.TimingType timingType = CoincidenceEG.TimingType.END_TO_END;
            EventGenerator egA = null;
            EventGenerator egB = null;
            TimeSpan window = TimeSpan.FromTicks(0);
            TimeSpan minDifference = TimeSpan.FromTicks(0);
            
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Coincidence Type":
                        coincidenceType = (CoincidenceEG.CoincidenceType)((EnumParameter)param).ToInt();
                        break;
                    case "Timing Type":
                        timingType = (CoincidenceEG.TimingType)((EnumParameter)param).ToInt();
                        break;
                    case "Event Generator A":
                        egA = ((SystemEventGeneratorParameter)param).ToEventGenerator();
                        break;
                    case "Event Generator B":
                        egB = ((SystemEventGeneratorParameter)param).ToEventGenerator();
                        break;
                    case "Window":
                        window = ((TimeSpanParameter)param).ToTimeSpan();
                        break;
                    case "Min Difference":
                        minDifference = ((TimeSpanParameter)param).ToTimeSpan();
                        break;
                }
            }
            CoincidenceEG eg = new CoincidenceEG(parent, newName, coincidenceType, timingType, id);
            eg.SetEventGeneratorA(egA);
            eg.SetEventGeneratorB(egB);
            eg.SetWindow(window);
            eg.SetMinDifference(minDifference);
            return eg;
        }

        public override string Type { get { return "Coincidence"; } }
    }
}
