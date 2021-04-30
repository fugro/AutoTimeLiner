using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace RoadmapLogic.Tests
{
    [TestClass]
    public class QuarterTests
    {
        [TestMethod]
        public void CreatesFourQuartersByDefault()
        {
            var startTime = new DateTime(2021, 1, 1);
            var result = Quarter.GetQuarters(startTime);

            Assert.AreEqual(4, result.Count());
            Assert.AreEqual(new Quarter(2021, 1), result[0]);
            Assert.AreEqual(new Quarter(2021, 2), result[1]);
            Assert.AreEqual(new Quarter(2021, 3), result[2]);
            Assert.AreEqual(new Quarter(2021, 4), result[3]);
        }

        [TestMethod]
        public void CreatesTwoQuarters()
        {
            var startTime = new DateTime(2021, 1, 1);
            var result = Quarter.GetQuarters(startTime, 2);

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(new Quarter(2021, 1), result[0]);
            Assert.AreEqual(new Quarter(2021, 2), result[1]);
        }

        [TestMethod]
        public void CreatesThreeQuartersBeginningInQ4()
        {
            var startTime = new DateTime(2021, 10, 1);
            var result = Quarter.GetQuarters(startTime, 3);

            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(new Quarter(2021, 4), result[0]);
            Assert.AreEqual(new Quarter(2022, 1), result[1]);
            Assert.AreEqual(new Quarter(2022, 2), result[2]);
        }
        
        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(5)]
        public void CreatesFourQuartersGivenBadQuarterInput(int quarters)
        {
            var startTime = new DateTime(2021, 10, 1);
            var result = Quarter.GetQuarters(startTime, quarters);

            Assert.AreEqual(4, result.Count());
        }


        [TestMethod]
        public void JulianDayRangeForQ1()
        {
            var julianDays = new Quarter(2021, 1).GetJulianDayRange();

            Assert.AreEqual(1, julianDays.Item1);
            Assert.AreEqual(31 + 28 + 31, julianDays.Item2);
        }
    }
}
