﻿// This software is open source software available under the BSD-3 license.
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

namespace Omniscient
{
    class MCAInstrument : Instrument
    {
        private const string FILE_EXTENSION = "chn";
        private const int NUM_CHANNELS = 1;
        private const int COUNT_RATE = 0;


        SpectrumParser spectrumParser;

        public override string FileExtension
        {
            get { return _fileExtension; }
            set
            {
                if (value == "chn")
                {
                    spectrumParser = new CHNParser();
                    _fileExtension = value;
                }
                else if (value == "spe")
                {
                    spectrumParser = new SPEParser();
                    _fileExtension = value;
                }
                else
                    throw new ArgumentException("File extension must be chn or spe!");
                ScanDataFolder();
            }
        }

        public MCAInstrument(string newName) : base(newName)
        {
            InstrumentType = "MCA";
            FileExtension = FILE_EXTENSION;
            filePrefix = "";
            spectrumParser = new CHNParser();

            numChannels = NUM_CHANNELS;
            channels = new Channel[numChannels];
            channels[COUNT_RATE] = new Channel(name + "-Count_Rate", this, Channel.ChannelType.DURATION_VALUE);
        }

        public override void ScanDataFolder()
        {
            if (string.IsNullOrEmpty(dataFolder))  return;
            List<string> chnFileList = new List<string>();
            List<DateTime> chnDateList = new List<DateTime>();

            string[] filesInDirectory = Directory.GetFiles(dataFolder);
            foreach (string file in filesInDirectory)
            {
                string fileAbrev = file.Substring(file.LastIndexOf('\\') + 1);
                if (fileAbrev.Substring(fileAbrev.Length - 4).ToLower() == ("." + FileExtension) && fileAbrev.ToLower().StartsWith(filePrefix.ToLower()))
                {
                    if (spectrumParser.ParseSpectrumFile(file) == ReturnCode.SUCCESS)
                    {
                        chnFileList.Add(file);
                        chnDateList.Add(spectrumParser.GetSpectrum().GetStartTime());
                    }
                    else
                    {
                        // Something should really go here...
                    }
                }
            }

            dataFileNames = chnFileList.ToArray();
            dataFileTimes = chnDateList.ToArray();

            Array.Sort(dataFileTimes, dataFileNames);
        }

        public override ReturnCode IngestFile(string fileName)
        {
            ReturnCode returnCode = spectrumParser.ParseSpectrumFile(fileName);
            DataFile dataFile = new DataFile(fileName);
            Spectrum spectrum = spectrumParser.GetSpectrum();
            DateTime time = spectrum.GetStartTime();
            TimeSpan duration = TimeSpan.FromSeconds(spectrum.GetRealTime());

            int counts = 0;
            for (int ch = 0; ch < spectrum.GetNChannels(); ch++)
            {
                counts += spectrum.GetCounts()[ch];
            }
            channels[COUNT_RATE].AddDataPoint(time, counts / spectrum.GetLiveTime(), duration, dataFile);


            foreach (VirtualChannel chan in virtualChannels)
            {
                if (chan is ROIChannel)
                {
                    ((ROIChannel)chan).AddDataPoint(time, spectrum, duration, dataFile);
                }
            }
            return ReturnCode.SUCCESS;
        }

        public override void ClearData()
        {
            foreach (Channel ch in channels)
            {
                ch.ClearData();
            }
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = GetStandardInstrumentParameters();
            parameters.Add(new EnumParameter("File Extension")
            {
                Value = FileExtension,
                ValidValues = { "chn", "spe" }
            });
            return parameters;
        }

        public override void ApplyParameters(List<Parameter> parameters)
        {
            ApplyStandardInstrumentParameters(this, parameters);
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "File Extension":
                        FileExtension = param.Value;
                        break;
                }
            }
        }
    }

    public class MCAInstrumentHookup : InstrumentHookup
    {
        public MCAInstrumentHookup()
        {
            TemplateParameters.Add(new ParameterTemplate("File Extension", ParameterType.Enum)
            {
                ValidValues = { "chn", "spe" }
            });
        }

        public override string Type { get { return "MCA"; } }

        public override Instrument FromParameters(string newName, List<Parameter> parameters)
        {
            MCAInstrument instrument = new MCAInstrument(newName);
            Instrument.ApplyStandardInstrumentParameters(instrument, parameters);
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "File Extension":
                        instrument.FileExtension = param.Value;
                        break;
                }
            }
            return instrument;
        }
    }
}
