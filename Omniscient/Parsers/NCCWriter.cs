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
    public class NCCWriter
    {
        public enum NCCType { BACKGROUND, NORMALIZATION, VERIFICATION };

        public struct Cycle
        {
            public DateTime DateAndTime;
            public ushort CountSeconds;
            public double Totals;
            public double RPlusA;
            public double A;
            public double Scaler1;
            public double Scaler2;
            public UInt32[] MultiplicityRPlusA;
            public UInt32[] MultiplicityA;
        }

        public NCCType NCCMode;
        private string detectorID;
        private string itemID;
        public List<Cycle> Cycles;

        private const string FILE_HEADER_CHECK = "IREV";

        public NCCWriter()
        {
            NCCMode = NCCType.VERIFICATION;
            detectorID = "XXXXXXXXXXX";
            itemID = "ItemID      ";
            Cycles = new List<Cycle>();
        }

        public ReturnCode WriteNeutronCyclesFile(string fileName)
        {
            // Make sure there is data to write
            if (Cycles.Count < 1) return ReturnCode.FAIL;

            // Open file
            FileStream writeStream;
            try { writeStream = new FileStream(fileName, FileMode.Create); }
            catch { return ReturnCode.COULD_NOT_OPEN_FILE; }
            BinaryWriter binaryWriter = new BinaryWriter(writeStream);

            // Write Header
            binaryWriter.Write(FILE_HEADER_CHECK.ToCharArray());
            switch(NCCMode)
            {
                case NCCType.BACKGROUND:
                    binaryWriter.Write('B');
                    break;
                case NCCType.NORMALIZATION:
                    binaryWriter.Write('N');
                    break;
                case NCCType.VERIFICATION:
                    binaryWriter.Write('V');
                    break;
            }
            binaryWriter.Write(detectorID.ToCharArray());         // 11 characters
            binaryWriter.Write(itemID.ToCharArray());             // 12 characters
            binaryWriter.Write(Cycles[0].DateAndTime.ToString("yy.MM.ddHH:mm:ss").ToCharArray());
            binaryWriter.Write((ushort)Cycles.Count);

            // Write Cycles
            foreach(Cycle cycle in Cycles)
            {
                binaryWriter.Write(cycle.DateAndTime.ToString("yy.MM.ddHH:mm:ss").ToCharArray());
                binaryWriter.Write(cycle.CountSeconds);
                binaryWriter.Write(cycle.Totals);
                binaryWriter.Write(cycle.RPlusA);
                binaryWriter.Write(cycle.A);
                binaryWriter.Write(cycle.Scaler1);
                binaryWriter.Write(cycle.Scaler2);
                if (!(cycle.MultiplicityRPlusA is null) && !(cycle.MultiplicityA is null))
                {
                    binaryWriter.Write((ushort)cycle.MultiplicityRPlusA.Length);
                    for (int i = 0; i < cycle.MultiplicityRPlusA.Length; ++i)
                        binaryWriter.Write(cycle.MultiplicityRPlusA[i]);
                    for (int i = 0; i < cycle.MultiplicityA.Length; ++i)
                        binaryWriter.Write(cycle.MultiplicityA[i]);
                }
                else
                {
                    binaryWriter.Write((ushort)0);
                }
            }

            writeStream.Close();

            return ReturnCode.SUCCESS;
        }

        public ReturnCode SetDetectorID(string newID)
        {
            if (newID.Length != 11) return ReturnCode.BAD_INPUT;
            detectorID = newID;
            return ReturnCode.SUCCESS;
        }

        public ReturnCode SetItemID(string newID)
        {
            if (newID.Length > 12) return ReturnCode.BAD_INPUT;
            itemID = newID;
            while (itemID.Length < 12) itemID = itemID + ' ';
            return ReturnCode.SUCCESS;
        }

        public string GetDetectorID() { return detectorID; }
        public string GetItemID() { return itemID.TrimEnd(' '); }
    }
}
