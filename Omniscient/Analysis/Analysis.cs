using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class Analysis
    {
        const string DELIMETER = "||";

        string name;
        string command;
        string resultsFile;
        ResultParser resultParser;

        List<AnalysisResult> results;

        public Analysis()
        {
            name = "";
            command = "";
            resultsFile = "";
            resultParser = null;
            results = new List<AnalysisResult>();
        }

        public ReturnCode Run(List<string> inputFiles)
        {
            results.Clear();

            // Replace variables in the command string with values
            string fullCommand = command;
            string symbol;
            for (int input = 0; input < inputFiles.Count(); input++)
            {
                symbol = DELIMETER + (input+1).ToString() + DELIMETER;
                if (!fullCommand.Contains(symbol))
                    return ReturnCode.BAD_INPUT;
                fullCommand = fullCommand.Replace(symbol, inputFiles[input]);
            }

            // Run command
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + fullCommand;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            process.Close();

            // Get results
            results.Add(resultParser.Parse(resultsFile));

            return ReturnCode.SUCCESS;
        }

        public List<AnalysisResult> GetResults() { return results; }

        public void SetCommand(string newCommand) { command = newCommand; }
        public void SetResultsFile(string newFile) { resultsFile = newFile; }
        public void SetResultParser(ResultParser newResultParser) { resultParser = newResultParser; }

        public string GetCommand() { return command; }
        public string GetResultsFile() { return resultsFile; }
        public ResultParser GetResultParser() { return resultParser; }
    }
}
