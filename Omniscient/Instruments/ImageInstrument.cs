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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Omniscient
{
    public class ImageInstrument : Instrument
    {
        private const string FILE_EXTENSION = "jpg";

        public string DatePattern { get; set; }
        public string DateRegexPattern { get; set; }

        public ImageInstrument(DetectionSystem parent, string name, uint id) : base(parent, name, id)
        {
            InstrumentType = "Image";
            FileExtension = FILE_EXTENSION;
            filePrefix = "";
            fileSuffix = "";
            numChannels = 1;
            channels = new Channel[numChannels];
            channels[0] = new Channel(Name + "-Channel", this, Channel.ChannelType.VIDEO, 0);
        }

        public override void ApplyParameters(List<Parameter> parameters)
        {
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Date Pattern":
                        DatePattern = param.Value;
                        DateRegexPattern = (param as DateTimeFormatParameter).GetRegexPattern();
                        break;
                }
            }
            ApplyStandardInstrumentParameters(this, parameters);
        }

        public override DateTime GetFileDate(string file)
        {
            Regex regex = new Regex(DateRegexPattern);
            Match match = regex.Match(file);
            if (match.Success)
            {
                return DateTime.ParseExact(match.Value, DatePattern, CultureInfo.InvariantCulture);
            }
            throw new FormatException("File does not contain a valid date");
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = GetStandardInstrumentParameters();
            parameters.Add(new DateTimeFormatParameter("Date Pattern")
            {
                Value = DatePattern
            });
            return parameters;
        }

        public override ReturnCode IngestFile(ChannelCompartment compartment, string fileName)
        {
            DateTime dateTime = GetFileDate(fileName);
            DataFile dataFile = new DataFile(fileName, dateTime);

            channels[0].AddDataPoint(compartment, dateTime, 1, dataFile);

            return ReturnCode.SUCCESS;
        }
    }

    public class ImageInstrumentHookup : InstrumentHookup
    {
        public ImageInstrumentHookup()
        {
            TemplateParameters.Add(new ParameterTemplate("Date Pattern", ParameterType.DateTimeFormat));
        }

        public override string Type { get { return "Image"; } }

        public override Instrument FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            ImageInstrument instrument = new ImageInstrument(parent, newName, id);
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Date Pattern":
                        instrument.DatePattern = param.Value;
                        instrument.DateRegexPattern = (param as DateTimeFormatParameter).GetRegexPattern();
                        break;
                }
            }
            Instrument.ApplyStandardInstrumentParameters(instrument, parameters);
            return instrument;
        }
    }
}
