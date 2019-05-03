using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    class CSVParser
    {
        private string fileName;
        public string FileName {
            get { return fileName; }
            set { fileName = value; }
        }

        private int nHeaders;
        public int NumberOfHeaders
        {
            get { return nHeaders; }
            set { nHeaders = value; }
        }

        private string timeStampFormat;
        public string TimeStampFormat
        {
            get { return timeStampFormat; }
            set { timeStampFormat = value; }
        }

        private int nColumns;
        public int NumberOfColumns
        {
            get { return nColumns; }
            set { nColumns = value; }
        }

        private double[,] data;
        public double[,] Data
        {
            get { return data; }
        }

        private DateTime[] timeStamps;
        public DateTime[] TimeStamps
        {
            get { return timeStamps; }
        }

        private int nRecords;

        public CSVParser()
        {
            fileName = "";
            nHeaders = 0;
            timeStampFormat = "";
            nColumns = 2;
            nRecords = 0;
        }

        public int GetNumRecords()
        {
            return nRecords;
        }

        public ReturnCode ParseFirstEntry(string newFileName)
        {
            fileName = newFileName;
            string[] lines;
            try
            {
                lines = IOUtility.PermissiveReadLines(fileName, nHeaders+1);
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            int nDataLines = lines.Length - nHeaders;
            if (nDataLines < 1)
                return ReturnCode.CORRUPTED_FILE;
            nDataLines = 1;

            data = new double[nDataLines, nColumns - 1];
            timeStamps = new DateTime[nDataLines];

            nRecords = nDataLines;
            int entry;
            try
            {
                string[] tokens;
                int i = nHeaders;
                entry = i - nHeaders;
                tokens = lines[i].Split(',');
                if (tokens.Length < nColumns)
                    return ReturnCode.CORRUPTED_FILE;
                timeStamps[entry] = DateTime.Parse(tokens[0]);
                for (int j = 1; j < nColumns; j++)
                {
                    data[entry, j - 1] = double.Parse(tokens[j]);
                }
            }
            catch
            {
                return ReturnCode.CORRUPTED_FILE;
            }

            return ReturnCode.SUCCESS;
        }

        public ReturnCode ParseFile(string newFileName)
        {
            fileName = newFileName;
            string[] lines;
            try
            {
                lines = IOUtility.PermissiveReadAllLines(fileName);
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            int nDataLines = lines.Length - nHeaders;
            if (nDataLines < 0)
                return ReturnCode.CORRUPTED_FILE;

            data = new double[nDataLines, nColumns - 1];
            timeStamps = new DateTime[nDataLines];

            nRecords = nDataLines;
            int entry;
            try
            {
                string[] tokens;
                for (int i = nHeaders; i < lines.Length; i++)
                {
                    entry = i - nHeaders;
                    tokens = lines[i].Split(',');
                    if (tokens.Length < nColumns)
                        return ReturnCode.CORRUPTED_FILE;
                    timeStamps[entry] = DateTime.Parse(tokens[0]);
                    for (int j = 1; j < nColumns; j++)
                    {
                        data[entry, j - 1] = double.Parse(tokens[j]);
                    }
                }
            }
            catch
            {
                return ReturnCode.CORRUPTED_FILE;
            }

            return ReturnCode.SUCCESS;
        }
    }
}
