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
    class SMMSParser
    {
        const string TIMESTAMP_FORMAT = "yyyy/MM/dd HH:mm:ss";
        const int COLUMNS_PER_INSTRUMENT = 6;
        private System.Globalization.CultureInfo CULTURE_INFO = new CultureInfo("en-US");

        public string[] Headers { get; private set; }
        public double[,] Data { get; private set; }
        public DateTime[] TimeStamps { get; private set; }

        public int NumberOfInstruments { get; set; }

        public SMMSParser()
        {

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

            int nDataLines = lines.Length - 1;
            if (nDataLines < 0)
                return ReturnCode.CORRUPTED_FILE;

            // Get headers from first line
            Headers = lines[0].Split(',');

            int nDataColumns = NumberOfInstruments * COLUMNS_PER_INSTRUMENT;
            if (Headers.Length != nDataColumns + 2 || Headers[0] != "SOH" || Headers[1] != "DateTime") return ReturnCode.CORRUPTED_FILE;

            // Iterate through data lines (data is stored in reverse order)
            Data = new double[nDataLines, nDataColumns];
            TimeStamps = new DateTime[nDataLines];
            string[] tokens;
            int maxIndex = nDataLines - 1;
            for (int dataIndex=0; dataIndex < nDataLines; dataIndex++)
            {
                tokens = lines[dataIndex+1].Split(',');
                TimeStamps[maxIndex-dataIndex] = DateTime.ParseExact(tokens[1], TIMESTAMP_FORMAT, CULTURE_INFO);
                for (int col = 0; col < nDataColumns; col++)
                {
                    Data[maxIndex - dataIndex, col] = double.Parse(tokens[col+2]);
                }
            }

            return ReturnCode.SUCCESS;
        }
    }
}
