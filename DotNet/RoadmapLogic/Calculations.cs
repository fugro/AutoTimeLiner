using System;
using System.Collections.Generic;
using System.Globalization;

namespace RoadmapLogic
{
    public static class Calculations
    {
        public static int GetJulianDay(string date)
        {
            string format = "M/d/yyyy";
            if (DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime decodedDate))
            {
                return (int)(decodedDate - new DateTime(decodedDate.Year, 1, 1)).TotalDays + 1;
            }
            else
            {
                return -1;
            }
        }

        public static Dictionary<int, float> JulianDayToPixel(Settings settings, List<Quarter> quarters)
        {
            float chevronXStart = settings.Margin.Left;
            var dict = new Dictionary<int, float>();
            var pixelIndex = chevronXStart;

            foreach (var quarter in quarters)
            {
                var quarterData = QuarterData(quarter);
                var start = quarterData.Item1;
                float pixelInterval = settings.ChevronLength / (quarterData.Item2 - 1);
                for (int i = 0; i < quarterData.Item2; i++)
                {   
                    if (dict.Count == 363)
                    {
                        Console.WriteLine("break");
                    }
                    dict.Add(quarterData.Item1 + i, pixelIndex);
                    pixelIndex += pixelInterval;
                }
                chevronXStart = chevronXStart + settings.ChevronLength + settings.ChevronGap;
                pixelIndex = chevronXStart;
            }            

            return dict;
        }
        
        public static float TrimFont(float fontSize)
        {
            // Scale to position 0.169230769            
            return (fontSize - 5) * -0.169230769F;
        }

        private static Tuple<int, int> QuarterData(Quarter quarter)
        {
            var result = new Tuple<int, int>(-1, -1);
            switch (quarter.Index)
            {
                case 1:
                    result = new Tuple<int, int>(GetJulianDay($"1/1/{quarter.Year}"), 
                        GetJulianDay($"3/31/{quarter.Year}"));
                    break;
                case 2:
                    result = new Tuple<int, int>(GetJulianDay($"4/1/{quarter.Year}"), 
                        GetJulianDay($"6/30/{quarter.Year}") - GetJulianDay($"3/31/{quarter.Year}"));
                    break;
                case 3:
                    result = new Tuple<int, int>(GetJulianDay($"7/1/{quarter.Year}"), 
                        GetJulianDay($"9/30/{quarter.Year}") - GetJulianDay($"6/30/{quarter.Year}"));
                    break;
                case 4:
                    result = new Tuple<int, int>(GetJulianDay($"10/1/{quarter.Year}"), 
                        GetJulianDay($"12/31/{quarter.Year}") - GetJulianDay($"9/30/{quarter.Year}"));
                    break;
            }
            return result;
        }
    }
}
