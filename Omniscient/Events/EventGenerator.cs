using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public abstract class EventGenerator
    {
        protected string name;
        protected string eventGeneratorType;
        protected List<Event> events;
        protected List<Action> actions;

        public EventGenerator(string newName)
        {
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
    }
}
