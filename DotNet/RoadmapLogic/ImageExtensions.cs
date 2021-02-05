using SixLabors.ImageSharp;
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
    }
}
