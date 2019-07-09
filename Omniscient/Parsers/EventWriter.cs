using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class EventWriter
    {
        private const int VERSION = 1;
        private const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";
        public ReturnCode WriteEventFile(string fileName, List<Event> events)
        {
            // Open file for writing
            StreamWriter writeStream;
            try { writeStream = new StreamWriter(fileName); }
            catch { return ReturnCode.COULD_NOT_OPEN_FILE; }

            // Write header
            writeStream.WriteLine("Event List,Version," + VERSION.ToString());
            writeStream.WriteLine("Event Start,Event End,Duration,Max Value,Max Time,Comments");

            // Write event content
            foreach(Event eve in events)
            {
                writeStream.Write(eve.StartTime.ToString(DATE_TIME_FORMAT) + ",");
                writeStream.Write(eve.EndTime.ToString(DATE_TIME_FORMAT) + ",");
                writeStream.Write((eve.EndTime - eve.StartTime).TotalSeconds.ToString() + ",");
                writeStream.Write(eve.MaxValue.ToString() + ",");
                writeStream.Write(eve.MaxTime.ToString(DATE_TIME_FORMAT) + ",");
                writeStream.Write(eve.Comment + "\n");
            }

            // Close it out
            writeStream.Flush();
            writeStream.Close();

            return ReturnCode.SUCCESS;
        }
    }
}
