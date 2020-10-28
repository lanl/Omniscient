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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    class SMMSInstrument : Instrument
    {
        public static List<string> ValidExtensions = new List<string>() { "csv", "cs2" };
        private const int CHANNELS_PER_INSTRUMENT = 6;

        public int NumberOfInstruments { get; private set; }

        public override string FileExtension
        {
            get { return _fileExtension; }
            set
            {
                if (value == "csv" || value == "cs2")
                {
                    _fileExtension = value;
                }
                else
                    throw new ArgumentException("File extension must be csv or csv2!");
            }
        }

        SMMSParser parser;
        public SMMSInstrument(DetectionSystem parent, string newName, string extension, int nInstruments, uint id) : base(parent, newName, id)
        {
            InstrumentType = "SMMS";
            NumberOfInstruments = nInstruments;
            FileExtension = extension;
            SetNumberOfInstruments(nInstruments);
        }

        private void MakeNewParser()
        {
            parser = new SMMSParser();
            parser.NumberOfInstruments = NumberOfInstruments;
        }

        public override ReturnCode IngestFile(ChannelCompartment compartment, string fileName)
        {
            DateTime time = DateTime.MinValue;
            DataFile dataFile = new DataFile(fileName);
            ReturnCode returnCode = parser.ParseFile(fileName);
            if (returnCode != ReturnCode.SUCCESS) return returnCode;
            if (parser.TimeStamps.Length > 0) dataFile.DataStart = parser.TimeStamps[0];
            else dataFile.DataStart = DateTime.MinValue;

            int numRecords = parser.TimeStamps.Length;
            DataFile[] dataFiles = new DataFile[numRecords];
            for (int r = 0; r < numRecords; ++r) dataFiles[r] = dataFile;
            DateTime[] times = parser.TimeStamps;
            double[][] data = new double[numChannels][];
            for (int c = 0; c < numChannels; c++) data[c] = new double[numRecords];

            for (int r = 0; r < numRecords; ++r)
            {
                time = times[r];
                for (int c = 0; c < numChannels; c++)
                {
                    data[c][r] = parser.Data[r, c];
                }
            }
            for (int c = 0; c < numChannels; c++)
            {
                channels[c].AddDataPoints(compartment, times, data[c], dataFiles);
            }
            dataFile.DataEnd = time;
            MakeNewParser();
            return ReturnCode.SUCCESS;
        }

        public override ReturnCode AutoIngestFile(ChannelCompartment compartment, string fileName)
        {
            foreach (string extension in ValidExtensions)
            {
                FileExtension = extension;
                for (int i = 1; i < 4; i++)
                {
                    SetNumberOfInstruments(i);
                    MakeNewParser();
                    if (parser.ParseFile(fileName) == ReturnCode.SUCCESS)
                    {
                        TryNamingChannelsFromHeaders(fileName);
                        return IngestFile(compartment, fileName);
                    }
                }
            }
            return ReturnCode.FAIL;
        }

        public override DateTime GetFileDate(string file)
        {
            if (parser.ParseFile(file) == ReturnCode.SUCCESS)
            {
                return parser.TimeStamps[0];
            }
            return DateTime.MinValue;
        }

        public ReturnCode SetNumberOfInstruments(int nInstruments)
        {
            if (nInstruments < 1) return ReturnCode.BAD_INPUT;
            NumberOfInstruments = nInstruments;
            numChannels = CHANNELS_PER_INSTRUMENT * nInstruments;
            channels = new Channel[numChannels];
            for (int i = 0; i < numChannels; ++i)
            {
                channels[i] = new Channel(Name + "-" + (i+1).ToString(), this, Channel.ChannelType.COUNT_RATE, 0);
            
            }
            MakeNewParser();
            return ReturnCode.SUCCESS;
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = GetStandardInstrumentParameters();
            parameters.Add(new EnumParameter("Extension")
            {
                Value = FileExtension,
                ValidValues = ValidExtensions
            }); ;
            parameters.Add(new IntParameter("Instruments") { Value = NumberOfInstruments.ToString() });
            return parameters;
        }


        /// <summary>
        /// Rename channels which have default names usingd file headers
        /// </summary>
        private void TryNamingChannelsFromHeaders()
        {
            List<string> directories = new List<string>();
            directories.Add(dataFolder);
            if (IncludeSubDirectories)
            {
                directories.AddRange(GetSubdirectories(dataFolder));
            }
            string fileName = "";
            string[] filesInDirectory;
            foreach (string directory in directories)
            {
                filesInDirectory = Directory.GetFiles(directory);

                foreach (string file in filesInDirectory)
                {
                    string fileAbrev = file.Substring(file.LastIndexOf('\\') + 1);
                    if (fileAbrev.Length > (fileSuffix.Length + FileExtension.Length)
                        && fileAbrev.Substring(fileAbrev.Length - (FileExtension.Length + 1)).ToLower() == ("." + FileExtension)
                        && fileAbrev.ToLower().StartsWith(filePrefix.ToLower())
                        && fileAbrev.Substring(fileAbrev.Length - (FileExtension.Length + 1 + fileSuffix.Length), fileSuffix.Length).ToLower() == fileSuffix.ToLower())
                    {
                        fileName = file;
                        break;
                    }
                }
                if (fileName != "") break;
            }
            TryNamingChannelsFromHeaders(fileName);
        }

        private void TryNamingChannelsFromHeaders(string fileName)
        {
            ReturnCode returnCode = parser.ParseFile(fileName);
            if (returnCode != ReturnCode.SUCCESS) return;
            for (int c = 0; c < numChannels; c++)
            {
                if (channels[c].Name == Name + "-" + (c + 1).ToString())
                {
                    channels[c].Name = parser.Headers[c + 2];
                }
            }
        }

        public override void ApplyParameters(List<Parameter> parameters)
        {
            ApplyStandardInstrumentParameters(this, parameters);
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Extension":
                        FileExtension = param.Value;
                        break;
                    case "Instruments":
                        SetNumberOfInstruments(((IntParameter)param).ToInt());
                        break;
                }
            }
            TryNamingChannelsFromHeaders();
        }
    }

    public class SMMSInstrumentHookup : InstrumentHookup
    {
        public SMMSInstrumentHookup()
        {
            TemplateParameters.AddRange(new List<ParameterTemplate>() {
                new ParameterTemplate("Extension", ParameterType.Enum)
                {
                    ValidValues = {"csv", "cs2"}
                },
                new ParameterTemplate("Instruments", ParameterType.Int)
                });
        }

        public override string Type { get { return "SMMS"; } }

        public override Instrument FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            string extension = "csv";
            int nInstruments = 1;
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Extension":
                        extension = ((EnumParameter)param).Value;
                        break;
                    case "Instruments":
                        nInstruments = ((IntParameter)param).ToInt();
                        if (nInstruments < 1) nInstruments = 1;
                        break;
                }
            }
            SMMSInstrument instrument = new SMMSInstrument(parent, newName, extension, nInstruments, id);
            Instrument.ApplyStandardInstrumentParameters(instrument, parameters);
            return instrument;
        }
    }
}
