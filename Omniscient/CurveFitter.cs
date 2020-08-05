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
    class CurveFitter
    {
        private double[] x;
        private double[] y;
        private double[] f; // Fitted values
        private int count;

        public double X_Bar { get; private set; }
        public double Y_Bar { get; private set; }
        public double R_Sq { get; private set; }

        public CurveFitter(double[] X, double[] Y)
        {
            x = X;
            y = Y;

            count = X.Length;

            if (count != y.Length) throw new ArgumentException("X and Y must be the same length!");
        }

        /// <summary>
        /// Calculates X_Bar and Y_Bar
        /// </summary>
        private void CalculateAverages()
        {
            X_Bar = 0;
            for (int i = 0; i < count; i++) X_Bar += x[i];
            X_Bar /= count;
            Y_Bar = 0;
            for (int i = 0; i < count; i++) Y_Bar += y[i];
            Y_Bar /= count;
        }

        /// <summary>
        /// Calculates R^2 value from y and f
        /// </summary>
        private void CalculateR_Sq()
        {
            double SStot = 0;
            double SSres = 0;
            for (int i=0; i<count; i++)
            {
                SStot += (y[i] - Y_Bar) * (y[i] - Y_Bar);
                SSres += (y[i] - f[i]) * (y[i] - f[i]);
            }
            R_Sq = 1 - SSres / SStot;
        }

        /// <summary>
        /// Returns c for the least squares line y=cx
        /// </summary>
        public double ProportionalLeastSq()
        {
            // Calculate coefficients
            if (count < 2) throw new Exception("There must be at least two data points to fit!");
            CalculateAverages();
            double Sxy = 0;
            double Sx2 = 0;
            for (int i = 0; i < count; i++)
            {
                Sxy += x[i]*y[i];
                Sx2 += x[i]*x[i];
            }
            if (Sx2 == 0) throw new Exception("The fit is vertical!");
            double c = Sxy / Sx2;

            // Calculate fitted values
            f = new double[count];
            for (int i = 0; i < count; i++) f[i] = c * x[i];
            CalculateR_Sq();

            return c;
        }

        /// <summary>
        /// Returns a Tuple<m, b> for the least squares line y=mx+b
        /// Does not perform statistical analysis on results
        /// </summary>
        private Tuple<double, double> GetLinearLeastSqMandB()
        {
            double Sxy = 0;
            double Sx2 = 0;
            double xMinusX_Bar;
            for (int i = 0; i < count; i++)
            {
                xMinusX_Bar = x[i] - X_Bar;
                Sxy += xMinusX_Bar * (y[i] - Y_Bar);
                Sx2 += xMinusX_Bar * xMinusX_Bar;
            }
            if (Sx2 == 0) throw new Exception("The fit is vertical!");
            double m = Sxy / Sx2;
            double b = Y_Bar - m * X_Bar;
            return new Tuple<double, double>(m, b);
        }

        /// <summary>
        /// Returns a Tuple<m, b> for the least squares line y=mx+b
        /// </summary>
        public Tuple<double, double> LinearLeastSq()
        {
            // Calculate coefficients
            if (count < 2) throw new Exception("There must be at least two data points to fit!");
            CalculateAverages();
            Tuple<double, double> coeffs = GetLinearLeastSqMandB();
            double m = coeffs.Item1;
            double b = coeffs.Item2;

            // Calculate fitted values
            f = new double[count];
            for (int i = 0; i < count; i++) f[i] = m * x[i] + b;
            CalculateR_Sq();

            return coeffs;
        }

        /// <summary>
        /// Returns a Tuple<a, b> for the least squares curve y=ax^b
        /// </summary>
        /// <returns></returns>
        public Tuple<double, double> PowerLawLeastSq()
        {
            // Transform to linear form
            double[] xOrig = x;
            double[] yOrig = y;
            double[] xPrime = new double[count];
            double[] yPrime = new double[count];
            for (int i=0; i<count; i++)
            {
                xPrime[i] = Math.Log(xOrig[i]);
                yPrime[i] = Math.Log(yOrig[i]);
            }
            x = xPrime;
            y = yPrime;

            // Calculate coefficients from linear least squares fit
            CalculateAverages();
            Tuple<double, double> coeffs = GetLinearLeastSqMandB();
            double b = coeffs.Item1;
            double a = Math.Exp(coeffs.Item2);
            x = xOrig;
            y = yOrig;
            CalculateAverages();

            // Calculate fitted values
            f = new double[count];
            for (int i = 0; i < count; i++) f[i] = a * Math.Pow(x[i], b);
            CalculateR_Sq();

            return new Tuple<double, double>(a, b);
        }

        /// <summary>
        /// Returns a Tuple<a, b> for the exponential curve y = a exp(bx)
        /// </summary>
        /// <returns></returns>
        public Tuple<double, double> ExponentialLeastSq()
        {
            // Transform to linear form
            double[] yOrig = y;
            double[] yPrime = new double[count];
            for (int i = 0; i < count; i++) yPrime[i] = Math.Log(yOrig[i]);
            y = yPrime;

            // Calculate coefficients from linear least squares fit
            CalculateAverages();
            Tuple<double, double> coeffs = GetLinearLeastSqMandB();
            double b = coeffs.Item1;
            double a = Math.Exp(coeffs.Item2);
            y = yOrig;
            CalculateAverages();

            // Calculate fitted values
            f = new double[count];
            for (int i = 0; i < count; i++) f[i] = a * Math.Exp(b*x[i]);
            CalculateR_Sq();

            return new Tuple<double, double>(a, b);
        }

        /// <summary>
        /// Returns a Tuple<a, b> for the logarithmic curve y = a + b ln(x)
        /// </summary>
        /// <returns></returns>
        public Tuple<double, double> LogLeastSq()
        {
            // Transform to linear form
            double[] xOrig = x;
            double[] xPrime = new double[count];
            for (int i = 0; i < count; i++) xPrime[i] = Math.Log(xOrig[i]);
            x = xPrime;

            // Calculate coefficients from linear least squares fit
            CalculateAverages();
            Tuple<double, double> coeffs = GetLinearLeastSqMandB();
            double b = coeffs.Item1;
            double a = coeffs.Item2;
            x = xOrig;
            CalculateAverages();

            // Calculate fitted values
            f = new double[count];
            for (int i = 0; i < count; i++) f[i] = a + b*Math.Log(x[i]);
            CalculateR_Sq();

            return new Tuple<double, double>(a, b);
        }
    }
}
