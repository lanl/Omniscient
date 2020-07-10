/*
This source code is Free Open Source Software. It is provided
with NO WARRANTY expressed or implied to the extent permitted by law.

This source code is distributed under the New BSD license:

================================================================================

   Copyright (c) 2020, International Atomic Energy Agency (IAEA), IAEA.org

   All rights reserved.

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice,
      this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice,
      this list of conditions and the following disclaimer in the documentation
      and/or other materials provided with the distribution.
    * Neither the name of IAEA nor the names of its contributors
      may be used to endorse or promote products derived from this software
      without specific prior written permission.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
   "AS IS"AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
   A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
   LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
   NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    class DATAZParser
    {
        private System.Globalization.CultureInfo CULTURE_INFO = new CultureInfo("en-US");
        const string TIMESTAMP_FORMAT = "yyyy-MM-dd HH:mm:ss";

        int dataStartIndex;
        public int DateTimeColumn { get; private set; }

        public string[] Headers { get; private set; }
        public double[,] Data { get; private set; }
        public DateTime[] TimeStamps { get; private set; }

        public DATAZParser() { }

        public ReturnCode ParseHeader(string[] lines)
        {
            // Skip non-data
            dataStartIndex = -1;
            for (int l = 0; l < lines.Length; l++)
            {
                if (lines[l].Substring(0, 5).ToLower() == "$data")
                {
                    dataStartIndex = l + 1;
                    break;
                }
            }
            if (dataStartIndex < 0 || dataStartIndex >= lines.Length - 1) return ReturnCode.CORRUPTED_FILE;

            // Read headers and determine timestamp column
            Headers = lines[dataStartIndex].Split(',');
            DateTimeColumn = -1;
            for (int c = 0; c < Headers.Length; c++)
            {
                if (Headers[c].ToLower() == "datetime")
                {
                    DateTimeColumn = c;
                    break;
                }
            }
            if (DateTimeColumn < 0 || DateTimeColumn == Headers.Length - 1) return ReturnCode.CORRUPTED_FILE;
            return ReturnCode.SUCCESS;
        }

        public ReturnCode ParseFirstRecord(string fileName)
        {
            // Read lines from file
            string[] lines;
            try
            {
                lines = IOUtility.PermissiveReadAllLines(fileName);
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            // parse header
            ReturnCode returnCode = ParseHeader(lines);
            if (returnCode != ReturnCode.SUCCESS) return returnCode;

            // Read data
            int nDataColumns = Headers.Length - DateTimeColumn - 1;
            int firstDataColumn = DateTimeColumn + 1;
            TimeStamps = new DateTime[1];
            Data = new double[1, nDataColumns];
            int lIndex = dataStartIndex + 1;
            string line = lines[lIndex];
            string[] tokens;
            tokens = line.Split(',');
            TimeStamps[0] = DateTime.ParseExact(tokens[DateTimeColumn], TIMESTAMP_FORMAT, CULTURE_INFO);
            for (int c = 0; c < nDataColumns; c++)
            {
                Data[0, c] = double.Parse(tokens[c + firstDataColumn]);
            }
            return ReturnCode.SUCCESS;
        }

        public ReturnCode ParseFile(string fileName)
        {
            // Read lines from file
            string[] lines;
            try
            {
                lines = IOUtility.PermissiveReadAllLines(fileName);
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            // parse header
            ReturnCode returnCode = ParseHeader(lines);
            if (returnCode != ReturnCode.SUCCESS) return returnCode;

            // Count data lines
            int lIndex = dataStartIndex + 1;
            string line = lines[lIndex];
            while (line[0] != '$')
            {
                lIndex++;
                line = lines[lIndex];
            }
            int nDataLines = lIndex - dataStartIndex - 1;

            // Read data
            int nDataColumns = Headers.Length - DateTimeColumn - 1;
            int firstDataColumn = DateTimeColumn + 1;
            TimeStamps = new DateTime[nDataLines];
            Data = new double[nDataLines, nDataColumns];
            lIndex = dataStartIndex + 1;
            line = lines[lIndex];
            string[] tokens;
            for(int d=0; d<nDataLines; d++)
            {
                tokens = line.Split(',');
                TimeStamps[d] = DateTime.ParseExact(tokens[DateTimeColumn], TIMESTAMP_FORMAT, CULTURE_INFO);
                for (int c=0; c<nDataColumns; c++)
                {
                    Data[d, c] = double.Parse(tokens[c + firstDataColumn]);
                }
                lIndex++;
                line = lines[lIndex];
            }

            return ReturnCode.SUCCESS;
        }
    }
}
