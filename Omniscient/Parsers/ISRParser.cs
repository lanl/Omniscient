using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Omniscient
{
    public struct ISRRecord
    {
        public UInt32 time;
        public UInt16 status;
        public double totals1;
        public double totals2;
        public double totals3;
        public double realsPlusAccidentals;
        public double accidentals;
        public double elapsedTime;
    }

    public class ISRParser
    {
        private string fileName;
        string MICVersion;
        string stationID;
        private int numRecords;
        private ISRRecord[] records;
        private DateTime date;

        private int headerSize;

        public ISRParser()
        {
            fileName = "";
            MICVersion = "";
            stationID = "";
            numRecords = 0;
            headerSize = 0;
        }

        public DateTime ISRTimeToDateTime(UInt32 timeIn)
        {
            return new DateTime(1952, 1, 1).AddSeconds((double)timeIn);
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
            numRecords = (int)((numBytes - 73) / 54);
            records = new ISRRecord[numRecords];
            for (int r = 0; r < numRecords; ++r)
            {
                records[r] = new ISRRecord();
                records[r].time = readBinary.ReadUInt32();
                records[r].status = readBinary.ReadUInt16();
                records[r].totals1 = readBinary.ReadDouble();
                records[r].totals2 = readBinary.ReadDouble();
                records[r].totals3 = readBinary.ReadDouble();
                records[r].realsPlusAccidentals = readBinary.ReadDouble();
                records[r].accidentals = readBinary.ReadDouble();
                records[r].elapsedTime = readBinary.ReadDouble();
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

        public ISRRecord GetRecord(int index)
        {
            if (index < numRecords) return records[index];
            else return new ISRRecord();                    // This should probably be handled better...
        }
    }
}
