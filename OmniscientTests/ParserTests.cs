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
﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omniscient;
using System.Collections.Generic;

namespace OmniscientTests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void WrittenEventFileCanBeParsed()
        {
            // Arrange
            string fileName = "testEvents.csv";
            List<Event> outEvents = new List<Event>()
            {
                new Event(null, new DateTime(2019,07,09,16,15,00), new DateTime(2019,07,09,16,16,00))
                {
                    Comment = "Events!"
                },
                new Event(null, new DateTime(2019,07,09,16,20,15), new DateTime(2019,07,09,16,21,30))
                {
                    Comment = "Events!",
                    MaxValue = 42,
                    MaxTime = new DateTime(2019,07,09,16,21,05)
                },
                new Event(null, new DateTime(2019,07,09,16,25,00), new DateTime(2019,07,09,16,29,00))
                {
                    Comment = "Events!"
                }
            };
            EventWriter writer = new EventWriter();
            EventParser parser = new EventParser();

            // Act
            writer.WriteEventFile(fileName, outEvents);
            parser.ParseFile(fileName);

            // Assert
            for(int i=0; i<outEvents.Count; i++)
            {
                Assert.AreEqual(outEvents[i].StartTime.Ticks, parser.StartTime[i].Ticks);
                Assert.AreEqual(outEvents[i].EndTime.Ticks, parser.EndTime[i].Ticks);
                Assert.AreEqual(outEvents[i].MaxValue, parser.MaxValue[i]);
                Assert.AreEqual(outEvents[i].MaxTime.Ticks, parser.MaxTime[i].Ticks);
                Assert.AreEqual(outEvents[i].Comment, parser.Comments[i]);
            }

        }

        [TestMethod]
        public void WrittenCHNCanBeParsed()
        {
            //Arrange
            Spectrum sourceSpectrum = new Spectrum(0, 1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7 });
            sourceSpectrum.SetStartTime(new DateTime(2017, 4, 14));
            sourceSpectrum.SetLiveTime(60);
            sourceSpectrum.SetRealTime(120);
            CHNWriter writer = new CHNWriter();
            writer.SetSpectrum(sourceSpectrum);
            string fileName = "testSpec.chn";
            CHNParser parser = new CHNParser();

            //Act
            writer.WriteSpectrumFile(fileName);
            parser.ParseSpectrumFile(fileName);
            Spectrum outSpec = parser.GetSpectrum();

            //Assert
            for (int i = 0; i < outSpec.GetNChannels(); i++)
                Assert.IsTrue(outSpec.GetCounts()[i] == sourceSpectrum.GetCounts()[i], "Channel count is not equal");
            Assert.IsTrue(outSpec.GetLiveTime() == sourceSpectrum.GetLiveTime(), "Live time is not equal");
            Assert.IsTrue(outSpec.GetRealTime() == sourceSpectrum.GetRealTime(), "Real time is not equal");
            Assert.IsTrue(outSpec.GetStartTime() == sourceSpectrum.GetStartTime(), "start time is not equal");
        }

        [TestMethod]
        public void CompiledCHNsCanBeParsed()
        {
            //Arrange
            Spectrum sourceSpectrum1 = new Spectrum(0, 1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7 });
            sourceSpectrum1.SetStartTime(new DateTime(2017, 4, 14, 0, 0, 0));
            sourceSpectrum1.SetLiveTime(60);
            sourceSpectrum1.SetRealTime(120);
            CHNWriter writer = new CHNWriter();
            writer.SetSpectrum(sourceSpectrum1);
            string fileName1 = "testSpec1.chn";
            writer.WriteSpectrumFile(fileName1);

            Spectrum sourceSpectrum2 = new Spectrum(0, 1, new int[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            sourceSpectrum2.SetStartTime(new DateTime(2017, 4, 14, 0, 3, 0));
            sourceSpectrum2.SetLiveTime(60);
            sourceSpectrum2.SetRealTime(120);
            writer.SetSpectrum(sourceSpectrum2);
            string fileName2 = "testSpec2.chn";
            writer.WriteSpectrumFile(fileName2);

            Spectrum sourceSpectrum3 = new Spectrum(0, 1, new int[] { 2, 3, 4, 5, 6, 7, 8, 9 });
            sourceSpectrum3.SetStartTime(new DateTime(2017, 4, 14, 0, 6, 0));
            sourceSpectrum3.SetLiveTime(60);
            sourceSpectrum3.SetRealTime(120);
            writer.SetSpectrum(sourceSpectrum3);
            string fileName3 = "testSpec3.chn";
            writer.WriteSpectrumFile(fileName3);

            SpectrumCompiler spectrumCompiler = new SpectrumCompiler("SpecCompiler");
            spectrumCompiler.SetSpectrumParser(new CHNParser());
            spectrumCompiler.SetSpectrumWriter(new CHNWriter());

            string compiledFile = "compiled.chn";
            List<string> sourceFiles = new List<string>(new string[] { fileName1, fileName2, fileName3 });

            CHNParser parser = new CHNParser();

            //Act
            spectrumCompiler.Compile(sourceFiles, new DateTime(2017, 4, 14, 0, 0, 0), new DateTime(2017, 4, 14, 0, 10, 0), compiledFile);
            parser.ParseSpectrumFile(compiledFile);
            Spectrum outSpec = parser.GetSpectrum();

            //Assert
            for (int i = 0; i < 8; i++)
                Assert.IsTrue(outSpec.GetCounts()[i] == 3 * (i + 1), "Channel count is not equal");
            Assert.IsTrue(outSpec.GetLiveTime() == 3 * 60, "Live time is not equal");
            Assert.IsTrue(outSpec.GetRealTime() == 3 * 120, "Real time is not equal");
            Assert.IsTrue(outSpec.GetStartTime() == sourceSpectrum1.GetStartTime(), "start time is not equal");
        }

        [TestMethod]
        public void WrittenNCCCanBeParsed()
        {
            //Arrange
            NCCWriter writer = new NCCWriter();
            writer.SetDetectorType("TEST");
            writer.SetDetectorID("007");
            writer.SetConfigurationID("42");
            writer.SetItemID("Test Item");
            writer.NCCMode = NCCWriter.NCCType.VERIFICATION;

            NCCWriter.Cycle cycle = new NCCWriter.Cycle();
            cycle.DateAndTime = new DateTime(2018, 1, 1, 0, 30, 0);
            cycle.CountSeconds = 600;
            cycle.Totals = 54321;
            cycle.RPlusA = 6070;
            cycle.A = 6001;
            cycle.Scaler1 = 999;
            cycle.Scaler2 = 0;
            cycle.MultiplicityRPlusA = new UInt32[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            cycle.MultiplicityA = new UInt32[] { 0, 0, 1, 1, 3, 5, 6, 7 };
            writer.Cycles.Add(cycle);

            cycle = new NCCWriter.Cycle();
            cycle.DateAndTime = new DateTime(2018, 1, 1, 0, 30, 0);
            cycle.CountSeconds = 600;
            cycle.Totals = 54321;
            cycle.RPlusA = 6070;
            cycle.A = 6001;
            cycle.Scaler1 = 999;
            cycle.Scaler2 = 0;
            cycle.MultiplicityRPlusA = new UInt32[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            cycle.MultiplicityA = new UInt32[] { 0, 0, 1, 1, 3, 5, 6, 7 };
            writer.Cycles.Add(cycle);

            NCCParser parser = new NCCParser();
            string fileName = "test.ncc";

            //Act
            writer.WriteNeutronCyclesFile(fileName);
            parser.ReadNeutronCyclesFile(fileName);

            //Assert
            Assert.IsTrue(parser.NCCMode == writer.NCCMode, "NCC Modes are not equal!");
            Assert.IsTrue(parser.GetDetectorType() == writer.GetDetectorType(), "Detector types are not equal!");
            Assert.IsTrue(parser.GetDetectorID() == writer.GetDetectorID(), "Detector IDs are not equal!");
            Assert.IsTrue(parser.GetConfigurationID() == writer.GetConfigurationID(), "Configuration IDs are not equal!");
            Assert.IsTrue(parser.GetStartDateTime() == writer.Cycles[0].DateAndTime, "Start times are not equal!");
            Assert.IsTrue(parser.Cycles.Count == writer.Cycles.Count, "Number of cycles are not equal");
            for (int c=0; c < parser.Cycles.Count; ++c)
            {
                Assert.IsTrue(parser.Cycles[c].DateAndTime == writer.Cycles[c].DateAndTime, "Cycle start times are not equal");
                Assert.IsTrue(parser.Cycles[c].CountSeconds == writer.Cycles[c].CountSeconds, "Cycle count times are not equal");
                Assert.IsTrue(parser.Cycles[c].Totals == writer.Cycles[c].Totals, "Cycle Totals are not equal");
                Assert.IsTrue(parser.Cycles[c].RPlusA == writer.Cycles[c].RPlusA, "Cycle RPlusA are not equal");
                Assert.IsTrue(parser.Cycles[c].A == writer.Cycles[c].A, "Cycle A are not equal");
                Assert.IsTrue(parser.Cycles[c].Scaler1 == writer.Cycles[c].Scaler1, "Cycle Scaler1 are not equal");
                Assert.IsTrue(parser.Cycles[c].Scaler2 == writer.Cycles[c].Scaler2, "Cycle Scaler2 are not equal");
                for (int m=0; m < parser.Cycles[c].MultiplicityRPlusA.Length; ++m)
                {
                    Assert.IsTrue(parser.Cycles[c].MultiplicityRPlusA[m] == writer.Cycles[c].MultiplicityRPlusA[m], "Multiplicity RPlusA are not equal");
                    Assert.IsTrue(parser.Cycles[c].MultiplicityA[m] == writer.Cycles[c].MultiplicityA[m], "Multiplicity A are not equal");
                }
            }
        }

        [TestMethod]
        public void WrittenDECCanBeParsed()
        {
            //Arrange
            DECFile output = new DECFile();
            output.Facility = "Lightly Advertised Nuclear Location";
            output.MBA = "Building-2";
            output.FromTime = new DateTime(2018, 1, 4, 12, 0, 0);
            output.ToTime = new DateTime(2018, 1, 4, 13, 0, 0);
            output.ItemName = "Alfred";
            output.ItemOriginDate = new DateTime(2017, 7, 17, 8, 0, 0);
            output.FullName = "Alfred Furgeson";
            output.Barcode = "0123456789";
            output.MassDate = new DateTime(2018, 1, 4, 11, 33, 55);
            output.Mass = 42;
            output.BatchName = "Stuffs";
            output.BatchSource = "Operator Declaration";
            output.Material.AmDate = new DateTime(1942, 12, 2, 15, 25, 0);
            output.Material.Am241MassPercent.Value = 1;
            output.Material.Am241MassPercent.Uncertainty = 0.2;
            output.Material.PuDate = new DateTime(1942, 12, 2, 15, 25, 0);
            output.Material.Pu238MassPercent.Value = 3;
            output.Material.Pu238MassPercent.Uncertainty = 1.5;
            output.Material.Pu239MassPercent.Value = 90;
            output.Material.Pu239MassPercent.Uncertainty = 0.05;
            output.Material.Pu240MassPercent.Value = 5;
            output.Material.Pu240MassPercent.Uncertainty = 0.1;
            output.Material.Pu241MassPercent.Value = 1.1;
            output.Material.Pu241MassPercent.Uncertainty = 0.11;
            output.Material.Pu242MassPercent.Value = 0.9;
            output.Material.Pu242MassPercent.Uncertainty = 0.12;
            output.CreationDate = new DateTime(2018, 1, 19, 14, 43, 29);
            output.ModificationDate = new DateTime(2018, 1, 19, 14, 43, 53);
            output.Note = "We would like to declare Alfred to be a very good item. Thank you.";
            DECFile input = new DECFile();
            string fileName = "test.dec";

            //Act
            output.WriteDeclarationFile(fileName);
            input.ParseDeclarationFile(fileName);

            //Assert
            Assert.IsTrue(input.Facility == output.Facility, "Facility is not equal");
            Assert.IsTrue(input.MBA == output.MBA, "MBA is not equal");
            Assert.IsTrue(input.FromTime == output.FromTime, "FromTime is not equal");
            Assert.IsTrue(input.ItemName == output.ItemName, "ItemName is not equal");
            Assert.IsTrue(input.ItemOriginDate == output.ItemOriginDate, "ItemOriginDate is not equal");
            Assert.IsTrue(input.FullName == output.FullName, "FullName is not equal");
            Assert.IsTrue(input.Barcode == output.Barcode, "Barcode is not equal");
            Assert.IsTrue(input.MassDate == output.MassDate, "MassDate is not equal");
            Assert.IsTrue(input.Mass == output.Mass, "Mass is not equal");
            Assert.IsTrue(input.BatchName == output.BatchName, "BatchName is not equal");
            Assert.IsTrue(input.Material.AmDate == output.Material.AmDate, "AmDate is not equal");
            Assert.IsTrue(input.Material.Am241MassPercent.Value == output.Material.Am241MassPercent.Value, "Am241MassPercent.Value is not equal");
            Assert.IsTrue(input.Material.Am241MassPercent.Uncertainty == output.Material.Am241MassPercent.Uncertainty, "Am241MassPercent.Uncertainty is not equal");
            Assert.IsTrue(input.Material.PuDate == output.Material.PuDate, "PuDate is not equal");
            Assert.IsTrue(input.Material.Pu238MassPercent.Value == output.Material.Pu238MassPercent.Value, "Pu238MassPercent.Value is not equal");
            Assert.IsTrue(input.Material.Pu238MassPercent.Uncertainty == output.Material.Pu238MassPercent.Uncertainty, "Pu238MassPercent.Uncertainty is not equal");
            Assert.IsTrue(input.Material.Pu239MassPercent.Value == output.Material.Pu239MassPercent.Value, "Pu239MassPercent.Value is not equal");
            Assert.IsTrue(input.Material.Pu239MassPercent.Uncertainty == output.Material.Pu239MassPercent.Uncertainty, "Pu239MassPercent.Uncertainty is not equal");
            Assert.IsTrue(input.Material.Pu240MassPercent.Value == output.Material.Pu240MassPercent.Value, "Pu240MassPercent.Value is not equal");
            Assert.IsTrue(input.Material.Pu240MassPercent.Uncertainty == output.Material.Pu240MassPercent.Uncertainty, "Pu240MassPercent.Uncertainty is not equal");
            Assert.IsTrue(input.Material.Pu241MassPercent.Value == output.Material.Pu241MassPercent.Value, "Pu241MassPercent.Value is not equal");
            Assert.IsTrue(input.Material.Pu241MassPercent.Uncertainty == output.Material.Pu241MassPercent.Uncertainty, "Pu241MassPercent.Uncertainty is not equal");
            Assert.IsTrue(input.Material.Pu242MassPercent.Value == output.Material.Pu242MassPercent.Value, "Pu242MassPercent.Value is not equal");
            Assert.IsTrue(input.Material.Pu242MassPercent.Uncertainty == output.Material.Pu242MassPercent.Uncertainty, "Pu242MassPercent.Uncertainty is not equal");
            Assert.IsTrue(input.CreationDate == output.CreationDate, "CreationDate is not equal");
            Assert.IsTrue(input.ModificationDate == output.ModificationDate, "ModificationDate is not equal");
            Assert.IsTrue(input.Note == output.Note, "Note is not equal");
        }
    }
}
