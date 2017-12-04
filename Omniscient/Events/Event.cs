using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class Event
    {
        List<AnalysisResult> analysisResults;

        public Event(EventGenerator myGenerator)
        {
            eventGenerator = myGenerator;
            analysisResults = new List<AnalysisResult>();
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

        public void AddAnalysisResult(AnalysisResult result) { analysisResults.Add(result); }
        public void AddAnalysisResults(List<AnalysisResult> results) { analysisResults.AddRange(results); }

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
        public List<AnalysisResult> GetAnalysisResults() { return analysisResults; }
    }
}
