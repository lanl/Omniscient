using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    class SMMSParser
    {
        const string TIMESTAMP_FORMAT = "yyyy/MM/dd HH:mm:ss";
        const int COLUMNS_PER_INSTRUMENT = 6;
        private System.Globalization.CultureInfo CULTURE_INFO = new CultureInfo("en-US");

        public string[] Headers { get; private set; }
        public double[,] Data { get; private set; }
        public DateTime[] TimeStamps { get; private set; }

        public int NumberOfInstruments { get; set; }

        public SMMSParser()
        {

        }

        public ReturnCode ParseFile(string fileName)
        {
            // Read lines from file
            string[] lines;
            try
            {
                lines = IOUtility.PermissiveReadAllLines(fileName);
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            int nDataLines = lines.Length - 1;
            if (nDataLines < 0)
                return ReturnCode.CORRUPTED_FILE;

            // Get headers from first line
            Headers = lines[0].Split(',');

            int nDataColumns = NumberOfInstruments * COLUMNS_PER_INSTRUMENT;
            if (Headers.Length != nDataColumns + 2 || Headers[0] != "SOH" || Headers[1] != "DateTime") return ReturnCode.CORRUPTED_FILE;

            // Iterate through data lines (data is stored in reverse order)
            Data = new double[nDataLines, nDataColumns];
            TimeStamps = new DateTime[nDataLines];
            string[] tokens;
            int maxIndex = nDataLines - 1;
            for (int dataIndex=0; dataIndex < nDataLines; dataIndex++)
            {
                tokens = lines[dataIndex+1].Split(',');
                TimeStamps[maxIndex-dataIndex] = DateTime.ParseExact(tokens[1], TIMESTAMP_FORMAT, CULTURE_INFO);
                for (int col = 0; col < nDataColumns; col++)
                {
                    Data[maxIndex - dataIndex, col] = double.Parse(tokens[col+2]);
                }
            }

            return ReturnCode.SUCCESS;
        }
    }
}
