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
using System.IO;

namespace Omniscient
{
    public class IOUtility
    {
        /// <summary>
        /// Reads all lines from a file without locking it
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string[] PermissiveReadAllLines(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    List<string> lines = new List<string>();
                    while (!reader.EndOfStream)
                    {
                        lines.Add(reader.ReadLine());
                    }
                    return lines.ToArray();
                }
            }
        }

        /// <summary>
        /// Reads up a number of lines (or fewer if the file is shorter) from a file without locking it
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string[] PermissiveReadLines(string fileName, int nLines)
        {
            int lineCount = 0;
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    List<string> lines = new List<string>();
                    while (!reader.EndOfStream && lineCount < nLines)
                    {
                        lines.Add(reader.ReadLine());
                        lineCount++;
                    }
                    return lines.ToArray();
                }
            }
        }

        /// <summary>
        /// Compacts a list of DateTimes into a list of longs (Ticks) using a
        /// twist on run length encoding: it looks at differences and applies
        /// RLE to those.
        /// </summary>
        /// <param name="times"></param>
        /// <returns></returns>
        public static List<long> DateCompact(List<DateTime> times)
        {
            int nTimes = times.Count();
            List<long> output = new List<long>(times.Count());
            if (nTimes < 1) return output;
            output.Add(times[0].Ticks);
            if (nTimes == 1) return output;
            output.Add(times[1].Ticks);
            if (nTimes == 2) return output;
            output.Add(times[2].Ticks);

            int outIndex = 3;
            long delta1 = output[1] - output[0];
            long delta2 = output[2] - output[1];

            bool inRun = delta2==delta1;
            long runCount = 0;

            for (int i=3; i<nTimes; i++)
            {
                if (inRun)
                {
                    if (times[i].Ticks - times[i - 1].Ticks == delta2)
                    {
                        runCount++;
                        continue;
                    }
                    else
                    {
                        inRun = false;
                        output.Add(runCount);
                        output.Add(times[i].Ticks);
                        outIndex+=2;
                        runCount = 0;
                    }
                }
                else
                {
                    delta1 = output[outIndex - 2] - output[outIndex - 3];
                    delta2 = output[outIndex - 1] - output[outIndex - 2];
                    if (delta1 == delta2)
                    {
                        inRun = true;
                        if (times[i].Ticks - times[i - 1].Ticks == delta2)
                        {
                            runCount++;
                            continue;
                        }
                        else
                        {
                            inRun = false;
                            output.Add(runCount);
                            output.Add(times[i].Ticks);
                            outIndex += 2;
                            runCount = 0;
                        }
                    }
                    else
                    {
                        output.Add(times[i].Ticks);
                        outIndex++;
                    }
                }
            }
            if (inRun) output.Add(runCount);

            return output;
        }

        /// <summary>
        /// Reverses DateCompact(List<DateTime> times)
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static List<DateTime> DateUnpack(List<long> ticks)
        {
            int nTicks = ticks.Count();
            List<DateTime> output = new List<DateTime>(nTicks);

            if (nTicks < 1) return output;
            output.Add(new DateTime(ticks[0]));
            if (nTicks == 1) return output;
            output.Add(new DateTime(ticks[1]));
            if (nTicks == 2) return output;
            output.Add(new DateTime(ticks[2]));

            long delta1, delta2;
            long count = 0;
            for(int t=3; t<nTicks; t++)
            {
                delta1 = ticks[t - 2] - ticks[t - 3];
                delta2 = ticks[t - 1] - ticks[t - 2];
                if(delta1==delta2)
                {
                    count = ticks[t-1];
                    for(int i=0; i <ticks[t]; i++)
                    {
                        count += delta1;
                        output.Add(new DateTime(count));
                    }
                }
                else
                {
                    output.Add(new DateTime(ticks[t]));
                }
            }

            return output;
        }

        /// <summary>
        /// Returns True if the string is an acceptable file name
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool ValidFileName(string fileName) 
        {
            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0) return false;
            if (fileName.Length == 0) return false;
            if (fileName.Length > 255) return false;
            return true;
        }
    }
}
