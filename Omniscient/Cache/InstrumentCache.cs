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
        public DataMonitor Monitor;
        public List<FileScan> Scans;
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

            Monitor = new DataMonitor();
            Monitor.FileListFile = cacheDirectory + "\\FileListFile.csv";
            Scans = new List<FileScan>();

            Days = new LinkedList<DayCache>();
        }
    }

    public class InstrumentCacheScanTask : CacheTask
    {
        InstrumentCache Cache;

        public InstrumentCacheScanTask(InstrumentCache cache) : base()
        {
            Cache = cache;
        }

        public override void RunTask()
        {
            Cache.Monitor.DirectoryToMonitor = Cache.Instrument.GetDataFolder();
            Cache.Monitor.ScanRecursively = Cache.Instrument.IncludeSubDirectories;
            Cache.Monitor.LazyMode = true;
            Cache.Monitor.FilePrefix = Cache.Instrument.GetFilePrefix();
            Cache.Monitor.FileSuffix = Cache.Instrument.FileExtension;
            Cache.Monitor.LoadFileListFile();
            Cache.Scans = Cache.Monitor.Scan();
            State = CacheTaskState.Complete;
        }
    }
}
