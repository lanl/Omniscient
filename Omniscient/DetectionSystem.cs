using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Omniscient.Instruments;
using Omniscient.Events;

namespace Omniscient
{
    public class DetectionSystem : EventWatcher
    {
        List<Instrument> instruments;
        string name;

        public DetectionSystem(string newName) : base()
        {
            name = newName;
            instruments = new List<Instrument>();
        }

        public void AddInstrument(Instrument newInstrument)
        {
            instruments.Add(newInstrument);
        }

        public List<Instrument> GetInstruments()
        {
            return instruments;
        }

        public void SetName(string newName)
        {
            name = newName;
        }

        public string GetName() { return name; }
    }
}
