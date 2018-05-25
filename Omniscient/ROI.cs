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

        double roiStart;    // keV
        double roiEnd;      // keV

        double bg1Start;
        double bg1End;
        double bg2Start;
        double bg2End;

        BG_Type bgType;

        public ROI()
        {
            roiStart = 0;
            roiEnd = 0;
            bg1Start = 0;
            bg1End = 0;
            bg2Start = 0;
            bg2End = 0;
            bgType = BG_Type.NONE;
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
            for (int i=0; i<bins.Length;i++)
            {
                if (bins[i] >= roiStart && bins[i] <= roiEnd)
                {
                    totalCounts += counts[i];
                    roiBins++;
                }
                if (bins[i] >= bg1Start && bins[i] <= bg1End)
                {
                    bg1Counts += counts[i];
                    bg1Bins++;
                }
                if (bins[i] >= bg2Start && bins[i] <= bg2End)
                {
                    bg2Counts += counts[i];
                    bg2Bins++;
                }
            }
            switch (bgType)
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

        public double GetROIStart() { return roiStart; }
        public double GetROIEnd() { return roiEnd; }
        public double GetBG1Start() { return bg1Start; }
        public double GetBG1End() { return bg1End; }
        public double GetBG2Start() { return bg2Start; }
        public double GetBG2End() { return bg2End; }
        public BG_Type GetBGType() { return bgType; }

        public void SetROIStart(double newStart) { roiStart = newStart; }
        public void SetROIEnd(double newEnd) { roiEnd = newEnd; }
        public void SetBG1Start(double newStart) { bg1Start = newStart; }
        public void SetBG1End(double newEnd) { bg1End = newEnd; }
        public void SetBG2Start(double newStart) { bg2Start = newStart; }
        public void SetBG2End(double newEnd) { bg2End = newEnd; }
        public void SetBGType(BG_Type newType) { bgType = newType; }
    }
}
