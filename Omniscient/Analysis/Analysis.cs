using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class Analysis
    {
        string name;
        string command;
        string resultsFile;
        ResultParser resultParser;

        List<AnalysisResult> results;

        public void Run(List<string> inputsFiles)
        {

        }

        public List<AnalysisResult> GetResults() { return results; }
    }
}
