using System;
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
    }

    public class InstrumentCache
    {
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
            for (int i=0; i<fileNames.Length-1; i++)
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
                foreach(Channel chan in channels)
                {
                    if (chan.GetTimeStamps(ChannelCompartment.Cache).Last() > lastTime)
                    {
                        lastTime = chan.GetTimeStamps(ChannelCompartment.Cache).Last();
                    }
                }
                DataFiles.Add(new DataFile(fileNames[fileIndex], times[fileIndex], lastTime));
            }
        }

        public DateTimeRange GetDataFilesTimeRange()
        {
            return new DateTimeRange(DataFiles[0].DataStart, DataFiles.Last().DataEnd);
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
                if(!timeRange.CompletelyInRange(loadedRange))
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
                            AddDaysToFront(days);
                        }
                        if (timeRange.End < loadedRange.End)
                        {
                            List<DateTime> days = timeRange.DaysAfter(loadedRange.End);
                            AddDaysToEnd(days);
                        }
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
                if(dayCache.Date >= dayRange.Start && dayCache.Date <= dayRange.End)
                {
                    for (int ch = 0; ch < Instrument.GetNumChannels(); ch++)
                    {
                        dayCache.BaseData[ch].ExportToChannel(channels[ch], compartment);
                    }
                }
            }
        }

        public void LoadFreshRange(DateTimeRange timeRange)
        {
            Days.Clear();
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

            dayCache.BaseData = new ChannelCacheLevelData[Instrument.GetNumChannels()];
            Channel[] channels = Instrument.GetChannels();
            for (int ch = 0; ch <Instrument.GetNumChannels(); ch++)
            {
                dayCache.BaseData[ch] = new ChannelCacheLevelData();
                dayCache.BaseData[ch].ImportFromChannel(channels[ch], ChannelCompartment.Cache);
            }
            Instrument.ClearData(ChannelCompartment.Cache);
            return dayCache;
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
