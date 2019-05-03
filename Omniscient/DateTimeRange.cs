using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class DateTimeRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public DateTimeRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public bool InRange(DateTime time)
        {
            if (time >= Start && time <= End) return true;
            return false;
        }

        public List<DateTime> DaysInRange()
        {
            List<DateTime> dates = new List<DateTime>();

            DateTime day = new DateTime(Start.Year, Start.Month, Start.Day);

            while(day < End)
            {
                dates.Add(day);
                day = day.AddDays(1);
            }

            return dates;
        }
    }
}
