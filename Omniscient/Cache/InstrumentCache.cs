using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    class DayCache
    {
        public DateTime Date { get; set; }
        LinkedList<CacheLevelData> BaseData { get; set; }
        LinkedList<CacheLevelData> FiveSecondData { get; set; }
        LinkedList<CacheLevelData> OneMinuteData { get; set; }
        LinkedList<CacheLevelData> TenMinuteData { get; set; }
        LinkedList<CacheLevelData> TwoHourData { get; set; }
    }

    class InstrumentCache
    {
        public Instrument Instrument { get; private set; }
        private DataMonitor monitor;
        public long MemoryUsed { get; private set; }
        public long MemoryAllocation { get; set; }
        LinkedList<DayCache> Days { get; set; }

        public InstrumentCache(Instrument instrument)
        {
            this.Instrument = instrument;
            MemoryUsed = 0;

            monitor = new DataMonitor();
            monitor.DirectoryToMonitor = Instrument.GetDataFolder();
            monitor.ScanRecursively = Instrument.IncludeSubDirectories;
            monitor.LazyMode = true;
            monitor.FilePrefix = Instrument.GetFilePrefix();
            monitor.FileSuffix = Instrument.FileExtension;

            Days = new LinkedList<DayCache>();
        }
    }
}
