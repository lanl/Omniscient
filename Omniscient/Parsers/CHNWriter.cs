// This software is open source software available under the BSD-3 license.
// 
// Copyright (c) 2020, International Atomic Energy Agency (IAEA), IAEA.org
// Copyright (c) 2018, Triad National Security, LLC
// All rights reserved.
// 
// Copyright 2018. Triad National Security, LLC. This software was produced under U.S. Government contract 89233218CNA000001 for Los Alamos National Laboratory (LANL), which is operated by Triad National Security, LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  NEITHER THE GOVERNMENT NOR TRIAD NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.  If software is modified to produce derivative works, such modified software should be clearly marked, so as not to confuse it with the version available from LANL.
// 
// Additionally, redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 1.       Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
// 2.       Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 
// 3.       Neither the name of Triad National Security, LLC, Los Alamos National Laboratory, LANL, the U.S. Government, nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 
//  
// THIS SOFTWARE IS PROVIDED BY TRIAD NATIONAL SECURITY, LLC AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL TRIAD NATIONAL SECURITY, LLC OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Omniscient
{
    public class CHNWriter : SpectrumWriter
    {
        const string WRITER_TYPE = "CHN";

        private const short FILE_HEADER_CHECK = -1;
        private const short MCA_NUMBER = 777;
        private const short SEG_NUMBER = 1;
        private static readonly DateTime YEAR_2000 = new DateTime(2000, 1, 1);
        private const short CHANNEL_OFFSET = 0;
        private const short FILE_FOOTER_CHECK = -102;
        private const short RESERVED = 0;
        private const string DETECTOR = "Omniscient";
        private const string SAMPLE = "Sample";

        public CHNWriter() : base(WRITER_TYPE)
        {
        }

        public override ReturnCode WriteSpectrumFile(string fileName)
        {
            // Open file
            FileStream writeStream;
            try { writeStream = new FileStream(fileName, FileMode.Create); }
            catch { return ReturnCode.COULD_NOT_OPEN_FILE; }
            BinaryWriter binaryWriter = new BinaryWriter(writeStream);

            // Write Header
            binaryWriter.Write(FILE_HEADER_CHECK);
            binaryWriter.Write(MCA_NUMBER);
            binaryWriter.Write(SEG_NUMBER);
            binaryWriter.Write(spectrum.GetStartTime().Second.ToString("00").ToCharArray());
            binaryWriter.Write((int)(spectrum.GetRealTime() * 50));     // units of 20 ms
            binaryWriter.Write((int)(spectrum.GetLiveTime() * 50));     // units of 20 ms
            string startDate = spectrum.GetStartTime().ToString("ddMMMyy");
            if (spectrum.GetStartTime() > YEAR_2000)
                startDate += "1";
            else
                startDate += "*";
            binaryWriter.Write(startDate.ToCharArray());
            binaryWriter.Write(spectrum.GetStartTime().ToString("HHmm").ToCharArray());
            binaryWriter.Write(CHANNEL_OFFSET);
            binaryWriter.Write((short)spectrum.GetNChannels());

            // Write Spectrum Data
            int[] counts = spectrum.GetCounts();
            for (int bin = 0; bin < spectrum.GetNChannels(); bin++)
                binaryWriter.Write(counts[bin]);

            // Write Footer
            binaryWriter.Write(FILE_FOOTER_CHECK);
            binaryWriter.Write(RESERVED);
            binaryWriter.Write((float)spectrum.GetCalibrationZero());
            binaryWriter.Write((float)spectrum.GetCalibrationSlope());
            binaryWriter.Write((float)0);       // Quadratic calibration term
            binaryWriter.Write((float)0);       // Peak shape 0
            binaryWriter.Write((float)0);       // Peak shape slope
            binaryWriter.Write((float)0);       // Peak shape quadratic term
            for (int i = 0; i < 114; i++)
                binaryWriter.Write(RESERVED);
            binaryWriter.Write((byte)DETECTOR.Length);
            binaryWriter.Write(DETECTOR.ToString());
            for (int i = 0; i < 63 - DETECTOR.Length; i++)
                binaryWriter.Write((byte)0);
            binaryWriter.Write((byte)SAMPLE.Length);
            binaryWriter.Write(SAMPLE.ToString());
            for (int i = 0; i < 63 - SAMPLE.Length; i++)
                binaryWriter.Write((byte)0);
            for (int i = 0; i < 64; i++)
                binaryWriter.Write(RESERVED);

            binaryWriter.Close();

            return ReturnCode.SUCCESS;
        }
    }
}
