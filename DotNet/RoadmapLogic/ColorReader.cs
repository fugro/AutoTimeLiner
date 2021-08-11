using System.Collections.Generic;
using System.IO;
using SixLabors.ImageSharp;
using Newtonsoft.Json;

namespace RoadmapLogic
{
    public static class ColorReader
    {
        public static ColorSettings Read(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return ColorSettings.Default;
            }
            
            using (var streamReader = new StreamReader(fileName))
            {
                var jsonSerializer = new JsonSerializer();
                var customColors = (CustomColors)jsonSerializer.Deserialize(streamReader, typeof(CustomColors));
                var quarterColors = MakeQuarterColors(customColors.QuarterColors);

                return new ColorSettings(MakeColor(customColors.LineColor),
                                         MakeColor(customColors.HeadingColor),
                                         MakeColor(customColors.TeamColor), 
                                         quarterColors);
            }
        }

        private static IDictionary<string, Color> MakeQuarterColors(QuarterColors quarterColors)
        {
            return new Dictionary<string, Color>
            {
                { nameof(quarterColors.Quarter1), MakeColor(quarterColors.Quarter1) },
                { nameof(quarterColors.Quarter2), MakeColor(quarterColors.Quarter2) },
                { nameof(quarterColors.Quarter3), MakeColor(quarterColors.Quarter3) },
                { nameof(quarterColors.Quarter4), MakeColor(quarterColors.Quarter4) },
                { nameof(quarterColors.Quarter5), MakeColor(quarterColors.Quarter5) },
                { nameof(quarterColors.Quarter6), MakeColor(quarterColors.Quarter6) }
            };
        }

        private static Color MakeColor(IList<int> rgb)
        {
            return Color.FromRgb(byte.Parse(rgb[0].ToString()),
                                 byte.Parse(rgb[1].ToString()),
                                 byte.Parse(rgb[2].ToString()));
        }
    }
}
