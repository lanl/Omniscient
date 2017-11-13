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
