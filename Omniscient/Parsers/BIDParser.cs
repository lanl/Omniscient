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

        private int contentSize;

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

        private ReturnCode ReadHeader(BinaryReader readBinary)
        {
            try
            { 
                headerSize = int.Parse(new string(readBinary.ReadChars(4)));
                readBinary.ReadBytes(5);                                        // Not used
                MICVersion = new string(readBinary.ReadChars(5));
                stationID = new string(readBinary.ReadChars(3));
                int year = 2000 + int.Parse(new string(readBinary.ReadChars(3)));
                int month = int.Parse(new string(readBinary.ReadChars(3)));
                int day = int.Parse(new string(readBinary.ReadChars(3)));
                date = new DateTime(year, month, day);

                if (headerSize != 69 || date < new DateTime(1952, 1, 1) || date > new DateTime(3000, 1, 1))
                    return ReturnCode.CORRUPTED_FILE;
                else
                    return ReturnCode.SUCCESS;
            }
            catch
            {
                return ReturnCode.CORRUPTED_FILE;
            }
        }

        private void ReadDataRecords(BinaryReader readBinary)
        {
            readBinary.ReadBytes(headerSize - 22);                            // Spare room in header
            
            // Read data records
            numRecords = (int)((contentSize - 73) / 36);
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

        /// <summary>
        /// Returns a BinaryReader which has skipped past the ASN1 header.
        /// Returns null if BID data cannot be found.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private BinaryReader GetASN1Content(byte[] bytes)
        {
            ASN1Skipper skipper = new ASN1Skipper(bytes);
            ASN1Element element = skipper.FindContentElement(new byte[] { 0x20, 0x20, 0x36, 0x39 });
            if (element is null) return null;
            contentSize = element.Length;
            MemoryStream stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            stream.Seek(element.DataStart, SeekOrigin.Begin);
            return reader;
        }

        // ParseHeader is for quickly reading some header data without reading in the whole file
        public ReturnCode ParseHeader(string newFileName)
        {
            fileName = newFileName;
            FileStream readStream;

            try
            {
                readStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader readBinary = new BinaryReader(readStream);
                int nBytesToRead = 4096;
                if (readStream.Length < nBytesToRead) nBytesToRead = (int)readStream.Length;
                byte[] bytes = readBinary.ReadBytes(nBytesToRead);
                readStream.Close();

                MemoryStream stream = new MemoryStream(bytes);
                readBinary = new BinaryReader(stream);

                if (ReadHeader(readBinary) != ReturnCode.SUCCESS)
                {
                    // Try as a signed file
                    readBinary = GetASN1Content(bytes);
                    if (readBinary is null) return ReturnCode.CORRUPTED_FILE;
                    if (ReadHeader(readBinary) != ReturnCode.SUCCESS) return ReturnCode.CORRUPTED_FILE;
                }

                stream.Close();
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
                readStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader readBinary = new BinaryReader(readStream);
                contentSize = (int)readStream.Length;
                byte[] bytes = readBinary.ReadBytes(contentSize);
                readStream.Close();

                MemoryStream stream = new MemoryStream(bytes);
                readBinary = new BinaryReader(stream);

                if (ReadHeader(readBinary) != ReturnCode.SUCCESS)
                {
                    // Try as a signed file
                    readBinary = GetASN1Content(bytes);
                    if (readBinary is null) return ReturnCode.CORRUPTED_FILE;
                    if (ReadHeader(readBinary) != ReturnCode.SUCCESS) return ReturnCode.CORRUPTED_FILE;
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
