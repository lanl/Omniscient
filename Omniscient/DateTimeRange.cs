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

        public bool CompletelyInRange(DateTimeRange range)
        {
            if (range.Start >= Start && range.Start <= End && range.End >= Start && range.End <= End) return true;
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

        public List<DateTime> DaysBefore(DateTime date)
        {
            List<DateTime> dates = new List<DateTime>();

            if (Start >= date) return dates;

            DateTime day = new DateTime(Start.Year, Start.Month, Start.Day);
            while (day < date)
            {
                dates.Add(day);
                day = day.AddDays(1);
            }

            return dates;
        }

        public List<DateTime> DaysAfter(DateTime date)
        {
            List<DateTime> dates = new List<DateTime>();
            if (End <= date) return dates;

            DateTime day = new DateTime(date.Year, date.Month, date.Day).AddDays(1);
            while (day < End)
            {
                dates.Add(day);
                day = day.AddDays(1);
            }

            return dates;
        }
    }
}
