/*
This source code is Free Open Source Software. It is provided
with NO WARRANTY expressed or implied to the extent permitted by law.

This source code is distributed under the New BSD license:

================================================================================

   Copyright (c) 2020, International Atomic Energy Agency (IAEA), IAEA.org

   All rights reserved.

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice,
      this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice,
      this list of conditions and the following disclaimer in the documentation
      and/or other materials provided with the distribution.
    * Neither the name of IAEA nor the names of its contributors
      may be used to endorse or promote products derived from this software
      without specific prior written permission.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
   "AS IS"AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
   A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
   LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
   NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Omniscient
{
    class FileInstrument : Instrument
    {
        public string DatePattern { get; set; }
        public string DateRegexPattern { get; set; }

        public FileInstrument(DetectionSystem parent, string name, uint id) : base(parent, name, id)
        {


            InstrumentType = "File";
            FileExtension = "txt";
            filePrefix = "";
            fileSuffix = "";
            numChannels = 2;
            channels = new Channel[numChannels];
            channels[0] = new Channel(Name + "-File", this, Channel.ChannelType.COUNT_RATE, 0);
            channels[1] = new Channel(Name + "-Modified", this, Channel.ChannelType.COUNT_RATE, 0);
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
                    case "Extension":
                        FileExtension = param.Value;
                        break;
                }
            }
            ApplyStandardInstrumentParameters(this, parameters);
        }

        public override DateTime GetFileDate(string file)
        {
            string fileAbrev = file.Substring(file.LastIndexOf('\\') + 1);
            string fileStrippedName = fileAbrev.Substring(filePrefix.Length,
                fileAbrev.Length - (filePrefix.Length + fileSuffix.Length + FileExtension.Length + 1));

            Regex regex = new Regex(DateRegexPattern);
            Match match = regex.Match(fileStrippedName);
            if (match.Success)
            {
                return DateTime.ParseExact(match.Value, DatePattern, CultureInfo.InvariantCulture);
            }
            throw new FormatException("File does not contain a valid date");
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = GetStandardInstrumentParameters();
            parameters.Add(new StringParameter("Extension")
            {
                Value = FileExtension
            });
            parameters.Add(new DateTimeFormatParameter("Date Pattern")
            {
                Value = DatePattern
            });
            return parameters;
        }

        public override ReturnCode IngestFile(ChannelCompartment compartment, string fileName)
        {
            DateTime dateTime;
            DateTime modified;
            try
            { 
                dateTime = GetFileDate(fileName);
                modified = File.GetLastWriteTime(fileName);
            }
            catch
            {
                return ReturnCode.FAIL;
            }
            DataFile dataFile = new DataFile(fileName, dateTime);

            channels[0].AddDataPoint(compartment, dateTime, 1, dataFile);
            channels[1].AddDataPoint(compartment, modified, 1, dataFile);

            return ReturnCode.SUCCESS;
        }

        public override ReturnCode AutoIngestFile(ChannelCompartment compartment, string fileName)
        {
            return ReturnCode.FAIL;
        }
    }

    public class FileInstrumentHookup : InstrumentHookup
    {
        public FileInstrumentHookup()
        {
            TemplateParameters.Add(new ParameterTemplate("Extension", ParameterType.String));
            TemplateParameters.Add(new ParameterTemplate("Date Pattern", ParameterType.DateTimeFormat));
        }

        public override string Type { get { return "File"; } }

        public override Instrument FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            FileInstrument instrument = new FileInstrument(parent, newName, id);
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Date Pattern":
                        instrument.DatePattern = param.Value;
                        instrument.DateRegexPattern = (param as DateTimeFormatParameter).GetRegexPattern();
                        break;
                    case "Extension":
                        instrument.FileExtension = param.Value;
                        break;
                }
            }
            Instrument.ApplyStandardInstrumentParameters(instrument, parameters);
            return instrument;
        }
    }
}
