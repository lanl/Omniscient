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

namespace Omniscient
{
    public class ROI
    {
        public enum BG_Type { NONE, FLAT, LINEAR}

        public static string BGTypeToString(BG_Type type)
        {
            switch (type)
            {
                case BG_Type.NONE:
                    return "None";
                case BG_Type.FLAT:
                    return "Flat";
                case BG_Type.LINEAR:
                    return "Linear";
            }
            return "Weird things are happening...";
        }

        /// <summary>
        /// If true, values are given in keV. 
        /// If false, values are given as channel number.
        /// </summary>
        public bool InputKeV { get; set; }

        public double ROIStart { get; set; }    // keV if InputKeV is true. Otherwise, channel.
        public double ROIEnd { get; set; }      // keV if InputKeV is true. Otherwise, channel.

        public double BG1Start { get; set; }
        public double BG1End { get; set; }
        public double BG2Start { get; set; }
        public double BG2End { get; set; }

        public BG_Type BGType { get; set; }

        public ROI()
        {
            ROIStart = 0;
            ROIEnd = 0;
            BG1Start = 0;
            BG1End = 0;
            BG2Start = 0;
            BG2End = 0;
            BGType = BG_Type.NONE;
        }

        public double GetROICounts(Spectrum spec)
        {
            int[] counts = spec.GetCounts();
            double[] bins = spec.GetBins();
            double totalCounts = 0;
            int roiBins = 0;
            double bg1Counts = 0;
            int bg1Bins = 0;
            double bg2Counts = 0;
            int bg2Bins = 0;

            if (InputKeV)
            { 
                for (int i=0; i<bins.Length;i++)
                {
                    if (bins[i] >= ROIStart && bins[i] <= ROIEnd)
                    {
                        totalCounts += counts[i];
                        roiBins++;
                    }
                    if (bins[i] >= BG1Start && bins[i] <= BG1End)
                    {
                        bg1Counts += counts[i];
                        bg1Bins++;
                    }
                    if (bins[i] >= BG2Start && bins[i] <= BG2End)
                    {
                        bg2Counts += counts[i];
                        bg2Bins++;
                    }
                }
            }
            else
            {
                for (int i = (int)BG1Start; i <= (int)BG1End; i++)
                {
                    bg1Counts += counts[i];
                    bg1Bins++;
                }
                for (int i = (int)ROIStart; i <= (int)ROIEnd; i++)
                {
                    totalCounts += counts[i];
                    roiBins++;
                }
                for (int i = (int)BG2Start; i <= (int)BG2End; i++)
                {
                    bg2Counts += counts[i];
                    bg2Bins++;
                }
            }
            switch (BGType)
            {
                case BG_Type.NONE:
                    return totalCounts;
                case BG_Type.FLAT:
                    return totalCounts - (bg1Counts * roiBins / bg1Bins);
                case BG_Type.LINEAR:
                    return totalCounts - roiBins * ((bg1Counts / bg1Bins) + (bg2Counts / bg2Bins)) / 2;
            }

            return 0;
        }

        public double GetROICountRate(Spectrum spec)
        {
            return GetROICounts(spec) / spec.GetLiveTime();
        }
    }
}
