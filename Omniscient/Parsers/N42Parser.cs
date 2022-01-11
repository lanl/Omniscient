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
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Omniscient
{
    public class N42Parser : SpectrumParser
    {
        const string PARSER_TYPE = "N42";

        private int[] counts;
        private double realTime;
        private double liveTime;
        private DateTime startDateTime;

        private float zero;
        private float keVPerChannel;

        public N42Parser() : base(PARSER_TYPE)
        {
            zero = 0;
            keVPerChannel = 1;

            realTime = 0.0;
            liveTime = 0.0;
            startDateTime = DateTime.Parse("14 April 1943");
            counts = new int[0];
        }

        public override Spectrum GetSpectrum() 
        {
            Spectrum spec = new Spectrum(zero, keVPerChannel, counts);
            spec.SetRealTime(realTime);
            spec.SetLiveTime(liveTime);
            spec.SetStartTime(startDateTime);
            return spec;
        }

        /// <summary>
        /// Parses the Spectrum node in an N42 file
        /// </summary>
        /// <param name="specNode"></param>
        /// <returns></returns>
        private ReturnCode ParseSpectrumNode(XmlNode specNode)
        {
            foreach (XmlNode childNode in specNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "StartTime":
                        try
                        {
                            string tString = childNode.InnerText;
                            startDateTime = DateTimeOffset.Parse(tString, null).DateTime;
                        }
                        catch
                        {
                            return ReturnCode.CORRUPTED_FILE;
                        }
                        break;
                    case "RealTime":
                        try
                        {
                            string tString = childNode.InnerText;
                            string[] badChars = new string[] { " ", "P", "T", "S"};
                            foreach(string c in badChars)
                            {
                                tString = tString.Replace(c, "");
                            }
                            realTime = double.Parse(tString);
                        }
                        catch
                        {
                            return ReturnCode.CORRUPTED_FILE;
                        }
                        break;
                    case "LiveTime":
                        try
                        {
                            string tString = childNode.InnerText;
                            string[] badChars = new string[] { " ", "P", "T", "S" };
                            foreach (string c in badChars)
                            {
                                tString = tString.Replace(c, "");
                            }
                            liveTime = double.Parse(tString);
                        }
                        catch
                        {
                            return ReturnCode.CORRUPTED_FILE;
                        }
                        break;
                    case "ChannelData":
                        try
                        {
                            string channelStr = childNode.InnerText;
                            string[] countStr = channelStr.Trim().Split(new char[] { ' ' });
                            counts = new int[countStr.Length];
                            for (int i = 0; i < countStr.Length; ++i)
                            {
                                counts[i] = int.Parse(countStr[i]);
                            }
                        }
                        catch
                        {
                            return ReturnCode.CORRUPTED_FILE;
                        }
                        break;
                }
            }
            return ReturnCode.SUCCESS;
        }

        private ReturnCode ParseMeasurementNode(XmlNode measNode)
        {
            foreach (XmlNode childNode in measNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "Spectrum":
                        ReturnCode returnCode = ParseSpectrumNode(childNode);
                        if (returnCode != ReturnCode.SUCCESS) return returnCode;
                        break;
                }
            }
            return ReturnCode.SUCCESS;
        }

        public override ReturnCode ParseSpectrumFile(string newFileName)
        {
            XmlDocument doc = new XmlDocument();
            doc.XmlResolver = null;
            doc.Load(newFileName);

            XmlNode N42Node = doc["N42InstrumentData"];
            foreach (XmlNode childNode in N42Node.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "Measurement":
                        ReturnCode returnCode = ParseMeasurementNode(childNode);
                        if (returnCode != ReturnCode.SUCCESS) return returnCode;
                        break;
                }
            }
            return ReturnCode.SUCCESS;
        }
    }
}
