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
    class MCAInstrument : Instrument
    {
        private const string FILE_EXTENSION = "chn";
        private const int NUM_CHANNELS = 1;
        private const int COUNT_RATE = 0;

        public static List<string> ValidExtensions = new List<string>() { "chn", "spe", "n42", "hgm" };

        public SpectrumParser SpectrumParser { get; private set; }

        public override string FileExtension
        {
            get { return _fileExtension; }
            set
            {
                if (value == "chn")
                {
                    SpectrumParser = new CHNParser();
                    _fileExtension = value;
                }
                else if (value == "spe")
                {
                    SpectrumParser = new SPEParser();
                    _fileExtension = value;
                }
                else if (value == "n42")
                {
                    SpectrumParser = new N42Parser();
                    _fileExtension = value;
                }
                else if (value == "hgm")
                {
                    SpectrumParser = new HGMParser();
                    _fileExtension = value;
                }
                else
                    throw new ArgumentException("File extension must be chn, spe, n42, or hgm!");
                //ScanDataFolder();
            }
        }

        public MCAInstrument(DetectionSystem parent, string name, uint id) : base(parent, name, id)
        {
            InstrumentType = "MCA";
            FileExtension = FILE_EXTENSION;
            filePrefix = "";
            fileSuffix = "";
            SpectrumParser = new CHNParser();

            numChannels = NUM_CHANNELS;
            channels = new Channel[numChannels];
            channels[COUNT_RATE] = new Channel(Name + "-Count_Rate", this, Channel.ChannelType.DURATION_VALUE, 0);
        }

        public override DateTime GetFileDate(string file)
        {
            if (SpectrumParser.ParseSpectrumFile(file) == ReturnCode.SUCCESS)
            {
                return SpectrumParser.GetSpectrum().GetStartTime();
            }
            return DateTime.MinValue;
        }

        public override ReturnCode IngestFile(ChannelCompartment compartment, string fileName)
        {
            ReturnCode returnCode = SpectrumParser.ParseSpectrumFile(fileName);
            if (returnCode != ReturnCode.SUCCESS) return returnCode;

            List<Spectrum> spectra = SpectrumParser.GetSpectra();
            
            DateTime time = spectra[0].GetStartTime();
            TimeSpan duration = TimeSpan.FromSeconds(spectra.Last().GetRealTime());
            DataFile dataFile = new DataFile(fileName, time, spectra.Last().GetStartTime() + duration);

            foreach (Spectrum spectrum in spectra)
            {
                time = spectrum.GetStartTime();
                duration = TimeSpan.FromSeconds(spectrum.GetRealTime());
                int counts = 0;
                for (int ch = 0; ch < spectrum.GetNChannels(); ch++)
                {
                    counts += spectrum.GetCounts()[ch];
                }
                channels[COUNT_RATE].AddDataPoint(compartment, time, counts / spectrum.GetLiveTime(), duration, dataFile);
            }
            
            return ReturnCode.SUCCESS;
        }

        public override ReturnCode AutoIngestFile(ChannelCompartment compartment, string fileName)
        {
            foreach (string extension in ValidExtensions)
            {
                FileExtension = extension;
                try
                {
                    if (IngestFile(compartment, fileName) == ReturnCode.SUCCESS) return ReturnCode.SUCCESS;
                }
                catch { }
            }

            return ReturnCode.FAIL;
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = GetStandardInstrumentParameters();
            parameters.Add(new EnumParameter("File Extension")
            {
                Value = FileExtension,
                ValidValues = ValidExtensions
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
                ValidValues = { "chn", "spe", "n42", "hgm" }
            });
        }

        public override string Type { get { return "MCA"; } }

        public override Instrument FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            MCAInstrument instrument = new MCAInstrument(parent, newName, id);
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
