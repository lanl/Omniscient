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

namespace Omniscient
{
    class ROIChannel : VirtualChannel
    {
        ROI roi;

        /// <summary>
        /// If true, values are given in keV. 
        /// If false, values are given as channel number.
        /// </summary>
        public bool InputKeV
        {
            get { return roi.InputKeV; }
            set { roi.InputKeV = value; }
        }

        public double Start
        {
            get { return roi.ROIStart; }
            set { roi.ROIStart = value; }
        }
        public double End
        {
            get { return roi.ROIEnd; }
            set { roi.ROIEnd = value; }
        }
        public double BG1_Start
        {
            get { return roi.BG1Start; }
            set { roi.BG1Start = value; }
        }
        public double BG1_End
        {
            get { return roi.BG1End; }
            set { roi.BG1End = value; }
        }
        public double BG2_Start
        {
            get { return roi.BG2Start; }
            set { roi.BG2Start = value; }
        }
        public double BG2_End
        {
            get { return roi.BG2End; }
            set { roi.BG2End = value; }
        }
        public ROI.BG_Type BGType
        {
            get { return roi.BGType; }
            set { roi.BGType = value; }
        }
        public MCAInstrument ParentMCAInstrument { get; }

        public ROIChannel(string newName, MCAInstrument parent, ChannelType newType, uint id) : base(newName, parent, newType, id)
        {
            VCType = "ROI";
            roi = new ROI();
            ParentMCAInstrument = parent;
        }

        public override void CalculateValues(ChannelCompartment compartment)
        {
            MCAInstrument mca = ParentMCAInstrument;
            List<DataFile> chanFiles = mca.GetChannels()[0].GetFiles(compartment);
            SpectrumParser parser = mca.SpectrumParser;

            // Initialize
            timeStamps[(int)compartment] = new List<DateTime>(chanFiles.Count);
            values[(int)compartment] = new List<double>(chanFiles.Count);
            durations[(int)compartment] = new List<TimeSpan>(chanFiles.Count);
            files[(int)compartment] = new List<DataFile>(chanFiles.Count);

            // Calculate
            ReturnCode returnCode;
            Spectrum spectrum;
            for (int i=0; i<chanFiles.Count; i++)
            {
                returnCode = parser.ParseSpectrumFile(chanFiles[i].FileName);
                spectrum = parser.GetSpectrum();
                timeStamps[(int)compartment].Add(spectrum.GetStartTime());
                durations[(int)compartment].Add(TimeSpan.FromSeconds(spectrum.GetRealTime()));
                values[(int)compartment].Add(roi.GetROICountRate(spectrum));
                files[(int)compartment].Add(chanFiles[i]);
            }
        }

        public override List<Parameter> GetParameters()
        {
            string bg_type = "";
            switch (BGType)
            {
                case ROI.BG_Type.NONE:
                    bg_type = "None";
                    break;
                case ROI.BG_Type.FLAT:
                    bg_type = "Flat";
                    break;
                case ROI.BG_Type.LINEAR:
                    bg_type = "Linear";
                    break;
            }
            return new List<Parameter>()
            {
                new EnumParameter("Input Mode")
                {
                    Value = InputKeV ? "keV" : "channel",
                    ValidValues = new List<string>(){ "keV", "channel"}
                },
                new DoubleParameter("Start (keV)")
                {
                    Value = Start.ToString()
                },
                new DoubleParameter("End (keV)")
                {
                    Value = End.ToString()
                },
                new EnumParameter("BG Type")
                {
                    Value = bg_type,
                    ValidValues = new List<string>(){ "None", "Flat", "Linear"}
                },
                new DoubleParameter("BG1 Start (keV)")
                {
                    Value = BG1_Start.ToString()
                },
                new DoubleParameter("BG1 End (keV)")
                {
                    Value = BG1_End.ToString()
                },
                new DoubleParameter("BG2 Start (keV)")
                {
                    Value = BG2_Start.ToString()
                },
                new DoubleParameter("BG2 End (keV)")
                {
                    Value = BG2_End.ToString()
                }
            };
        }

        public ROI GetROI() { return roi; }

        public void SetROI(ROI newROI) { roi = newROI; }
    }


    public class ROIChannelHookup : VirtualChannelHookup
    {
        public ROIChannelHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
            {
                new ParameterTemplate("Input Mode", ParameterType.Enum, new List<string>(){"keV", "channel" }),
                new ParameterTemplate("Start (keV)", ParameterType.Double),
                new ParameterTemplate("End (keV)", ParameterType.Double),
                new ParameterTemplate("BG Type", ParameterType.Enum, new List<string>(){ "None", "Flat", "Linear" }),
                new ParameterTemplate("BG1 Start (keV)", ParameterType.Double),
                new ParameterTemplate("BG1 End (keV)", ParameterType.Double),
                new ParameterTemplate("BG2 Start (keV)", ParameterType.Double),
                new ParameterTemplate("BG2 End (keV)", ParameterType.Double),
            };
        }

        public override string Type { get { return "ROI"; } }

        public override VirtualChannel FromParameters(Instrument parent, string newName, List<Parameter> parameters, uint id)
        {
            ROIChannel roiChannel = new ROIChannel(newName, (MCAInstrument)parent, Channel.ChannelType.DURATION_VALUE, id);
            roiChannel.InputKeV = true;

            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Input Mode":
                        switch (param.Value)
                        {
                            case "keV":
                                roiChannel.InputKeV = true;
                                break;
                            case "channel":
                                roiChannel.InputKeV = false;
                                break;
                        }
                        break;
                    case "Start (keV)":
                        roiChannel.Start = ((DoubleParameter)param).ToDouble();
                        break;
                    case "End (keV)":
                        roiChannel.End = ((DoubleParameter)param).ToDouble();
                        break;
                    case "BG Type":
                        switch (param.Value)
                        {
                            case "None":
                                roiChannel.BGType = ROI.BG_Type.NONE;
                                break;
                            case "Flat":
                                roiChannel.BGType = ROI.BG_Type.FLAT;
                                break;
                            case "Linear":
                                roiChannel.BGType = ROI.BG_Type.LINEAR;
                                break;
                        }
                        break;
                    case "BG1 Start (keV)":
                        roiChannel.BG1_Start = ((DoubleParameter)param).ToDouble();
                        break;
                    case "BG1 End (keV)":
                        roiChannel.BG1_End = ((DoubleParameter)param).ToDouble();
                        break;
                    case "BG2 Start (keV)":
                        roiChannel.BG2_Start = ((DoubleParameter)param).ToDouble();
                        break;
                    case "BG2 End (keV)":
                        roiChannel.BG2_End = ((DoubleParameter)param).ToDouble();
                        break;
                }
            }
            return roiChannel;
        }
    }
}

