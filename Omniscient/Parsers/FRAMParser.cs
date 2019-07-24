// This software is open source software available under the BSD-3 license.
// 
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

namespace Omniscient
{
    class FRAMParser
    {
        string[] lines;

        private NuclearComposition nuclearComposition;

        public FRAMParser()
        {
            lines = null;
            nuclearComposition = new NuclearComposition();
        }

        public ReturnCode ReadFRAMOutput(string fileName)
        {
            nuclearComposition = new NuclearComposition();

            if (!File.Exists(fileName))
                return ReturnCode.COULD_NOT_OPEN_FILE;
            lines = IOUtility.PermissiveReadAllLines(fileName);

            return ReturnCode.SUCCESS;
        }

        public ReturnCode ParseUraniumResults()
        {
            // Locate the correct part of the file to parse
            int targetLine = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("5Isotopic"))
                {
                    targetLine = i;
                    break;
                }
            }
            if (targetLine < 0 || !lines[targetLine + 3].Contains("U235") || !lines[targetLine + 4].StartsWith("5mass%"))
                return ReturnCode.CORRUPTED_FILE;

            // Read mass percents
            string[] tokens = lines[targetLine + 4].Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            try
            {
                nuclearComposition.U234MassPercent.Value = double.Parse(tokens[1]);
                nuclearComposition.U235MassPercent.Value = double.Parse(tokens[2]);
                nuclearComposition.U236MassPercent.Value = double.Parse(tokens[3]);
                nuclearComposition.U238MassPercent.Value = double.Parse(tokens[4]);
            }
            catch
            {
                return ReturnCode.CORRUPTED_FILE;
            }

            // Read mass sigmas
            tokens = lines[targetLine + 5].Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            try
            {
                nuclearComposition.U234MassPercent.Uncertainty = double.Parse(tokens[1]);
                nuclearComposition.U235MassPercent.Uncertainty = double.Parse(tokens[2]);
                nuclearComposition.U236MassPercent.Uncertainty = double.Parse(tokens[3]);
                nuclearComposition.U238MassPercent.Uncertainty = double.Parse(tokens[4]);
            }
            catch
            {
                return ReturnCode.CORRUPTED_FILE;
            }

            return ReturnCode.SUCCESS;
        }

        public ReturnCode ParsePlutoniumResults()
        {
            // Locate the correct part of the file to parse
            int targetLine = -1;
            for (int i=0; i<lines.Length; i++)
            {
                if (lines[i].StartsWith("5Isotopic"))
                {
                    targetLine = i;
                    break;
                }
            }
            if (targetLine < 0 || !lines[targetLine + 3].Contains("Pu239") || !lines[targetLine+4].StartsWith("5mass%"))
                return ReturnCode.CORRUPTED_FILE;

            // Read mass percents
            string[] tokens = lines[targetLine + 4].Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            try
            {
                nuclearComposition.Pu238MassPercent.Value = double.Parse(tokens[1]);
                nuclearComposition.Pu239MassPercent.Value = double.Parse(tokens[2]);
                nuclearComposition.Pu240MassPercent.Value = double.Parse(tokens[3]);
                nuclearComposition.Pu241MassPercent.Value = double.Parse(tokens[4]);
                nuclearComposition.Pu242MassPercent.Value = double.Parse(tokens[5]);
                nuclearComposition.Am241MassPercent.Value = double.Parse(tokens[6]);
            }
            catch
            {
                return ReturnCode.CORRUPTED_FILE;
            }

            // Read mass sigmas
            tokens = lines[targetLine + 5].Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            try
            {
                nuclearComposition.Pu238MassPercent.Uncertainty = double.Parse(tokens[1]);
                nuclearComposition.Pu239MassPercent.Uncertainty = double.Parse(tokens[2]);
                nuclearComposition.Pu240MassPercent.Uncertainty = double.Parse(tokens[3]);
                nuclearComposition.Pu241MassPercent.Uncertainty = double.Parse(tokens[4]);
                nuclearComposition.Pu242MassPercent.Uncertainty = double.Parse(tokens[5]);
                nuclearComposition.Am241MassPercent.Uncertainty = double.Parse(tokens[6]);
            }
            catch
            {
                return ReturnCode.CORRUPTED_FILE;
            }

            return ReturnCode.SUCCESS;
        }

        public NuclearComposition GetNuclearComposition() { return nuclearComposition; }
    }
}
