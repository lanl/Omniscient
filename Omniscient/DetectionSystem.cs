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
    public class DetectionSystem : Persister
    {
        public override string Species { get { return "System"; } }

        List<Instrument> instruments;
        private DeclarationInstrument declarationInstrument;
        public Facility ParentFacility { get; }

        public DetectionSystem(Facility parent, string name, uint id) : base(parent, name, id)
        {
            parent.GetSystems().Add(this);

            instruments = new List<Instrument>();
            declarationInstrument = null;

            eventGenerators = new List<EventGenerator>();
            events = new List<Event>();
            ParentFacility = parent;
        }

        public void AddInstrument(Instrument newInstrument)
        {
            if (newInstrument is DeclarationInstrument)
                SetDeclarationInstrument((DeclarationInstrument)newInstrument);
            else
                instruments.Add(newInstrument);
        }

        public List<Instrument> GetInstruments()
        {
            return instruments;
        }

        public void SetDeclarationInstrument(DeclarationInstrument inst)
        {
            if (declarationInstrument != null)
            {
                instruments.Remove(declarationInstrument);
            }
            declarationInstrument = inst;
            instruments.Add(inst);
        }

        public void RemoveDeclarationInstrument()
        {
            if (declarationInstrument != null)
            {
                instruments.Remove(declarationInstrument);
            }
            declarationInstrument = null;
        }

        public DeclarationInstrument GetDeclarationInstrument() { return declarationInstrument; }

        public bool HasDeclarationInstrument()
        {
            if (declarationInstrument == null) return false;
            return true;
        }

        List<EventGenerator> eventGenerators;
        List<Event> events;

        public void GenerateEvents(DateTime start, DateTime end)
        {
            foreach (EventGenerator eg in eventGenerators)
                events.AddRange(eg.GenerateEvents(start, end));
        }

        public List<EventGenerator> GetEventGenerators() { return eventGenerators; }
        public List<Event> GetEvents() { return events; }

        public override void ToXML(XmlWriter xmlWriter)
        {
            StartToXML(xmlWriter);
            foreach (Instrument inst in GetInstruments())
            {
                inst.ToXML(xmlWriter);
            }
            foreach (EventGenerator eg in GetEventGenerators())
            {
                eg.ToXML(xmlWriter);
            }
            xmlWriter.WriteEndElement();
        }

        public static DetectionSystem FromXML(XmlNode node, Facility parent)
        {
            string name;
            uint id;
            Persister.StartFromXML(node, out name, out id);
            DetectionSystem system = new DetectionSystem(parent, name, id);
            return system;
        }

        public override bool SetIndex(int index)
        {
            base.SetIndex(index);
            ParentFacility.GetSystems().Remove(this);
            ParentFacility.GetSystems().Insert(index, this);
            return true;
        }
        public override void Delete()
        {
            base.Delete();
            ParentFacility.GetSystems().Remove(this);
        }
    }
}
