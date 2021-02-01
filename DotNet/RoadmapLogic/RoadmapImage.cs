using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;

namespace RoadmapLogic
{
    public static class RoadmapImage
    {
        private enum FontWeight
        {
            Normal,
            Bold
        }

        public static MemoryStream MakeImage(Input input)
        {
            var normalFontFamily = GetFontFamily(FontWeight.Normal);
            var boldFontFamily = GetFontFamily(FontWeight.Bold);

            var headerFont = normalFontFamily.CreateFont(57);
            var teamFont = normalFontFamily.CreateFont(32);
            var chevronFont = boldFontFamily.CreateFont(36);

            var teamText = string.IsNullOrWhiteSpace(input.Team) ? "[No team supplied]" : input.Team;

            using (var image = new Image<Rgba32>(1486, 839))
            {
                image.Mutate(x => x.DrawText("Product delivery roadmap", headerFont, Color.Black, new PointF(100, 59) ));

                image.Mutate(x => x.DrawText(teamText, teamFont, Color.Black, new PointF(100, 200)));

                // Draw chevron/flag symbols
                int chevronXStart = 103;
                int chevronYStart = 460;
                int chevronLength = 310;
                int chevronHeight = 50;
                IPath chevronPath;
                Color[] chevronColors = {
                    Color.FromRgb(47, 68, 93),
                    Color.FromRgb(71, 156, 170),
                    Color.FromRgb(140, 182, 128),
                    Color.FromRgb(217, 190, 137)
                };

                for (int i = 0; i < 4; i++)
                {
                    chevronPath = BuildChevronSymbol(chevronXStart, chevronYStart, chevronLength, chevronHeight);
                    image.Mutate(x => x.Fill(chevronColors[i], chevronPath));
                    chevronXStart += chevronLength + 4;
                }

                image.Mutate(x => x.DrawText("2021 Q2", chevronFont, Color.White, new PointF(210, 464)));
                image.Mutate(x => x.DrawText("2021 Q3", chevronFont, Color.White, new PointF(520, 464)));
                image.Mutate(x => x.DrawText("2021 Q4", chevronFont, Color.White, new PointF(830, 464)));
                image.Mutate(x => x.DrawText("2022 Q1", chevronFont, Color.White, new PointF(1140, 464)));

                using (var ms = new MemoryStream())
                {
                    image.SaveAsPng(ms);
                    return ms;
                }
            }
        }

        private static FontFamily GetFontFamily(FontWeight weight)
        {
            var fontCollection = new FontCollection();

            var fontBase64 = weight == FontWeight.Bold
                ? FontsBase64.SegoeUiBoldBase64
                : FontsBase64.SegoeUiNormalBase64;

            var bytes = Convert.FromBase64String(fontBase64);

            using (var ms = new MemoryStream(bytes))
            {
                return fontCollection.Install(ms);
            }
        }

        private static IPath BuildChevronSymbol(int xStart, int yStart, int length, int height)
        {
            return new PathBuilder()
                .AddLine(new Point(xStart, yStart), new Point(xStart + length, yStart))
                .AddLine(new Point(xStart + length, yStart), new Point(xStart + length + 23, yStart + (height / 2)))
                .AddLine(new Point(xStart + length + 23, yStart + (height / 2)), new Point(xStart + length, yStart + height))
                .AddLine(new Point(xStart + length, yStart + height), new Point(100, yStart + height))
                .AddLine(new Point(xStart, yStart + height), new Point(xStart + 23, yStart + (height / 2)))
                .Build();
        }
    }
}
