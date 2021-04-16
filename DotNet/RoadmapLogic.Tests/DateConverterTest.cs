using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace RoadmapLogic.Tests
{
    [TestClass]
    public class DateConverterTest
    {
        [TestMethod]
        public void TestConvertToDate()
        {
            bool proceed;
            DateTime date;

            string[] startDates = {
                "2021/09/05",
                "2021/09/5",
                "2021/9/05",
                "2021-09-05",
                "2021-09-5",
                "2021-9-05",
                "05 Sep 2021",
                "5 Sep 2021",
                "05-Sep-2021",
                "5-Sep-2021",
                "09/05/2021",
                "9/05/2021",
                "09/5/2021",
                "09-05-2021",
                "9-05-2021",
                "09-5-2021",};

            foreach (var startDate in startDates)
            {
                proceed =  DateConverter.ConvertToDate(startDate, out date);
                Assert.AreEqual(true, proceed);

                Assert.AreEqual("09/05/2021", date.ToString("MM/dd/yyyy", CultureInfo.CurrentCulture.DateTimeFormat));
            }

            proceed = DateConverter.ConvertToDate("Sep 5, 2021", out date);

            Assert.AreEqual(false, proceed);
            Assert.AreNotEqual("09/05/2021", date);

            proceed = DateConverter.ConvertToDate("Mca 4, 021", out date);

            Assert.AreEqual(false, proceed);
            Assert.AreNotEqual("09/05/2021", date);
        }
    }
}
