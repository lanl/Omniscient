using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient.Instruments
{
    /// <summary>
    /// Creates a channel using data from other channels.</summary>
    /// <remarks>Requires channels that have the same number of data points, 
    /// corresponding to the same timestamps.</remarks>
    public class VirtualChannel : Channel
    {
        public enum VirtualChannelType { RATIO, SUM, DIFFERENCE, ADD_CONST, SCALE, DELAY, ROI}

        protected VirtualChannelType virtualType;

        Channel chanA;
        Channel chanB;
        double constant;
        TimeSpan delay;

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

        public VirtualChannelType GetVirtualChannelType() { return virtualType; }
        public Channel GetChannelA() { return chanA; }
        public Channel GetChannelB() { return chanB; }
        public double GetConstant() { return constant; }
        public TimeSpan GetDelay() { return delay; }
    }
}
