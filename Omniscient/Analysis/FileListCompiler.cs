using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Omniscient
{
    public class FileListCompiler : DataCompiler
    {
        public FileListCompiler(string newName) : base(newName)
        {
        }

        public override ReturnCode Compile(List<string> sourceFiles, DateTime start, DateTime end, string targetFileName)
        {
            try
            {
                File.WriteAllLines(targetFileName, sourceFiles);
            }
            catch
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }
            
            return ReturnCode.SUCCESS;
        }
    }
}
