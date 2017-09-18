using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Omniscient.Parsers
{
    public struct BIDRecord
    {
        public UInt32 time;
        public UInt16 status;
        public double chlACountRate;
        public double chBCountRate;
        public double chCCountRate;
        public double gamInGamCh1;
        public double gamCh1Sigma;
        public double gamInGamCh2;
        public double gamCh2Sigma;
        public UInt16 elapsedTime;
    }

    class BIDParser
    {
        public enum ReturnCode { SUCCESS, FAIL, COULD_NOT_OPEN_FILE }

        private string fileName;
        string MICVersion;
        string stationID;
        private int numRecords;
        private BIDRecord[] records;
        private DateTime date;

        private int headerSize;

        public BIDParser()
        {
            fileName = "";
            MICVersion = "";
            stationID = "";
            numRecords = 0;
            headerSize = 0;
        }

        private void ReadHeader(BinaryReader readBinary)
        {
            headerSize = int.Parse(new string(readBinary.ReadChars(4)));
            readBinary.ReadBytes(5);                                        // Not used
            MICVersion = new string(readBinary.ReadChars(5));
            stationID = new string(readBinary.ReadChars(3));
            int year = 2000 + int.Parse(new string(readBinary.ReadChars(3)));
            int month = int.Parse(new string(readBinary.ReadChars(3)));
            int day = int.Parse(new string(readBinary.ReadChars(3)));
            date = new DateTime(year, month, day);
        }

        private void ReadDataRecords(BinaryReader readBinary)
        {
            readBinary.ReadBytes(headerSize - 22);                            // Spare room in header
            long numBytes = readBinary.BaseStream.Length;
            // Read data records
            numRecords = (int)((numBytes - 73) / 36);
            records = new BIDRecord[numRecords];
            for (int r = 0; r < numRecords; ++r)
            {
                records[r] = new BIDRecord();
                records[r].time = readBinary.ReadUInt32();
                records[r].status = readBinary.ReadUInt16();
                records[r].chlACountRate = readBinary.ReadDouble();
                records[r].chBCountRate = readBinary.ReadDouble();
                records[r].chCCountRate = readBinary.ReadDouble();
                records[r].gamInGamCh1 = readBinary.ReadDouble();
                records[r].gamCh1Sigma = readBinary.ReadDouble();
                records[r].gamInGamCh2 = readBinary.ReadDouble();
                records[r].gamCh2Sigma = readBinary.ReadDouble();
                records[r].elapsedTime = readBinary.ReadUInt16();
            }
        }

        // ParseHeader is for quickly reading some header data without reading in the whole file
        public ReturnCode ParseHeader(string newFileName)
        {
            fileName = newFileName;
            FileStream readStream;

            try
            {
                readStream = new FileStream(fileName, FileMode.Open);
                BinaryReader readBinary = new BinaryReader(readStream);
                ReadHeader(readBinary);
                readStream.Close();
                numRecords = 0;
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            return ReturnCode.SUCCESS;
        }

        public ReturnCode ParseFile(string newFileName)
        {
            fileName = newFileName;
            FileStream readStream;

            try
            {
                readStream = new FileStream(fileName, FileMode.Open);
                BinaryReader readBinary = new BinaryReader(readStream);
                ReadHeader(readBinary);
                ReadDataRecords(readBinary);
                readStream.Close();
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            return ReturnCode.SUCCESS;
        }

        public string GetMICVerions() { return MICVersion; }
        public string GetStationID() { return stationID; }
        public DateTime GetDate() { return date; }
        public int GetNumRecords() { return numRecords; }

        public BIDRecord GetRecord(int index)
        {
            if (index < numRecords) return records[index];
            else return new BIDRecord();                    // This should probably be handled better...
        }
    }
}
