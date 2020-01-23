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
﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class CSVParser
    {
        private const string NOT_A_TIMESTAMP = "NOT A TIMESTAMP";
        private DateTime lowerBound = new DateTime(1900, 1, 1);
        private DateTime upperBound = new DateTime(3000, 1, 1);

        public enum DelimiterType { Comma, CommaOrWhitespace };

        private System.Globalization.CultureInfo CULTURE_INFO = new CultureInfo("en-US");
        private char[] TRIM_CHARS = new char[] { ':' };

        private string fileName;
        public string FileName {
            get { return fileName; }
            set { fileName = value; }
        }

        public DelimiterType Delimiter { get; set; }

        private int nHeaders;
        public int NumberOfHeaders
        {
            get { return nHeaders; }
            set { nHeaders = value; }
        }

        private string timeStampFormat;
        public string TimeStampFormat
        {
            get { return timeStampFormat; }
            set { timeStampFormat = value; }
        }

        private int nColumns;
        public int NumberOfColumns
        {
            get { return nColumns; }
            set { nColumns = value; }
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

        private int nRecords;

        public CSVParser()
        {
            fileName = "";
            nHeaders = 0;
            timeStampFormat = "";
            nColumns = 2;
            nRecords = 0;
        }

        public int GetNumRecords()
        {
            return nRecords;
        }

        public ReturnCode ParseFirstEntry(string newFileName)
        {
            fileName = newFileName;
            string[] lines;
            try
            {
                lines = IOUtility.PermissiveReadLines(fileName, nHeaders+1);
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            int nDataLines = lines.Length - nHeaders;
            if (nDataLines < 1)
                return ReturnCode.CORRUPTED_FILE;
            nDataLines = 1;

            if (GetEndTimes)
            {
                data = new double[nDataLines, nColumns - 2];
            }
            else
            {
                data = new double[nDataLines, nColumns - 1];
            }
            timeStamps = new DateTime[nDataLines];
            if (GetEndTimes) endTimes = new DateTime[nDataLines];

            nRecords = nDataLines;
            int entry;
            try
            {
                string[] tokens;
                int i = nHeaders;
                entry = i - nHeaders;
                char[] delimiters = new char[] { ',' };
                switch (Delimiter)
                {
                    case DelimiterType.Comma:
                        delimiters = new char[] { ',' };
                        break;
                    case DelimiterType.CommaOrWhitespace:
                        delimiters = new char[] { ',', ' ', '\t' };
                        break;
                }
                tokens = lines[i].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length < nColumns)
                    return ReturnCode.CORRUPTED_FILE;
                if (TimeStampFormat == "")
                {
                    timeStamps[entry] = DateTime.Parse(tokens[0].Trim(TRIM_CHARS));
                    if (GetEndTimes) endTimes[entry] = DateTime.Parse(tokens[1].Trim(TRIM_CHARS));
                }
                else
                {
                    timeStamps[entry] = DateTime.ParseExact(tokens[0].Trim(TRIM_CHARS), TimeStampFormat, CULTURE_INFO);
                    if (GetEndTimes) endTimes[entry] = DateTime.ParseExact(tokens[0].Trim(TRIM_CHARS), TimeStampFormat, CULTURE_INFO);
                }
                int dataColStart = 1;
                if (GetEndTimes) dataColStart = 2;
                for (int j = dataColStart; j < nColumns; j++)
                {
                    data[entry, j - dataColStart] = double.Parse(tokens[j]);
                }
            }
            catch
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
            // Read all lines
            fileName = newFileName;
            string[] lines;
            try
            {
                lines = IOUtility.PermissiveReadAllLines(fileName);
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }
            if (lines.Length < 1) return ReturnCode.CORRUPTED_FILE;

            // Initialize
            bool successfullyConfigured = false;
            int[] timeStampPosition = new int[lines.Length];
            int[] nCells = new int[lines.Length];
            for (int i = 0; i < timeStampPosition.Length; i++)
            {
                timeStampPosition[i] = -1;
                nCells[i] = -1;
            }
            List<DelimiterType> delimeterOptions = new List<DelimiterType>() { DelimiterType.Comma, DelimiterType.CommaOrWhitespace };
            // Try to find a time stamp in each line and count number of cells
            foreach (DelimiterType delimiterType in delimeterOptions)
            {
                char[] delimiters = GetDelimiterChars(delimiterType);
                string[] tokens;
                for (int l=0; l<lines.Length; l++)
                {
                    tokens = lines[l].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    nCells[l] = tokens.Length;
                    for (int t=0; t< tokens.Length; t++)
                    { 
                        if (GetTimeStampFormat(tokens[t].Trim(TRIM_CHARS)) != NOT_A_TIMESTAMP)
                        {
                            timeStampPosition[l] = t;
                            break;
                        }
                    }
                }

                // Assumption: last line is definitely data. Data lines above it should look like it.
                if (timeStampPosition[lines.Length - 1] != 0) continue;
                int totalColumns = nCells[lines.Length - 1];
                if (totalColumns < 2) continue;     // Needs to be a timestamp and some data

                // At this point we should have a working file. Just need to know how many header lines

                NumberOfColumns = totalColumns;
                Delimiter = delimiterType;
                TimeStampFormat = GetTimeStampFormat(lines[lines.Length - 1].
                    Split(delimiters, StringSplitOptions.RemoveEmptyEntries)[0].
                    Trim(TRIM_CHARS));
                GetEndTimes = false;                        // TODO: Make this more flexible
                for (int l = lines.Length - 2; l>=0; l--)
                {
                    if (timeStampPosition[l] != timeStampPosition[lines.Length-1] ||
                        nCells[l] != totalColumns)
                    {
                        NumberOfHeaders = l + 1;
                        break;
                    }
                }
                successfullyConfigured = true;
                break;
            }

            if(successfullyConfigured) return ReturnCode.SUCCESS;
            else return ReturnCode.FAIL;
        }

        private string GetTimeStampFormat(string str)
        {
            
            string[] formatOptions = new string[] { "yyyy-MM-dd HH:mm:ss",
                                                    "yyyy-MM-ddTHH:mm:ss",
                                                    "yyyy/MM/dd HH:mm:ss",
                                                    "yyyy/MM/ddTHH:mm:ss",
                                                    "MM-dd-yyyy HH:mm:ss",
                                                    "MM/dd/yyyy HH:mm:ss",
                                                    "MM-dd-yyyyTHH:mm:ss",
                                                    "MM/dd/yyyyTHH:mm:ss",
                                                    "yyyyMMddTHHmmss" };
            DateTime dateTime;

            // First try the default parser
            try
            {
                dateTime = DateTime.Parse(str);
                if (dateTime > lowerBound && dateTime < upperBound) return "";
            }
            catch { }

            // Try each format option
            foreach (string format in formatOptions)
            {
                try
                {
                    dateTime = DateTime.ParseExact(str, format, CULTURE_INFO);
                    if (dateTime > lowerBound && dateTime < upperBound) return format;
                }
                catch { }
            }

            return NOT_A_TIMESTAMP;
        }

        private char[] GetDelimiterChars(DelimiterType delimiterType)
        {
            char[] delimiters = new char[] { ',' };
            switch (delimiterType)
            {
                case DelimiterType.Comma:
                    delimiters = new char[] { ',' };
                    break;
                case DelimiterType.CommaOrWhitespace:
                    delimiters = new char[] { ',', ' ', '\t' };
                    break;
            }
            return delimiters;
        }

        public ReturnCode ParseFile(string newFileName)
        {
            fileName = newFileName;
            string[] lines;
            try
            {
                lines = IOUtility.PermissiveReadAllLines(fileName);
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            int nDataLines = lines.Length - nHeaders;
            if (nDataLines < 0)
                return ReturnCode.CORRUPTED_FILE;

            if (GetEndTimes)
            {
                data = new double[nDataLines, nColumns - 2];
            }
            else
            {
                data = new double[nDataLines, nColumns - 1];
            }
            timeStamps = new DateTime[nDataLines];
            if (GetEndTimes) endTimes = new DateTime[nDataLines];

            nRecords = nDataLines;

            char[] delimiters = GetDelimiterChars(Delimiter);

            int entry;
            try
            {
                string[] tokens;

                if (TimeStampFormat == "")
                {
                    if (GetEndTimes)
                    {
                        for (int i = nHeaders; i < lines.Length; i++)
                        {
                            entry = i - nHeaders;
                            tokens = lines[i].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            if (tokens.Length < nColumns)
                                return ReturnCode.CORRUPTED_FILE;
                            timeStamps[entry] = DateTime.Parse(tokens[0].Trim(TRIM_CHARS));
                            endTimes[entry] = DateTime.Parse(tokens[1].Trim(TRIM_CHARS));
                            for (int j = 2; j < nColumns; j++)
                            {
                                data[entry, j - 2] = double.Parse(tokens[j]);
                            }
                        }
                    }
                    else
                    {
                        for (int i = nHeaders; i < lines.Length; i++)
                        {
                            entry = i - nHeaders;
                            tokens = lines[i].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            if (tokens.Length < nColumns)
                                return ReturnCode.CORRUPTED_FILE;
                            timeStamps[entry] = DateTime.Parse(tokens[0].Trim(TRIM_CHARS));
                            for (int j = 1; j < nColumns; j++)
                            {
                                data[entry, j - 1] = double.Parse(tokens[j]);
                            }
                        }
                    }
                }
                else
                {
                    if (GetEndTimes)
                    {
                        for (int i = nHeaders; i < lines.Length; i++)
                        {
                            entry = i - nHeaders;
                            tokens = lines[i].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            if (tokens.Length < nColumns)
                                return ReturnCode.CORRUPTED_FILE;
                            timeStamps[entry] = DateTime.ParseExact(tokens[0].Trim(TRIM_CHARS), TimeStampFormat, CULTURE_INFO);
                            endTimes[entry] = DateTime.ParseExact(tokens[1].Trim(TRIM_CHARS), TimeStampFormat, CULTURE_INFO);
                            for (int j = 2; j < nColumns; j++)
                            {
                                data[entry, j - 2] = double.Parse(tokens[j]);
                            }
                        }
                    }
                    else
                    {
                        for (int i = nHeaders; i < lines.Length; i++)
                        {
                            entry = i - nHeaders;
                            tokens = lines[i].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            if (tokens.Length < nColumns)
                                return ReturnCode.CORRUPTED_FILE;
                            timeStamps[entry] = DateTime.ParseExact(tokens[0].Trim(TRIM_CHARS), TimeStampFormat, CULTURE_INFO);
                            for (int j = 1; j < nColumns; j++)
                            {
                                data[entry, j - 1] = double.Parse(tokens[j]);
                            }
                        }
                    }
                }
            }
            catch
            {
                return ReturnCode.CORRUPTED_FILE;
            }

            return ReturnCode.SUCCESS;
        }
    }
}
