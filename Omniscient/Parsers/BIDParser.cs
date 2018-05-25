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
using System.IO;
using System.Globalization;

namespace Omniscient
{
    public struct BIDRecord
    {
        public UInt32 time;
        public UInt16 status;
        public float chACountRate;
        public float chBCountRate;
        public float chCCountRate;
        public float gamInGamCh1;
        public float gamCh1Sigma;
        public float gamInGamCh2;
        public float gamCh2Sigma;
        public UInt16 elapsedTime;
    }

    public class BIDParser
    {
        private string fileName;
        string MICVersion;
        string stationID;
        private int numRecords;
        private BIDRecord[] records;
        private DateTime date;

        private int headerSize;

        public BIDParser()
        {
            fileName = "";
            MICVersion = "";
            stationID = "";
            numRecords = 0;
            headerSize = 0;
        }

        public DateTime BIDTimeToDateTime(UInt32 timeIn)
        {
            return new DateTime(1952, 1, 1).AddSeconds((double)timeIn);
        }

        private void ReadHeader(BinaryReader readBinary)
        {
            headerSize = int.Parse(new string(readBinary.ReadChars(4)));
            readBinary.ReadBytes(5);                                        // Not used
            MICVersion = new string(readBinary.ReadChars(5));
            stationID = new string(readBinary.ReadChars(3));
            int year = 2000 + int.Parse(new string(readBinary.ReadChars(3)));
            int month = int.Parse(new string(readBinary.ReadChars(3)));
            int day = int.Parse(new string(readBinary.ReadChars(3)));
            date = new DateTime(year, month, day);
        }

        private void ReadDataRecords(BinaryReader readBinary)
        {
            readBinary.ReadBytes(headerSize - 22);                            // Spare room in header
            long numBytes = readBinary.BaseStream.Length;
            // Read data records
            numRecords = (int)((numBytes - 73) / 36);
            records = new BIDRecord[numRecords];
            for (int r = 0; r < numRecords; ++r)
            {
                records[r] = new BIDRecord();
                records[r].time = readBinary.ReadUInt32();
                records[r].status = readBinary.ReadUInt16();
                records[r].chACountRate = readBinary.ReadSingle();
                records[r].chBCountRate = readBinary.ReadSingle();
                records[r].chCCountRate = readBinary.ReadSingle();
                records[r].gamInGamCh1 = readBinary.ReadSingle();
                records[r].gamCh1Sigma = readBinary.ReadSingle();
                records[r].gamInGamCh2 = readBinary.ReadSingle();
                records[r].gamCh2Sigma = readBinary.ReadSingle();
                records[r].elapsedTime = readBinary.ReadUInt16();
            }
        }

        // ParseHeader is for quickly reading some header data without reading in the whole file
        public ReturnCode ParseHeader(string newFileName)
        {
            fileName = newFileName;
            FileStream readStream;

            try
            {
                readStream = new FileStream(fileName, FileMode.Open);
                BinaryReader readBinary = new BinaryReader(readStream);
                ReadHeader(readBinary);
                readStream.Close();
                numRecords = 0;
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            return ReturnCode.SUCCESS;
        }

        public ReturnCode ParseFile(string newFileName)
        {
            fileName = newFileName;
            FileStream readStream;

            try
            {
                readStream = new FileStream(fileName, FileMode.Open);
                BinaryReader readBinary = new BinaryReader(readStream);
                ReadHeader(readBinary);
                ReadDataRecords(readBinary);
                readStream.Close();
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            return ReturnCode.SUCCESS;
        }

        public string GetMICVerions() { return MICVersion; }
        public string GetStationID() { return stationID; }
        public DateTime GetDate() { return date; }
        public int GetNumRecords() { return numRecords; }

        public BIDRecord GetRecord(int index)
        {
            if (index < numRecords) return records[index];
            else return new BIDRecord();                    // This should probably be handled better...
        }
    }
}
