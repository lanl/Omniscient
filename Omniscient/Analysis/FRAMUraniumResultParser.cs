using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    class FRAMUraniumResultParser : ResultParser
    {
        FRAMParser framParser;

        public FRAMUraniumResultParser()
        {
            framParser = new FRAMParser();
        }

        public override AnalysisResult Parse(string resultsFile)
        {
            AnalysisResult analysisResult = new AnalysisResult();
            framParser.ReadFRAMOutput(resultsFile);
            framParser.ParseUraniumResults();
            analysisResult.Composition = framParser.GetNuclearComposition();

            return analysisResult;
        }
    }
}
