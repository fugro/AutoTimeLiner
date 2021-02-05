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

            var width = 1486;
            var height = 839;

            using (var image = new Image<Rgba32>(1486, 839))
            {
                // Draw short line just above image title
                image.Mutate(x => x.DrawLines(FugroColors.QuantumBlue, 3.3f, new PointF(85, 45), new PointF(185, 45)));

                image.Mutate(x => x.DrawText("Product delivery roadmap", headerFont, FugroColors.QuantumBlue, new PointF(100, 59) ));

                image.Mutate(x => x.DrawText(teamText, teamFont, Color.Black, new PointF(100, 184)));

                // Draw chevron/flag symbols
                int chevronXStart = 103;
                int chevronYStart = 460;
                int chevronLength = 310;
                int chevronHeight = 50;
                IPath chevronPath;
                Color[] chevronColors = {
                    FugroColors.WhatColorIsThisBlue,
                    FugroColors.StrataTurquoise,
                    FugroColors.MotionGreen,
                    FugroColors.CosmicSand
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

                image.DrawImage(FontsBase64.FugroLogoQuantumBlueBase64, new Point(width - 224, height - 86), 1);

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
