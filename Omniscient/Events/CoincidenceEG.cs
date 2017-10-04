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
        public enum TimingType { START_TO_START, START_TO_END, END_TO_START, END_TO_END, MAX_TO_MAX}

        EventGenerator eventGeneratorA;
        EventGenerator eventGeneratorB;

        CoincidenceType coincidenceType;
        TimingType timingType;

        TimeSpan window;
        TimeSpan minDifference;

        public CoincidenceEG(string newName)
        {
            name = newName;
            eventGeneratorType = "Coincidence";
            events = new List<Event>();
            coincidenceType = CoincidenceType.A_THEN_B;
            timingType = TimingType.START_TO_END;
            eventGeneratorA = null;
            eventGeneratorB = null;
            window = TimeSpan.FromTicks(0);
            minDifference = TimeSpan.FromTicks(0);
        }

        public CoincidenceEG(string newName, CoincidenceType newCoincidenceType, TimingType newTimingType)
        {
            name = newName;
            eventGeneratorType = "Coincidence";
            events = new List<Event>();
            coincidenceType = newCoincidenceType;
            timingType = newTimingType;
            eventGeneratorA = null;
            eventGeneratorB = null;
            window = TimeSpan.FromTicks(0);
            minDifference = TimeSpan.FromTicks(0);
        }

        public override List<Event> GenerateEvents(DateTime start, DateTime end)
        {
            events = new List<Event>();

            List<Event> eventsA;
            List<Event> eventsB;

            if (coincidenceType == CoincidenceType.B_THEN_A)
            {
                eventsA = eventGeneratorB.GetEvents();
                eventsB = eventGeneratorA.GetEvents();
            }
            else
            {
                eventsA = eventGeneratorA.GetEvents();
                eventsB = eventGeneratorB.GetEvents();
            }

            if (eventsA.Count == 0 || eventsB.Count == 0) return events;

            int aIndex = 0;
            int bIndex = 0;

            DateTime aTime = new DateTime();
            DateTime bTime = new DateTime();
            TimeSpan delta;

            while(aIndex < eventsA.Count && bIndex < eventsB.Count)
            {
                switch (timingType)
                {
                    case TimingType.START_TO_START:
                        aTime = eventsA[aIndex].GetStartTime();
                        bTime = eventsB[bIndex].GetStartTime();
                        break;
                    case TimingType.START_TO_END:
                        aTime = eventsA[aIndex].GetStartTime();
                        bTime = eventsB[bIndex].GetEndTime();
                        break;
                    case TimingType.END_TO_START:
                        aTime = eventsA[aIndex].GetEndTime();
                        bTime = eventsB[bIndex].GetStartTime();
                        break;
                    case TimingType.END_TO_END:
                        aTime = eventsA[aIndex].GetEndTime();
                        bTime = eventsB[bIndex].GetEndTime();
                        break;
                    case TimingType.MAX_TO_MAX:
                        aTime = eventsA[aIndex].GetMaxTime();
                        bTime = eventsB[bIndex].GetMaxTime();
                        break;
                }
                delta = bTime - aTime;
                if (coincidenceType == CoincidenceType.EITHER_ORDER) delta = delta.Duration();
                if(delta <= window && delta >= minDifference)   // Coincidence event has occured
                {
                    Event eve = new Event(this);
                    if (aTime < bTime)
                    {
                        eve.SetStartTime(eventsA[aIndex].GetStartTime());
                        eve.SetEndTime(eventsB[bIndex].GetEndTime());
                    }
                    else
                    {
                        eve.SetStartTime(eventsB[bIndex].GetStartTime());
                        eve.SetEndTime(eventsA[aIndex].GetEndTime());
                    }
                    eve.SetComment("Coincidence!");
                    events.Add(eve);
                    aIndex++;
                    bIndex++;
                }
                else if(aTime < bTime)
                {
                    aIndex++;
                }
                else
                {
                    bIndex++;
                }
            }
            return events;
        }

        public void SetEventGeneratorA(EventGenerator newEg) { eventGeneratorA = newEg; }
        public void SetEventGeneratorB(EventGenerator newEg) {eventGeneratorB = newEg; }
        public void SetCoincidenceType(CoincidenceType newCoincidenceType) { coincidenceType = newCoincidenceType; }
        public void SetTimingType(TimingType newTimingType) { timingType = newTimingType; }
        public void SetWindow(TimeSpan newWindow) { window = newWindow; }
        public void SetMinDifference(TimeSpan newDifference) { minDifference = newDifference; }

        public EventGenerator GetEventGeneratorA() { return eventGeneratorA; }
        public EventGenerator GetEventGeneratorB() { return eventGeneratorB; }
        public CoincidenceType GetCoincidenceType() { return coincidenceType; }
        public TimingType GetTimingType() { return timingType; }
        public TimeSpan GetWindow() { return window; }
        public TimeSpan GetMinDifference() { return minDifference; }

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
