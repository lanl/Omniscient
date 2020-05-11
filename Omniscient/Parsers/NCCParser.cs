// This software is open source software available under the BSD-3 license.
// 
// Copyright (c) 2020, International Atomic Energy Agency (IAEA), IAEA.org
// Copyright (c) 2018, Triad National Security, LLC
// All rights reserved.
// 
// Copyright 2018. Triad National Security, LLC. This software was produced under U.S. Government contract 89233218CNA000001 for Los Alamos National Laboratory (LANL), which is operated by Triad National Security, LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  NEITHER THE GOVERNMENT NOR TRIAD NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.  If software is modified to produce derivative works, such modified software should be clearly marked, so as not to confuse it with the version available from LANL.
// 
// Additionally, redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 1.       Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
// 2.       Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 
// 3.       Neither the name of Triad National Security, LLC, Los Alamos National Laboratory, LANL, the U.S. Government, nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 
//  
// THIS SOFTWARE IS PROVIDED BY TRIAD NATIONAL SECURITY, LLC AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL TRIAD NATIONAL SECURITY, LLC OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Omniscient
{
    public class NCCParser
    {
        public NCCWriter.NCCType NCCMode;
        private string detectorID;
        private string itemID;
        private DateTime startDateTime;
        public List<NCCWriter.Cycle> Cycles;

        private string fileTypeCheck;

        public NCCParser()
        {
            NCCMode = NCCWriter.NCCType.VERIFICATION;
            detectorID = "XXXXXXXXXXX";
            itemID = "ItemID      ";
            Cycles = new List<NCCWriter.Cycle>();
        }

        private ReturnCode ParseHeader(BinaryReader readBinary)
        {
            fileTypeCheck = new string(readBinary.ReadChars(4));
            char modeChar = readBinary.ReadChar();
            switch (modeChar)
            {
                case 'B':
                    NCCMode = NCCWriter.NCCType.BACKGROUND;
                    break;
                case 'N':
                    NCCMode = NCCWriter.NCCType.NORMALIZATION;
                    break;
                case 'V':
                    NCCMode = NCCWriter.NCCType.VERIFICATION;
                    break;
                default:
                    return ReturnCode.CORRUPTED_FILE;
            }
            detectorID = new string(readBinary.ReadChars(11));
            itemID = new string(readBinary.ReadChars(12));
            string dateTimeString = new string(readBinary.ReadChars(16));
            startDateTime = new DateTime(2000 + int.Parse(dateTimeString.Substring(0, 2)),
                                            int.Parse(dateTimeString.Substring(3, 2)),
                                            int.Parse(dateTimeString.Substring(6, 2)),
                                            int.Parse(dateTimeString.Substring(8, 2)),
                                            int.Parse(dateTimeString.Substring(11, 2)),
                                            int.Parse(dateTimeString.Substring(14, 2)));
            // Stop short of cycles
            return ReturnCode.SUCCESS;
        }

        private ReturnCode ParseCycles(BinaryReader readBinary)
        {
            ushort nCycles = readBinary.ReadUInt16();
            Cycles = new List<NCCWriter.Cycle>(nCycles);
            NCCWriter.Cycle cycle;
            ushort nMultiplicityBins;
            for (int c = 0; c < nCycles; ++c)
            {
                cycle = new NCCWriter.Cycle();
                string dateTimeString = new string(readBinary.ReadChars(16));
                cycle.DateAndTime = new DateTime(2000 + int.Parse(dateTimeString.Substring(0, 2)),
                                                int.Parse(dateTimeString.Substring(3, 2)),
                                                int.Parse(dateTimeString.Substring(6, 2)),
                                                int.Parse(dateTimeString.Substring(8, 2)),
                                                int.Parse(dateTimeString.Substring(11, 2)),
                                                int.Parse(dateTimeString.Substring(14, 2)));
                cycle.CountSeconds = readBinary.ReadUInt16();
                cycle.Totals = readBinary.ReadDouble();
                cycle.RPlusA = readBinary.ReadDouble();
                cycle.A = readBinary.ReadDouble();
                cycle.Scaler1 = readBinary.ReadDouble();
                cycle.Scaler2 = readBinary.ReadDouble();
                nMultiplicityBins = readBinary.ReadUInt16();
                cycle.MultiplicityRPlusA = new UInt32[nMultiplicityBins];
                for (int m = 0; m < nMultiplicityBins; ++m)
                    cycle.MultiplicityRPlusA[m] = readBinary.ReadUInt32();
                cycle.MultiplicityA = new UInt32[nMultiplicityBins];
                for (int m = 0; m < nMultiplicityBins; ++m)
                    cycle.MultiplicityA[m] = readBinary.ReadUInt32();
                Cycles.Add(cycle);
            }

            return ReturnCode.SUCCESS;
        }

        public ReturnCode ReadNeutronCyclesFile(string fileName)
        {
            FileStream readStream;

            try
            {
                readStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader readBinary = new BinaryReader(readStream);

                // Read header
                ReturnCode returnCode = ParseHeader(readBinary);
                if (returnCode != ReturnCode.SUCCESS) return returnCode;

                // Read cycles
                returnCode = ParseCycles(readBinary);
                if (returnCode != ReturnCode.SUCCESS) return returnCode;

                readStream.Close();
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            return ReturnCode.SUCCESS;
        }

        public string GetDetectorID() { return detectorID; }
        public string GetItemID() { return itemID.TrimEnd(' '); }
        public DateTime GetStartDateTime() { return startDateTime; }
    }
}
