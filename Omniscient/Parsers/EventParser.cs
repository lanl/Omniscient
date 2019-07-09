using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class EventParser
    {
        public int Version { get; private set; }
        public List<DateTime> StartTime { get; private set; }
        public List<DateTime> EndTime { get; private set; }
        public List<double> MaxValue { get; private set; }
        public List<DateTime> MaxTime { get; private set; }
        public List<string> Comments { get; private set; }

        public EventParser()
        {
            Version = 0;
            StartTime = new List<DateTime>();
            EndTime = new List<DateTime>();
            MaxValue = new List<double>();
            MaxTime = new List<DateTime>();
            Comments = new List<string>();
        }

        public int GetNumRecords()
        {
            return StartTime.Count;
        }

        public ReturnCode ParseFile(string fileName)
        {
            // Try reading the file into lines
            string[] lines;
            try
            {
                lines = IOUtility.PermissiveReadAllLines(fileName);
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }
            if (lines.Length < 2) return ReturnCode.CORRUPTED_FILE;

            // Read the first line
            string[] tokens;
            tokens = lines[0].Split(',');
            if (tokens[0].ToLower() != "event list") return ReturnCode.CORRUPTED_FILE;
            if (tokens[1].ToLower() != "version") return ReturnCode.CORRUPTED_FILE;
            int version;
            if (!int.TryParse(tokens[2], out version)) return ReturnCode.CORRUPTED_FILE;
            Version = version;

            // Read the column headers
            int eventStartCol = 0;
            int eventEndCol = 1;
            int durationCol = 2;
            int maxValCol = 3;
            int maxTimeCol = 4;
            int commentsCol = 5;
            int nColumns = 5;
            tokens = lines[1].Split(',');
            for (int col = 0; col < tokens.Length; col++)
            {
                switch(tokens[col].ToLower())
                {
                    case "event start":
                        eventStartCol = col;
                        if (col > nColumns) nColumns = col;
                        break;
                    case "event end":
                        eventEndCol = col;
                        if (col > nColumns) nColumns = col;
                        break;
                    case "duration":
                        durationCol = col;
                        if (col > nColumns) nColumns = col;
                        break;
                    case "max value":
                        maxValCol = col;
                        if (col > nColumns) nColumns = col;
                        break;
                    case "max time":
                        maxTimeCol = col;
                        if (col > nColumns) nColumns = col;
                        break;
                    case "comments":
                        commentsCol = col;
                        if (col > nColumns) nColumns = col;
                        break;
                }
            }
            nColumns++;

            // Read event content
            DateTime start;
            DateTime end;
            for (int l = 2; l <lines.Length; ++l)
            {
                tokens = lines[l].Split(',');
                if (tokens.Length < nColumns) return ReturnCode.CORRUPTED_FILE;
                start = DateTime.Parse(tokens[eventStartCol]);
                end = DateTime.Parse(tokens[eventEndCol]);
                StartTime.Add(start);
                EndTime.Add(end);
                MaxValue.Add(double.Parse(tokens[maxValCol]));
                MaxTime.Add(DateTime.Parse(tokens[maxTimeCol]));
                Comments.Add(tokens[commentsCol]);
            }

            return ReturnCode.SUCCESS;
        }
    }
}
