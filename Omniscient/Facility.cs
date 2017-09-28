using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class Facility
    {
        List<DetectionSystem> systems;
        string name;

        public Facility(string newName)
        {
            name = newName;
            systems = new List<DetectionSystem>();
        }

        public void AddSystem(DetectionSystem newSys)
        {
            systems.Add(newSys);
        }

        public List<DetectionSystem> GetSystems()
        {
            return systems;
        }

        public void SetName(string newName)
        {
            name = newName;
        }

        public string GetName() { return name; }

    }
}
