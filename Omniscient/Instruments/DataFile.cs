using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class DataFile
    {
        public string FileName { get; set; }
        public DateTime DataStart { get; set; }
        public DateTime DataEnd { get; set; }

        public DataFile(string fileName)
        {
            FileName = fileName;
            DataStart = DateTime.MinValue;
            DataEnd = DateTime.MinValue;
        }

        public DataFile(string fileName, DateTime dataStart)
        {
            FileName = fileName;
            DataStart = dataStart;
            DataEnd = DateTime.MinValue;
        }

        public DataFile(string fileName, DateTime dataStart, DateTime dataEnd)
        {
            FileName = fileName;
            DataStart = dataStart;
            DataEnd = dataEnd;
        }
    }
}
