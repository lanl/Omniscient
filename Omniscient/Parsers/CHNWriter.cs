using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Omniscient
{
    public class CHNWriter : SpectrumWriter
    {
        const string WRITER_TYPE = "CHN";

        private const short FILE_HEADER_CHECK = -1;
        private const short MCA_NUMBER = 777;
        private const short SEG_NUMBER = 1;
        private static readonly DateTime YEAR_2000 = new DateTime(2000, 1, 1);
        private const short CHANNEL_OFFSET = 0;
        private const short FILE_FOOTER_CHECK = -102;
        private const short RESERVED = 0;
        private const string DETECTOR = "Omniscient";
        private const string SAMPLE = "Sample";

        public CHNWriter() : base(WRITER_TYPE)
        {
        }

        public override ReturnCode WriteSpectrumFile(string fileName)
        {
            // Open file
            FileStream writeStream;
            try { writeStream = new FileStream(fileName, FileMode.Create); }
            catch { return ReturnCode.COULD_NOT_OPEN_FILE; }
            BinaryWriter binaryWriter = new BinaryWriter(writeStream);

            // Write Header
            binaryWriter.Write(FILE_HEADER_CHECK);
            binaryWriter.Write(MCA_NUMBER);
            binaryWriter.Write(SEG_NUMBER);
            binaryWriter.Write(spectrum.GetStartTime().Second.ToString("00").ToCharArray());
            binaryWriter.Write((int)(spectrum.GetRealTime() * 50));     // units of 20 ms
            binaryWriter.Write((int)(spectrum.GetLiveTime() * 50));     // units of 20 ms
            string startDate = spectrum.GetStartTime().ToString("ddMMMyy");
            if (spectrum.GetStartTime() > YEAR_2000)
                startDate += "1";
            else
                startDate += "*";
            binaryWriter.Write(startDate.ToCharArray());
            binaryWriter.Write(spectrum.GetStartTime().ToString("HHmm").ToCharArray());
            binaryWriter.Write(CHANNEL_OFFSET);
            binaryWriter.Write((short)spectrum.GetNChannels());

            // Write Spectrum Data
            int[] counts = spectrum.GetCounts();
            for (int bin = 0; bin < spectrum.GetNChannels(); bin++)
                binaryWriter.Write(counts[bin]);

            // Write Footer
            binaryWriter.Write(FILE_FOOTER_CHECK);
            binaryWriter.Write(RESERVED);
            binaryWriter.Write((float)spectrum.GetCalibrationZero());
            binaryWriter.Write((float)spectrum.GetCalibrationSlope());
            binaryWriter.Write((float)0);       // Quadratic calibration term
            binaryWriter.Write((float)0);       // Peak shape 0
            binaryWriter.Write((float)0);       // Peak shape slope
            binaryWriter.Write((float)0);       // Peak shape quadratic term
            for (int i = 0; i < 114; i++)
                binaryWriter.Write(RESERVED);
            binaryWriter.Write((byte)DETECTOR.Length);
            binaryWriter.Write(DETECTOR.ToString());
            for (int i = 0; i < 63 - DETECTOR.Length; i++)
                binaryWriter.Write((byte)0);
            binaryWriter.Write((byte)SAMPLE.Length);
            binaryWriter.Write(SAMPLE.ToString());
            for (int i = 0; i < 63 - SAMPLE.Length; i++)
                binaryWriter.Write((byte)0);
            for (int i = 0; i < 64; i++)
                binaryWriter.Write(RESERVED);

            writeStream.Close();

            return ReturnCode.SUCCESS;
        }
    }
}
