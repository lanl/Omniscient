using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient.Events
{
    public abstract class EventGenerator
    {
        protected string name;
        protected List<Event> events;
        public abstract List<Event> GenerateEvents(DateTime start, DateTime end);
        public abstract string GetName();
        public abstract void SetName(string newName);
    }
}
