using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Omniscient
{
    public class IOUtility
    {
        /// <summary>
        /// Reads all lines from a file without locking it
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string[] PermissiveReadAllLines(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    List<string> lines = new List<string>();
                    while (!reader.EndOfStream)
                    {
                        lines.Add(reader.ReadLine());
                    }
                    return lines.ToArray();
                }
            }
        }
    }
}
