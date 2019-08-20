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
using System.Threading;
using System.Threading.Tasks;

namespace Omniscient
{
    public class DayCache
    {
        public DateTime Date { get; private set; }
        public ChannelCacheLevelData[] BaseData { get; set; }
        public ChannelCacheLevelData[] FiveSecondData { get; set; }
        public ChannelCacheLevelData[] OneMinuteData { get; set; }
        public ChannelCacheLevelData[] TenMinuteData { get; set; }
        public ChannelCacheLevelData[] TwoHourData { get; set; }

        public DayCache(DateTime date)
        {
            Date = date;
        }

        /// <summary>
        /// Returns the approximate memory usage of the BaseData in bytes
        /// </summary>
        /// <returns></returns>
        public int GetBaseDataUsage()
        {
            int bytes = 0;
            foreach (ChannelCacheLevelData cacheData in BaseData)
            {
                bytes += cacheData.GetMemoryUsage();
            }
            return bytes;
        }
    }

    public class InstrumentCache
    {
        private const int BASE_DAY_ESTIMATE_BYTES = 250_000_000;

        public List<DataFile> DataFiles { get; private set; }
        public Instrument Instrument { get; private set; }
        public DataMonitor TenMinuteMonitor;
        public List<FileScan> TenMinuteScans;
        public long MemoryUsed { get; private set; }
        public long MemoryAllocation { get; set; }
        LinkedList<DayCache> Days { get; set; }

        private string cacheDirectory;

        public InstrumentCache(Instrument instrument)
        {
            this.Instrument = instrument;
            MemoryUsed = 0;
            MemoryAllocation = 1_250_000_000;

            cacheDirectory = "Cache\\" + Instrument.ID.ToString("X8");

            if (!Directory.Exists("Cache")) Directory.CreateDirectory("Cache");
            if (!Directory.Exists(cacheDirectory)) Directory.CreateDirectory(cacheDirectory);

            TenMinuteMonitor = new DataMonitor();
            TenMinuteMonitor.FileListFile = cacheDirectory + "\\FileListFile.csv";
            TenMinuteScans = new List<FileScan>();

            DataFiles = new List<DataFile>();

            Days = new LinkedList<DayCache>();
        }

        public void SetDataFiles(string[] fileNames, DateTime[] times)
        {
            DataFiles = new List<DataFile>(fileNames.Length);
            for (int i = 0; i < fileNames.Length - 1; i++)
            {
                DataFiles.Add(new DataFile(fileNames[i], times[i]));
            }
            if (fileNames.Length > 0)
            {
                Instrument.ClearData(ChannelCompartment.Cache);
                int fileIndex = fileNames.Length - 1;
                Instrument.IngestFile(ChannelCompartment.Cache, fileNames[fileIndex]);
                Instrument.LoadVirtualChannels(ChannelCompartment.Cache);
                Channel[] channels = Instrument.GetChannels();
                DateTime lastTime = DateTime.MinValue;
                foreach (Channel chan in channels)
                {
                    if (chan.GetTimeStamps(ChannelCompartment.Cache).Last() > lastTime)
                    {
                        lastTime = chan.GetTimeStamps(ChannelCompartment.Cache).Last();
                    }
                }
                Instrument.ClearData(ChannelCompartment.Cache);
                DataFiles.Add(new DataFile(fileNames[fileIndex], times[fileIndex], lastTime));
            }
        }

        public DateTimeRange GetDataFilesTimeRange()
        {
            if (DataFiles.Count > 0)
                return new DateTimeRange(DataFiles[0].DataStart, DataFiles.Last().DataEnd);
            else
                return new DateTimeRange(DateTime.MinValue, DateTime.MinValue);
        }

