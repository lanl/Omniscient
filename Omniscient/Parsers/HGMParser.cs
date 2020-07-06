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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    class HGMParser : SpectrumParser
    {
        const string PARSER_TYPE = "HGM";

        public List<Spectrum> Spectra { get; private set; }

        public HGMParser() : base(PARSER_TYPE)
        {
            Spectra = new List<Spectrum>();
        }

        public override Spectrum GetSpectrum()
        {
            if (Spectra.Count > 0)
                return Spectra[0];
            else return null;
        }

        public override List<Spectrum> GetSpectra()
        {
            return Spectra;
        }

        char[] spaceSplitter = new char[] { ' ' };

        string[] lines;
        int lineIndex;
        int nBins;
        int recordPeriod;
        int recordsPerHGM;
        

        private void ParseHeader()
        {
            string[] tokens;
            string lowerline;
            while (lineIndex < lines.Length && (lines[lineIndex].Length > 2))
            {
                lowerline = lines[lineIndex].ToLower();
                if (lowerline.StartsWith("nbins"))
                {
                    tokens = lowerline.Split(spaceSplitter, StringSplitOptions.RemoveEmptyEntries);
                    nBins = int.Parse(tokens[1]);
                }
                else if (lowerline.StartsWith("recordperiod"))
                {
                    tokens = lowerline.Split(spaceSplitter, StringSplitOptions.RemoveEmptyEntries);
                    recordPeriod = int.Parse(tokens[1]);
                }
                else if (lowerline.StartsWith("recordsperhgm"))
                {
                    tokens = lowerline.Split(spaceSplitter, StringSplitOptions.RemoveEmptyEntries);
                    recordsPerHGM = int.Parse(tokens[1]);
                }
                lineIndex++;
            }
            lineIndex++;
        }

        public override ReturnCode ParseSpectrumFile(string newFileName)
        {
            Spectra = new List<Spectrum>();
            
            try
            {
                lines = IOUtility.PermissiveReadAllLines(newFileName);
            }
            catch
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            lineIndex = 3;
            nBins = -1;
            recordPeriod = -1;
            recordsPerHGM = -1;

            ParseHeader();

            string[] tokens;
            string[] dateTokens;
            string[] timeTokens;
            int[] counts;
            int elapsedTime = -1;
            DateTime dateTime;
            while (lineIndex < (lines.Length - nBins + 2))
            {
                tokens = lines[lineIndex].Split(',');
                if (tokens[0] == "Quaesta Instruments")
                {
                    ParseHeader();
                    continue;
                }
                dateTokens = tokens[0].Split('/');
                timeTokens = tokens[1].Split(':');
                dateTime = new DateTime(int.Parse(dateTokens[0]),
                    int.Parse(dateTokens[1]),
                    int.Parse(dateTokens[2]),
                    int.Parse(timeTokens[0]),
                    int.Parse(timeTokens[1]),
                    int.Parse(timeTokens[2]));
                lineIndex++;
                counts = new int[nBins];
                for (int bin = 0; bin < nBins; bin++)
                {
                    tokens = lines[lineIndex].Split(',');
                    counts[bin] = int.Parse(tokens[1]);
                    lineIndex++;
                }
                timeTokens = lines[lineIndex].Split('=');
                elapsedTime = int.Parse(timeTokens[1]);
                lineIndex += 2;
                if (elapsedTime > 0) Spectra.Add(new Spectrum(0, 1, counts, dateTime, elapsedTime, elapsedTime));
                else Spectra.Add(new Spectrum(0, 1, counts, dateTime, recordPeriod * recordsPerHGM, recordPeriod * recordsPerHGM));
            }

            return ReturnCode.SUCCESS;
        }
    }
}
