using System.Collections.Generic;
using SixLabors.ImageSharp;

namespace RoadmapLogic
{
    public class ColorSettings
    {
        public ColorSettings(Color lineColor, Color headingColor, Color teamColor, IDictionary<string, Color> quarterColors)
        {
            LineColor = lineColor;
            HeadingColor = headingColor;
            TeamColor = teamColor;
            QuarterColors = quarterColors;
        }

        public static ColorSettings Default => new ColorSettings(DefaultColors.QuantumBlue, DefaultColors.QuantumBlue, Color.Black, GetDefaultQuarterColors());

        public Color LineColor
        {
            get;
        }

        public Color HeadingColor
        {
            get;
        }

        public Color TeamColor
        {
            get;
        }

        public IDictionary<string, Color> QuarterColors
        {
            get;
        }

        private static IDictionary<string, Color> GetDefaultQuarterColors()
        {
            return new Dictionary<string, Color>()
            {
                { "Quarter1", DefaultColors.PulseBlue03 },
                { "Quarter2", DefaultColors.StrataTurquoise },
                { "Quarter3", DefaultColors.MotionGreen },
                { "Quarter4", DefaultColors.CosmicSand },
                { "Quarter5", DefaultColors.PulseBlue },
                { "Quarter6", DefaultColors.MintGreen },
            };
        }
    }
}
