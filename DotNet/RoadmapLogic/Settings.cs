namespace RoadmapLogic
{
    public class Settings
    {
        /// <summary>
        /// Settings for the Setup of the Generated image.
        /// </summary>
        /// <param name="imageWidth"></param>
        /// <param name="imageHeight"></param>
        /// <param name="margin"></param>
        /// <param name="midpoint"></param>
        /// <param name="chevronHeight"></param>
        /// <param name="chevronOffset"></param>
        /// <param name="chevronGap"></param>
        /// <param name="dogLegWidth"></param>
        /// <param name="fontSize"></param>        
        public Settings(
            int imageWidth,
            int imageHeight,
            Margin margin,
            int midpoint,
            int chevronHeight,
            int chevronOffset,
            int chevronGap,
            float dogLegWidth,
            int fontSize,
            int[] topOffsets,
            int[] bottomOffsets,
            int placardXSpacing,
            int placardYSpacing)
        {
            Heading = new Heading();
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;
            Margin = margin;
            MidPoint = midpoint;
            ChevronHeight = chevronHeight;
            ChevronOffset = chevronOffset;
            ChevronGap = chevronGap;
            ChevronLength = ((ImageWidth - Margin.Left - Margin.Right - ChevronOffset) - (3 * ChevronGap)) / 4F;
            DogLegWidth = dogLegWidth;
            FontSize = fontSize;
            PlotHeight = ImageHeight - Margin.Bottom - MidPoint - (ChevronHeight / 2);
            TopOffsets = topOffsets;
            BottomOffsets = bottomOffsets;
            PlacardXSpacing = placardXSpacing;
            PlacardYSpacing = placardYSpacing;
        }

        public static Settings Default => new Settings(1486, 839, new Margin(103, 70), 485, 50, 25, 4, 30, 32,
            new int[] { 217, 141, 65 }, new int[] { 176, 100, 24 }, 10, 20);

        /// <summary>
        /// Plot Border (X, Y)
        /// </summary>
        public Margin Margin
        { 
            get; 
        }

        public int MidPoint
        {
            get;
        }

        public int ChevronHeight
        {
            get;
        }

        public int ChevronOffset
        {
            get;
        }

        public int ImageWidth 
        { 
            get; 
        }

        public int ImageHeight
        {
            get;
        }

        public int ChevronGap
        {
            get;
        }

        public float ChevronLength
        {
            get;
        }

        public float DogLegWidth 
        { 
            get; 
        }

        public Heading Heading
        {
            get;
        }

        public int FontSize
        {
            get;
        }

        public float PlotHeight
        {
            get;
        }

        public int[] TopOffsets
        {
            get;
        }

        public int[] BottomOffsets
        {
            get;
        }

        public int PlacardXSpacing
        {
            get;
        }

        public int PlacardYSpacing
        {
            get;
        }
    }
}
