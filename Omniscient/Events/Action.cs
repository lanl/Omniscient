using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public abstract class Action
    {
        protected string name;
        protected string actionType;
        protected EventGenerator eventGenerator;

        public Action(EventGenerator parent, string newName)
        {
            eventGenerator = parent;
            name = newName;
        }

        public abstract void Execute(Event eve);

        public abstract void SetName(string newName);

        public string GetName() { return name; }
        public string GetActionType() { return actionType; }
        public EventGenerator GetEventGenerator() { return eventGenerator; }
    }
}
