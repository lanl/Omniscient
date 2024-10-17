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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Omniscient.CSVParser;

namespace Omniscient
{
    public class Canister
    {
        [JsonProperty(PropertyName = "FrontRightCounts")]
        public int frontRightCounts;
        [JsonProperty(PropertyName = "FrontLeftCounts")]
        public int frontLeftCounts;
        [JsonProperty(PropertyName = "BackRightCounts")]
        public int backRightCounts;
        [JsonProperty(PropertyName = "BackLeftCounts")]
        public int backLeftCounts;
        [JsonProperty(PropertyName = "GrossWeight")]
        public double grossWeight;
        [JsonProperty(PropertyName = "NetWeight")]
        public double netWeight;
    }
    public class Scales
    {
        [JsonProperty(PropertyName = "Type30B")]
        public Canister ThirtyB;
        [JsonProperty(PropertyName = "Type48Y")]
        public Canister FortyEightY;
    }
    public class LDAQRecord
    {
        [JsonProperty(PropertyName = "Time")]
        public string time;
        [JsonProperty(PropertyName = "Scales")]
        public Scales scales;
    }


    public class JSONParser
    {
        private LDAQRecord[] records;
        public DelimiterType Delimiter { get; set; }

        private const string NOT_A_TIMESTAMP = "NOT A TIMESTAMP";
        private DateTime lowerBound = new DateTime(1900, 1, 1);
        private DateTime upperBound = new DateTime(3000, 1, 1);


        private System.Globalization.CultureInfo CULTURE_INFO = new CultureInfo("en-US");
        private char[] TRIM_CHARS = new char[] { ':' };

        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        private string timeStampFormat = "yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss";
        public string TimeStampFormat
        {
            get { return timeStampFormat; }
            set { timeStampFormat = value; }
        }

        private string fileFormat;
        public string FileFormat
        {
            get { return fileFormat; }
            set { fileFormat = value; }
        }

        private double[,] data;
        public double[,] Data
        {
            get { return data; }
        }

        private DateTime[] timeStamps;
        public DateTime[] TimeStamps
        {
            get { return timeStamps; }
        }

        public bool GetEndTimes { get; set; }

        private DateTime[] endTimes;
        public DateTime[] EndTimes
        {
            get { return endTimes; }
        }

       // public JSONParser()
       // {
       //     fileName = "";
       //     TimeStampFormat = "yyyy-MM-ddTHH:mm:ss";
       //     nRecords = 0;
       // }

        private int nRecords;
        public int GetNumRecords()
        {
            return nRecords;
        }


        //parse first file entry to check for corrupted file
        //set timeStamps, nRecords
        public ReturnCode ParseFirstEntry(string newFileName)
        {
            fileName = newFileName;
            string json;
            string[] tokens;
            LDAQRecord record = new LDAQRecord();
            try
            {
                using (StreamReader r = new StreamReader(fileName))
                {
                    json = r.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }
            try
            {
                // remove white space
                json = json.Replace("\r", ""); json = json.Replace("\n", ""); json = json.Replace("\t", ""); json = json.Replace(" ", "");
                //chop up by "},{" . aka divide entries
                tokens = json.Split(new string[] { "},{" }, StringSplitOptions.RemoveEmptyEntries);
                //put first record in format the JSON converter will recognize
                string firstRecord = tokens[0].Replace("[", "");
                firstRecord += "}";

                using (StreamReader r = new StreamReader(fileName))
                {
                    record = JsonConvert.DeserializeObject<LDAQRecord>(firstRecord);
                } 
                //records.Append(record);
                //set values for 
                //timestamps
                //nRecords
                //tokens?
                //data
                int nDataLines = 1;
                nRecords = nDataLines;
                timeStamps = new DateTime[nDataLines];

                string[] temp = record.time.Split('.');
                string date = temp[0];
                timeStamps[0] = DateTime.ParseExact(date, TimeStampFormat, CULTURE_INFO);
                
            }
            catch (Exception ex)
            {
                return ReturnCode.CORRUPTED_FILE;
            }
            return ReturnCode.SUCCESS;
        }

        /// <summary>
        /// Attempt to configure parser to parse file
        /// </summary>
        /// <param name="newFileName"></param>
        /// <returns></returns>
        public ReturnCode AutoConfigureFromFile(string newFileName)
        {
            //todo: obs
            // do we need this?
            return ReturnCode.FAIL;
        }

        public ReturnCode ParseFile(string newFileName)
        {
            fileName = newFileName;
            try
            {
                using (StreamReader r = new StreamReader("file.json"))
                {
                    string json = r.ReadToEnd();
                    List<string> items = JsonConvert.DeserializeObject<List<string>>(json);
                }
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            return ReturnCode.SUCCESS;
        }
    }
}
