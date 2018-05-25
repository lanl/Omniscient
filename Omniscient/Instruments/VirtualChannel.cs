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
    /// <summary>
    /// Creates a channel using data from other channels.</summary>
    /// <remarks>Requires channels that have the same number of data points, 
    /// corresponding to the same timestamps.</remarks>
    public class VirtualChannel : Channel
    {
        public enum VirtualChannelType { RATIO, SUM, DIFFERENCE, ADD_CONST, SCALE, DELAY, ROI, CONVOLVE}

        protected VirtualChannelType virtualType;

        Channel chanA;
        Channel chanB;
        double constant;
        TimeSpan delay;
        string dataFileName;

        public VirtualChannel(string newName, Instrument parent, ChannelType newType) : base(newName, parent, newType)
        {
        }

        public virtual void CalculateValues()
        {
            double[] arrayVals = new double[chanA.GetValues().Count];

            if (channelType == ChannelType.DURATION_VALUE)
                durations = chanA.GetDurations();

            List<double> A;
            List<double> B;
            List<DateTime> ATime;

            switch (virtualType)
            {
                case VirtualChannelType.RATIO:
                    timeStamps = chanA.GetTimeStamps();
                    A = chanA.GetValues();
                    B = chanB.GetValues();
                    for (int i = 0; i < A.Count; i++)
                        arrayVals[i] = A[i] / B[i];
                    break;
                case VirtualChannelType.SUM:
                    timeStamps = chanA.GetTimeStamps();
                    A = chanA.GetValues();
                    B = chanB.GetValues();
                    for (int i = 0; i < A.Count; i++)
                        arrayVals[i] = A[i] + B[i];
                    break;
                case VirtualChannelType.DIFFERENCE:
                    timeStamps = chanA.GetTimeStamps();
                    A = chanA.GetValues();
                    B = chanB.GetValues();
                    for (int i = 0; i < A.Count; i++)
                        arrayVals[i] = A[i] - B[i];
                    break;
                case VirtualChannelType.ADD_CONST:
                    timeStamps = chanA.GetTimeStamps();
                    A = chanA.GetValues();
                    for (int i = 0; i < A.Count; i++)
                        arrayVals[i] = A[i] + constant;
                    break;
                case VirtualChannelType.SCALE:
                    timeStamps = chanA.GetTimeStamps();
                    A = chanA.GetValues();
                    for (int i = 0; i < A.Count; i++)
                        arrayVals[i] = constant*A[i];
                    break;
                case VirtualChannelType.DELAY:
                    A = chanA.GetValues();
                    arrayVals = A.ToArray();
                    ATime = chanA.GetTimeStamps();
                    DateTime[] arrayTimeStamps = new DateTime[ATime.Count];
                    for (int i = 0; i < A.Count; i++)
                        arrayTimeStamps[i] = ATime[i].AddSeconds(delay.TotalSeconds);
                    timeStamps = arrayTimeStamps.ToList();
                    break;
                case VirtualChannelType.CONVOLVE:
                    timeStamps = chanA.GetTimeStamps();
                    A = chanA.GetValues();
                    arrayVals = SignalProcessor.Convolve(A.ToArray(), SignalProcessor.FromFile(dataFileName));
                    break;
            }
            values = arrayVals.ToList();
        }

        public void SetChannelA(Channel newChan)
        {
            chanA = newChan;
            channelType = chanA.GetChannelType();
        }

        public void SetVirtualChannelType(VirtualChannelType newType) { virtualType = newType; }
        public void SetChannelB(Channel newChan) { chanB = newChan; }
        public void SetConstant(double newConst) { constant = newConst; }
        public void SetDelay(TimeSpan newDelay) { delay = newDelay; }
        public void SetDataFileName(string newDataFileName) { dataFileName = newDataFileName; }

        public VirtualChannelType GetVirtualChannelType() { return virtualType; }
        public Channel GetChannelA() { return chanA; }
        public Channel GetChannelB() { return chanB; }
        public double GetConstant() { return constant; }
        public TimeSpan GetDelay() { return delay; }
        public string GetDataFileName() { return dataFileName; }
    }
}
