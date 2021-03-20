using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace RoadmapLogic
{
    public class Quarter
    {
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

        public static bool GetQuarters(string startDay, out List<Quarter> quarters)
        {
            quarters = new List<Quarter>();
            string format = "M/d/yyyy";
            if (DateTime.TryParseExact(startDay, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                int year = date.Year;
                int month = date.Month;
                int quarter;
                if (month < 4)
                {
                    quarter = 1;
                }
                else if (month < 7)
                {
                    quarter = 2;
                }
                else if (month < 10)
                {
                    quarter = 3;
                }
                else
                {
                    quarter = 4;
                }
                quarters.Add(new Quarter(year, quarter));

                for (int i = 1; i < 4; i++)
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

                return true;
            }
            else
            {
                return false;
            }
        }

        public static void DrawQuarters(Image<Rgba32> image, Settings settings, List<Quarter> quarters, Font chevronFont)
        {
            IPath chevronPath;
            Color[] chevronColors = {
                FugroColors.WhatColorIsThisBlue,
                FugroColors.StrataTurquoise,
                FugroColors.MotionGreen,
                FugroColors.CosmicSand
            };

            var chevronXStart = (float)settings.Margin.Left;

            for (int i = 0; i < 4; i++)
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
    }
}
