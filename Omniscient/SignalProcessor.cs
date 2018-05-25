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
using System.IO;

namespace Omniscient
{
    internal class SignalProcessor
    {
        public SignalProcessor()
        {
        }

        /// <summary>
        /// Reads a file that contains a single number on each line and 
        /// returns it as an array of doubles.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static double[] FromFile(string fileName)
        {
            string[] lines;
            lines = File.ReadAllLines(fileName);

            List<double> list = new List<double>();
            foreach (string line in lines)
            {
                try
                {
                    list.Add(double.Parse(line));
                }
                catch
                {
                    break;
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// Returns the convolution of signalA and signalB.
        /// </summary>
        /// <param name="signalA"></param>
        /// <param name="signalB"></param>
        /// <returns>An array with length A.Length + B.Length - 1</returns>
        public static double[] Convolve(double[] signalA, double[] signalB)
        {
            double[] A;
            double[] B;
            if (signalA.Length > signalB.Length)
            {
                A = signalA;
                B = signalB;
            }
            else
            {
                B = signalA;
                A = signalB;
            }

            int length = A.Length + B.Length - 1;
            int bLength = B.Length;
            double[] result = new double[length];
            for (int i = 0; i < length; ++i)
            {
                result[i] = 0;
                for (int bi = 0; bi < bLength; ++bi)
                {
                    if (i - bi >= 0 && i - bi < A.Length)
                        result[i] += A[i - bi] * B[bi];
                }
            }

            return result;
        }

        public static double[] ConvolveSameLength(double[] signalA, double[] signalB)
        {
            double[] C = Convolve(signalA, signalB);
            int smallLength;
            int longLength;
            if (signalA.Length < signalB.Length)
            {
                smallLength = signalA.Length;
                longLength = signalB.Length;
            }
            else
            {
                smallLength = signalB.Length;
                longLength = signalA.Length;
            }

            double[] result = new double[longLength];

            int offset = smallLength / 2;
            for (int i = 0; i < longLength; ++i)
            {
                result[i] = C[i + offset];
            }
            return result;
        }
    }
}