// This software is open source software available under the BSD-3 license.
// 
// Copyright (c) 2018, Los Alamos National Security, LLC
// All rights reserved.
// 
// Copyright 2018. Los Alamos National Security, LLC. This software was produced under U.S. Government contract DE-AC52-06NA25396 for Los Alamos National Laboratory (LANL), which is operated by Los Alamos National Security, LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  NEITHER THE GOVERNMENT NOR LOS ALAMOS NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.  If software is modified to produce derivative works, such modified software should be clearly marked, so as not to confuse it with the version available from LANL.
// 
// Additionally, redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 1.       Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
// 2.       Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 
// 3.       Neither the name of Los Alamos National Security, LLC, Los Alamos National Laboratory, LANL, the U.S. Government, nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 
//  
// THIS SOFTWARE IS PROVIDED BY LOS ALAMOS NATIONAL SECURITY, LLC AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL LOS ALAMOS NATIONAL SECURITY, LLC OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
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
