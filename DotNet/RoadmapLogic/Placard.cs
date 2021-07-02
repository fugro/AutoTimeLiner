using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace RoadmapLogic
{
    public static class Placard
    {
        /// <summary>
        /// Draw Placard text to image.
        /// </summary>
        public static void Draw(Image<Rgba32> image, Project project, PointF[] points, Font font, float width, float height)
        {
            var bg = new PathBuilder()
            .AddLine(new PointF(points[0].X, points[0].Y), new PointF(points[0].X + width, points[0].Y))
            .AddLine(new PointF(points[0].X + width, points[0].Y), new PointF(points[0].X + width, points[0].Y + height))
            .AddLine(new PointF(points[0].X + width, points[0].Y + height), new PointF(points[0].X, points[0].Y + height))
            .AddLine(new PointF(points[0].X, points[0].Y + height), new PointF(points[0].X, points[0].Y))
            .Build();
            image.Mutate(x => x.Fill(Color.White, bg));

            int index = 0;
            if (!string.IsNullOrWhiteSpace(project.Name))
            {
                DrawText(image, project.Name, points[index], font);
                index++;
            }

            if (!string.IsNullOrWhiteSpace(project.Label))
            {
                DrawText(image, project.Label, points[index], font);
                index++;
            }

            DrawText(image, project.Date.ToString("dd MMM yyyy"), points[index], font); // ToDo: Make available through settings?
        }

        private static void DrawText(Image<Rgba32> image, string text, PointF point, Font font)
        {
            image.Mutate(x => x.DrawText(
                        text,
                        font,
                        Color.Black,
                        point));
        }
    }
}
