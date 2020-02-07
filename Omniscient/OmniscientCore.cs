// This software is open source software available under the BSD-3 license.
// 
// Copyright (c) 2018, Triad National Security, LLC
// All rights reserved.
// 
// Copyright 2018. Triad National Security, LLC. This software was produced under U.S. Government contract 89233218CNA000001 for Los Alamos National Laboratory (LANL), which is operated by Triad National Security, LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  NEITHER THE GOVERNMENT NOR TRIAD NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.  If software is modified to produce derivative works, such modified software should be clearly marked, so as not to confuse it with the version available from LANL.
// 
// Additionally, redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 1.       Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
// 2.       Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 
// 3.       Neither the name of Triad National Security, LLC, Los Alamos National Laboratory, LANL, the U.S. Government, nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 
//  
// THIS SOFTWARE IS PROVIDED BY TRIAD NATIONAL SECURITY, LLC AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL TRIAD NATIONAL SECURITY, LLC OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
﻿using System;
using System.Collections.Generic;
using System.IO;
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
        public static readonly string VERSION = "0.4.8i";

        public event EventHandler ViewChanged;

        CacheManager Cache;

        /// <summary>
        /// Contains all of the Sites in the instance of Omniscient
        /// </summary>
        public SiteManager SiteManager { get; set; }

        /// <summary>
        /// Contains all activated Instruments
        /// </summary>
        public List<Instrument> ActiveInstruments { get; private set; }

        /// <summary>
        /// Contains all activated EventGenerators
        /// </summary>
        public List<EventGenerator> ActiveEventGenerators { get; private set; }


        public List<Event> Events { get; private set; }

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

        private string appDataDirectory;

        /// <summary>
        /// Constructor
        /// </summary>
        public OmniscientCore(string directory)
        {
            appDataDirectory = directory;
            string siteManagerDirectory = Path.Combine(appDataDirectory, "SiteManager.xml");

            ErrorMessage = "";
            changingView = false;
            SiteManager = new SiteManager(siteManagerDirectory, VERSION);
            ReturnCode returnCode = SiteManager.Reload();
            if (returnCode == ReturnCode.FILE_DOESNT_EXIST)
            {
                SiteManager.WriteBlank();
            }
            else if (returnCode != ReturnCode.SUCCESS)
            {
                ErrorMessage = "Warning: Bad trouble loading the site manager!";
            }

            Cache = new CacheManager(appDataDirectory);
            //Cache.Start();

            ActiveInstruments = new List<Instrument>();
            ActiveEventGenerators = new List<EventGenerator>();
            Events = new List<Event>();

            GlobalStart = DateTime.Today.AddDays(-1);
            GlobalEnd = DateTime.Today;
            ChangeView(GlobalStart, GlobalEnd);
        }

        public void ActivateInstrument(Instrument instrument)
        {
            ActiveInstruments.Add(instrument);
            instrument.ScanDataFolder();
            //instrument.LoadData(ChannelCompartment.View, new DateTime(1900, 1, 1), new DateTime(2100, 1, 1));
            Cache.AddInstrumentCache(instrument.Cache);
            UpdateGlobalStartEnd();
            instrument.LoadData(ChannelCompartment.View, _viewStart, _viewEnd);
        }

        public void DeactivateInstrument(Instrument instrument)
        {
            ActiveInstruments.Remove(instrument);
            instrument.ClearData(ChannelCompartment.View);
            Cache.RemoveInstrumentCache(instrument.Cache);
            UpdateGlobalStartEnd();
        }

        /// <summary>
        /// Adds an EventGenerator to the list of ActiveEventGenerators
        /// </summary>
        /// <param name="eventGenerator"></param>
        public void ActivateEventGenerator(EventGenerator eventGenerator)
        {
            ActiveEventGenerators.Add(eventGenerator);
        }

        /// <summary>
        /// Removes an EventGenerator from the list of ActiveEventGenerators
        /// </summary>
        /// <param name="eventGenerator"></param>
        public void DeactivateEventGenerator(EventGenerator eventGenerator)
        {
            ActiveEventGenerators.Remove(eventGenerator);
        }

        /// <summary>
        /// Generate events with active event generators
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void GenerateEvents(DateTime start, DateTime end)
        {
            Events = new List<Event>();

            foreach (EventGenerator eg in ActiveEventGenerators)
            {
                Events.AddRange(eg.GenerateEvents(start, end));
                eg.RunActions();
            }
            Events.Sort((x, y) => x.StartTime.CompareTo(y.StartTime));
        }

        /// <summary>
        /// Change ViewStart and ViewEnd by an equal amount
        /// </summary>
        /// <param name="shift"></param>
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
                    
                    foreach (Instrument instrument in ActiveInstruments)
                    {
                        instrument.LoadData(ChannelCompartment.View, _viewStart, _viewEnd);
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
            DateTime ABSOLUTE_EARLIST = new DateTime(3000, 1, 1);
            DateTime earliest = ABSOLUTE_EARLIST;
            DateTime ABSOLUTE_LATEST = new DateTime(1000, 1, 1);
            DateTime latest = ABSOLUTE_LATEST;
            DateTime chStart;
            DateTime chEnd;

            if (ActiveInstruments.Count == 0) return;

            // Figure out the earliest and latest data point
            foreach (Instrument inst in ActiveInstruments)
            {
                /*
                foreach (Channel ch in inst.GetChannels())
                {
                    if (ch.GetTimeStamps(ChannelCompartment.View).Count > 0)
                    {
                        chStart = ch.GetTimeStamps(ChannelCompartment.View)[0];
                        chEnd = ch.GetTimeStamps(ChannelCompartment.View)[ch.GetTimeStamps(ChannelCompartment.View).Count - 1];
                        if (ch.GetChannelType() == Channel.ChannelType.DURATION_VALUE)
                            chEnd += ch.GetDurations(ChannelCompartment.View)[ch.GetDurations(ChannelCompartment.View).Count - 1];
                        if (chStart < earliest)
                            earliest = chStart;
                        if (chEnd > latest)
                            latest = chEnd;
                    }
                }
                */
                DateTimeRange range = inst.Cache.GetDataFilesTimeRange();
                if (earliest > DateTime.MinValue && range.Start < earliest) earliest = range.Start;
                if (latest < DateTime.MaxValue && range.End > latest) latest = range.End;
            }

            if (earliest > latest) return;
            if (earliest == ABSOLUTE_EARLIST || latest == ABSOLUTE_LATEST) return;

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

        public void Shutdown()
        {
            //Cache.Stop();
        }
    }

    public class DatesOutOfOrderException : Exception
    {

    }
}
