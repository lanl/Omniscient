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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class Spectrum
    {
        DateTime startTime;
        double calibrationZero;
        double calibrationSlope;
        private double realTime;
        private double liveTime;
        int[] counts;
        
        public static Spectrum Sum(List<Spectrum> spectra)
        {
            DateTime startTime = DateTime.Now;
            Spectrum spectrum;
            double CalibrationZero, CalibrationSlope;

            if (spectra.Count == 0)
            {
                spectrum = new Spectrum(0, 1, new int[1024]);
            }
            else
            {
                CalibrationZero = spectra[0].GetCalibrationZero();
                CalibrationSlope = spectra[0].GetCalibrationSlope();
                int nBins = spectra[0].GetCounts().Length;
                startTime = spectra[0].GetStartTime();

                foreach (Spectrum subspec in spectra)
                {
                    if (subspec.GetCalibrationSlope() != CalibrationSlope &&
                        subspec.GetCalibrationZero() != CalibrationZero)
                    {
                        CalibrationSlope = 1.0;
                        CalibrationZero = 0.0;
                    }
                    if (subspec.GetCounts().Length > nBins) nBins = subspec.GetCounts().Length;
                    if (subspec.GetStartTime() < startTime) startTime = subspec.GetStartTime();
                }

                spectrum = new Spectrum(CalibrationZero, CalibrationSlope, new int[nBins],
                startTime, 0, 0);

                foreach (Spectrum subspec in spectra)
                {
                    spectrum.Add(subspec);
                }
            }
            return spectrum;
        }

        public Spectrum()
        {
            calibrationZero = 0.0;
            calibrationSlope = 1.0;
            realTime = 1.0;
            liveTime = 1.0;
            counts = new int[0];
        }

        public Spectrum(double zero, double slope, int[] newCounts)
        {
            calibrationZero = zero;
            calibrationSlope = slope;
            counts = newCounts;
        }

        public Spectrum(double zero, double slope, int[] newCounts,
            DateTime newStartTime, double newRealTime, double newLiveTime)
        {
            calibrationZero = zero;
            calibrationSlope = slope;
            counts = newCounts;
            startTime = newStartTime;
            realTime = newRealTime;
            liveTime = newLiveTime;
        }

        public double[] GetBins()
        {
            double[] bins = new double[counts.Length];
            for (int i = 0; i < bins.Length; i++)
                bins[i] = calibrationZero + calibrationSlope*i;
            return bins;
        }

        public ReturnCode Add(Spectrum spectrum)
        {
            if (spectrum.GetNChannels() != counts.Length) return ReturnCode.FAIL;
            int[] otherCounts = spectrum.GetCounts();
            for(int i=0; i<counts.Length; i++)
            {
                counts[i] += otherCounts[i];
            }
            realTime += spectrum.GetRealTime();
            liveTime += spectrum.GetLiveTime();
            return ReturnCode.SUCCESS;
        }

        public int GetNChannels() { return counts.Length; }

        public double GetCalibrationZero() { return calibrationZero; }
        public double GetCalibrationSlope() { return calibrationSlope; }
        public double GetRealTime() { return realTime; }
        public double GetLiveTime() { return liveTime; }
        public int[] GetCounts() { return counts; }
        public DateTime GetStartTime() { return startTime; }

        public void SetCalibrationZero(double zero) { calibrationZero = zero; }
        public void SetCalibrationSlope(double slope) { calibrationSlope = slope; }
        public void SetRealTime(double newTime) { realTime = newTime; }
        public void SetLiveTime(double newTime) { liveTime = newTime; }
        public void SetCounts(int[] newCounts) { counts = newCounts; }
        public void SetStartTime(DateTime dateTime) { startTime = dateTime; }
    }
}
