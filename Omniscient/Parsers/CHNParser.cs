// This software is open source software available under the BSD-3 license.
// 
// Copyright (c) 2018, Los Alamos National Security, LLC
// All rights reserved.
// 
// Copyright 2018. Los Alamos National Security, LLC. This software was produced under U.S. Government contract DE-AC52-06NA25396 for Los Alamos National Laboratory (LANL), which is operated by Los Alamos National Security, LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  NEITHER THE GOVERNMENT NOR LOS ALAMOS NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.  If software is modified to produce derivative works, such modified software should be clearly marked, so as not to confuse it with the version available from LANL.
// 
// Additionally, redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 1.       Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
// 2.       Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 
// 3.       Neither the name of Los Alamos National Security, LLC, Los Alamos National Laboratory, LANL, the U.S. Government, nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 
//  
// THIS SOFTWARE IS PROVIDED BY LOS ALAMOS NATIONAL SECURITY, LLC AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL LOS ALAMOS NATIONAL SECURITY, LLC OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Omniscient
{
    public class CHNParser : SpectrumParser
    {
        const string PARSER_TYPE = "CHN";

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

        public CHNParser() : base(PARSER_TYPE)
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

        public override ReturnCode ParseSpectrumFile(string newFileName)
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

        public override Spectrum GetSpectrum()
        {
            Spectrum spec = new Spectrum(zero, keVPerChannel, counts);
            spec.SetRealTime(realTime);
            spec.SetLiveTime(liveTime);
            spec.SetStartTime(startDateTime);
            return spec;
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
