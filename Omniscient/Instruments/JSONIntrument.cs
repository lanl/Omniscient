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

namespace Omniscient
{
    public class JSONInstrument : Instrument
    {
        public override string FileExtension
        {
            get { return _fileExtension; }
            set
            {
                _fileExtension = value;
                if (_fileExtension is null) _fileExtension = "json";

                bool hasChar = false;
                foreach (char c in _fileExtension)
                {
                    if (Char.IsLetterOrDigit(c)) hasChar = true;
                }
                if (!hasChar)
                {
                    _fileExtension = "json";
                }

            }

        }
        private string _timeStampFormat;
        public string TimeStampFormat
        {
            get { return _timeStampFormat; }
            set
            {
                _timeStampFormat = value;
                if (_timeStampFormat is null) _timeStampFormat = "yyyy-MM-ddTHH:mm:ss";

                bool hasChar = false;
                foreach (char c in _timeStampFormat)
                {
                    if (Char.IsLetterOrDigit(c)) hasChar = true;
                }
                if (!hasChar)
                {
                    _timeStampFormat = "yyyy-MM-ddTHH:mm:ss";
                }

                if (jsonParser != null) jsonParser.TimeStampFormat = _timeStampFormat;
            }
        }

        JSONParser jsonParser;

        public JSONInstrument(DetectionSystem parent, string newName, int nChannels, uint id) : base(parent, newName, id)
        {
            InstrumentType = "JSON";
            numChannels = nChannels;
            filePrefix = "";
            fileSuffix = "";
            

            TimeStampFormat = "";
            MakeNewParser();

            ReinitializeChannels();
        }

        private void ReinitializeChannels()
        {
            channels = new Channel[numChannels];

            for (int i = 0; i < numChannels; i++)
                channels[i] = new Channel(Name + "-" + i.ToString(), this, Channel.ChannelType.COUNT_RATE, 0);
        }

        private void MakeNewParser()
        {
            jsonParser = new JSONParser();
            jsonParser.TimeStampFormat = TimeStampFormat;
            jsonParser.nChannels = numChannels;
        }

        public ReturnCode SetNumberOfChannels(int nChannels)
        {
            if (nChannels < 1) return ReturnCode.BAD_INPUT;
            Channel[] newChannels = new Channel[nChannels];

            // Put as many of the original channels back as can fit in the new array
            if (numChannels < nChannels)
            {
                for (int i = 0; i < numChannels; ++i)
                {
                    newChannels[i] = channels[i];
                }
                
                for (int i = numChannels; i < nChannels; ++i)
                {
                    newChannels[i] = new Channel(Name + "-" + i.ToString(), this, Channel.ChannelType.COUNT_RATE, 0);
                }
                
            }
            else
            {
                for (int i = 0; i < nChannels; ++i)
                {
                    newChannels[i] = channels[i];
                }
            }
            numChannels = nChannels;
            channels = newChannels;
            return ReturnCode.SUCCESS;
        }

        public override ReturnCode IngestFile(ChannelCompartment compartment, string fileName)
        {
            DateTime time = DateTime.MinValue;
            DataFile dataFile = new DataFile(fileName);
            dataFile.DataStart = GetFileDate(fileName);

            ReturnCode returnCode = jsonParser.ParseFile(fileName);

            int numRecords = jsonParser.nRecords;
            DataFile[] dataFiles = new DataFile[numRecords];

            for (int r = 0; r < numRecords; ++r) dataFiles[r] = dataFile;
            DateTime[] times = jsonParser.TimeStamps;
            double[][] data = new double[numChannels][];
            for (int c = 0; c < numChannels; c++) data[c] = new double[numRecords];

       
            for (int r = 0; r < numRecords; ++r)
            {
                time = times[r];
                for (int c = 0; c < numChannels; c++)
                {
                    data[c][r] = jsonParser.Data[r, c];
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
            if (jsonParser.AutoConfigureFromFile(fileName) == ReturnCode.SUCCESS)
            {
                SetNumberOfChannels(jsonParser.nChannels);
                TimeStampFormat = jsonParser.TimeStampFormat;
                MakeNewParser();
                return IngestFile(compartment, fileName);
            }
            else return ReturnCode.FAIL;
        }

        public override DateTime GetFileDate(string file)
        {
            if (jsonParser.GetFirstDate(file) == ReturnCode.SUCCESS)
            {
                return jsonParser.TimeStamps[0];
            }
            return DateTime.MinValue;
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = GetStandardInstrumentParameters();
 
            parameters.Add(new StringParameter("Extension") { Value = FileExtension });
            parameters.Add(new StringParameter("Time Stamp Format") { Value = TimeStampFormat });
            parameters.Add(new IntParameter("Number of Channels") { Value =  numChannels.ToString() });
            return parameters;
        }

        public override void ApplyParameters(List<Parameter> parameters)
        {
            ApplyStandardInstrumentParameters(this, parameters);
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Extension":
                        FileExtension = ((StringParameter)param).Value;
                        break;
                    case "Time Stamp Format":                                                          
                        TimeStampFormat = ((StringParameter)param).Value;
                        break;
                    case "Number of Channels":
                        SetNumberOfChannels(((IntParameter)param).ToInt());
                        break;
                }
            }
        }
    }

    public class JSONInstrumentHookup : InstrumentHookup
    {
        public JSONInstrumentHookup()
        {
            TemplateParameters.AddRange(new List<ParameterTemplate>() {

                new ParameterTemplate("Extension", ParameterType.String),
                new ParameterTemplate("Time Stamp Format", ParameterType.String),
                new ParameterTemplate("Number of Channels", ParameterType.Int)
                });
        }

        public override string Type { get { return "JSON"; } }

        public override Instrument FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            string tStampFormat = "yyyy-MM-ddTHH:mm:ss";
            string fileExtension = "json";
            int nChannels = 0;

           foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Extension":
                        fileExtension = ((StringParameter)param).Value;
                        break;
                    case "Time Stamp Format":
                        tStampFormat = ((StringParameter)param).Value;
                        break;
                    case "Number of Channels":
                        nChannels = ((IntParameter)param).ToInt(); 
                        break;
                }
            }
          
            JSONInstrument instrument = new JSONInstrument(parent, newName, nChannels, id);
            instrument.TimeStampFormat = tStampFormat;
            instrument.FileExtension = fileExtension;
            Instrument.ApplyStandardInstrumentParameters(instrument, parameters);
            return instrument;
        }
    }
}
