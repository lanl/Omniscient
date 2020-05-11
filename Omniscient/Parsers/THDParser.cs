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
    public struct THDRecord
    {
        public DateTime time;
        public double data0;
        public double data1;
        public double data2;
    }

    public class THDParser
    {
        public DateTime Date { get; private set; }
        public List<THDRecord> Records { get; private set; }

        string fileString;
        int dataStart = -1;

        private bool IsStartOfRecord(int i)
        {
            if (fileString[i + 4] == '-' && fileString[i + 7] == '-' && fileString[i + 10] == ' ' &&
                        fileString[i + 13] == ':' && fileString[i + 16] == ':' && fileString[i + 19] == ',')
            {
                return true;
            }
            return false;
        }

        private void FindDataStart()
        {
            int nChar = fileString.Length;
            dataStart = -1;
            for(int i = 0; i< nChar - 22; i++)
            {
                if (fileString[i] == '2' && fileString[i+1] == '0') // Initial check
                {
                    if (IsStartOfRecord(i))
                    {
                        dataStart = i;
                        break;
                    }
                }
            }
        }

        private void ReadRecords()
        {
            Records = new List<THDRecord>();

            int nChar = fileString.Length;
            int i = dataStart;
            THDRecord record;
            int recordStart = i;
            string recordString;
            string[] tokens;

            while(i<nChar)
            {
                while(fileString[i] != '\n') i++;
                recordString = fileString.Substring(recordStart, i - recordStart);
                tokens = recordString.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
                
                record = new THDRecord();
                record.time = DateTime.Parse(tokens[0]);
                record.data0 = double.Parse(tokens[1]);
                record.data1 = double.Parse(tokens[2]);
                record.data2 = double.Parse(tokens[3]);
                Records.Add(record);

                i++;

                if (i <nChar-22 && IsStartOfRecord(i))
                {
                    recordStart = i;
                }
                else
                {
                    break;
                }
            }

            if (Records.Count > 0) Date = Records[0].time;
        }

        private void ReadFirstRecord()
        {
            Records = new List<THDRecord>();

            int nChar = fileString.Length;
            int i = dataStart;
            THDRecord record;
            int recordStart = i;
            string recordString;
            string[] tokens;

            while (fileString[i] != '\n') i++;
            recordString = fileString.Substring(recordStart, i - recordStart);
            tokens = recordString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            record = new THDRecord();
            record.time = DateTime.Parse(tokens[0]);
            record.data0 = double.Parse(tokens[1]);
            record.data1 = double.Parse(tokens[2]);
            record.data2 = double.Parse(tokens[3]);
            Records.Add(record);

            if (Records.Count > 0) Date = Records[0].time;
        }

        public ReturnCode ParseFile(string newFileName)
        {
            try
            {
                using (FileStream readStream = new FileStream(newFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    StreamReader textReader = new StreamReader(readStream);
                    fileString = textReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            try
            {
                FindDataStart();
                ReadRecords();
            }
            catch (Exception ex)
            {
                return ReturnCode.CORRUPTED_FILE;
            }

            return ReturnCode.SUCCESS;
        }

        public ReturnCode ParseFirstRecord(string newFileName)
        {
            try
            {
                using (FileStream readStream = new FileStream(newFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    StreamReader textReader = new StreamReader(readStream);
                    fileString = textReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            try
            {
                FindDataStart();
                ReadFirstRecord();
            }
            catch (Exception ex)
            {
                return ReturnCode.CORRUPTED_FILE;
            }

            return ReturnCode.SUCCESS;
        }
    }
}
