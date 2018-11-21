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
    public abstract class EventGenerator
    {
        /// <summary>
        /// This array contains a hookup for each type of EventGenerator. In 
        /// order for Omniscient to recognize a new kind of EventGenerator, a
        /// corresponding EventGeneratorHookup must be made and added to this
        /// array. This is to make it easier to integrate new EventGenerators 
        /// into Omniscient.
        /// </summary>
        public static readonly EventGeneratorHookup[] Hookups = new EventGeneratorHookup[] 
        {
            new ThresholdEGHookup(),
            new CoincidenceEGHookup(),
            new GapEGHookup()
        };

        protected EventWatcher eventWatcher;
        protected string name;
        protected string eventGeneratorType;
        protected List<Event> events;
        protected List<Action> actions;

        public EventGenerator(EventWatcher parent, string newName)
        {
            eventWatcher = parent;
            name = newName;
            events = new List<Event>();
            actions = new List<Action>();
        }

        public abstract List<Event> GenerateEvents(DateTime start, DateTime end);
        public List<Event> GetEvents() { return events; }
        public List<Action> GetActions() { return actions; }
        public abstract string GetName();
        public abstract void SetName(string newName);

        public string GetEventGeneratorType() { return eventGeneratorType; }

        public void RunActions()
        {
            foreach (Event eve in events)
                foreach (Action action in actions)
                    action.Execute(eve);
        }

        public EventWatcher GetEventWatcher() { return eventWatcher; }

        public abstract List<Parameter> GetParameters();

        public static EventGeneratorHookup GetHookup(string type)
        {
            foreach (EventGeneratorHookup hookup in Hookups)
            {
                if (hookup.Type == type)
                {
                    return hookup;
                }
            }
            return null;
        }

        public static EventGenerator FromXML(XmlNode eventNode, DetectionSystem system)
        {
            string name = eventNode.Attributes["Name"]?.InnerText;
            EventGeneratorHookup hookup = GetHookup(eventNode.Attributes["Type"]?.InnerText);
            List<Parameter> parameters = new List<Parameter>();
            
            foreach (ParameterTemplate pTemplate in hookup.TemplateParameters)
            {
                string paramNameStr = pTemplate.Name.Replace(' ', '_');
                switch (pTemplate.Type)
                {
                    case ParameterType.Double:
                        parameters.Add(new DoubleParameter(pTemplate.Name) { Value = eventNode.Attributes[paramNameStr]?.InnerText });
                        break;
                    case ParameterType.Enum:
                        parameters.Add(new EnumParameter(pTemplate.Name)
                        {
                            Value = eventNode.Attributes[paramNameStr]?.InnerText,
                            ValidValues = pTemplate.ValidValues
                        });
                        break;
                    case ParameterType.SystemChannel:
                        parameters.Add(new SystemChannelParameter(pTemplate.Name, system) { Value = eventNode.Attributes[paramNameStr]?.InnerText });
                        break;
                    case ParameterType.SystemEventGenerator:
                        parameters.Add(new SystemEventGeneratorParameter(pTemplate.Name, system) { Value = eventNode.Attributes[paramNameStr]?.InnerText });
                        break;
                    case ParameterType.TimeSpan:
                        parameters.Add(new TimeSpanParameter(pTemplate.Name) { Value = eventNode.Attributes[paramNameStr]?.InnerText });
                        break;
                }
            }
            return hookup?.FromParameters(system, name, parameters);
        }

        public static void ToXML(XmlWriter xmlWriter, EventGenerator eg)
        {
            xmlWriter.WriteStartElement("EventGenerator");
            xmlWriter.WriteAttributeString("Name", eg.GetName());
            xmlWriter.WriteAttributeString("Type", eg.GetEventGeneratorType());
            EventGeneratorHookup hookup = GetHookup(eg.GetEventGeneratorType());
            List<Parameter> parameters = eg.GetParameters();
            foreach (Parameter param in parameters)
            {
                xmlWriter.WriteAttributeString(param.Name.Replace(' ','_'), param.Value);
            }
            //hookup.GenerateXML(xmlWriter, eg);
        }
    }

    public abstract class EventGeneratorHookup
    {
        public abstract EventGenerator FromParameters(EventWatcher parent, string newName, List<Parameter> parameters);
        public abstract string Type { get; }
        public List<ParameterTemplate> TemplateParameters { get; set; }
    }
}
