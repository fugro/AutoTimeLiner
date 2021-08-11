using System.Collections.Generic;

namespace RoadmapLogic
{
    public sealed class CustomColors
    {
        public CustomColors(IList<int> lineColor, IList<int> headingColor, IList<int> teamColor, QuarterColors quarterColors)
        {
            LineColor = lineColor;
            HeadingColor = headingColor;
            TeamColor = teamColor;
            QuarterColors = quarterColors;
        }

        public IList<int> LineColor
        {
            get;
            set;
        }

        public IList<int> HeadingColor 
        { 
            get; 
            set; 
        }

        public IList<int> TeamColor 
        { 
            get;
            set;
        }

        public QuarterColors QuarterColors 
        {
            get;
            set;
        }
    }
}
