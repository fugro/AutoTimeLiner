using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace RoadmapLogic.Tests
{
    [TestClass]
    public class CalculationsTest
    {
        [TestMethod]
        public void GetQuartersTest()
        {
            Assert.IsFalse(Quarter.GetQuarters("13/5/2020", out List<Quarter> _));

            if (Quarter.GetQuarters("7/15/2020", out List<Quarter> quarters))
            {
                Assert.AreEqual(2020, quarters[0].Year);
                Assert.AreEqual(3, quarters[0].Index);
                Assert.AreEqual(2020, quarters[1].Year);
                Assert.AreEqual(4, quarters[1].Index);
                Assert.AreEqual(2021, quarters[2].Year);
                Assert.AreEqual(1, quarters[2].Index);
                Assert.AreEqual(2021, quarters[3].Year);
                Assert.AreEqual(2, quarters[3].Index);
            }
        }

        [TestMethod]
        public void GetJulianDayTest()
        {
            Assert.AreEqual(31, Calculations.GetJulianDay("1/31/2020"));
            Assert.AreEqual(61, Calculations.GetJulianDay("3/1/2020"));
            Assert.AreEqual(60, Calculations.GetJulianDay("3/1/2021"));            
            Assert.AreEqual(-1, Calculations.GetJulianDay("2/31/2020"));
            Assert.AreEqual(365, Calculations.GetJulianDay("12/31/2021"));
        }

        [TestMethod]
        public void JulianDayToPixelTest()
        {
            Settings settings = new Settings(378, 366, new Margin(0, 0), 485, 50, 23, 4, 50, 32,
                                            new int[] { 217, 141, 65 }, new int[] { 176, 100, 24 }, 10, 20);

            if (Quarter.GetQuarters("3/1/2020", out List<Quarter> quarters))
            {
                Assert.AreEqual(366, Calculations.JulianDayToPixel(settings, quarters).Values.Count);
            }

            if (Quarter.GetQuarters("3/1/2021", out quarters))
            {
                Assert.AreEqual(365, Calculations.JulianDayToPixel(settings, quarters).Values.Count);
            }

        }
    }
}