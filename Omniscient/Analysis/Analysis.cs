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
        List<string> inputFiles;
        string resultsFile;
        ResultParser resultParser;

        List<AnalysisResult> results;

        public void Run()
        {

        }

        public List<AnalysisResult> GetResults() { return results; }
    }
}
