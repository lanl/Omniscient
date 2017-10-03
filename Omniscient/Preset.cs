using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Omniscient.Instruments;
using Omniscient.Events;

namespace Omniscient
{
    public class Preset
    {
        private const int N_CHARTS = 4;
        List<EventGenerator> activeEventGenerators;
        List<Instrument> activeInstruments;
        List<Channel>[] activeChannels;
        string name;

        public Preset(string newName)
        {
            name = newName;
            activeEventGenerators = new List<EventGenerator>();
            activeInstruments = new List<Instrument>();
            activeChannels = new List<Channel>[N_CHARTS];
            for (int i = 0; i < N_CHARTS; i++)
                activeChannels[i] = new List<Channel>();
        }

        public void AddChannel(Channel ch, int chart)
        {
            activeChannels[chart].Add(ch);
        }

        public string GetName() { return name; }
        public List<EventGenerator> GetActiveEventGenerators() { return activeEventGenerators; }
        public List<Instrument> GetActiveInstruments(){ return activeInstruments; }
        public List<Channel>[] GetActiveChannels() { return activeChannels; }
    }
}
