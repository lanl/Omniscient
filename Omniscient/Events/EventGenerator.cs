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
    public abstract class EventGenerator : Persister
    {
        public override string Species { get { return "Event Generator"; } }

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
            new GapEGHookup(),
            new DataPresentEGHookup(),
            new ManualEGHookup()
        };

        protected DetectionSystem eventWatcher;
        protected string eventGeneratorType;
        protected List<Event> events;
        protected List<Action> actions;

        public EventGenerator(DetectionSystem parent, string name, uint id) : base(parent, name, id)
        {
            parent.GetEventGenerators().Add(this);

            eventWatcher = parent;
            events = new List<Event>();
            actions = new List<Action>();
        }

        public abstract List<Event> GenerateEvents(DateTime start, DateTime end);
        public List<Event> GetEvents() { return events; }
        public List<Action> GetActions() { return actions; }

        public string GetEventGeneratorType() { return eventGeneratorType; }

        public void RunActions()
        {
            foreach (Event eve in events)
                foreach (Action action in actions)
                    action.Execute(eve);
        }

        public DetectionSystem GetEventWatcher() { return eventWatcher; }

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
            string name;
            uint id;
            Persister.StartFromXML(eventNode, out name, out id);
            EventGeneratorHookup hookup = GetHookup(eventNode.Attributes["Type"]?.InnerText);
            List<Parameter> parameters = Parameter.FromXML(eventNode, hookup.TemplateParameters, system);
            return hookup?.FromParameters(system, name, parameters, id);
        }

        public override void ToXML(XmlWriter xmlWriter)
        {
            StartToXML(xmlWriter);
            xmlWriter.WriteAttributeString("Type", GetEventGeneratorType());
            List<Parameter> parameters = GetParameters();
            foreach (Parameter param in parameters)
            {
                xmlWriter.WriteAttributeString(param.Name.Replace(' ','_'), param.Value);
            }

            foreach (Action action in GetActions())
            {
                action.ToXML(xmlWriter);
            }
            xmlWriter.WriteEndElement();
        }

        public override bool SetIndex(int index)
        {
            eventWatcher.GetEventGenerators().Remove(this);
            eventWatcher.GetEventGenerators().Insert(index, this);
            eventWatcher.Children.Remove(this);
            eventWatcher.Children.Insert(index + eventWatcher.GetInstruments().Count, this);
            return true;
        }

        public override void Delete()
        {
            base.Delete();
            eventWatcher.GetEventGenerators().Remove(this);
        }
    }

    public abstract class EventGeneratorHookup
    {
        public abstract EventGenerator FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id);
        public abstract string Type { get; }
        public List<ParameterTemplate> TemplateParameters { get; set; }
    }
}
