// This software is open source software available under the BSD-3 license.
// 
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
    public class SPEParser : SpectrumParser
    {
        const string PARSER_TYPE = "SPE";

        private double realTime;
        private double liveTime;
        private DateTime startDateTime;

        private int[] counts;

        private float zero;
        private float keVPerChannel;

        public SPEParser() : base(PARSER_TYPE)
        {
            realTime = 0.0;
            liveTime = 0.0;
            startDateTime = DateTime.Parse("14 April 1943");
            counts = new int[0];
            zero = 0;
            keVPerChannel = 0;
        }

        public override Spectrum GetSpectrum()
        {
            Spectrum spec = new Spectrum(zero, keVPerChannel, counts);
            spec.SetRealTime(realTime);
            spec.SetLiveTime(liveTime);
            spec.SetStartTime(startDateTime);
            return spec;
        }

        public override ReturnCode ParseSpectrumFile(string newFileName)
        {
            string[] lines;
            try
            {
                lines = IOUtility.PermissiveReadAllLines(newFileName);
            }
            catch
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            // Grab the counts
            int dataLine = -1;
            for (int l = 0; l < lines.Length; l++)
            {
                if (lines[l].ToUpper().StartsWith("$DATA"))
                {
                    dataLine = l;
                    break;
                }
            }
            if (dataLine < 0) return ReturnCode.CORRUPTED_FILE;
            try
            {
                string[] tokens = lines[dataLine + 1].Split(' ');
                int first_chan = int.Parse(tokens[0]);
                int last_chan = int.Parse(tokens[1]);
                int nChannels = last_chan - first_chan + 1;
                counts = new int[nChannels];
                for (int chan = 0; chan < nChannels; chan++)
                {
                    counts[chan] = int.Parse(lines[dataLine + chan + 2]);
                }
            }
            catch
            {
                return ReturnCode.CORRUPTED_FILE;
            }

            // Grab measurement time
            int measTimLine = -1;
            for (int l = 0; l < lines.Length; l++)
            {
                if (lines[l].ToUpper().StartsWith("$MEAS_TIM"))
                {
                    measTimLine = l;
                    break;
                }
            }
            if (measTimLine > 0)
            {
                try
                {
                    string[] tokens = lines[measTimLine + 1].Split(' ');
                    liveTime = double.Parse(tokens[0]);
                    realTime = double.Parse(tokens[1]);
                }
                catch
                {
                    return ReturnCode.CORRUPTED_FILE;
                }
            }

            // Grab start DateTime
            int dateMeaLine = -1;
            for (int l = 0; l < lines.Length; l++)
            {
                if (lines[l].ToUpper().StartsWith("$DATE_MEA"))
                {
                    dateMeaLine = l;
                    break;
                }
            }
            if (dateMeaLine > 0)
            {
                try
                {
                    string[] tokens = lines[dateMeaLine + 1].Replace('-',' ').Replace(':', ' ').Split(' ');
                    int month = int.Parse(tokens[0]);
                    int day = int.Parse(tokens[1]);
                    int year = int.Parse(tokens[2]);
                    int hour = int.Parse(tokens[3]);
                    int min = int.Parse(tokens[4]);
                    int sec = int.Parse(tokens[5]);
                    startDateTime = new DateTime(year, month, day, hour, min, sec);
                }
                catch
                {
                    return ReturnCode.CORRUPTED_FILE;
                }
            }

            // Grab calibration
            int enerFitLine = -1;
            for (int l = 0; l < lines.Length; l++)
            {
                if (lines[l].ToUpper().StartsWith("$ENER_FIT"))
                {
                    enerFitLine = l;
                    break;
                }
            }
            if (enerFitLine > 0)
            {
                try
                {
                    string[] tokens = lines[enerFitLine + 1].Split(' ');
                    zero = float.Parse(tokens[0]);
                    keVPerChannel = float.Parse(tokens[1]);
                }
                catch
                {
                    return ReturnCode.CORRUPTED_FILE;
                }
            }
            return ReturnCode.SUCCESS;
        }
    }
}
