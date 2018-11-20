using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Omniscient
{
    class GapEG : EventGenerator
    {
        TimeSpan interval;
        Channel channel;

        public GapEG(EventWatcher parent, string newName, Channel newChannel, double newInterval) : base(parent, newName)
        {
            eventGeneratorType = "Gap";
            channel = newChannel;
            interval = TimeSpan.FromSeconds(newInterval);
        }

        public override List<Event> GenerateEvents(DateTime start, DateTime end)
        {
            List<DateTime> times = channel.GetTimeStamps();
            events = new List<Event>();
            Event eve = new Event(this);        // Really shouldn't need to make an event here but visual studio freaks out without it

            for (int i = 1; i < times.Count; i++)
            {
                if(times[i] - times[i-1] > interval)
                {
                    eve = new Event(this);
                    eve.SetStartTime(times[i-1]);
                    eve.SetEndTime(times[i]);
                    eve.SetMaxTime(times[i - 1]);
                    eve.SetMaxValue(0);
                    eve.SetComment(channel.GetName() + " has a gap.");
                }
            }
            return events;
        }

        public override string GetName()
        {
            return name;
        }

        public override void SetName(string newName)
        {
            name = newName;
        }

        public void SetChannel(Channel newChannel) { channel = newChannel; }
        public Channel GetChannel() { return channel; }
        public void SetInterval(TimeSpan newInterval) { interval = newInterval; }
        public TimeSpan GetInterval() { return interval; }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new SystemChannelParameter("Channel", (DetectionSystem)eventWatcher){ Value = channel.GetName() },
                new TimeSpanParameter("Interval") { Value = interval.ToString() }
            };
            return parameters;
        }
    }

    public class GapEGHookup : EventGeneratorHookup
    {
        public override EventGenerator FromParameters(EventWatcher parent, string newName, List<Parameter> parameters)
        {
            Channel channel = null;
            double interval = 0;

            foreach(Parameter param in parameters)
            {
                switch(param.Name)
                {
                    case "Channel":
                        channel = ((SystemChannelParameter)param).ToChannel();
                        break;
                    case "Interval":
                        interval = ((TimeSpanParameter)param).ToTimeSpan().TotalSeconds;
                        break;
                }
            }
            return new GapEG(parent, newName, channel, interval);
        }

        public override string Type { get { return "Gap"; } }

        public GapEGHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
            {
                new ParameterTemplate("Channel", ParameterType.SystemChannel),
                new ParameterTemplate("Interval", ParameterType.TimeSpan)
            };
        }

        public override EventGenerator GenerateFromXML(XmlNode eventNode, DetectionSystem system)
        {
            Channel channel = null;
            foreach (Instrument inst in system.GetInstruments())
            {
                foreach (Channel ch in inst.GetChannels())
                {
                    if (ch.GetName() == eventNode.Attributes["channel"]?.InnerText)
                        channel = ch;
                }
            }
            if (channel is null) return null;
            try
            {
                GapEG eg = new GapEG(system, eventNode.Attributes["name"]?.InnerText, channel, double.Parse(eventNode.Attributes["interval"]?.InnerText));
                return eg;
            }
            catch
            {
                return null;
            }
        }

        public override void GenerateXML(XmlWriter xmlWriter, EventGenerator eg)
        {
            xmlWriter.WriteAttributeString("channel", ((GapEG)eg).GetChannel().GetName());
            xmlWriter.WriteAttributeString("interval", ((GapEG)eg).GetInterval().TotalSeconds.ToString());
        }
    }
}
