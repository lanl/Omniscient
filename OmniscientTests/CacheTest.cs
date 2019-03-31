using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omniscient;

namespace OmniscientTests
{
    [TestClass]
    public class CacheTest
    {
        [TestMethod]
        public void DateCompactWorks()
        {
            List<DateTime> times = new List<DateTime>()
            {
                new DateTime(91),
                new DateTime(91),
                new DateTime(91),
                new DateTime(91),
                new DateTime(90),
                new DateTime(91),
                new DateTime(90),
                new DateTime(92),
                new DateTime(93),
                new DateTime(94),
                new DateTime(95),
                new DateTime(96),
                new DateTime(97),
                new DateTime(96),
                new DateTime(95),
                new DateTime(96),
            };

            List<long> cTimes = IOUtility.DateCompact(times);

            Assert.AreEqual(cTimes[0], 91);
            Assert.AreEqual(cTimes[1], 91);
            Assert.AreEqual(cTimes[2], 91);
            Assert.AreEqual(cTimes[3], 1);
            Assert.AreEqual(cTimes[4], 90);
            Assert.AreEqual(cTimes[5], 91);
            Assert.AreEqual(cTimes[6], 90);
            Assert.AreEqual(cTimes[7], 92);
            Assert.AreEqual(cTimes[8], 93);
            Assert.AreEqual(cTimes[9], 94);
            Assert.AreEqual(cTimes[10], 3);
            Assert.AreEqual(cTimes[11], 96);
            Assert.AreEqual(cTimes[12], 95);
            Assert.AreEqual(cTimes[13], 96);
        }

        [TestMethod]
        public void DateUnpackWorks()
        {
            List<long> ticks = new List<long>()
            {
                91,91,91,1,90,91,90,92,93,94,3,96,95,96
            };

            List<DateTime> times = IOUtility.DateUnpack(ticks);

            Assert.AreEqual(times[0].Ticks, 91);
            Assert.AreEqual(times[1].Ticks, 91);
            Assert.AreEqual(times[2].Ticks, 91);
            Assert.AreEqual(times[3].Ticks, 91);
            Assert.AreEqual(times[4].Ticks, 90);
            Assert.AreEqual(times[5].Ticks, 91);
            Assert.AreEqual(times[6].Ticks, 90);
            Assert.AreEqual(times[7].Ticks, 92);
            Assert.AreEqual(times[8].Ticks, 93);
            Assert.AreEqual(times[9].Ticks, 94);
            Assert.AreEqual(times[10].Ticks, 95);
            Assert.AreEqual(times[11].Ticks, 96);
            Assert.AreEqual(times[12].Ticks, 97);
            Assert.AreEqual(times[13].Ticks, 96);
            Assert.AreEqual(times[14].Ticks, 95);
            Assert.AreEqual(times[15].Ticks, 96);
        }
    }
}
