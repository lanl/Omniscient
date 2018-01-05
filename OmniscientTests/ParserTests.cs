using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omniscient;
using System.Collections.Generic;

namespace OmniscientTests
{
    [TestClass]
    public class ParserTests
    {
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
    }
}
