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
//using Newtonsoft.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Omniscient
{
    public class JSONParser
    {
        //private LDAQRecord[] records;

        private const string NOT_A_TIMESTAMP = "NOT A TIMESTAMP";
        private DateTime lowerBound = new DateTime(1900, 1, 1);
        private DateTime upperBound = new DateTime(3000, 1, 1);


        private System.Globalization.CultureInfo CULTURE_INFO = new CultureInfo("en-US");
        private char[] TRIM_CHARS = new char[] { ':' };

        private string timeStampFormat = "yyyy�-�MM�-�dd�T�HH�:�mm�:�ss";
        public string TimeStampFormat
        {
            get { return timeStampFormat; }
            set { timeStampFormat = value; }
        }

        private dynamic[,] data;
        public dynamic[,] Data
        {
            get { return data; }
        }

        private DateTime[] timeStamps;
        public DateTime[] TimeStamps
        {
            get { return timeStamps; }
        }



       // public JSONParser()
       // {
       //     fileName = "";
       //     TimeStampFormat = "yyyy-MM-ddTHH:mm:ss";
       //     nRecords = 0;
       // }

        private int numRecords;
        public int nRecords
        {
            get { return numRecords; }
            set { numRecords = value; }
        }

        private int numChannels;
        public int nChannels {
            get { return numChannels; }
            set { numChannels = value; }
        }

        public List<dynamic> ParseJObject(JObject o)
        {


            List<dynamic> attributes = new List<dynamic>();
            foreach (JProperty prop in o.Properties())
            {
                string name = prop.Name;
                dynamic value = prop.Value;
                if (value.GetType() == typeof(JObject))
                {
                    attributes.AddRange(ParseJObject(value));

                }
                else
                {
                    attributes.Add(value);
                }
            }

            return attributes;
        }
        public void ParseAndReadJSON(string json)
        {
            dynamic parsedJSON = JArray.Parse(json);
            List<dynamic> attributes = new List<dynamic>();

            // parse each row into attributes
            foreach (JObject o in parsedJSON.Children<JObject>())
            {
                attributes.Add(ParseJObject(o));
            }

            nRecords = attributes.Count;
            timeStamps = new DateTime[nRecords];
            nChannels = attributes[0].Count-1;
            data = new dynamic[nRecords, nChannels];

            // update timeStamps and data class members
            for (int row = 0; row < nRecords; row++)
            {
                timeStamps[row] = attributes[row][0];// this assumes first column is the time stamp
                attributes[row].RemoveAt(0);// this assumes first column is the time stamp  
                for (int col = 0; col < nChannels; col++)
                {
                    data[row, col] = attributes[row][col];

                }
            }
        }
        //parse first file entry to check for corrupted file
        //set timeStamps, nRecords
        public ReturnCode GetFirstDate(string newFileName)
        {
            string fileName = newFileName;
            string json;
            string[] tokens;

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
                //isolate time value
                string[] time_token = tokens[0].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                time_token = time_token[0].Split(new string[] {"Time\":"}, StringSplitOptions.RemoveEmptyEntries);
                time_token = time_token[1].Split('.');
                string time_t = time_token[0].Replace("\"", "");
                // update class members
                nRecords = 1;
                timeStamps = new DateTime[nRecords];
                timeStamps[0] = DateTime.ParseExact(time_t, TimeStampFormat, CULTURE_INFO);
                
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
            string fileName = newFileName;
            string json;
            try
            {
                using (StreamReader r = new StreamReader(newFileName))
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

                ParseAndReadJSON(json);
            }
            catch (Exception ex)
            {
                return ReturnCode.CORRUPTED_FILE;
            }
            return ReturnCode.SUCCESS;
        }
    }
}
