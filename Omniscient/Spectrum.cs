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