        public void LoadDataIntoInstrument(ChannelCompartment compartment, DateTimeRange timeRange)
        {
            // Buffer around the time range
            timeRange.Start = timeRange.Start.AddDays(-1);
            timeRange.End = timeRange.End.AddDays(1);

            // Make sure needed data is in cache
            if (Days.Count == 0)
            {
                // No data - start fresh
                LoadFreshRange(timeRange);
            }
            else
            {
                DateTimeRange loadedRange = new DateTimeRange(Days.First.Value.Date, Days.Last.Value.Date);
                if (!loadedRange.CompletelyInRange(timeRange))
                {
                    if (timeRange.End < loadedRange.Start.AddDays(-1) || timeRange.Start > loadedRange.End.AddDays(1))
                    {
                        // No overlap - start fresh
                        LoadFreshRange(timeRange);
                    }
                    else
                    {
                        // The following two conditions are not mutually exclusive
                        if (timeRange.Start < loadedRange.Start)
                        {
                            List<DateTime> days = timeRange.DaysBefore(loadedRange.Start);
                            PreventativeMemoryManagement(days, timeRange);
                            AddDaysToFront(days);
                        }
                        if (timeRange.End > loadedRange.End)
                        {
                            List<DateTime> days = timeRange.DaysAfter(loadedRange.End);
                            PreventativeMemoryManagement(days, timeRange);
                            AddDaysToEnd(days);
                        }
                        ReactiveMemoryManagement(timeRange);
                    }
                }
            }

            // Copy data into instrument
            Instrument.ClearData(compartment);

            DateTimeRange dayRange = new DateTimeRange(
                new DateTime(timeRange.Start.Year, timeRange.Start.Month, timeRange.Start.Day),
                new DateTime(timeRange.End.Year, timeRange.End.Month, timeRange.End.Day));

            Channel[] channels = Instrument.GetChannels();
            foreach (DayCache dayCache in Days)
            {
                if (dayCache.Date >= dayRange.Start && dayCache.Date <= dayRange.End)
                {
                    for (int ch = 0; ch < Instrument.GetChannels().Length; ch++)
                    {
                        dayCache.BaseData[ch].ExportToChannel(channels[ch], compartment);
                    }
                }
            }
        }

        private void ReactiveMemoryManagement(DateTimeRange timeRange)
        {
            DateTimeRange loadedRange = new DateTimeRange(Days.First.Value.Date, Days.Last.Value.Date);
            TimeSpan startDiff;
            TimeSpan endDiff;

            while (MemoryUsed > MemoryAllocation)
            {
                startDiff = Days.First.Value.Date - timeRange.Start.Date;
                endDiff = timeRange.End.Date - Days.Last.Value.Date;

                if (startDiff == TimeSpan.Zero && endDiff == TimeSpan.Zero)
                {
                    // No solution
                    return;
                }
                if (endDiff > startDiff)
                {
                    UnloadDay(false);
                }
                else
                {
                    UnloadDay(true);
                }
            }
        }

        private void PreventativeMemoryManagement(List<DateTime> newDays, DateTimeRange timeRange)
        {
            if (newDays.Count * BASE_DAY_ESTIMATE_BYTES + MemoryUsed < MemoryAllocation)
            {
                // No problem
                return;
            }
            DateTimeRange loadedRange = new DateTimeRange(Days.First.Value.Date, Days.Last.Value.Date);
            if (timeRange.Start.Date <= loadedRange.Start && timeRange.End.Date >= loadedRange.End)
            {
                // No solution
                return;
            }

            if (newDays[0] < loadedRange.Start)
            {
                // Take off the end
                while (Days.Last.Value.Date > timeRange.End.Date)
                {
                    UnloadDay(false);
                    if (newDays.Count * BASE_DAY_ESTIMATE_BYTES + MemoryUsed < MemoryAllocation)
                    {
                        // No problem
                        break;
                    }
                }
            }
            else
            {
                // Take off the front
                while (Days.First.Value.Date < timeRange.Start.Date)
                {
                    UnloadDay(true);
                    if (newDays.Count * BASE_DAY_ESTIMATE_BYTES + MemoryUsed < MemoryAllocation)
                    {
                        // No problem
                        break;
                    }
                }
            }

        }

