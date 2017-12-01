using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public abstract class DataCompiler
    {
        DateTime startTime;
        DateTime endTime;
        string targetFileName;
        List<string> sourceFiles;

        public abstract void Compile();
    }
}
