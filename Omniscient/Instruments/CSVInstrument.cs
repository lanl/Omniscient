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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class CSVInstrument : Instrument
    {
        private const string FILE_EXTENSION = "csv";

        private CSVParser.DelimiterType _delimiter;
        public CSVParser.DelimiterType Delimiter
        {
            get { return _delimiter; }
            set
            {
                _delimiter = value;
                if (csvParser != null) csvParser.Delimiter = value;
            }
        }

        private int _numberOfHeaders;
        public int NumberOfHeaders
        {
            get { return _numberOfHeaders; }
            set
            {
                _numberOfHeaders = value;
                if(csvParser != null) csvParser.NumberOfHeaders = value;
            }
        }

        private string _timeStampFormat;
        public string TimeStampFormat
        {
            get { return _timeStampFormat; }
            set
            {
                _timeStampFormat = value;
                if (_timeStampFormat is null) _timeStampFormat = "";

                bool hasChar = false;
                foreach (char c in _timeStampFormat)
                {
                    if (Char.IsLetterOrDigit(c)) hasChar = true;
                }
                if (!hasChar)
                {
                    _timeStampFormat = "";
                }

                if (csvParser != null) csvParser.TimeStampFormat = _timeStampFormat;
            }
        }

        private bool _hasEndTimes;
        public bool HasEndTimes
        {
            get { return _hasEndTimes; }
            set
            {
                if (value != _hasEndTimes)
                {
                    _hasEndTimes = value;
                    ReinitializeChannels();
                }
            }
        }

        CSVParser csvParser;

        public CSVInstrument(DetectionSystem parent, string newName, int nChannels, bool hasEndTimes, uint id) : base(parent, newName, id)
        {
            InstrumentType = "CSV";
            numChannels = nChannels;
            FileExtension = FILE_EXTENSION;
            filePrefix = "";
            fileSuffix = "";

            Delimiter = CSVParser.DelimiterType.Comma;
            NumberOfHeaders = 0;
            TimeStampFormat = "";
            HasEndTimes = hasEndTimes;
            MakeNewParser();

            ReinitializeChannels();
        }

        private void ReinitializeChannels()
        {
            channels = new Channel[numChannels];
            if (HasEndTimes)
            {
                for (int i = 0; i < numChannels; i++)
                    channels[i] = new Channel(Name + "-" + i.ToString(), this, Channel.ChannelType.DURATION_VALUE, 0);
            }
            else
            {
                for (int i = 0; i < numChannels; i++)
                    channels[i] = new Channel(Name + "-" + i.ToString(), this, Channel.ChannelType.COUNT_RATE, 0);
            }
        }

        private void MakeNewParser()
        {
            csvParser = new CSVParser();
            csvParser.Delimiter = Delimiter;
            if (HasEndTimes)
            {
                csvParser.NumberOfColumns = numChannels + 2;
            }
            else
            {
                csvParser.NumberOfColumns = numChannels + 1;
            }
            csvParser.NumberOfHeaders = NumberOfHeaders;
            csvParser.TimeStampFormat = TimeStampFormat;
            csvParser.GetEndTimes = HasEndTimes;
        }

        public ReturnCode SetNumberOfChannels(int nChannels)
        {
            if (nChannels < 1) return ReturnCode.BAD_INPUT;
            Channel[] newChannels = new Channel[nChannels];

            // Put as many of the original channels back as can fit in the new array
            if (numChannels < nChannels)
            {
                for (int i=0; i<numChannels; ++i)
                {
                    newChannels[i] = channels[i];
                }
                if (HasEndTimes)
                {
                    for (int i = numChannels; i < nChannels; ++i)
                    {
                        newChannels[i] = new Channel(Name + "-" + i.ToString(), this, Channel.ChannelType.DURATION_VALUE, 0);
                    }
                }
                else
                {
                    for (int i = numChannels; i < nChannels; ++i)
                    {
                        newChannels[i] = new Channel(Name + "-" + i.ToString(), this, Channel.ChannelType.COUNT_RATE, 0);
                    }
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
            csvParser.NumberOfColumns = numChannels + 1;
            return ReturnCode.SUCCESS;
        }

        public override ReturnCode IngestFile(ChannelCompartment compartment, string fileName)
        {
            DateTime time = DateTime.MinValue;
            DataFile dataFile = new DataFile(fileName);
            dataFile.DataStart = GetFileDate(fileName);

            ReturnCode returnCode = csvParser.ParseFile(fileName);

            int numRecords = csvParser.GetNumRecords();
            DataFile[] dataFiles = new DataFile[numRecords];
            for (int r = 0; r < numRecords; ++r) dataFiles[r] = dataFile;
            DateTime[] times = csvParser.TimeStamps;
            double[][] data = new double[numChannels][];
            for (int c = 0; c < numChannels; c++) data[c] = new double[numRecords];

            if (HasEndTimes)
            {
                TimeSpan[] durations = new TimeSpan[numRecords];
                for (int r = 0; r < numRecords; ++r)
                {
                    time = csvParser.EndTimes[r];
                    durations[r] = time - times[r];
                    for (int c = 0; c < numChannels; c++)
                    {
                        data[c][r] = csvParser.Data[r, c];
                    }
                }
                for (int c = 0; c < numChannels; c++)
                {
                    channels[c].AddDataPoints(compartment, times, data[c], durations, dataFiles);
                }
            }
            else
            {
                for (int r = 0; r < numRecords; ++r)
                {
                    time = times[r];
                    for (int c = 0; c < numChannels; c++)
                    {
                        data[c][r] = csvParser.Data[r, c];
                    }
                }
                for (int c = 0; c < numChannels; c++)
                {
                    channels[c].AddDataPoints(compartment, times, data[c], dataFiles);
                }
            }

            dataFile.DataEnd = time;

            MakeNewParser();

            return ReturnCode.SUCCESS;
        }

        public override ReturnCode AutoIngestFile(ChannelCompartment compartment, string fileName)
        {
            if (csvParser.AutoConfigureFromFile(fileName) == ReturnCode.SUCCESS)
            {
                Delimiter = csvParser.Delimiter;
                HasEndTimes = csvParser.GetEndTimes;
                if (HasEndTimes)
                {
                    SetNumberOfChannels(csvParser.NumberOfColumns - 2);
                }
                else
                {
                    SetNumberOfChannels(csvParser.NumberOfColumns - 1);
                }
                NumberOfHeaders = csvParser.NumberOfHeaders;
                TimeStampFormat = csvParser.TimeStampFormat;
                MakeNewParser();
                return IngestFile(compartment, fileName);
            }
            else return ReturnCode.FAIL;
        }

        public override DateTime GetFileDate(string file)
        {
            if (csvParser.ParseFirstEntry(file) == ReturnCode.SUCCESS)
            {
                return csvParser.TimeStamps[0];
            }
            return DateTime.MinValue;
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = GetStandardInstrumentParameters();
            string delim = "";
            switch (Delimiter)
            {
                case CSVParser.DelimiterType.Comma:
                    delim = "Comma";
                    break;
                case CSVParser.DelimiterType.CommaOrWhitespace:
                    delim = "Comma or Whitespace";
                    break;
            }
            parameters.Add(new EnumParameter("Delimiter")
            {
                Value = delim,
                ValidValues = { "Comma", "Comma or Whitespace" }
            });
            parameters.Add(new StringParameter("Extension") { Value = FileExtension });
            parameters.Add(new IntParameter("Headers") { Value = NumberOfHeaders.ToString() });
            parameters.Add(new IntParameter("Channels") { Value = numChannels.ToString() });
            parameters.Add(new StringParameter("Time Stamp Format") { Value = TimeStampFormat });
            parameters.Add(new BoolParameter("Has End Times", HasEndTimes));
            return parameters;
        }

        public override void ApplyParameters(List<Parameter> parameters)
        {
            ApplyStandardInstrumentParameters(this, parameters);
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Delimiter":
                        switch(((EnumParameter)param).Value)
                        {
                            case "Comma":
                                Delimiter = CSVParser.DelimiterType.Comma;
                                break;
                            case "Comma or Whitespace":
                                Delimiter = CSVParser.DelimiterType.CommaOrWhitespace;
                                break;
                        }
                        break;
                    case "Extension":
                        FileExtension = ((StringParameter)param).Value;
                        break;
                    case "Headers":
                        NumberOfHeaders = ((IntParameter)param).ToInt();
                        break;
                    case "Channels":
                        SetNumberOfChannels(((IntParameter)param).ToInt());
                        break;
                    case "Time Stamp Format":
                        TimeStampFormat = ((StringParameter)param).Value;
                        break;
                    case "Has End Times":
                        HasEndTimes = ((BoolParameter)param).ToBool();
                        break;
                }
            }
        }
    }

    public class CSVInstrumentHookup : InstrumentHookup
    {
        public CSVInstrumentHookup()
        {
            TemplateParameters.AddRange( new List<ParameterTemplate>() {
                new ParameterTemplate("Delimiter", ParameterType.Enum)
                {
                    ValidValues = {"Comma", "Comma or Whitespace"}
                },
                new ParameterTemplate("Extension", ParameterType.String),
                new ParameterTemplate("Headers", ParameterType.Int),
                new ParameterTemplate("Channels", ParameterType.Int),
                new ParameterTemplate("Time Stamp Format", ParameterType.String),
                new ParameterTemplate("Has End Times", ParameterType.Bool)
                });
        }

        public override string Type { get { return "CSV"; } }

        public override Instrument FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            int nHeaders = 0;
            int nChannels = 0;
            string tStampFormat = "";
            string fileExtension = "csv";
            CSVParser.DelimiterType delimiter = CSVParser.DelimiterType.Comma;
            bool hasEndTimes = false;
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Delimiter":
                        switch (((EnumParameter)param).Value)
                        {
                            case "Comma":
                                delimiter = CSVParser.DelimiterType.Comma;
                                break;
                            case "Comma or Whitespace":
                                delimiter = CSVParser.DelimiterType.CommaOrWhitespace;
                                break;
                        }
                        break;
                    case "Extension":
                        fileExtension = ((StringParameter)param).Value;
                        break;
                    case "Headers":
                        nHeaders = ((IntParameter)param).ToInt();
                        break;
                    case "Channels":
                        nChannels = ((IntParameter)param).ToInt();
                        break;
                    case "Time Stamp Format":
                        tStampFormat = ((StringParameter)param).Value;
                        break;
                    case "Has End Times":
                        hasEndTimes = ((BoolParameter)param).ToBool();
                        break;
                }
            }
            CSVInstrument instrument = new CSVInstrument(parent, newName, nChannels, hasEndTimes, id);
            instrument.Delimiter = delimiter;
            instrument.NumberOfHeaders = nHeaders;
            instrument.TimeStampFormat = tStampFormat;
            instrument.FileExtension = fileExtension;
            Instrument.ApplyStandardInstrumentParameters(instrument, parameters);
            return instrument;
        }
    }
}
