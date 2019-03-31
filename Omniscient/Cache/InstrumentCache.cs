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
        public DateTime Date { get; set; }
        LinkedList<CacheLevelData> BaseData { get; set; }
        LinkedList<CacheLevelData> FiveSecondData { get; set; }
        LinkedList<CacheLevelData> OneMinuteData { get; set; }
        LinkedList<CacheLevelData> TenMinuteData { get; set; }
        LinkedList<CacheLevelData> TwoHourData { get; set; }
    }

    public class InstrumentCache
    {
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

            Days = new LinkedList<DayCache>();
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
