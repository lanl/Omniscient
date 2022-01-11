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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public struct ATPMRecord
    {
        public UInt32 time;
        public float volumFlow;
        public float tempSupply;
        public float tempReturn;
        public float actualPow;
    }

    public class ATPMParser
    {
        private DateTime TIMEBASE = new DateTime(1970, 1, 1, 0, 0, 0);
        private const int DEFAULT_HEADER_SIZE = 1024;

        public string Version { get; private set; } = "";
        public string FileName { get; private set; } = "";
        public string FacilityName { get; private set; } = "";
        public string FacilityCode { get; private set; } = "";
        public string EquipmentCode { get; private set; } = "";
        public int HeaderSize { get; private set; } = 0;
        public ATPMRecord[] Records { get; private set; }
        public DateTime Date { get; private set; }

        public DateTime ATPMTimeToDateTime(UInt32 timeIn)
        {
            return TIMEBASE.AddSeconds((double)timeIn);
        }

        private void ReadHeader(BinaryReader readBinary)
        {
            string header = new string(readBinary.ReadChars(DEFAULT_HEADER_SIZE));
            string[] lines = header.Split(new char[] { '\r', '\n' });
            string[] tokens;
            foreach (string line in lines)
            {
                if (line.Length < 2) continue;

                tokens = line.Split(new char[]{ ':'}, 2);
                if (tokens[0][0] != '$') continue;
                switch (tokens[0])
                {
                    case "$SoftwareVersion":
                        Version = tokens[1];
                        break;
                    case "$FileName":
                        FileName = tokens[1];
                        break;
                    case "$FacilityName":
                        FacilityName = tokens[1];
                        break;
                    case "$FacilityCode":
                        FacilityCode = tokens[1];
                        break;
                    case "$EquipmentCode":
                        EquipmentCode = tokens[1];
                        break;
                    case "$HeaderSize":
                        HeaderSize = int.Parse(tokens[1]);
                        break;
                }
            }
        }

        private void ReadDataRecords(BinaryReader readBinary)
        {
            readBinary.BaseStream.Seek(HeaderSize, SeekOrigin.Begin);
            long numBytes = readBinary.BaseStream.Length;
            // Read data records
            int numRecords = (int)((numBytes - HeaderSize) / 20);
            Records = new ATPMRecord[numRecords];
            for (int r = 0; r < numRecords; ++r)
            {
                Records[r] = new ATPMRecord();
                Records[r].time = readBinary.ReadUInt32();
                Records[r].volumFlow = readBinary.ReadSingle();
                Records[r].tempSupply = readBinary.ReadSingle();
                Records[r].tempReturn = readBinary.ReadSingle();
                Records[r].actualPow = readBinary.ReadSingle();
            }

            Date = ATPMTimeToDateTime(Records[0].time);
        }

        // ParseHeader is for quickly reading some header data without reading in the whole file
        public ReturnCode ParseHeader(string newFileName)
        {
            string fileName = newFileName;
            FileStream readStream;

            try
            {
                readStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader readBinary = new BinaryReader(readStream);
                ReadHeader(readBinary);
                Records = new ATPMRecord[1];
                int r = 0;
                Records[r] = new ATPMRecord();
                Records[r].time = readBinary.ReadUInt32();
                Records[r].volumFlow = readBinary.ReadSingle();
                Records[r].tempSupply = readBinary.ReadSingle();
                Records[r].tempReturn = readBinary.ReadSingle();
                Records[r].actualPow = readBinary.ReadSingle();
                Date = ATPMTimeToDateTime(Records[0].time);
                readStream.Close();
                
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            return ReturnCode.SUCCESS;
        }

        public ReturnCode ParseFile(string newFileName)
        {
            string fileName = newFileName;
            FileStream readStream;

            try
            {
                readStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader readBinary = new BinaryReader(readStream);
                ReadHeader(readBinary);
                if (HeaderSize < 1)
                {
                    readStream.Close();
                    return ReturnCode.CORRUPTED_FILE;
                }
                ReadDataRecords(readBinary);
                readStream.Close();
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            return ReturnCode.SUCCESS;
        }
    }
}
