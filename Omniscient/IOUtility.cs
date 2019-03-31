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

        /// <summary>
        /// Compacts a list of DateTimes into a list of longs (Ticks) using a
        /// twist on run length encoding: it looks at differences and applies
        /// RLE to those.
        /// </summary>
        /// <param name="times"></param>
        /// <returns></returns>
        public static List<long> DateCompact(List<DateTime> times)
        {
            int nTimes = times.Count();
            List<long> output = new List<long>(times.Count());
            if (nTimes < 1) return output;
            output.Add(times[0].Ticks);
            if (nTimes == 1) return output;
            output.Add(times[1].Ticks);
            if (nTimes == 2) return output;
            output.Add(times[2].Ticks);

            int outIndex = 3;
            long delta1 = output[1] - output[0];
            long delta2 = output[2] - output[1];

            bool inRun = delta2==delta1;
            long runCount = 0;

            for (int i=3; i<nTimes; i++)
            {
                if (inRun)
                {
                    if (times[i].Ticks - times[i - 1].Ticks == delta2)
                    {
                        runCount++;
                        continue;
                    }
                    else
                    {
                        inRun = false;
                        output.Add(runCount);
                        output.Add(times[i].Ticks);
                        outIndex+=2;
                        runCount = 0;
                    }
                }
                else
                {
                    delta1 = output[outIndex - 2] - output[outIndex - 3];
                    delta2 = output[outIndex - 1] - output[outIndex - 2];
                    if (delta1 == delta2)
                    {
                        inRun = true;
                        if (times[i].Ticks - times[i - 1].Ticks == delta2)
                        {
                            runCount++;
                            continue;
                        }
                        else
                        {
                            inRun = false;
                            output.Add(runCount);
                            output.Add(times[i].Ticks);
                            outIndex += 2;
                            runCount = 0;
                        }
                    }
                    else
                    {
                        output.Add(times[i].Ticks);
                        outIndex++;
                    }
                }
            }
            if (inRun) output.Add(runCount);

            return output;
        }

        /// <summary>
        /// Reverses DateCompact(List<DateTime> times)
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static List<DateTime> DateUnpack(List<long> ticks)
        {
            int nTicks = ticks.Count();
            List<DateTime> output = new List<DateTime>(nTicks);

            if (nTicks < 1) return output;
            output.Add(new DateTime(ticks[0]));
            if (nTicks == 1) return output;
            output.Add(new DateTime(ticks[1]));
            if (nTicks == 2) return output;
            output.Add(new DateTime(ticks[2]));

            long delta1, delta2;
            long count = 0;
            for(int t=3; t<nTicks; t++)
            {
                delta1 = ticks[t - 2] - ticks[t - 3];
                delta2 = ticks[t - 1] - ticks[t - 2];
                if(delta1==delta2)
                {
                    count = ticks[t-1];
                    for(int i=0; i <ticks[t]; i++)
                    {
                        count += delta1;
                        output.Add(new DateTime(count));
                    }
                }
                else
                {
                    output.Add(new DateTime(ticks[t]));
                }
            }

            return output;
        }
    }
}
