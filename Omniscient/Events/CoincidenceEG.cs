using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient.Events
{
    /// <summary>
    /// Generates events when events it watches for occur in coincidence.
    /// </summary>
    public class CoincidenceEG : EventGenerator
    {
        public enum CoincidenceType { A_THEN_B, B_THEN_A, EITHER_ORDER }
        public enum TimingType { START_TO_START, START_TO_END, END_TO_START, END_TO_END}

        EventGenerator eventGeneratorA;
        EventGenerator eventGeneratorB;

        TimeSpan window;
        TimeSpan minDifference;

        public override List<Event> GenerateEvents(DateTime start, DateTime end)
        {
            return new List<Event>();
        }

        public void SetEventGeneratorA(EventGenerator newEg) { eventGeneratorA = newEg; }
        public void SetEventGeneratorB(EventGenerator newEg) {eventGeneratorB = newEg; }

        public EventGenerator GetEventGeneratorA() { return eventGeneratorA; }
        public EventGenerator GetEventGeneratorB() { return eventGeneratorB; }

        public override string GetName()
        {
            return name;
        }

        public override void SetName(string newName)
        {
            name = newName;
        }
    }
}
