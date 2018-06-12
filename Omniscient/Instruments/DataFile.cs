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

        public DataFile(string fileName)
        {
            FileName = fileName;
        }
    }
}
