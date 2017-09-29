using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient.Events
{
    public abstract class EventGenerator
    {
        public abstract List<Event> GenerateEvents(DateTime start, DateTime end);
    }
}
