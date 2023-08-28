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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class EventParser
    {
        public int Version { get; private set; }
        public List<DateTime> StartTime { get; private set; }
        public List<DateTime> EndTime { get; private set; }
        public List<double> MaxValue { get; private set; }
        public List<DateTime> MaxTime { get; private set; }
        public List<string> Comments { get; private set; }

        public EventParser()
        {
            Version = 0;
            StartTime = new List<DateTime>();
            EndTime = new List<DateTime>();
            MaxValue = new List<double>();
            MaxTime = new List<DateTime>();
            Comments = new List<string>();
        }

        public int GetNumRecords()
        {
            return StartTime.Count;
        }

        public ReturnCode ParseFile(string fileName)
        {
            // Try reading the file into lines
            string[] lines;
            try
            {
                lines = IOUtility.PermissiveReadAllLines(fileName);
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }
            if (lines.Length < 2) return ReturnCode.CORRUPTED_FILE;

            // Read the first line
            string[] tokens;
            tokens = lines[0].Split(',');
            if (tokens[0].ToLower() != "event list") return ReturnCode.CORRUPTED_FILE;
            if (!tokens[1].ToLower().Contains("version")) return ReturnCode.CORRUPTED_FILE;
            int version;
            // TODO: track file version
            // if (!int.TryParse(tokens[2], out version)) return ReturnCode.CORRUPTED_FILE;
            // Version = version;

            // Read the column headers
            int eventStartCol = 0;
            int eventEndCol = 1;
            int durationCol = 2;
            int maxValCol = 3;
            int maxTimeCol = 4;
            int commentsCol = 5;
            int nColumns = 5;
            tokens = lines[1].Split(',');
            for (int col = 0; col < tokens.Length; col++)
            {
                switch(tokens[col].ToLower())
                {
                    case "event start":
                        eventStartCol = col;
                        if (col > nColumns) nColumns = col;
                        break;
                    case "event end":
                        eventEndCol = col;
                        if (col > nColumns) nColumns = col;
                        break;
                    case "duration":
                        durationCol = col;
                        if (col > nColumns) nColumns = col;
                        break;
                    case "max value":
                        maxValCol = col;
                        if (col > nColumns) nColumns = col;
                        break;
                    case "max time":
                        maxTimeCol = col;
                        if (col > nColumns) nColumns = col;
                        break;
                    case "comments":
                        commentsCol = col;
                        if (col > nColumns) nColumns = col;
                        break;
                }
            }
            nColumns++;

            // Read event content
            DateTime start;
            DateTime end;
            for (int l = 2; l <lines.Length; ++l)
            {
                tokens = lines[l].Split(',');
                if (tokens.Length < nColumns) return ReturnCode.CORRUPTED_FILE;
                start = DateTime.Parse(tokens[eventStartCol]);
                end = DateTime.Parse(tokens[eventEndCol]);
                StartTime.Add(start);
                EndTime.Add(end);
                MaxValue.Add(double.Parse(tokens[maxValCol]));
                MaxTime.Add(DateTime.Parse(tokens[maxTimeCol]));
                Comments.Add(tokens[commentsCol]);
            }

            return ReturnCode.SUCCESS;
        }
    }
}
