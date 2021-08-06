using SixLabors.ImageSharp;

namespace RoadmapLogic
{
    public class Settings
    {
        private static readonly string s_SegoeUiNormalBase64 = new SegoeUiNormal().Base64;
        private static readonly string s_SegoeUiBoldBase64 = new SegoeUiBold().Base64;
        private static readonly string s_CompanyLogo = new CompanyLogo().Base64;

        /// <summary>
        /// Settings for the Setup of the Generated image.
        /// </summary>
        public Settings(
            int imageWidth,
            int imageHeight,
            Margin margin,
            int midpoint,
            int chevronHeight,
            int chevronOffset,
            int chevronGap,
            float dogLegWidth,
            int teamFontSize,
            int[] topOffsets,
            int[] bottomOffsets,
            int placardXSpacing,
            int placardYSpacing,
            int placardFontSize,
            int quarterFontSize,
            string headingTitle,
            int headingFontSize,
            int headingHeight,
            Color headingColor,
            string segoeUiNormalBase64,
            string segoeUiBoldBase64,
            string companyLogo)
        {
            Heading = new Heading(headingTitle, headingFontSize, headingHeight, headingColor);
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;
            Margin = margin;
            MidPoint = midpoint;
            ChevronHeight = chevronHeight;
            ChevronOffset = chevronOffset;
            ChevronGap = chevronGap;
            DogLegWidth = dogLegWidth;
            TeamFontSize = teamFontSize;
            PlotHeight = ImageHeight - Margin.Bottom - MidPoint - (ChevronHeight / 2);
            TopOffsets = topOffsets;
            BottomOffsets = bottomOffsets;
            PlacardXSpacing = placardXSpacing;
            PlacardYSpacing = placardYSpacing;
            PlacardFontSize = placardFontSize;
            QuarterFontSize = quarterFontSize;
            SegoeUiNormalBase64 = !string.IsNullOrWhiteSpace(segoeUiNormalBase64) ? segoeUiNormalBase64 : s_SegoeUiNormalBase64;
            SegoeUiBoldBase64 = !string.IsNullOrWhiteSpace(segoeUiBoldBase64) ? segoeUiBoldBase64 : SegoeUiBoldBase64;
            CopmanyLogo = !string.IsNullOrWhiteSpace(companyLogo) ? companyLogo : s_CompanyLogo;
        }

        public static Settings Default => new Settings(1486, 839, new Margin(20, 70), 485, 50, 25, 4, 30, 32,
            new int[] { 217, 141, 65 }, new int[] { 176, 100, 24 }, 10, 20, 16, 36,
            "Product delivery roadmap", 57, 100, DefaultColors.QuantumBlue,
            s_SegoeUiNormalBase64, s_SegoeUiBoldBase64, s_CompanyLogo);

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
            internal set;
        }

        public float DogLegWidth
        {
            get;
        }

        public Heading Heading
        {
            get;
        }

        public int TeamFontSize
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

        public int PlacardFontSize
        {
            get;
            internal set;
        }

        public int QuarterFontSize
        {
            get;
        }

        public string SegoeUiNormalBase64
        {
            get;
        }

        public string SegoeUiBoldBase64
        {
            get;
        }

        public string CopmanyLogo
        {
            get;
        }
    }
}
