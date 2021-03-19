using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RoadmapLogic
{
    public static class RoadmapImage
    {
        public static MemoryStream MakeImage(Input input, Settings settings)
        {
            const string MissingProjectMessageText = "Alert! Project(s) have been omitted from the roadmap. Please Review.";
            var normalFontFamily = GetFontFamily(FontWeight.Normal);
            var boldFontFamily = GetFontFamily(FontWeight.Bold);

            var headerFont = normalFontFamily.CreateFont(settings.Heading.FontSize);
            var teamFont = normalFontFamily.CreateFont(settings.FontSize);
            var chevronFont = boldFontFamily.CreateFont(36);
            var placardFont = normalFontFamily.CreateFont(16);
            RendererOptions rendererOptions = new RendererOptions(placardFont);

            var teamText = string.IsNullOrWhiteSpace(input.Team) ? "[No team supplied]" : input.Team;

            using (var image = new Image<Rgba32>(settings.ImageWidth, settings.ImageHeight))
            {
                // Draw short line just above image title
                image.Mutate(x => x.DrawLines(
                    FugroColors.QuantumBlue,
                    3.3f,
                    new PointF(settings.Margin.Left - 20, settings.Margin.Top - 25),
                    new PointF(settings.Margin.Left + 80, settings.Margin.Top - 25)));

                image.Mutate(x => x.DrawText(
                    settings.Heading.Title,
                    headerFont,
                    settings.Heading.Color,
                    new PointF(settings.Margin.Right, settings.Margin.Top + Calculations.TrimFont(settings.Heading.FontSize))));
                
                image.Mutate(x => x.DrawText(
                    teamText,
                    teamFont,
                    Color.Black,
                    new PointF(settings.Margin.Right, settings.MidPoint - settings.PlotHeight - settings.FontSize - 10)));

                bool hasMissingProjects = false;

                if (Quarter.GetQuarters(input.StartDate, out List<Quarter> quarters))
                {
                    var positions = Calculations.JulianDayToPixel(settings, quarters);
                    var projects = SortProjects(input.Projects, quarters[0]);
                    Position position = Position.Top;                    
                    int index = 0;
                    var placardEndPixel = new float[,] 
                    { 
                        { 0F, 0F, 0F }, 
                        { 0F, 0F, 0F } 
                    };
                    
                    float temp = 0F;
                    int count = 0;
                    
                    foreach (var project in projects)
                    {
                        if (positions.TryGetValue(Calculations.GetJulianDay(project.Date), out float xPos))
                        {
                            if (index == 0)
                            {
                                int i = index;
                                for (; i < 2; i++)
                                {
                                    if (xPos > placardEndPixel[(int)position, i])
                                    {
                                        index = i;
                                        break;
                                    }
                                }
                                if (i == 2)
                                {
                                    hasMissingProjects = true;
                                    continue;
                                }
                            }

                            var placardPoints = new PlacardLocation(xPos, index, settings, position).Points;
                            float placardWidth = 0;

                            foreach (var item in project.ToList())
                            {
                                if (placardWidth < TextMeasurer.Measure(item, rendererOptions).Width)
                                {
                                    placardWidth = TextMeasurer.Measure(item, rendererOptions).Width;
                                    temp = placardPoints[0].X + placardWidth;
                                }
                            }

                            if (temp > placardEndPixel[(int)position, index])
                            {
                                placardEndPixel[(int)position, index] = temp;
                            }

                            // Draw DogLeg
                            image.Mutate(x => x.DrawLines(Color.Black, 3, new DogLeg(xPos, index, settings, position).Points));

                            if (temp > settings.ImageWidth)
                            {
                                placardPoints = new PlacardLocation(xPos, index, settings, position, true).Points;
                            }

                            if (count < 1)
                            {
                                // Draw Placard
                                Placard.Draw(image, project, placardPoints, placardFont, temp, TextMeasurer.Measure("|", rendererOptions).Height * 3);
                            }

                            if (index == 2)
                            {
                                if (placardEndPixel[(int)position, 1] < placardEndPixel[(int)position, index])
                                {
                                    placardEndPixel[(int)position, 1] = placardEndPixel[(int)position, index];
                                }

                                if (placardEndPixel[(int)position, 0] < placardEndPixel[(int)position, index])
                                {
                                    placardEndPixel[(int)position, 0] = placardEndPixel[(int)position, index];
                                }

                                if (position == Position.Top)
                                {
                                    position = Position.Bottom;
                                }
                                else
                                {
                                    position = Position.Top;
                                }

                                index = 0;
                            }
                            else
                            {
                                index++;
                            }
                        }
                    }
                    
                    DrawQuarters(image, settings, quarters, chevronFont);

                    if (hasMissingProjects)
                    {
                        image.Mutate(x => x.DrawText(
                        MissingProjectMessageText,
                        placardFont, 
                        Color.Red, 
                        new PointF(10F, settings.ImageHeight - TextMeasurer.Measure(MissingProjectMessageText, rendererOptions).Height)));
                    }

                    image.DrawImage(
                        FontsBase64.FugroLogoQuantumBlueBase64,
                        new Point(settings.ImageWidth - 224, settings.ImageHeight - 86),
                        1);
                }

                using (var ms = new MemoryStream())
                {
                    image.SaveAsPng(ms);
                    return ms;
                }
            }
        }

        private static void DrawQuarters(Image<Rgba32> image, Settings settings, List<Quarter> quarters, Font chevronFont)
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

        private static List<Project> SortProjects(IEnumerable<Project> projects, Quarter startQuarter)
        {
            projects = projects.OrderBy(p => Calculations.GetJulianDay(p.Date)).ToList();

            int startJulianDay = 1;
            switch (startQuarter.Index)
            {
                case 2:
                    startJulianDay = Calculations.GetJulianDay($"04/01/{startQuarter.Year}");
                    break;
                case 3:
                    startJulianDay = Calculations.GetJulianDay($"07/01/{startQuarter.Year}");
                    break;
                case 4:
                    startJulianDay = Calculations.GetJulianDay($"10/1/{startQuarter.Year}");
                    break;
            }

            var list = new List<Project>();
            var list2 = new List<Project>();
            
            foreach (var project in projects)
            {
                if (Calculations.GetJulianDay(project.Date) >= startJulianDay)
                {
                    list.Add(project);
                }
                else
                {
                    list2.Add(project);
                }
            }

            list.AddRange(list2);            
            return list;
        }

        private static FontFamily GetFontFamily(FontWeight weight)
        {
            var fontCollection = new FontCollection();

            string fontBase64 = weight == FontWeight.Bold
                ? FontsBase64.SegoeUiBoldBase64
                : FontsBase64.SegoeUiNormalBase64;

            var bytes = Convert.FromBase64String(fontBase64);

            using (var ms = new MemoryStream(bytes))
            {
                return fontCollection.Install(ms);
            }
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
