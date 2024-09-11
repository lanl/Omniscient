using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class AnalyzerReport
    {
        public Analyzer Analyzer { get; set; }
        public List<ReportSection> Sections { get; set; }
        public AnalyzerReport(Analyzer analyzer) 
        { 
            Analyzer = analyzer;
            Sections = new List<ReportSection>()
            {
                new AnalyzerHeaderReportSection(analyzer)
            };
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder("Omniscient ");
            builder.Append(OmniscientCore.VERSION);
            builder.Append("\n");
            foreach (ReportSection section in Sections)
            { 
                builder.Append(section.ToString());
                builder.Append("\n\n");
            }
            return builder.ToString();
        }

        public ReturnCode ToFile(string fileName)
        {
            File.WriteAllText(fileName, this.ToString());
            return ReturnCode.SUCCESS;
        }
    }
}
