using SixLabors.Fonts;
using System;
using System.IO;

namespace RoadmapLogic
{
    public sealed class Fonts
    {
        public Fonts(Settings settings)
        {
            var normalFontFamily = GetFontFamily(FontWeight.Normal, settings);
            var boldFontFamily = GetFontFamily(FontWeight.Bold, settings);

            HeaderFont = normalFontFamily.CreateFont(settings.Heading.FontSize);
            TeamFont = normalFontFamily.CreateFont(settings.TeamFontSize);
            QuarterFont = boldFontFamily.CreateFont(settings.QuarterFontSize);
            PlacardFont = normalFontFamily.CreateFont(settings.PlacardFontSize);
        }

        public Font HeaderFont { get; }

        public Font TeamFont { get; }

        public Font QuarterFont { get; }

        public Font PlacardFont { get; }

        private static FontFamily GetFontFamily(FontWeight weight, Settings settings)
        {
            var fontCollection = new FontCollection();

            string fontBase64 = weight == FontWeight.Bold
                ? settings.SegoeUiBoldBase64
                : settings.SegoeUiNormalBase64;

            var bytes = Convert.FromBase64String(fontBase64);

            using (var ms = new MemoryStream(bytes))
            {
                return fontCollection.Install(ms);
            }
        }
    }
}
