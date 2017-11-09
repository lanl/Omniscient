using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient.Instruments
{
    class ROIChannel : VirtualChannel
    {
        ROI roi;

        public ROIChannel(string newName, MCAInstrument parent, ChannelType newType) : base(newName, parent, newType)
        {
            virtualType = VirtualChannelType.ROI;
            roi = new ROI();
        }

        public void AddDataPoint(DateTime time, Spectrum spectrum, string file)
        {
            timeStamps.Add(time);
            values.Add(roi.GetROICounts(spectrum));
            files.Add(file);
        }

        public override void CalculateValues(){}

        public ROI GetROI() { return roi; }

        public void SetROI(ROI newROI) { roi = newROI; }
    }
}
