using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class DataPresentEG : EventGenerator
    {
        Channel channel;
        public DataPresentEG(DetectionSystem parent, string newName, Channel newChannel, uint id) : base(parent, newName, id)
        {
            eventGeneratorType = "Data Present";
            channel = newChannel;
        }

        public override List<Event> GenerateEvents(DateTime start, DateTime end)
        {
            channel.GetInstrument().LoadData(ChannelCompartment.Process, start, end);
            events = new List<Event>();
            List<DateTime> times = channel.GetTimeStamps(ChannelCompartment.Process);
            List<TimeSpan> durations = channel.GetDurations(ChannelCompartment.Process);
            List<double> vals = channel.GetValues(ChannelCompartment.Process);
            if (durations is null || durations.Count != times.Count) return events;

            Event eve = new Event(this);
            for (int i = 0; i < times.Count; i++)
            {
                eve = new Event(this, times[i], times[i] + durations[i]);
                eve.MaxValue = vals[i];
                eve.MaxTime = times[i] + TimeSpan.FromTicks(durations[i].Ticks / 2);
                eve.Comment = "Data present in " + channel.Name;
                events.Add(eve);
            }
            return events;
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new SystemChannelParameter("Channel", (DetectionSystem)eventWatcher){ Value = channel.Name }
            };
            return parameters;
        }
    }

    public class DataPresentEGHookup : EventGeneratorHookup
    {
        public DataPresentEGHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
            {
                new ParameterTemplate("Channel", ParameterType.SystemChannel)
            };
        }

        public override string Type { get { return "Data Present"; } }

        public override EventGenerator FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            Channel channel = null;
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Channel":
                        channel = ((SystemChannelParameter)param).ToChannel();
                        break;
                }
            }
            return new DataPresentEG(parent, newName, channel, id);
        }
    }
}
