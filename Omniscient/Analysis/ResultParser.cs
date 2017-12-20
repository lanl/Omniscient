using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public abstract class ResultParser
    {
        public abstract AnalysisResult Parse(string resultsFile);
    }
}
