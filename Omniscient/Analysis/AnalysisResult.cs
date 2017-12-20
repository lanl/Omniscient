using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class AnalysisResult
    {
        public string Description { get; set; }
        public DateTime AnalysisDate { get; set; }
        public NuclearComposition Composition { get; set; }
        public DateTime DataStart { get; set; }
        public DateTime DataEnd { get; set; }

        public AnalysisResult()
        {
        }
    }
}
