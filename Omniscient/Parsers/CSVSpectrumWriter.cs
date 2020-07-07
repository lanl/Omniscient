using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    class CSVSpectrumWriter : SpectrumWriter
    {
        const string WRITER_TYPE = "CSV-SPECTRUM";

        public CSVSpectrumWriter() : base(WRITER_TYPE) {}

        public override ReturnCode WriteSpectrumFile(string fileName)
        {
            // Open file
            FileStream writeStream;
            try { writeStream = new FileStream(fileName, FileMode.Create); }
            catch { return ReturnCode.COULD_NOT_OPEN_FILE; }

            try
            { 
                StreamWriter file = new StreamWriter(writeStream);

                double[] bins = spectrum.GetBins();
                int[] counts = spectrum.GetCounts();

                file.Write("Start Time," + spectrum.GetStartTime().ToString() + "\r\n");
                file.Write("Real Time (s)," + spectrum.GetRealTime().ToString() + "\r\n");
                file.Write("Live Time (s)," + spectrum.GetLiveTime().ToString() + "\r\n");
                file.Write("Bin,Counts\r\n");
                for (int line=0; line<bins.Length; line++)
                {
                    file.Write(bins[line].ToString() + "," + counts[line].ToString() + "\r\n");
                }
                file.Close();
            }
            catch
            {
                return ReturnCode.FAIL;
            }
            return ReturnCode.SUCCESS;
        }
    }
}