        public void LoadFreshRange(DateTimeRange timeRange)
        {
            UnloadAllDays();
            foreach (DateTime day in timeRange.DaysInRange())
            {
                Days.AddLast(LoadDay(day));
            }
        }

        public void AddDaysToFront(List<DateTime> days)
        {
            for(int i=days.Count-1; i>=0; i--)
            {
                Days.AddFirst(LoadDay(days[i]));
            }
        }

        public void AddDaysToEnd(List<DateTime> days)
        {
            foreach (DateTime day in days)
            {
                Days.AddLast(LoadDay(day));
            }
        }

        public DayCache LoadDay(DateTime day)
        {
            DayCache dayCache = new DayCache(day);

            Instrument.ClearData(ChannelCompartment.Cache);
            foreach (DataFile file in DataFiles)
            {
                if (file.DataStart >= day && file.DataStart < day.AddDays(1))
                {
                    Instrument.IngestFile(ChannelCompartment.Cache, file.FileName);
                }
            }
            Instrument.LoadVirtualChannels(ChannelCompartment.Cache);

            Channel[] channels = Instrument.GetChannels();
            dayCache.BaseData = new ChannelCacheLevelData[channels.Length];
            for (int ch = 0; ch < channels.Length; ch++)
            {
                dayCache.BaseData[ch] = new ChannelCacheLevelData(CacheLevel.Base);
                dayCache.BaseData[ch].ImportFromChannel(channels[ch], ChannelCompartment.Cache);
            }
            Instrument.ClearData(ChannelCompartment.Cache);

            MemoryUsed += dayCache.GetBaseDataUsage();

            return dayCache;
        }

        public void UnloadAllDays()
        {
            while(Days.Count > 0)
            {
                UnloadDay(true);
            }
        }

        public void UnloadDay(bool frontEnd)
        {
            if (Days.Count == 0) return;

            if (frontEnd)
            {
                MemoryUsed -= Days.First.Value.GetBaseDataUsage();
                Days.RemoveFirst();
            }
            else
            {
                MemoryUsed -= Days.Last.Value.GetBaseDataUsage();
                Days.RemoveLast();
            }
        }
    }

    public class InstrumentCacheScanTask : CacheTask
    {
        InstrumentCache Cache;

        public InstrumentCacheScanTask(CacheManager manager, InstrumentCache cache) : base(manager)
        {
            Cache = cache;
        }

        public override void RunTask()
        {
            if (Cache.Instrument.GetChannels()[0].GetChannelType() != Channel.ChannelType.COUNT_RATE) return;
            Cache.TenMinuteMonitor.DirectoryToMonitor = Cache.Instrument.GetDataFolder();
            Cache.TenMinuteMonitor.ScanRecursively = Cache.Instrument.IncludeSubDirectories;
            Cache.TenMinuteMonitor.LazyMode = true;
            Cache.TenMinuteMonitor.FilePrefix = Cache.Instrument.GetFilePrefix();
            Cache.TenMinuteMonitor.FileSuffix = Cache.Instrument.FileExtension;
            Cache.TenMinuteMonitor.LoadFileListFile();
            Cache.TenMinuteScans = Cache.TenMinuteMonitor.Scan();
            foreach(FileScan scan in Cache.TenMinuteScans)
            {
                Manager.AddNonurgentTask(new GenerateFileCache(Manager, Cache, scan));
            }
            State = CacheTaskState.Complete;
        }
    }

    public class GenerateFileCache : CacheTask
    {
        FileScan Scan;
        InstrumentCache ICache;

        public GenerateFileCache(CacheManager manager, InstrumentCache instrumentCache, FileScan scan) : base(manager)
        {
            ICache = instrumentCache;
            Scan = scan;
        }
        public override void RunTask()
        {

            State = CacheTaskState.Complete;
        }
    }
}
