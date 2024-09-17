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

        public static Dictionary<string, Dictionary<string,string>> FileToDictionary(string fileName) 
        {
            Dictionary<string, Dictionary<string, string>> reportDict = new Dictionary<string, Dictionary<string, string>>();
            string[] lines;
            try
            {
                lines = IOUtility.PermissiveReadAllLines(fileName);
            }
            catch { return reportDict; }
            if (lines is null || lines.Length < 5) { return reportDict; }

            string[] pieces;
            string sectionTitle;
            
            int lineNumber = 1;
            bool header = true;

            char[] separator = {':'};

            while (true)
            {
                Dictionary<string, string> sectionDict = new Dictionary<string, string>();
                if (header)
                {
                    sectionTitle = "Header";
                    sectionDict.Add("Type", lines[lineNumber].Trim());
                    sectionDict.Add("File Name", fileName);
                    header = false;
                }
                else sectionTitle = lines[lineNumber].Trim();

                lineNumber++;
                while (!string.IsNullOrWhiteSpace(lines[lineNumber]))
                {
                    pieces = lines[lineNumber].Split(separator, 2);
                    sectionDict.Add(pieces[0].Trim(), pieces[1].Trim());
                    if (lines.Length == lineNumber) break;
                    lineNumber++;
                }
                reportDict.Add(sectionTitle, sectionDict);

                while (lineNumber<lines.Length && string.IsNullOrWhiteSpace(lines[lineNumber])) lineNumber++;
                if (lines.Length == lineNumber) break;
            }

            return reportDict;
        }

        public static List<Dictionary<string, Dictionary<string, string>>> FromDirectory(string directory)
        {
            string filePattern = "*.rep";
            List<Dictionary<string, Dictionary<string, string>>> reports = new List<Dictionary<string, Dictionary<string, string>>>();
            if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory)) return reports;
            IEnumerable<string> patternFiles = Directory.EnumerateFiles(directory, filePattern, SearchOption.TopDirectoryOnly);
            foreach (string file in patternFiles)
            {
                try
                {
                    Dictionary<string, Dictionary<string, string>> report = FileToDictionary(file);
                    reports.Add(report);
                }
                catch (Exception ex) { }
            }
            return reports;
        }
    }
}
