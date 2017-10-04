using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Omniscient.Instruments;

namespace Omniscient.Events
{
    /// <summary>
    /// Generates events during which the channel value is at or above the threshold.
    /// </summary>
    public class ThresholdEG : EventGenerator
    {
        Channel channel;
        double threshold;
        TimeSpan debounceTime;

        public ThresholdEG(string newName, Channel newChannel, double newThreshold, TimeSpan newDebounceTime)
        {
            name = newName;
            eventGeneratorType = "Threshold";
            events = new List<Event>();
            channel = newChannel;
            threshold = newThreshold;
            debounceTime = newDebounceTime;
        }

        public ThresholdEG(string newName, Channel newChannel, double newThreshold)
        {
            name = newName;
            eventGeneratorType = "Threshold";
            events = new List<Event>();
            channel = newChannel;
            threshold = newThreshold;
            debounceTime = TimeSpan.FromTicks(0);
        }

        public override List<Event> GenerateEvents(DateTime start, DateTime end)
        {
            List<DateTime> times = channel.GetTimeStamps();
            List<double> vals = channel.GetValues();
            events = new List<Event>();
            Event eve = new Event(this);        // Really shouldn't need to make an event here but visual studio freaks out without it
            DateTime maxTime = new DateTime();
            double maxValue = 0;

            bool inEvent = false;
            bool onTheDrop = false;
            DateTime lastDrop = new DateTime();
            for (int i=0; i<times.Count; i++)
            {
                if (!inEvent)
                {
                    if (vals[i] >= threshold)
                    {
                        eve = new Event(this);
                        eve.SetStartTime(times[i]);
                        eve.SetComment(channel.GetName() + " is above threshold.");
                        maxValue = vals[i];
                        maxTime = times[i];
                        inEvent = true;
                        onTheDrop = false;
                    }
                }
                else
                {
                    if (vals[i] < threshold)
                    {
                        if (!onTheDrop)
                        {
                            lastDrop = times[i];
                            onTheDrop = true;
                        }

                        if (times[i] - lastDrop >= debounceTime)
                        {
                            eve.SetEndTime(lastDrop);
                            eve.SetMaxValue(maxValue);
                            eve.SetMaxTime(maxTime);
                            events.Add(eve);
                            inEvent = false;
                            onTheDrop = false;
                        }
                    }
                    else
                    {
                        if(vals[i] > maxValue)
                        {
                            maxValue = vals[i];
                            maxTime = times[i];
                        }
                        onTheDrop = false;
                    }
                }
            }
            if (inEvent)
            {
                eve.SetEndTime(times[times.Count - 1]);
                eve.SetMaxValue(maxValue);
                eve.SetMaxTime(maxTime);
                events.Add(eve);
                inEvent = false;
            }
            return events;
        }

        public void SetChannel(Channel newChannel) { channel = newChannel; }
        public void SetThreshold(double newThreshold) { threshold = newThreshold; }
        public void SetDebounceTime(TimeSpan newBounce) { debounceTime = newBounce; }

        public Channel GetChannel() { return channel; }
        public double GetThreshold() { return threshold; }
        public TimeSpan GetDebounceTime() { return debounceTime; }
        public List<Event> GetEvents() { return events; }

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
