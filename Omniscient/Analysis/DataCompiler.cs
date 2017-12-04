using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public abstract class DataCompiler
    {
        string name;

        public DataCompiler(string newName) { name = newName; }

        public abstract ReturnCode Compile(List<string> sourceFiles, DateTime start, DateTime end, string targetFileName);
        
        public void SetName(string newName) { name = newName; }
        public string GetName() { return name; }
    }
}
