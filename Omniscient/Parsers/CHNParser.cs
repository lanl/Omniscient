using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Omniscient.Parsers
{
    class CHNParser
    {
        public enum ReturnCode { SUCCESS, FAIL, COULD_NOT_OPEN_FILE }


        private string fileName;
        private Int16 fileTypeCheck;
        private Int16 MCANumber;
        private Int16 segmentNumber;
        private double realTime;
        private double liveTime;
        private DateTime startDateTime;
        private Int16 channelOffset;
        private Int16 numChannels;

        private int[] counts;

        private Int16 footerType;
        private float zero;
        private float keVPerChannel;
        private float peakZero;
        private float peakSlope;
        private string detectorDesc;
        private string sampleDesc;

        public CHNParser()
        {
            fileName = "";
            fileTypeCheck = -999;
            MCANumber = 0;
            segmentNumber = 0;
            realTime = 0.0;
            liveTime = 0.0;
            startDateTime = DateTime.Parse("14 April 1943");
            channelOffset = 0;
            numChannels = 0;
            counts = new int[0];
            footerType = -99;
            zero = 0;
            keVPerChannel = 0;
            peakZero = 0;
            peakSlope = 0;
            detectorDesc = "";
            sampleDesc = "";
        }

        public ReturnCode ParseFile(string newFileName)
        {
            fileName = newFileName;
            FileStream readStream;

            try
            {
                // Read header
                readStream = new FileStream(fileName, FileMode.Open);
                BinaryReader readBinary = new BinaryReader(readStream);
                fileTypeCheck = readBinary.ReadInt16();
                MCANumber = readBinary.ReadInt16();
                segmentNumber = readBinary.ReadInt16();
                string startSecondsStr = new string (readBinary.ReadChars(2));
                realTime = readBinary.ReadInt32() * 0.02;
                liveTime = readBinary.ReadInt32() * 0.02;
                string dateStr = new string (readBinary.ReadChars(8));
                string timeStr = new string(readBinary.ReadChars(4));
                System.Globalization.CultureInfo cultureInfo = new CultureInfo("en-US");
                if (dateStr[7] == '1')
                {
                    cultureInfo.Calendar.TwoDigitYearMax = 2099;
                }
                else
                {
                    cultureInfo.Calendar.TwoDigitYearMax = 1999;
                }
                startDateTime = DateTime.ParseExact(dateStr.Substring(0, 7), "ddMMMyy", cultureInfo);
                startDateTime = startDateTime.Date.Add(DateTime.ParseExact(timeStr, "HHmm", CultureInfo.InvariantCulture).TimeOfDay);
                startDateTime = startDateTime.AddSeconds(int.Parse(startSecondsStr));
                channelOffset = readBinary.ReadInt16();
                numChannels = readBinary.ReadInt16();

                // Read counts
                counts = new int[numChannels];
                for (int chan=0; chan<numChannels; chan++)
                {
                    counts[chan] = readBinary.ReadInt32();
                }

                // Read footer
                footerType = readBinary.ReadInt16();
                readBinary.ReadInt16();                     // Reserved
                zero = readBinary.ReadSingle();
                keVPerChannel = readBinary.ReadSingle();
                readBinary.ReadSingle();                    // Reserved
                peakZero = readBinary.ReadSingle();
                peakSlope = readBinary.ReadSingle();
                readBinary.ReadBytes(208);                  // Reserved
                byte descLength = readBinary.ReadByte();
                detectorDesc = new string(readBinary.ReadChars(descLength));
                readBinary.ReadBytes(63 - descLength);      // End of description
                descLength = readBinary.ReadByte();
                sampleDesc = new string(readBinary.ReadChars(descLength));

                readStream.Close();
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            return ReturnCode.SUCCESS;
        }

        public double GetRealTime() { return realTime; }
        public double GetLiveTime() { return liveTime; }
        public DateTime GetStartDateTime() { return startDateTime; }
        public int GetNumChannels() { return numChannels; }
        public int[] GetCounts() { return counts; }
        public double GetCalibrationZero() { return zero; }
        public double GetCalibrationSlope() { return keVPerChannel; }
        public double GetShapeZero() { return peakZero; }
        public double GetShapeSlope() { return peakSlope; }
        public string GetDetectorDescription() { return detectorDesc; }
        public string GetSampleDescription() { return sampleDesc; }
    }
}
