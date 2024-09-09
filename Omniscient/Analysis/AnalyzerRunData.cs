using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class AnalyzerRunData
    {
        public Declaration Declaration { get; set; }
        public Event Event { get; set; }
        public Dictionary<string, CustomParameter> CustomParameters { get; set; }

        public AnalyzerRunData(Event eve, Dictionary<string, CustomParameter> parameters) 
        {
            Declaration = null;
            Event = eve;
            CustomParameters = parameters;
        }
    }
}
