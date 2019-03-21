using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class OmniscientCore
    {
        /// <summary>
        /// Version of Omniscient
        /// </summary>
        public const string VERSION = "0.3.4";

        public event EventHandler ViewChanged;

        /// <summary>
        /// Contains all of the Sites in the instance of Omniscient
        /// </summary>
        public SiteManager SiteManager { get; set; }

        /// <summary>
        /// Contains all activated Instruments
        /// </summary>
        public List<Instrument> ActiveInstruments { get; private set; }

        /// <summary>
        /// The earliest time off any data that is active
        /// </summary>
        public DateTime GlobalStart { get; set; }
        
        /// <summary>
        /// The latest time off any data that is active
        /// </summary>
        public DateTime GlobalEnd { get; set; }

        private DateTime _viewStart;
        /// <summary>
        /// The start time of the current view - i.e. data being dsiplayed.
        /// </summary>
        public DateTime ViewStart
        {
            get
            {
                return _viewStart;
            }
            set
            {
                ChangeView(value, ViewEnd);
            }
        }

        private DateTime _viewEnd;
        /// <summary>
        /// The end time of the current view - i.e. data being dsiplayed.
        /// </summary>
        public DateTime ViewEnd
        {
            get
            {
                return _viewEnd;
            }
            set
            {
                ChangeView(ViewStart, value);
            }
        }

        /// <summary>
        /// If any errors occur, a message is put in ErrorMessage
        /// </summary>
        public string ErrorMessage { get; private set; }

        private bool changingView;

        /// <summary>
        /// Constructor
        /// </summary>
        public OmniscientCore()
        {
            ErrorMessage = "";
            changingView = false;
            SiteManager = new SiteManager("SiteManager.xml", VERSION);
            ReturnCode returnCode = SiteManager.Reload();
            if (returnCode == ReturnCode.FILE_DOESNT_EXIST)
            {
                SiteManager.WriteBlank();
            }
            else if (returnCode != ReturnCode.SUCCESS)
            {
                ErrorMessage = "Warning: Bad trouble loading the site manager!";
            }

            ActiveInstruments = new List<Instrument>();

            GlobalStart = DateTime.Today.AddDays(-1);
            GlobalEnd = DateTime.Today;
            ChangeView(GlobalStart, GlobalEnd);
        }

        public void ActivateInstrument(Instrument instrument)
        {
            ActiveInstruments.Add(instrument);
            instrument.LoadData(new DateTime(1900, 1, 1), new DateTime(2100, 1, 1));
            UpdateGlobalStartEnd();
        }

        public void DeactivateInstrument(Instrument instrument)
        {
            ActiveInstruments.Remove(instrument);
            instrument.ClearData();
            UpdateGlobalStartEnd();
        }

        public void ShiftView(TimeSpan shift)
        {
            if (shift == TimeSpan.Zero) return;
            ChangeView(ViewStart + shift, ViewEnd + shift, true);
        }

        /// <summary>
        /// Change the time range of data to be dsiplayed.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void ChangeView(DateTime start, DateTime end, bool holdRange=false)
        {
            if (!changingView)
            {
                changingView = true;
                if (start > end) throw new DatesOutOfOrderException();
                bool newStart = start != ViewStart;
                bool newEnd = end != ViewEnd;

                // Keep start in global range
                if (newStart)
                {
                    if (start < GlobalStart)
                    {
                        start = GlobalStart;
                    }
                    else if (start > GlobalEnd)
                    {
                        start = GlobalEnd;
                    }
                    newStart = start != ViewStart;
                    _viewStart = start;
                }

                // Keep end in global range
                if (newEnd)
                {
                    if (end < GlobalStart)
                    {
                        end = GlobalStart;
                    }
                    else if (end > GlobalEnd)
                    {
                        end = GlobalEnd;
                    }
                    newEnd = end != ViewEnd;
                    _viewEnd = end;
                }

                if (newStart || newEnd)
                {
                    if (!holdRange)
                    {
                        // Try to align the view to be sensible
                        bool reRanged = false;

                        long startT = _viewStart.Ticks;
                        long endT = _viewEnd.Ticks;
                        long range = endT - startT;
                        long newRange;


                        if (range > 6 * TimeSpan.TicksPerDay)
                        {
                            newRange = (range / TimeSpan.TicksPerDay) * TimeSpan.TicksPerDay;
                            startT = startT - (startT % TimeSpan.TicksPerDay);
                            if (startT < GlobalStart.Ticks) startT = GlobalStart.Ticks;

                            endT = startT + newRange;
                            if (endT <= GlobalEnd.Ticks)
                            {
                                reRanged = true;
                            }
                            else reRanged = false;
                        }
                        if (!reRanged && range > 6 * TimeSpan.TicksPerHour)
                        {
                            newRange = (range / TimeSpan.TicksPerHour) * TimeSpan.TicksPerHour;
                            startT = startT - (startT % TimeSpan.TicksPerHour);
                            if (startT < GlobalStart.Ticks) startT = GlobalStart.Ticks;
                            endT = startT + newRange;
                            if (endT <= GlobalEnd.Ticks)
                            {
                                reRanged = true;
                            }
                            else reRanged = false;
                        }
                        if (!reRanged && range > 6 * TimeSpan.TicksPerMinute)
                        {
                            newRange = (range / TimeSpan.TicksPerMinute) * TimeSpan.TicksPerMinute;
                            startT = startT - (startT % TimeSpan.TicksPerMinute);
                            if (startT < GlobalStart.Ticks) startT = GlobalStart.Ticks;
                            endT = startT + newRange;
                            if (endT <= GlobalEnd.Ticks)
                            {
                                reRanged = true;
                            }
                            else reRanged = false;
                        }
                        if (reRanged)
                        {
                            _viewStart = new DateTime(startT);
                            _viewEnd = new DateTime(endT);
                        }
                    }
                    // Invoke event handlers (i.e. update UI)
                    ViewChanged?.Invoke(this, EventArgs.Empty);
                }
                changingView = false;
            }
        }

        /// <summary>
        /// Updates the global start and stop based on the active instruments.
        /// </summary>
        private void UpdateGlobalStartEnd()
        {
            DateTime earliest = new DateTime(3000, 1, 1);
            DateTime latest = new DateTime(1000, 1, 1);
            DateTime chStart;
            DateTime chEnd;

            if (ActiveInstruments.Count == 0) return;

            // Figure out the earliest and latest data point
            foreach (Instrument inst in ActiveInstruments)
            {
                foreach (Channel ch in inst.GetChannels())
                {
                    if (ch.GetTimeStamps().Count > 0)
                    {
                        chStart = ch.GetTimeStamps()[0];
                        chEnd = ch.GetTimeStamps()[ch.GetTimeStamps().Count - 1];
                        if (ch.GetChannelType() == Channel.ChannelType.DURATION_VALUE)
                            chEnd += ch.GetDurations()[ch.GetDurations().Count - 1];
                        if (chStart < earliest)
                            earliest = chStart;
                        if (chEnd > latest)
                            latest = chEnd;
                    }
                }
            }

            if (earliest > latest) return;

            // Update global start and end
            GlobalStart = earliest;
            GlobalEnd = latest;

            DateTime tempViewStart = ViewStart;
            DateTime tempViewEnd = ViewEnd;

            bool changedRange = false;
            if (tempViewStart < GlobalStart || tempViewStart > GlobalEnd)
            {
                tempViewStart = GlobalStart;
                changedRange = true;
            }
            if (tempViewEnd > GlobalEnd || tempViewEnd < GlobalStart)
            {
                tempViewEnd = GlobalEnd;
                changedRange = true;
            }
            if (changedRange)
            {
                if (TimeSpan.FromTicks(tempViewEnd.Ticks - tempViewStart.Ticks).TotalDays > 1)
                {
                    tempViewStart = tempViewEnd.AddDays(-1);
                }
                else if (TimeSpan.FromTicks(tempViewEnd.Ticks - tempViewStart.Ticks).TotalHours > 1)
                {
                    tempViewStart = tempViewEnd.AddHours(-1);
                }
                else if (TimeSpan.FromTicks(tempViewEnd.Ticks - tempViewStart.Ticks).TotalMinutes > 1)
                {
                    tempViewStart = tempViewEnd.AddMinutes(-1);
                }
                else if (TimeSpan.FromTicks(tempViewEnd.Ticks - tempViewStart.Ticks).TotalSeconds > 1)
                {
                    tempViewStart = tempViewEnd.AddSeconds(-1);
                }
                ChangeView(tempViewStart, tempViewEnd);
            }
        }
    }

    public class DatesOutOfOrderException : Exception
    {

    }
}
