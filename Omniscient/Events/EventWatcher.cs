using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public abstract class EventWatcher
    {
        List<EventGenerator> eventGenerators;
        List<Event> events;

        public EventWatcher()
        {
            eventGenerators = new List<EventGenerator>();
            events = new List<Event>();
        }

        public void GenerateEvents(DateTime start, DateTime end)
        {
            foreach(EventGenerator eg in eventGenerators)
                events.AddRange(eg.GenerateEvents(start, end));
        }

        public List<EventGenerator> GetEventGenerators() { return eventGenerators; }
        public List<Event> GetEvents() { return events; }
    }
}
