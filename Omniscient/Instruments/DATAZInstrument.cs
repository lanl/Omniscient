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
    public class DATAZInstrument : Instrument
    {
        private const string FILE_EXTENSION = "dataz";

        DATAZParser parser;

        public DATAZInstrument(DetectionSystem parent, string newName, uint id) : base(parent, newName, id)
        {
            InstrumentType = "DATAZ";
            FileExtension = FILE_EXTENSION;
            parser = new DATAZParser();
            SetNumberOfChannels(1);
        }

        public ReturnCode SetNumberOfChannels(int nChannels)
        {
            numChannels = nChannels;
            if (numChannels < 1) return ReturnCode.BAD_INPUT;
            channels = new Channel[numChannels];
            for (int i = 0; i < numChannels; ++i)
            {
                channels[i] = new Channel(Name + "-" + (i + 1).ToString(), this, Channel.ChannelType.COUNT_RATE, 0);

            }
            return ReturnCode.SUCCESS;
        }

        public override ReturnCode IngestFile(ChannelCompartment compartment, string fileName)
        {
            DateTime time = DateTime.MinValue;
            DataFile dataFile = new DataFile(fileName);
            ReturnCode returnCode = parser.ParseFile(fileName);
            if (returnCode != ReturnCode.SUCCESS) return returnCode;
            if (parser.TimeStamps.Length > 0) dataFile.DataStart = parser.TimeStamps[0];
            else dataFile.DataStart = DateTime.MinValue;

            if (parser.Data.GetLength(1) != numChannels)
            {
                SetNumberOfChannels(parser.Data.GetLength(1));
                TryNamingChannelsFromHeaders(fileName);
            }

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
            parser = new DATAZParser();
            return ReturnCode.SUCCESS;
        }

        public override ReturnCode AutoIngestFile(ChannelCompartment compartment, string fileName)
        {
            parser = new DATAZParser();
            if (parser.ParseFile(fileName) == ReturnCode.SUCCESS)
            {
                TryNamingChannelsFromHeaders(fileName);
                return IngestFile(compartment, fileName);
            }
            return ReturnCode.FAIL;
        }

        public override DateTime GetFileDate(string file)
        {
            if (parser.ParseFirstRecord(file) == ReturnCode.SUCCESS)
            {
                return parser.TimeStamps[0];
            }
            return DateTime.MinValue;
        }

        public override List<Parameter> GetParameters()
        {
            return GetStandardInstrumentParameters();
        }

        /// <summary>
        /// Rename channels which have default names usingd file headers
        /// </summary>
        private void TryNamingChannelsFromHeaders()
        {
            try
            { 
                if (FileMode)
                {
                    TryNamingChannelsFromHeaders(FileModeFile);
                    return;
                }
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
            catch
            { }
        }

        private void TryNamingChannelsFromHeaders(string fileName)
        {
            ReturnCode returnCode = parser.ParseFirstRecord(fileName);
            if (returnCode != ReturnCode.SUCCESS) return;

            if (parser.Data.GetLength(1) != numChannels) SetNumberOfChannels(parser.Data.GetLength(1));
            int firstDataCol = parser.DateTimeColumn + 1;
            for (int c = 0; c < numChannels; c++)
            {
                if (channels[c].Name == Name + "-" + (c + 1).ToString())
                {
                    channels[c].Name = parser.Headers[c + firstDataCol];
                }
            }
        }

        public override void ApplyParameters(List<Parameter> parameters)
        {
            ApplyStandardInstrumentParameters(this, parameters);
            TryNamingChannelsFromHeaders();
        }
    }

    public class DATAZInstrumentHookup : InstrumentHookup
    {
        public override string Type { get { return "DATAZ"; } }

        public override Instrument FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            DATAZInstrument instrument = new DATAZInstrument(parent, newName, id);
            //Instrument.ApplyStandardInstrumentParameters(instrument, parameters);
            instrument.ApplyParameters(parameters);
            return instrument;
        }
    }
}
