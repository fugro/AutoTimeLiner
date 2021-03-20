using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using System;

namespace RoadmapLogic
{
    public static class ImageExtensions
    {
        public static void DrawImage(
            this Image image,
            string base64ImageToDraw,
            Point location,
            float opacity)
        {
            var bytes = Convert.FromBase64String(base64ImageToDraw);

            using (var imageToDraw = Image.Load(bytes))
            {
                image.Mutate(x => x.DrawImage(imageToDraw, location, opacity));
            }
        }

        public static void DrawText(
            this Image image,
            string text,
            Font font,
            Color color,
            PointF location
            )
        {
            image.Mutate(x => x.DrawText(
                    text,
                    font,
                    color,
                    location));
        }

        public static void DrawLine(
            this Image image,
            Color color,
            float thickness,
            PointF[] points
            )
        {
            image.Mutate(x => x.DrawLines(
                color,
                thickness,
                points));
        }
    }
}
