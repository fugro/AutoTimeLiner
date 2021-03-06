using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace RoadmapLogic
{
    public static class Placard
    {
        public static void Draw(Image<Rgba32> image, Project project, PointF[] points, Font font)
        {
            int index = 0;
            if (!string.IsNullOrWhiteSpace(project.Name))
            {
                DrawText(image, project.Name, points[index], font);
                index++;
            }

            if(!string.IsNullOrWhiteSpace(project.Label))
            {
                DrawText(image, project.Label, points[index], font);
                index++;
            }

            if (!string.IsNullOrWhiteSpace(project.Date))
            {
                DrawText(image, project.Date, points[index], font);
            }
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
