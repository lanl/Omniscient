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
        /// Returns a Tuple<m, b> for the least squares line y=mx+b for the CurveFitter data
        /// </summary>
        /// <returns></returns>
        public Tuple<double,double> LinearLeastSq()
        {
            // Calculate coefficients
            if (count < 2) throw new Exception("There must be at least two data points to fit!");
            CalculateAverages();
            double Sxy = 0;
            double Sx2 = 0;
            double xMinusX_Bar;
            for (int i=0; i<count; i++)
            {
                xMinusX_Bar = x[i] - X_Bar;
                Sxy += xMinusX_Bar * (y[i] - Y_Bar);
                Sx2 += xMinusX_Bar * xMinusX_Bar;
            }
            if (Sx2 == 0) throw new Exception("The fit is vertical!");
            double m = Sxy / Sx2;
            double b = Y_Bar - m * X_Bar;

            // Calculate fitted values
            f = new double[count];
            for (int i = 0; i < count; i++) f[i] = m * x[i] + b;
            CalculateR_Sq();

            return new Tuple<double, double>(m, b);
        }

    }
}
