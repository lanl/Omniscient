using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    class Site
    {
        private List<Facility> facilities;
        private string name;

        public Site(string newName)
        {
            name = newName;
            facilities = new List<Facility>();
        }

        public void AddFacility(Facility newFac)
        {
            facilities.Add(newFac);
        }

        public List<Facility> GetFacilities()
        {
            return facilities;
        }

        public void SetName(string newName)
        {
            name = newName;
        }

        public string GetName() { return name; }

    }
}
