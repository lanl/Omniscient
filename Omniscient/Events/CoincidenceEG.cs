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

        public CoincidenceEG(EventWatcher parent, string newName) : base(parent, newName)
        {
            eventGeneratorType = "Coincidence";
            coincidenceType = CoincidenceType.A_THEN_B;
            timingType = TimingType.START_TO_END;
            eventGeneratorA = null;
            eventGeneratorB = null;
            window = TimeSpan.FromTicks(0);
            minDifference = TimeSpan.FromTicks(0);
        }

        public CoincidenceEG(EventWatcher parent, string newName, CoincidenceType newCoincidenceType, TimingType newTimingType) : base(parent, newName)
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
                        aTime = eventsA[aIndex].GetStartTime();
                        bTime = eventsB[bIndex].GetStartTime();
                        break;
                    case TimingType.START_TO_END:
                        aTime = eventsA[aIndex].GetStartTime();
                        bTime = eventsB[bIndex].GetEndTime();
                        break;
                    case TimingType.END_TO_START:
                        aTime = eventsA[aIndex].GetEndTime();
                        bTime = eventsB[bIndex].GetStartTime();
                        break;
                    case TimingType.END_TO_END:
                        aTime = eventsA[aIndex].GetEndTime();
                        bTime = eventsB[bIndex].GetEndTime();
                        break;
                    case TimingType.MAX_TO_MAX:
                        aTime = eventsA[aIndex].GetMaxTime();
                        bTime = eventsB[bIndex].GetMaxTime();
                        break;
                }
                delta = bTime - aTime;
                if (coincidenceType == CoincidenceType.EITHER_ORDER) delta = delta.Duration();
                if(delta <= window && delta >= minDifference)   // Coincidence event has occured
                {
                    Event eve = new Event(this);
                    if (aTime < bTime)
                    {
                        eve.SetStartTime(eventsA[aIndex].GetStartTime());
                        eve.SetEndTime(eventsB[bIndex].GetEndTime());
                    }
                    else
                    {
                        eve.SetStartTime(eventsB[bIndex].GetStartTime());
                        eve.SetEndTime(eventsA[aIndex].GetEndTime());
                    }
                    eve.SetComment("Coincidence!");
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
        public void SetCoincidenceType(CoincidenceType newCoincidenceType) { coincidenceType = newCoincidenceType; }
        public void SetTimingType(TimingType newTimingType) { timingType = newTimingType; }
        public void SetWindow(TimeSpan newWindow) { window = newWindow; }
        public void SetMinDifference(TimeSpan newDifference) { minDifference = newDifference; }

        public EventGenerator GetEventGeneratorA() { return eventGeneratorA; }
        public EventGenerator GetEventGeneratorB() { return eventGeneratorB; }
        public CoincidenceType GetCoincidenceType() { return coincidenceType; }
        public TimingType GetTimingType() { return timingType; }
        public TimeSpan GetWindow() { return window; }
        public TimeSpan GetMinDifference() { return minDifference; }

        public override string GetName()
        {
            return name;
        }

        public override void SetName(string newName)
        {
            name = newName;
        }

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
                new SystemEventGeneratorParameter("Event Generator A", (DetectionSystem)eventWatcher){ Value = eventGeneratorA.GetName() },
                new SystemEventGeneratorParameter("Event Generator B", (DetectionSystem)eventWatcher){ Value = eventGeneratorB.GetName() },
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

        public override EventGenerator FromParameters(EventWatcher parent, string newName, List<Parameter> parameters)
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
            CoincidenceEG eg = new CoincidenceEG(parent, newName, coincidenceType, timingType);
            eg.SetEventGeneratorA(egA);
            eg.SetEventGeneratorB(egB);
            eg.SetWindow(window);
            eg.SetMinDifference(minDifference);
            return eg;
        }

        public override string Type { get { return "Coincidence"; } }

        public override EventGenerator GenerateFromXML(XmlNode eventNode, DetectionSystem system)
        {
            CoincidenceEG coinkEG;
            try
            {
                coinkEG = new CoincidenceEG(system, eventNode.Attributes["name"]?.InnerText);
                switch (eventNode.Attributes["coincidence_type"]?.InnerText)
                {
                    case "A_THEN_B":
                        coinkEG.SetCoincidenceType(CoincidenceEG.CoincidenceType.A_THEN_B);
                        break;
                    case "B_THEN_A":
                        coinkEG.SetCoincidenceType(CoincidenceEG.CoincidenceType.B_THEN_A);
                        break;
                    case "EITHER_ORDER":
                        coinkEG.SetCoincidenceType(CoincidenceEG.CoincidenceType.EITHER_ORDER);
                        break;
                    default:
                        return null;
                }
                switch (eventNode.Attributes["timing_type"]?.InnerText)
                {
                    case "START_TO_START":
                        coinkEG.SetTimingType(CoincidenceEG.TimingType.START_TO_START);
                        break;
                    case "START_TO_END":
                        coinkEG.SetTimingType(CoincidenceEG.TimingType.START_TO_END);
                        break;
                    case "END_TO_START":
                        coinkEG.SetTimingType(CoincidenceEG.TimingType.END_TO_START);
                        break;
                    case "END_TO_END":
                        coinkEG.SetTimingType(CoincidenceEG.TimingType.END_TO_END);
                        break;
                    case "MAX_TO_MAX":
                        coinkEG.SetTimingType(CoincidenceEG.TimingType.MAX_TO_MAX);
                        break;
                    default:
                        return null;
                }
                foreach (EventGenerator watchedEG in system.GetEventGenerators())
                {
                    if (watchedEG.GetName() == eventNode.Attributes["event_generator_A"]?.InnerText)
                        coinkEG.SetEventGeneratorA(watchedEG);
                    if (watchedEG.GetName() == eventNode.Attributes["event_generator_B"]?.InnerText)
                        coinkEG.SetEventGeneratorB(watchedEG);
                }
                coinkEG.SetWindow(TimeSpan.FromSeconds(double.Parse(eventNode.Attributes["window"]?.InnerText)));
                coinkEG.SetMinDifference(TimeSpan.FromSeconds(double.Parse(eventNode.Attributes["min_difference"]?.InnerText)));
            }
            catch
            {
                return null;
            }
            return coinkEG;
        }

        public override void GenerateXML(XmlWriter xmlWriter, EventGenerator eg)
        {
            CoincidenceEG coinkEg = (CoincidenceEG)eg;
            switch (coinkEg.GetCoincidenceType())
            {
                case CoincidenceEG.CoincidenceType.A_THEN_B:
                    xmlWriter.WriteAttributeString("coincidence_type", "A_THEN_B");
                    break;
                case CoincidenceEG.CoincidenceType.B_THEN_A:
                    xmlWriter.WriteAttributeString("coincidence_type", "B_THEN_A");
                    break;
                case CoincidenceEG.CoincidenceType.EITHER_ORDER:
                    xmlWriter.WriteAttributeString("coincidence_type", "EITHER_ORDER");
                    break;
            }
            switch (coinkEg.GetTimingType())
            {
                case CoincidenceEG.TimingType.START_TO_START:
                    xmlWriter.WriteAttributeString("timing_type", "START_TO_START");
                    break;
                case CoincidenceEG.TimingType.START_TO_END:
                    xmlWriter.WriteAttributeString("timing_type", "START_TO_END");
                    break;
                case CoincidenceEG.TimingType.END_TO_START:
                    xmlWriter.WriteAttributeString("timing_type", "END_TO_START");
                    break;
                case CoincidenceEG.TimingType.END_TO_END:
                    xmlWriter.WriteAttributeString("timing_type", "END_TO_END");
                    break;
                case CoincidenceEG.TimingType.MAX_TO_MAX:
                    xmlWriter.WriteAttributeString("timing_type", "MAX_TO_MAX");
                    break;
            }
            xmlWriter.WriteAttributeString("event_generator_A", coinkEg.GetEventGeneratorA().GetName());
            xmlWriter.WriteAttributeString("event_generator_B", coinkEg.GetEventGeneratorB().GetName());
            xmlWriter.WriteAttributeString("window", coinkEg.GetWindow().TotalSeconds.ToString());
            xmlWriter.WriteAttributeString("min_difference", coinkEg.GetMinDifference().TotalSeconds.ToString());
        }
    }
}
