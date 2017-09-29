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

        public ThresholdEG(string newName, Channel newChannel, double newThreshold)
        {
            name = newName;
            channel = newChannel;
            threshold = newThreshold;
        }

        public override List<Event> GenerateEvents(DateTime start, DateTime end)
        {
            List<DateTime> times = channel.GetTimeStamps();
            List<double> vals = channel.GetValues();
            List<Event> events = new List<Event>();
            Event eve = new Event();        // Really shouldn't need to make an event here but visual studio freaks out without it

            bool inEvent = false;
            for (int i=0; i<times.Count; i++)
            {
                if (!inEvent)
                {
                    if (vals[i] >= threshold)
                    {
                        eve = new Event();
                        eve.SetStartTime(times[i]);
                        eve.SetComment(channel.GetName() + " is above threshold.");
                        inEvent = true;
                    }
                }
                else
                {
                    if (vals[i] < threshold)
                    {
                        eve.SetEndTime(times[i - 1]);
                        events.Add(eve);
                        inEvent = false;
                    }
                }
            }
            if (inEvent)
            {
                eve.SetEndTime(times[times.Count - 1]);
                events.Add(eve);
                inEvent = false;
            }
            return events;
        }

        public void SetChannel(Channel newChannel) { channel = newChannel; }
        public void SetThreshold(double newThreshold) { threshold = newThreshold; }

        public Channel GetChannel() { return channel; }
        public double GetThreshold() { return threshold; }

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
