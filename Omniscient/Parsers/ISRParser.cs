using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Omniscient.Parsers
{
    struct ISRRecord
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

    class ISRParser
    {
        public enum ReturnCode { SUCCESS, FAIL, COULD_NOT_OPEN_FILE }

        private string fileName;
        string MICVersion;
        string stationID;
        private int numRecords;
        private ISRRecord[] records;
        private DateTime date;

        public ISRParser()
        {
            fileName = "";
            MICVersion = "";
            stationID = "";
            numRecords = 0;
        }

        public DateTime ISRTimeToDateTime(UInt32 timeIn)
        {
            return new DateTime(1952, 1, 1).AddSeconds((double)timeIn);
        }

        public ReturnCode ParseFile(string newFileName)
        {
            fileName = newFileName;
            FileStream readStream;

            try
            {
                // Read header
                readStream = new FileStream(fileName, FileMode.Open);
                long numBytes = readStream.Length;
                BinaryReader readBinary = new BinaryReader(readStream);
                int headerSize = int.Parse(new string(readBinary.ReadChars(4)));
                readBinary.ReadBytes(5);                                        // Not used
                MICVersion = new string(readBinary.ReadChars(5));
                stationID = new string(readBinary.ReadChars(3));
                int year = 2000 + int.Parse(new string(readBinary.ReadChars(3)));
                int month = int.Parse(new string(readBinary.ReadChars(3)));
                int day = int.Parse(new string(readBinary.ReadChars(3)));
                date = new DateTime(year, month, day);
                readBinary.ReadBytes(headerSize-22);                            // Spare room

                // Read data records
                numRecords = (int)((numBytes - 74) / 54);
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
