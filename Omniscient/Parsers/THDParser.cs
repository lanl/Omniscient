using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public struct THDRecord
    {
        public DateTime time;
        public double data0;
        public double data1;
        public double data2;
    }

    public class THDParser
    {
        public DateTime Date { get; private set; }
        public List<THDRecord> Records { get; private set; }

        string fileString;
        int dataStart = -1;

        private bool IsStartOfRecord(int i)
        {
            if (fileString[i + 4] == '-' && fileString[i + 7] == '-' && fileString[i + 10] == ' ' &&
                        fileString[i + 13] == ':' && fileString[i + 16] == ':' && fileString[i + 19] == ',')
            {
                return true;
            }
            return false;
        }

        private void FindDataStart()
        {
            int nChar = fileString.Length;
            dataStart = -1;
            for(int i = 0; i< nChar - 22; i++)
            {
                if (fileString[i] == '2' && fileString[i+1] == '0') // Initial check
                {
                    if (IsStartOfRecord(i))
                    {
                        dataStart = i;
                        break;
                    }
                }
            }
        }

        private void ReadRecords()
        {
            Records = new List<THDRecord>();

            int nChar = fileString.Length;
            int i = dataStart;
            THDRecord record;
            int recordStart = i;
            string recordString;
            string[] tokens;

            while(i<nChar)
            {
                while(fileString[i] != '\n') i++;
                recordString = fileString.Substring(recordStart, i - recordStart);
                tokens = recordString.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
                
                record = new THDRecord();
                record.time = DateTime.Parse(tokens[0]);
                record.data0 = double.Parse(tokens[1]);
                record.data1 = double.Parse(tokens[2]);
                record.data2 = double.Parse(tokens[3]);
                Records.Add(record);

                i++;

                if (i <nChar-22 && IsStartOfRecord(i))
                {
                    recordStart = i;
                }
                else
                {
                    break;
                }
            }

            if (Records.Count > 0) Date = Records[0].time;
        }

        private void ReadFirstRecord()
        {
            Records = new List<THDRecord>();

            int nChar = fileString.Length;
            int i = dataStart;
            THDRecord record;
            int recordStart = i;
            string recordString;
            string[] tokens;

            while (fileString[i] != '\n') i++;
            recordString = fileString.Substring(recordStart, i - recordStart);
            tokens = recordString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            record = new THDRecord();
            record.time = DateTime.Parse(tokens[0]);
            record.data0 = double.Parse(tokens[1]);
            record.data1 = double.Parse(tokens[2]);
            record.data2 = double.Parse(tokens[3]);
            Records.Add(record);

            if (Records.Count > 0) Date = Records[0].time;
        }

        public ReturnCode ParseFile(string newFileName)
        {
            try
            {
                fileString = File.ReadAllText(newFileName);
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            try
            {
                FindDataStart();
                ReadRecords();
            }
            catch (Exception ex)
            {
                return ReturnCode.CORRUPTED_FILE;
            }

            return ReturnCode.SUCCESS;
        }

        public ReturnCode ParseFirstRecord(string newFileName)
        {
            try
            {
                fileString = File.ReadAllText(newFileName);
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            try
            {
                FindDataStart();
                ReadFirstRecord();
            }
            catch (Exception ex)
            {
                return ReturnCode.CORRUPTED_FILE;
            }

            return ReturnCode.SUCCESS;
        }
    }
}
