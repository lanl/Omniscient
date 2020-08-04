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
    }
}
