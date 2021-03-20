using SixLabors.ImageSharp;

namespace RoadmapLogic
{
    public class Heading
    {
        public Heading(
            string title,
            int fontSize,
            int height,
            Color color)
        {
            Title = title;
            FontSize = fontSize;
            Height = height;
            Color = color;
        }

        public string Title
        {
            get;
        }

        public float FontSize 
        { 
            get;
        }

        public int Height
        {
            get;
        }

        public Color Color 
        { 
            get; 
        }
    }
}
