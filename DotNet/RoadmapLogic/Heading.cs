using SixLabors.ImageSharp;

namespace RoadmapLogic
{
    public class Heading
    {
        public Heading()
        {
            Title = "Product delivery roadmap";
            FontSize = 57;
            Height = 100;
            Color = FugroColors.QuantumBlue;
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
