/*
This source code is Free Open Source Software. It is provided
with NO WARRANTY expressed or implied to the extent permitted by law.

This source code is distributed under the New BSD license:

================================================================================

   Copyright (c) 2020, International Atomic Energy Agency (IAEA), IAEA.org

   All rights reserved.

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice,
      this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice,
      this list of conditions and the following disclaimer in the documentation
      and/or other materials provided with the distribution.
    * Neither the name of IAEA nor the names of its contributors
      may be used to endorse or promote products derived from this software
      without specific prior written permission.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
   "AS IS"AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
   A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
   LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
   NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    class ChartingUtil
    {
        /// <summary>
        /// Rounds the min and max values to for a pleasant viewing experince
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static Tuple<double, double> AutoRoundRange(double min, double max, bool log)
        {
            double maxOrderOfMagnitude = Math.Pow(10, Math.Floor(Math.Log10(max)));
            double firstDigit = Math.Floor(max / maxOrderOfMagnitude);
            double maxMinRatio = max / min;

            if (log)
            {
                double minOrderOfMagnitude = Math.Pow(10, Math.Floor(Math.Log10(min)));
                return new Tuple<double, double>(minOrderOfMagnitude, maxOrderOfMagnitude * 10);
            }
            else if (maxMinRatio > 2)
            {
                if (firstDigit < 3)
                {
                    double secondDigit = Math.Floor(10*max/maxOrderOfMagnitude) % 10;
                    return new Tuple<double, double>(0, (10*firstDigit + secondDigit + 1) *maxOrderOfMagnitude/10);
                }
                else return new Tuple<double, double>(0, (firstDigit + 1) * maxOrderOfMagnitude);
            }
            else
            {
                double minOrderOfMagnitude = Math.Pow(10, Math.Floor(Math.Log10(min)));
                double maxMinDifference = max - min;
                double diffOoM = Math.Pow(10, Math.Floor(Math.Log10(maxMinDifference)));

                return new Tuple<double, double>(Math.Floor(min / (diffOoM)) * diffOoM, Math.Ceiling(max / (diffOoM)) * diffOoM);
            }
        }

        /// <summary>
        /// Formats a double as a string, nicely
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string FormatDoubleNicely(double d, int nChars=7)
        {
            if (nChars < 3) throw new ArgumentException("nChars must be at least 3!");

            double smallInt = Math.Pow(10, nChars);

            // Try small integer (within reasonable machine error)
            if ((d % 1 < double.Epsilon*1e9 && d % 1 > double.Epsilon*-1e9) && 
                d < smallInt && d > -(smallInt/10))
            {
                return ((int)d).ToString();
            }

            // Try decimal point
            int iters = nChars - 3;
            for (int i=1; i<= iters; i++)
            {
                if (d < Math.Pow(10, i) && d > Math.Pow(10, i - 1))
                {
                    return String.Format("{0:" + new string('0', i) + "." + new string('0', nChars - i - 1) + "}",
                        d);
                }
            }

            // Give up
            return d.ToString("G" + (nChars-3).ToString());
        }
    }
}
