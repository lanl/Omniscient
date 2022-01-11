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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class EventWriter
    {
        private const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";
        public ReturnCode WriteEventFile(string fileName, List<Event> events)
        {
            // Open file for writing
            StreamWriter writeStream = null;
            try { writeStream = new StreamWriter(fileName); }
            catch
            {
                writeStream?.Close();
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            // Write header
            writeStream.WriteLine("Event List, Omniscient Version," + OmniscientCore.VERSION);
            writeStream.WriteLine("Event Start,Event End,Duration,Mean Value,Integral (hr),Max Value,Max Time,Comments");

            // Write event content
            foreach(Event eve in events)
            {
                writeStream.Write(eve.StartTime.ToString(DATE_TIME_FORMAT) + ",");
                writeStream.Write(eve.EndTime.ToString(DATE_TIME_FORMAT) + ",");
                writeStream.Write((eve.EndTime - eve.StartTime).TotalSeconds.ToString() + ",");
                writeStream.Write(eve.MeanValue.ToString() + ",");
                writeStream.Write(eve.MeanValue * (eve.EndTime - eve.StartTime).TotalHours + ",");
                writeStream.Write(eve.MaxValue.ToString() + ",");
                writeStream.Write(eve.MaxTime.ToString(DATE_TIME_FORMAT) + ",");
                writeStream.Write(eve.Comment + "\n");
            }

            // Close it out
            writeStream.Flush();
            writeStream.Close();

            return ReturnCode.SUCCESS;
        }
    }
}
