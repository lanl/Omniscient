using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    /// <summary>
    /// An interval of review is not a region of interest.
    /// </summary>
    class IntervalOfReview
    {
        public DateTimeRange TimeRange { get; set; }

        public IntervalOfReview(DateTime start, DateTime end)
        {
            TimeRange = new DateTimeRange(start, end);
        }
    }
}
