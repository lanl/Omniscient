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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class ManualEG : EventGenerator
    {
        string dataFile;
        string dataFolder;
        List<Event> Events;
        public ManualEG(DetectionSystem parent, string newName, uint id) : base(parent, newName, id)
        {
            eventGeneratorType = "Manual";
            dataFolder = "Data\\" + id.ToString("X8");
            dataFile = dataFolder + "\\Events.csv";
            CheckFoldersExist();
            LoadEvents();
        }

        private void CheckFoldersExist()
        {
            if (!Directory.Exists("Data")) Directory.CreateDirectory("Data");
            if (!Directory.Exists(dataFolder)) Directory.CreateDirectory(dataFolder);
        }

        private void LoadEvents()
        {
            Events = new List<Event>();
            if (!File.Exists(dataFile)) return;

            EventParser parser = new EventParser();
            parser.ParseFile(dataFile);
            for (int e = 0; e < parser.StartTime.Count; e++)
            {
                Events.Add(new Event(this, parser.StartTime[e], parser.EndTime[e])
                {
                    MaxValue = 1,
                    MaxTime = parser.StartTime[e],
                    Comment = parser.Comments[e]
                });
            }
        }

        public void AddEvent(DateTime start, DateTime end, string comment)
        {
            Events.Add(new Event(this, start, end)
            {
                MaxValue = 1,
                MaxTime = start,
                Comment = comment
            });
            Events.Sort();
            WriteEvents();
        }

        public void RemoveEvent(Event eve)
        {
            Events.Remove(eve);
            WriteEvents();
        }

        private void WriteEvents()
        {
            EventWriter writer = new EventWriter();
            writer.WriteEventFile(dataFile, Events);
        }

        public override List<Event> GenerateEvents(DateTime start, DateTime end)
        {
            List<Event> outEvents = new List<Event>();
            foreach (Event eve in Events)
            {
                if ((eve.StartTime > start && eve.StartTime < end) ||
                    (eve.EndTime > start && eve.EndTime < end))
                {
                    outEvents.Add(eve);
                }
            }
            return outEvents;
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = new List<Parameter>();
            return parameters;
        }
    }

    public class ManualEGHookup : EventGeneratorHookup
    {
        public ManualEGHookup()
        {
            TemplateParameters = new List<ParameterTemplate>();
        }

        public override string Type { get { return "Manual"; } }

        public override EventGenerator FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            return new ManualEG(parent, newName, id);
        }
    }
}
