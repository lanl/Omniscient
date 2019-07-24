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
using System.Globalization;

namespace Omniscient
{
    public struct VBFRecord
    {
        public double time;
        //public UInt16 status;
        public float[] data;            // 8 element array
        //public UInt16 elapsedTime;
    }

    public class VBFParser
    {
        private const int RECORD_SIZE = 40;
        private const int HEADER_SKIP_SIZE = 272;
        private const int DATE_TIME_SIZE = 25;
        private const int HEADER_SIZE = 3072;
        private const int RECORD_CHUNK_SIZE = 1000000;

        private string fileName;
        private DateTime date;
        private int numRecords;
        private VBFRecord[] records;


        /// <summary>
        /// Constructor
        /// </summary>
        public VBFParser()
        {
            fileName = "";   
            numRecords = 0;
        }

        /// <summary>
        /// Convert vbf file time (seconds since 1/1/1904) to DateTime
        /// </summary>
        public DateTime VBFTimeToDateTime(double timeIn)
        {
            return new DateTime(1904, 1, 1).AddSeconds(timeIn);
        }

        /// <summary>
        /// Read header information from an open vbf file.
        /// </summary>
        private void ReadHeader(BinaryReader readBinary)
        {
            readBinary.ReadBytes(HEADER_SKIP_SIZE);
            string rawDateTime = new string(readBinary.ReadChars(DATE_TIME_SIZE));
            date = DateTime.Parse(rawDateTime);
        }

        /// <summary>
        /// Read the data records from an open vbf file.
        /// </summary>
        private void ReadDataRecords(BinaryReader readBinary)
        {
            long numBytes = readBinary.BaseStream.Length;
            byte[] buffer8 = new byte[8];
            byte[] buffer4 = new byte[4];
            byte[] chunk;
            // Read data records
            numRecords = (int)((numBytes - HEADER_SIZE) / RECORD_SIZE);
            records = new VBFRecord[numRecords];
            readBinary.BaseStream.Seek(HEADER_SIZE, SeekOrigin.Begin);

            int remainingRecords = numRecords;
            int r = 0;
            int chunkSize;
            while(r < numRecords)
            {
                if (remainingRecords > RECORD_CHUNK_SIZE)
                    chunkSize = RECORD_CHUNK_SIZE;
                else
                    chunkSize = remainingRecords;
                chunk = readBinary.ReadBytes(chunkSize * RECORD_SIZE);
                for (int i = 0; i < chunkSize; ++i)
                {
                    records[r] = new VBFRecord();
                    Array.Copy(chunk, i * RECORD_SIZE, buffer8, 0, 8);
                    Array.Reverse(buffer8);  // Convert endianness
                    records[r].time = BitConverter.ToDouble(buffer8, 0);
                    records[r].data = new float[8];
                    for (int j = 0; j < 8; j++)
                    {
                        Array.Copy(chunk, i * RECORD_SIZE + 4 * j + 8, buffer4, 0, 4);
                        Array.Reverse(buffer4);  // Convert endianness
                        records[r].data[j] = BitConverter.ToUInt32(buffer4, 0);
                    }
                    r++;
                }
                remainingRecords -= chunkSize;
            }
        }

        /// <summary>
        /// Parse an entire vbf file
        /// </summary>
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

        /// <summary>
        /// Quickly read some header data without reading in the whole file
        /// </summary>
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

        public DateTime GetDate() { return date; }
        public int GetNumRecords() { return numRecords; }

        public VBFRecord GetRecord(int index)
        {
            if (index < numRecords) return records[index];
            else return new VBFRecord();                    // This should probably be handled better...
        }
    }
}
