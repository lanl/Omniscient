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