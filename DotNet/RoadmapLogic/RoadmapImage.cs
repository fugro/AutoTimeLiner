using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RoadmapLogic
{
    public static class RoadmapImage
    {
        public static MemoryStream MakeImage(Input input, Settings settings)
        {
            var fonts = new Fonts(settings);
            
            if (input.IsValid)
            {
                var teamText = string.IsNullOrWhiteSpace(input.Team) ? "[No team supplied]" : input.Team;

                using (var image = new Image<Rgba32>(settings.ImageWidth, settings.ImageHeight))
                {
                    // Draw short line just above image title
                    DrawShortLine(image, settings);

                    image.DrawText(
                        settings.Heading.Title,
                        fonts.HeaderFont,
                        settings.Heading.Color,
                        new PointF(settings.Margin.Right, settings.Margin.Top + Calculations.TrimFont(settings.Heading.FontSize)));

                    image.DrawText(
                        teamText,
                        fonts.TeamFont,
                        Color.Black,
                        new PointF(settings.Margin.Right, settings.MidPoint - settings.PlotHeight - settings.TeamFontSize - 10));

                    var quarters = Quarter.GetQuarters(input.StartDate, input.Quarters);

                    DrawLegsAndPlacards(input, settings, fonts, image, quarters);

                    Quarter.DrawQuarters(image, settings, quarters, fonts.QuarterFont);

                    image.DrawImage(
                        settings.CopmanyLogo,
                        new Point(settings.ImageWidth - 224, settings.ImageHeight - 86),
                        1);

                    using (var ms = new MemoryStream())
                    {
                        image.SaveAsPng(ms);
                        return ms;
                    }
                }
            }
            else
            {
                using (var image = new Image<Rgba32>(settings.ImageWidth, settings.ImageHeight))
                {
                    image.DrawText(
                        "Failed! Provided Json is Not Valid!\nPlease review input and retry.",
                        fonts.HeaderFont,
                        settings.Heading.Color,
                        new PointF(settings.Margin.Right, settings.Margin.Top + Calculations.TrimFont(settings.Heading.FontSize)));

                    using (var ms = new MemoryStream())
                    {
                        image.SaveAsPng(ms);
                        return ms;
                    }
                }
            }
        }

        private static void DrawLegsAndPlacards(Input input, Settings settings, Fonts fonts, Image<Rgba32> image, IEnumerable<Quarter> quarters)
        {
            string missingProjectMessageText = "Alert! Project(s) have been omitted from the roadmap. Please Review.";
            RendererOptions renderOptions = new RendererOptions(fonts.PlacardFont);
            bool hasMissingProjects = false;

            try
            {
                if (!input.Projects.Any(p => quarters.Any(q => q.Equals(Quarter.GetQuarter(p.Date)))))
                {
                    missingProjectMessageText = $"Alert! No Project is within start date and '{quarters.Count()}' qaurters to report. Please Review.";
                    hasMissingProjects = true;
                    return;
                }

                var positions = Calculations.JulianDayToPixel(settings, quarters);
                var projects = Project.SortProjects(input.Projects, quarters.First());
                Position position = Position.Top;
                int index = 0;
                var placardEndPixel = new float[,]
                {
                { 0F, 0F, 0F },
                { 0F, 0F, 0F }
                };
                float temp = 0F;
                //bool addProject = true;
                int count = 0;
                Quarter quarter;
                foreach (var project in projects)
                {
                    quarter = Quarter.GetQuarter(project.Date);
                    if (!quarters.Any(q => q.Equals(quarter)))
                    {
                        hasMissingProjects = true;
                        continue;
                    }

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
                            if (placardWidth < TextMeasurer.Measure(item, renderOptions).Width)
                            {
                                placardWidth = TextMeasurer.Measure(item, renderOptions).Width;
                                temp = placardPoints[0].X + placardWidth;
                            }
                        }

                        if (temp > placardEndPixel[(int)position, index])
                        {
                            placardEndPixel[(int)position, index] = temp;
                        }

                        // Draw DogLeg
                        image.DrawLine(Color.Black, 3, new DogLeg(xPos, index, settings, position).Points);

                        if (temp > settings.ImageWidth)
                        {
                            placardPoints = new PlacardLocation(xPos, index, settings, position, true).Points;
                        }

                        if (count < 1)
                        {
                            // Draw Placard
                            Placard.Draw(image, project, placardPoints, fonts.PlacardFont, temp, TextMeasurer.Measure("|", renderOptions).Height * 3);
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
            }
            finally
            {
                if (hasMissingProjects)
                {
                    image.DrawText(
                        missingProjectMessageText,
                        fonts.PlacardFont,
                        Color.Red,
                        new PointF(10F, settings.ImageHeight - TextMeasurer.Measure(missingProjectMessageText, renderOptions).Height));
                }
            }
        }

        private static void DrawShortLine(Image<Rgba32> image, Settings settings)
        {
            image.DrawLine(
            DefaultColors.QuantumBlue,
            3.3f,
            new PointF[] { new PointF(settings.Margin.Left - 20, settings.Margin.Top - 25),
                new PointF(settings.Margin.Left + 80, settings.Margin.Top - 25)});
        }
    }
}
