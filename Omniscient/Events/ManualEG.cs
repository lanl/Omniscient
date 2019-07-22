using System;
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
