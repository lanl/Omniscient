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

namespace Omniscient
{
    class DataDecimator
    {
        public List<double> Values { get; set; }
        public List<DateTime> TimeStamps { get; set; }

        public DataDecimator()
        {
        }

        public void Decimate(int bins, DateTime start, DateTime end)
        {
            int nValues = Values.Count();

            if (nValues < 4 * bins) return;     // No sense in going through all this trouble...

            List<double> newValues = new List<double>(4*bins + 1);
            List<DateTime> newTimeStamps = new List<DateTime>(4 * bins + 1);

            DateTime veryFirst = TimeStamps.First();

            TimeSpan range = end - start;

            DateTime binStart;
            DateTime binEnd;

            double firstVal;
            double maxVal;
            double minVal;
            double lastVal;
            DateTime firstTime;
            DateTime maxTime;
            DateTime minTime;
            DateTime lastTime;

            int index = 0;
            bool outOfData = false;
            for(int i=0; i<bins; ++i)
            {
                binStart = start.AddSeconds(range.TotalSeconds*i/bins);
                binEnd = start.AddSeconds(range.TotalSeconds * (i + 1) / bins);

                while (TimeStamps[index] < binStart)
                {
                    index += 1;
                    if (index == nValues)
                    {
                        outOfData = true;
                        break;
                    }
                }
                if (outOfData) break;

                if (TimeStamps[index] >= end) continue;     // Nothing in the bin? Fine, move on to the next one

                // Keep the first data point
                firstTime = maxTime = minTime = TimeStamps[index];
                firstVal = maxVal = minVal = Values[index];

                index += 1;
                while (TimeStamps[index] < binEnd)
                {
                    if(Values[index] > maxVal)  // Update max value
                    {
                        maxVal = Values[index];
                        maxTime = TimeStamps[index];
                    }
                    if (Values[index] < minVal) // Update min value
                    {
                        minVal = Values[index];
                        minTime = TimeStamps[index];
                    }
                    index += 1;
                }

                // Keep the last data point
                lastTime = TimeStamps[index - 1];
                lastVal = Values[index - 1];

                // Add values to new lists
                newTimeStamps.Add(firstTime);
                newValues.Add(firstVal);

                if(lastTime == firstTime)
                {
                    continue;
                }

                bool keepMax = false;
                bool keepMin = false;
                if (maxTime > firstTime && maxTime < lastTime)
                    keepMax = true;
                if (minTime > firstTime && minTime < lastTime)
                    keepMin = true;
                if (keepMax && keepMin)
                {
                    if(minTime<maxTime)
                    {
                        newTimeStamps.Add(minTime);
                        newValues.Add(minVal);
                        newTimeStamps.Add(maxTime);
                        newValues.Add(maxVal);
                    }
                    else
                    {
                        newTimeStamps.Add(maxTime);
                        newValues.Add(maxVal);
                        newTimeStamps.Add(minTime);
                        newValues.Add(minVal);
                    }
                }
                else if(keepMax)
                {
                    newTimeStamps.Add(maxTime);
                    newValues.Add(maxVal);
                }
                else if (keepMin)
                {
                    newTimeStamps.Add(maxTime);
                    newValues.Add(maxVal);
                }
                newTimeStamps.Add(lastTime);
                newValues.Add(lastVal);
            }
            if (index < nValues && TimeStamps[index] <= end)
            {
                newTimeStamps.Add(TimeStamps[index]);
                newValues.Add(Values[index]);
            }
            TimeStamps = newTimeStamps;
            Values = newValues;
        }
    }
}
