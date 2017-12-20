using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    class FRAMPlutoniumResultParser : ResultParser
    {
        FRAMParser framParser;

        public FRAMPlutoniumResultParser()
        {
            framParser = new FRAMParser();
        }

        public override AnalysisResult Parse(string resultsFile)
        {
            AnalysisResult analysisResult = new AnalysisResult();
            framParser.ReadFRAMOutput(resultsFile);
            framParser.ParsePlutoniumResults();
            analysisResult.Composition = framParser.GetNuclearComposition();

            return analysisResult;
        }
    }
}
