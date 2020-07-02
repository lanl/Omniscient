// This software is open source software available under the BSD-3 license.
// 
// Copyright (c) 2020, International Atomic Energy Agency (IAEA), IAEA.org

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

        public override ReturnCode ParseSpectrumFile(string newFileName)
        {
            Spectra = new List<Spectrum>();

            string[] lines;
            try
            {
                lines = IOUtility.PermissiveReadAllLines(newFileName);
            }
            catch
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            int lineIndex = 3;
            int nBins = -1;
            int recordPeriod = -1;
            int recordsPerHGM = -1;
            string lowerline;
            string[] tokens;
            char[] spaceSplitter = new char[] { ' ' };
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
            
            string[] dateTokens;
            string[] timeTokens;
            int[] counts;
            DateTime dateTime;
            while (lineIndex < (lines.Length - nBins + 2))
            {
                tokens = lines[lineIndex].Split(',');
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
                lineIndex+=2;
                Spectra.Add(new Spectrum(0, 1, counts, dateTime, recordPeriod * recordsPerHGM, recordPeriod * recordsPerHGM));
            }

            return ReturnCode.SUCCESS;
        }
    }
}
