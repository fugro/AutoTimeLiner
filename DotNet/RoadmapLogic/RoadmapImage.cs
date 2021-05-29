using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RoadmapLogic
{
    public static class RoadmapImage
    {
        private const string  s_MissingProjectMessage = "Alert! Projects have been omitted from the roadmap. Please Review.";
        private const string  s_BeforeStartQuarterMessage = "Projects before start quarter were omitted.";
        private const string  s_AfterLastQuarterMessage = "Projects after last quarter were omitted.";
        private const string s_OutsideQuarterRangeMessage = "Projects before start and after last quarter were omitted.";

        private const string s_MissingProjectKey = "MissingProject";
        private const string s_BeforeStartQuarterKey = "BeforeStartQuarter";
        private const string s_AfterLastQuarterKey = "AfterLastQuarter";

        public static MemoryStream MakeImage(Input input, Settings settings)
        {
            var quarters = Quarter.GetQuarters(input.StartDate, input.Quarters);
            // Reduce the font size as number of quarters increase to fit more projects in the image.
            settings.PlacardFontSize = settings.PlacardFontSize - (quarters.Count - 1);
            var fonts = new Fonts(settings);

            // Increase quarter width as number of quarters decrease
            settings.ChevronLength = (settings.ImageWidth - settings.Margin.Left - settings.Margin.Right - settings.ChevronOffset - 
                                        ((quarters.Count - 1) * settings.ChevronGap)) / quarters.Count;

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
            var noProjectWithinQuartersMessage = string.Empty;

            RendererOptions renderOptions = new RendererOptions(fonts.PlacardFont);
            var missingProjectsMessage = string.Empty;
            var beforeStartQuarterMessage = string.Empty;
            var afterLastQuarterMessage = string.Empty;

            try
            {
                if (!input.Projects.Any(p => quarters.Any(q => q.Equals(Quarter.GetQuarter(p.Date)))))
                {
                    noProjectWithinQuartersMessage = $"Alert! No Project is within start date and '{quarters.Count()}' quarters to report. Please Review.";
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

                int count = 0;
                Quarter quarter;
                foreach (var project in projects)
                {
                    quarter = Quarter.GetQuarter(project.Date);
                    if (!quarters.Any(q => q.Equals(quarter)) && quarters.Count() == 4)
                    {
                        if (DateTime.Compare(project.Date, input.StartDate) < 0)
                        {
                            beforeStartQuarterMessage = s_BeforeStartQuarterMessage;
                        }
                        else
                        {
                            afterLastQuarterMessage = s_AfterLastQuarterMessage;
                        }
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
                                missingProjectsMessage = s_MissingProjectMessage;
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
                var messages = new Dictionary<string, string> { { s_MissingProjectKey, missingProjectsMessage }, { s_BeforeStartQuarterKey, beforeStartQuarterMessage }, { s_AfterLastQuarterKey, afterLastQuarterMessage } };
                var message = BuildMessage(messages);
                var messagToWrite =  string.IsNullOrWhiteSpace(noProjectWithinQuartersMessage) ? message : $"{noProjectWithinQuartersMessage} {message}";
                if (!string.IsNullOrWhiteSpace(messagToWrite))
                {
                    image.DrawText(
                        messagToWrite,
                        fonts.PlacardFont,
                        Color.Red,
                        new PointF(10F, settings.ImageHeight - TextMeasurer.Measure(messagToWrite, renderOptions).Height));
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

        private static string BuildMessage(IDictionary<string, string> messages)
        {
            var message = messages[s_MissingProjectKey];

            if (!string.IsNullOrWhiteSpace(messages[s_BeforeStartQuarterKey]) && !string.IsNullOrWhiteSpace(messages[s_AfterLastQuarterKey]))
            {
                message =  string.IsNullOrWhiteSpace(message) ? s_OutsideQuarterRangeMessage : $"{message} {s_OutsideQuarterRangeMessage}";
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(messages[s_BeforeStartQuarterKey]))
                {
                    message = string.IsNullOrWhiteSpace(message) ? messages[s_BeforeStartQuarterKey] : $"{message} {messages[s_BeforeStartQuarterKey]}";
                }

                if (!string.IsNullOrWhiteSpace(messages[s_AfterLastQuarterKey]))
                {
                    message = string.IsNullOrWhiteSpace(message) ? messages[s_AfterLastQuarterKey] : $"{message} {messages[s_AfterLastQuarterKey]}";
                }
            }

            return message;
        }
    }
}
