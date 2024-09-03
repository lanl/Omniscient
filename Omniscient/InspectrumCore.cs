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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{

    class InspectrumCore
    {
        public bool InstrumentMode { get; private set; }
        string[] dataFiles;
        DateTime[] dataFileDates;
        public bool LaterSpectra { get; private set; }
        public bool EarlierSpectra { get; private set; }

        private double fileCalibrationZero;
        private double fileCalibrationSlope;

        public double CalibrationZero { get; set; }
        public double CalibrationSlope { get; set; }
        public int[] Counts { get; private set; }

        CHNParser chnParser;
        SPEParser speParser;
        N42Parser n42Parser;
        HGMParser hgmParser;

        public string FileName { get; private set; }
        public string FileExt { get; private set; }
        public bool FileLoaded { get; private set; } = false;

        public DateTime SpectrumStart { get; private set; }
        public TimeSpan SpectrumRealTime { get; private set; }
        public TimeSpan SpectrumLiveTime { get; private set; }
        public double SpectrumDeadTimePercent { get; private set; }

        public int FileSpectrumNumber { get; private set; }
        public int FileSpectraCount { get; private set; }

        public InspectrumCore()
        {
            fileCalibrationZero = 0;
            fileCalibrationSlope = 1;

            CalibrationZero = 0;
            CalibrationSlope = 1;
            Counts = new int[0];

            chnParser = new CHNParser();
            speParser = new SPEParser();
            n42Parser = new N42Parser();
            hgmParser = new HGMParser();
            InstrumentMode = false;
        }

        public ReturnCode LoadSpectrumFile(string fileName, DateTime? specTime = null)
        {
            Spectrum spectrum;
            string fileAbrev = fileName.Substring(fileName.LastIndexOf('\\') + 1);
            FileExt = fileAbrev.Substring(fileAbrev.Length - 3).ToLower();
            if (FileExt == "chn")
            {
                FileSpectraCount = 1;
                FileSpectrumNumber = 1;
                chnParser.ParseSpectrumFile(fileName);
                spectrum = chnParser.GetSpectrum();
            }
            else if (FileExt == "spe")
            {
                FileSpectraCount = 1;
                FileSpectrumNumber = 1;
                speParser.ParseSpectrumFile(fileName);
                spectrum = speParser.GetSpectrum();
            }
            else if (FileExt == "n42")
            {
                FileSpectraCount = 1;
                FileSpectrumNumber = 1;
                n42Parser.ParseSpectrumFile(fileName);
                spectrum = n42Parser.GetSpectrum();
            }
            else if (FileExt == "hgm")
            {
                hgmParser.ParseSpectrumFile(fileName);
                List<Spectrum> spectra = hgmParser.Spectra;
                FileSpectraCount = spectra.Count;
                if (specTime != null)
                {
                    // If need be, fail gracefully
                    spectrum = spectra[0];
                    FileSpectrumNumber = 1;

                    for (int i = 0; i < spectra.Count; ++i)
                    {
                        if (spectra[i].GetStartTime() == specTime)
                        {
                            spectrum = spectra[i];
                            FileSpectrumNumber = i + 1;
                            break;
                        }
                    }
                }
                else
                {
                    spectrum = hgmParser.GetSpectrum();
                    FileSpectrumNumber = 1;
                }
            }
            else
            {
                return ReturnCode.BAD_INPUT;
            }

            LoadSpectrum(spectrum);
            InstrumentMode = false;
            FileName = fileName;
            FileLoaded = true;
            return ReturnCode.SUCCESS;
        }

        public void SumAndLoadSpectra(List<Spectrum> spectra)
        {
            FileName = "";
            FileExt = "";

            Spectrum spectrum = Spectrum.Sum(spectra);
            
            LoadSpectrum(spectrum);

            FileSpectraCount = 1;
            FileSpectrumNumber = 1;
        }

        public void EnterInstrumentMode(string startFile, DateTime startTime, string[] files, DateTime[] dates)
        {
            LoadSpectrumFile(startFile, startTime);
            dataFiles = files;
            dataFileDates = dates;
            SortNearbyInstrumentSpectra();
            LoadSpectrumFile(startFile, startTime);

            int index = Array.IndexOf(dataFiles, FileName);
            CheckIndex(index);

            InstrumentMode = true;
        }

        private void SortNearbyInstrumentSpectra()
        {
            string initialFileName = FileName;
            DateTime initialTime = SpectrumStart;

            DateTime lowerDate = initialTime.Date.AddDays(-1).AddSeconds(-1);
            DateTime upperDate = initialTime.Date.AddDays(1).AddSeconds(1);

            for (int i=0; i<dataFileDates.Length; i++)
            {
                if (dataFileDates[i] > lowerDate && dataFileDates[i] < upperDate && 
                    dataFileDates[i].Minute == 0 && dataFileDates[i].Second == 0 && dataFileDates[i].Hour == 0)
                {
                    LoadSpectrumFile(dataFiles[i]);
                    dataFileDates[i] = SpectrumStart;
                }
            }
            Array.Sort(dataFileDates, dataFiles);
            LoadSpectrumFile(initialFileName);
        }

        private void CheckIndex(int index)
        {
            EarlierSpectra = true;
            LaterSpectra = true;
            if (index == 0 && FileSpectrumNumber == 1) EarlierSpectra = false;
            if (index == dataFiles.Length - 1 && FileSpectrumNumber == FileSpectraCount) LaterSpectra = false;
        }

        public void LoadNextInstrumentSpectrum()
        {
            if (FileSpectrumNumber < FileSpectraCount)
            {
                SwitchFileSpectrum(FileSpectrumNumber + 1);
                int index = Array.IndexOf(dataFiles, FileName);
                CheckIndex(index);
            }
            else
            {
                SortNearbyInstrumentSpectra();
                int index = Array.IndexOf(dataFiles, FileName);
                if (index < dataFiles.Length-1)
                { 
                    LoadSpectrumFile(dataFiles[index+1]);
                }
                CheckIndex(index + 1);
            }
            InstrumentMode = true;
        }

        public void LoadPreviousInstrumentSpectrum()
        {
            if (FileSpectrumNumber > 1)
            {
                SwitchFileSpectrum(FileSpectrumNumber - 1);
                int index = Array.IndexOf(dataFiles, FileName);
                CheckIndex(index);
            }
            else
            {
                SortNearbyInstrumentSpectra();
                int index = Array.IndexOf(dataFiles, FileName);
                if (index > 0)
                {
                    LoadSpectrumFile(dataFiles[index - 1]);
                    if (FileSpectraCount > 1)
                    {
                        SwitchFileSpectrum(FileSpectraCount);
                    }
                }
                CheckIndex(index - 1);
            }
            InstrumentMode = true;
        }

        private void LoadSpectrum(Spectrum spectrum)
        {
            CalibrationZero = spectrum.GetCalibrationZero();
            CalibrationSlope = spectrum.GetCalibrationSlope();
            Counts = spectrum.GetCounts();
            SpectrumStart = spectrum.GetStartTime();
            double realTime = spectrum.GetRealTime();
            double liveTime = spectrum.GetLiveTime();
            SpectrumRealTime = TimeSpan.FromSeconds(realTime);
            SpectrumLiveTime = TimeSpan.FromSeconds(liveTime);
            SpectrumDeadTimePercent = 100 * (realTime - liveTime) / realTime;
        }

        public void ResetCalibration()
        {
            CalibrationSlope = fileCalibrationSlope;
            CalibrationZero = fileCalibrationZero;
        }

        public void SwitchFileSpectrum(int index)
        {
            if (index > FileSpectraCount || index < 1) throw new IndexOutOfRangeException("Invalid index for spectra in file!");
            LoadSpectrum(hgmParser.Spectra[index - 1]);
            FileSpectrumNumber = index;
        }

        public ReturnCode ExportSpectrum(string fileName)
        {
            string extension = fileName.Substring(fileName.Length - 3).ToLower();
            SpectrumWriter spectrumWriter;
            switch(extension)
            {
                case "csv":
                    spectrumWriter = new CSVSpectrumWriter();
                    break;
                case "chn":
                    spectrumWriter = new CHNWriter();
                    break;
                default:
                    return ReturnCode.BAD_INPUT;
            }
            spectrumWriter.SetSpectrum(new Spectrum(CalibrationZero, CalibrationSlope, Counts,
                SpectrumStart, SpectrumRealTime.TotalSeconds, SpectrumLiveTime.TotalSeconds));

            return spectrumWriter.WriteSpectrumFile(fileName);
        }
    }
}
