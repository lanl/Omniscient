using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    /// <summary>
    /// Generates events during which the channel value is at or above the threshold.
    /// </summary>
    public class ThresholdEG : EventGenerator
    {
        Channel channel;
        double threshold;
        TimeSpan debounceTime;

        public ThresholdEG(EventWatcher parent, string newName, Channel newChannel, double newThreshold, TimeSpan newDebounceTime) : base(parent, newName)
        {
            eventGeneratorType = "Threshold";
            channel = newChannel;
            threshold = newThreshold;
            debounceTime = newDebounceTime;
        }

        public ThresholdEG(EventWatcher parent, string newName, Channel newChannel, double newThreshold) : base(parent, newName)
        {
            eventGeneratorType = "Threshold";
            channel = newChannel;
            threshold = newThreshold;
            debounceTime = TimeSpan.FromTicks(0);
        }

        public override List<Event> GenerateEvents(DateTime start, DateTime end)
        {
            List<DateTime> times = channel.GetTimeStamps();
            List<double> vals = channel.GetValues();
            List<TimeSpan> durations = null;
            if(channel.GetChannelType() == Channel.ChannelType.DURATION_VALUE)
                durations = channel.GetDurations();
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
                    // New event
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
                    // Event ended and new one created by exceeding debounce time between data points
                    TimeSpan deltaT;
                    if (durations != null)
                        deltaT = times[i] - (times[i - 1] + durations[i - 1]);
                    else
                        deltaT = times[i] - times[i - 1];
                    if (deltaT >= debounceTime)
                    {
                        if (durations != null)
                            eve.SetEndTime(times[i - 1] + durations[i - 1]);
                        else
                            eve.SetEndTime(times[i - 1]);
                        eve.SetMaxValue(maxValue);
                        eve.SetMaxTime(maxTime);
                        events.Add(eve);
                        inEvent = false;
                        onTheDrop = false;

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
                    else if (vals[i] < threshold)
                    {
                        if (!onTheDrop)
                        {
                            if (durations != null)
                                lastDrop = times[i] + durations[i];
                            else
                                lastDrop = times[i];
                            onTheDrop = true;
                        }

                        // Event ended by dropping below threshold
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
                        // In event
                        if (vals[i] > maxValue)
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
        new public List<Event> GetEvents() { return events; }

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
