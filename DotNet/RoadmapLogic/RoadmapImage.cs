using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;
using System.IO;

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

                    List<Quarter> quarters = Quarter.GetQuarters(input.StartDate);

                    DrawLegsAndPlacards(input, settings, fonts, image, quarters);

                    Quarter.DrawQuarters(image, settings, quarters, fonts.QuarterFont);

                    image.DrawImage(
                        settings.FugroLogoQuantumBlueBase64,
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

        private static void DrawLegsAndPlacards(Input input, Settings settings, Fonts fonts, Image<Rgba32> image, List<Quarter> quarters)
        {
            const string missingProjectMessageText = "Alert! Project(s) have been omitted from the roadmap. Please Review.";
            RendererOptions renderOptions = new RendererOptions(fonts.PlacardFont);
            bool hasMissingProjects = false;
            
            var positions = Calculations.JulianDayToPixel(settings, quarters);
            var projects = Project.SortProjects(input.Projects, quarters[0]);
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

            if (hasMissingProjects)
            {
                image.DrawText(
                    missingProjectMessageText,
                    fonts.PlacardFont,
                    Color.Red,
                    new PointF(10F, settings.ImageHeight - TextMeasurer.Measure(missingProjectMessageText, renderOptions).Height));
            }
        }

        private static void DrawShortLine(Image<Rgba32> image, Settings settings)
        {
            image.DrawLine(
            FugroColors.QuantumBlue,
            3.3f,
            new PointF[] { new PointF(settings.Margin.Left - 20, settings.Margin.Top - 25),
                new PointF(settings.Margin.Left + 80, settings.Margin.Top - 25)});
        }
    }
}
