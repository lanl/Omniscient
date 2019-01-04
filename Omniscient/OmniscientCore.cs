using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    class OmniscientCore
    {
        public DateTime GlobalStart { get; set; }
        public DateTime GlobalEnd { get; set; }

        public OmniscientCore()
        {
            GlobalStart = DateTime.Today.AddDays(-1);
            GlobalEnd = DateTime.Today;
        }
    }
}
