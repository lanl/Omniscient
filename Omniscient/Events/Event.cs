using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class Event
    {
        public Event(EventGenerator myGenerator)
        {
            eventGenerator = myGenerator;
        }

        public Event(EventGenerator myGenerator, DateTime start, DateTime end)
        {
            eventGenerator = myGenerator;
            StartTime = start;
            EndTime = end;
        }

        EventGenerator eventGenerator;
        DateTime StartTime;
        DateTime EndTime;

        DateTime MaxTime;
        double maxValue;

        string comment;

        public TimeSpan GetDuration() { return EndTime - StartTime; }

        public void SetStartTime(DateTime start) { StartTime = start; }
        public void SetEndTime(DateTime end) { EndTime = end; }
        public void SetMaxTime(DateTime max) { MaxTime = max; }
        public void SetMaxValue(double max) { maxValue = max; }
        public void SetComment(string newComment) { comment = newComment; }

        public DateTime GetStartTime() { return StartTime; }
        public DateTime GetEndTime() { return EndTime; }
        public DateTime GetMaxTime() { return MaxTime; }
        public double GetMaxValue() { return maxValue; }
        public string GetComment() { return comment; }
        public EventGenerator GetEventGenerator() { return eventGenerator; }
    }
}
