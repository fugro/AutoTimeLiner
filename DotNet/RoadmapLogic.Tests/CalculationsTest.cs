using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RoadmapLogic.Tests
{
    [TestClass]
    public class CalculationsTest
    {
        [TestMethod]
        public void GetQuartersTest()
        {
            var quarters = Quarter.GetQuarters(new DateTime(2020, 7, 15));
            
            Assert.AreEqual(2020, quarters[0].Year);
            Assert.AreEqual(3, quarters[0].Index);
            Assert.AreEqual(2020, quarters[1].Year);
            Assert.AreEqual(4, quarters[1].Index);
            Assert.AreEqual(2021, quarters[2].Year);
            Assert.AreEqual(1, quarters[2].Index);
            Assert.AreEqual(2021, quarters[3].Year);
            Assert.AreEqual(2, quarters[3].Index);
        }

        [TestMethod]
        public void GetJulianDayTest()
        {
            Assert.AreEqual(31, Calculations.GetJulianDay(new DateTime(2020, 1, 31)));
            Assert.AreEqual(61, Calculations.GetJulianDay(new DateTime(2020, 3, 1)));
            Assert.AreEqual(60, Calculations.GetJulianDay(new DateTime(2021, 3, 1)));          
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Calculations.GetJulianDay(new DateTime(2020, 2, 31)));
            Assert.AreEqual(366, Calculations.GetJulianDay(new DateTime(2020, 12, 31)));
            Assert.AreEqual(365, Calculations.GetJulianDay(new DateTime(2021, 12, 31)));
        }

        [TestMethod]
        public void JulianDayToPixelTest()
        {
            Settings settings = new Settings(378, 366, new Margin(0, 0), 485, 50, 23, 4, 50, 32,
                                            new int[] { 217, 141, 65 }, new int[] { 176, 100, 24 }, 10, 20,
                                            12, 38, "Product delivery roadmap", 57, 100, DefaultColors.QuantumBlue,
                                            string.Empty, string.Empty, string.Empty);

            var quarters = Quarter.GetQuarters(new DateTime(2020, 3, 1));

            Assert.AreEqual(547, Calculations.JulianDayToPixel(settings, quarters).Values.Count);
            
            quarters = Quarter.GetQuarters(new DateTime(2021, 3, 1));
            
            Assert.AreEqual(546, Calculations.JulianDayToPixel(settings, quarters).Values.Count);
        }
    }
}