﻿using System;
using System.Collections.Generic;

namespace RoadmapLogic
{
    public static class Calculations
    {
        public static int GetJulianDay(DateTime date)
        {
            return (int)(date - new DateTime(date.Year, 1, 1)).TotalDays + 1;
        }

        public static Dictionary<int, float> JulianDayToPixel(Settings settings, IEnumerable<Quarter> quarters)
        {
            float chevronXStart = settings.Margin.Left;
            var dict = new Dictionary<int, float>();
            var pixelIndex = chevronXStart;

            foreach (var quarter in quarters)
            {
                var quarterData = quarter.GetJulianDayRange();
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
    }
}
