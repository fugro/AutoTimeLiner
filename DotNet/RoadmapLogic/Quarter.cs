﻿using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoadmapLogic
{
    public class Quarter
    {
        const int QuartersDefault = 4;

        public int Year { get; }

        /// <summary>
        /// The number of this quarter from 1 to 4.
        /// </summary>
        public int Index { get; }

        public Quarter(int year, int index)
        {
            Year = year;
            Index = index;
        }

        public static IList<Quarter> GetQuarters(DateTime startDay, int? numberOfQuarters = QuartersDefault)
        {
            var numQuarters = QuartersDefault;
            if (numberOfQuarters.HasValue)
            {
                numQuarters = numberOfQuarters.Value;

                if (numQuarters < 1 || numQuarters > 4)
                {
                    numQuarters = QuartersDefault;
                }
            }
            
            List<Quarter> quarters = new List<Quarter>();
            int quarter;
            int year = startDay.Year;
            
            if (startDay.Month < 4)
            {
                quarter = 1;
            }
            else if (startDay.Month < 7)
            {
                quarter = 2;
            }
            else if (startDay.Month < 10)
            {
                quarter = 3;
            }
            else
            {
                quarter = 4;
            }

            quarters.Add(new Quarter(year, quarter));

            for (int i = 1; i < numQuarters; i++)
            {
                if (quarter == 4)
                {
                    year++;
                    quarter = 1;
                }
                else
                {
                    quarter++;
                }
                quarters.Add(new Quarter(year, quarter));
            }

            return quarters;
        }

        public static void DrawQuarters(Image<Rgba32> image, Settings settings, IEnumerable<Quarter> quarters, Font chevronFont)
        {
            IPath chevronPath;
            Color[] chevronColors = {
                DefaultColors.PulseBlue03,
                DefaultColors.StrataTurquoise,
                DefaultColors.MotionGreen,
                DefaultColors.CosmicSand
            };

            var chevronXStart = (float)settings.Margin.Left;

            for (int i = 0; i < quarters.Count(); i++)
            {
                chevronPath = BuildChevronSymbol(chevronXStart, settings.MidPoint - (settings.ChevronHeight / 2), settings);
                image.Mutate(x => x.Fill(chevronColors[i], chevronPath));
                chevronXStart += settings.ChevronLength + settings.ChevronGap;
            }

            float xOffset = 210;
            const float yOffset = 464;
            foreach (var quarter in quarters)
            {
                image.Mutate(x => x.DrawText(
                    quarter.ToString(),
                    chevronFont,
                    Color.White,
                    new PointF(xOffset, yOffset)));

                xOffset += settings.ChevronLength;
            }
        }

        public override string ToString()
        {
            return $"{Year} Q{Index}";
        }

        public Tuple<int, int> GetJulianDayRange()
        {
            var result = new Tuple<int, int>(-1, -1);
            switch (Index)
            {
                case 1:
                    result = new Tuple<int, int>(Calculations.GetJulianDay(new DateTime(Year, 1, 1)),
                        Calculations.GetJulianDay(new DateTime(Year, 3, 31)));
                    break;
                case 2:
                    result = new Tuple<int, int>(Calculations.GetJulianDay(new DateTime(Year, 4, 1)),
                        Calculations.GetJulianDay(new DateTime(Year, 6, 30)) - Calculations.GetJulianDay(new DateTime(Year, 3, 31)));
                    break;
                case 3:
                    result = new Tuple<int, int>(Calculations.GetJulianDay(new DateTime(Year, 7, 1)),
                        Calculations.GetJulianDay(new DateTime(Year, 9, 30)) - Calculations.GetJulianDay(new DateTime(Year, 6, 30)));
                    break;
                case 4:
                    result = new Tuple<int, int>(Calculations.GetJulianDay(new DateTime(Year, 10, 1)),
                        Calculations.GetJulianDay(new DateTime(Year, 12, 31)) - Calculations.GetJulianDay(new DateTime(Year, 9, 30)));
                    break;
            }
            return result;
        }

        private static IPath BuildChevronSymbol(float xStart, float yStart, Settings settings)
        {
            return new PathBuilder()
                .AddLine(new PointF(xStart, yStart), new PointF(xStart + settings.ChevronLength, yStart))
                .AddLine(new PointF(xStart + settings.ChevronLength, yStart),
                         new PointF(xStart + settings.ChevronLength + settings.ChevronOffset, yStart + (settings.ChevronHeight / 2)))
                .AddLine(new PointF(xStart + settings.ChevronLength + settings.ChevronOffset, yStart + (settings.ChevronHeight / 2)),
                         new PointF(xStart + settings.ChevronLength, yStart + settings.ChevronHeight))
                .AddLine(new PointF(xStart, yStart + settings.ChevronHeight),
                         new PointF(xStart + settings.ChevronOffset, yStart + (settings.ChevronHeight / 2)))
                .Build();
        }

        public override bool Equals(object other)
        {
            if (other is null)
            {
                return false;
            }

            if (other is Quarter q)
            {
                return Index == q.Index
                    && Year == q.Year;
            }

            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            return (Index, Year).GetHashCode();
        }
    }
}
