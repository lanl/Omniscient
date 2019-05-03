using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omniscient;

namespace OmniscientTests
{
    /// <summary>
    /// Summary description for DateTimeRangeTests
    /// </summary>
    [TestClass]
    public class DateTimeRangeTests
    {
        public DateTimeRangeTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void InRageMakesSense()
        {
            DateTimeRange range = new DateTimeRange(new DateTime(2019, 5, 3, 12, 1, 1), new DateTime(2019, 5, 3, 14, 1, 1));
            Assert.IsTrue(range.InRange(new DateTime(2019, 5, 3, 12, 1, 1)));
            Assert.IsTrue(range.InRange(new DateTime(2019, 5, 3, 14, 1, 1)));
            Assert.IsTrue(range.InRange(new DateTime(2019, 5, 3, 13, 1, 1)));
            Assert.IsFalse(range.InRange(new DateTime(2019, 5, 3, 14, 1, 2)));
            Assert.IsFalse(range.InRange(new DateTime(2019, 5, 3, 11, 59, 59)));
            Assert.IsFalse(range.InRange(DateTime.MinValue));
            Assert.IsFalse(range.InRange(DateTime.MaxValue));
        }

        [TestMethod]
        public void DaysInRangeMakesSenseForOneDay()
        {
            DateTimeRange range = new DateTimeRange(new DateTime(2019, 5, 3, 12, 1, 1), new DateTime(2019, 5, 3, 14, 1, 1));
            List<DateTime> days = range.DaysInRange();

            Assert.AreEqual(days.Count, 1);
            Assert.AreEqual(days[0].Ticks, (new DateTime(2019, 5, 3)).Ticks);
        }

        [TestMethod]
        public void DaysInRangeMakesSenseForMultipleDays()
        {
            DateTimeRange range = new DateTimeRange(new DateTime(2019, 5, 1, 12, 1, 1), new DateTime(2019, 5, 3, 14, 1, 1));
            List<DateTime> days = range.DaysInRange();

            Assert.AreEqual(days.Count, 3);
            Assert.AreEqual(days[0].Ticks, (new DateTime(2019, 5, 1)).Ticks);
            Assert.AreEqual(days[1].Ticks, (new DateTime(2019, 5, 2)).Ticks);
            Assert.AreEqual(days[2].Ticks, (new DateTime(2019, 5, 3)).Ticks);
        }

        [TestMethod]
        public void DaysBeforeMakesSense()
        {
            DateTimeRange range = new DateTimeRange(new DateTime(2019, 5, 1, 12, 1, 1), new DateTime(2019, 5, 7, 14, 1, 1));
            List<DateTime> days = range.DaysBefore(new DateTime(2019, 5, 3));
            Assert.AreEqual(days.Count, 2);
            Assert.AreEqual(days[0].Ticks, (new DateTime(2019, 5, 1)).Ticks);
            Assert.AreEqual(days[1].Ticks, (new DateTime(2019, 5, 2)).Ticks);

            range = new DateTimeRange(new DateTime(2019, 5, 4, 12, 1, 1), new DateTime(2019, 5, 7, 14, 1, 1));
            days = range.DaysBefore(new DateTime(2019, 5, 3));
            Assert.AreEqual(days.Count, 0);

            range = new DateTimeRange(new DateTime(2019, 5, 3), new DateTime(2019, 5, 7, 14, 1, 1));
            days = range.DaysBefore(new DateTime(2019, 5, 3));
            Assert.AreEqual(days.Count, 0);
        }

        [TestMethod]
        public void DaysAfterMakesSense()
        {
            DateTimeRange range = new DateTimeRange(new DateTime(2019, 5, 1, 12, 1, 1), new DateTime(2019, 5, 7, 14, 1, 1));
            List<DateTime> days = range.DaysAfter(new DateTime(2019, 5, 3));
            Assert.AreEqual(days.Count, 4);
            Assert.AreEqual(days[0].Ticks, (new DateTime(2019, 5, 4)).Ticks);
            Assert.AreEqual(days[1].Ticks, (new DateTime(2019, 5, 5)).Ticks);
            Assert.AreEqual(days[2].Ticks, (new DateTime(2019, 5, 6)).Ticks);
            Assert.AreEqual(days[3].Ticks, (new DateTime(2019, 5, 7)).Ticks);

            range = new DateTimeRange(new DateTime(2019, 5, 4, 12, 1, 1), new DateTime(2019, 5, 7, 14, 1, 1));
            days = range.DaysAfter(new DateTime(2019, 5, 8));
            Assert.AreEqual(days.Count, 0);

            range = new DateTimeRange(new DateTime(2019, 5, 3), new DateTime(2019, 5, 7));
            days = range.DaysAfter(new DateTime(2019, 5, 7));
            Assert.AreEqual(days.Count, 0);
        }
    }
}
