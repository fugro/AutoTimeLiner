using Microsoft.VisualStudio.TestTools.UnitTesting;
using SixLabors.ImageSharp;

namespace RoadmapLogic.Tests
{
    [TestClass]
    public class ColorReaderTest
    {
        [TestMethod]
        public void ReadDefaultColorsTest()
        {
            var colorSettings = ColorReader.Read(null);
            Assert.IsNotNull(colorSettings);
            Assert.IsNotNull(colorSettings);
            Assert.IsNotNull(colorSettings.LineColor);
            Assert.AreEqual(colorSettings.LineColor, Color.FromRgb(1, 30, 65));
            Assert.IsNotNull(colorSettings.HeadingColor);
            Assert.AreEqual(colorSettings.HeadingColor, Color.FromRgb(1, 30, 65));
            Assert.IsNotNull(colorSettings.TeamColor);
            Assert.AreEqual(colorSettings.TeamColor, Color.FromRgb(0, 0, 0));
            Assert.IsNotNull(colorSettings.QuarterColors);
            Assert.AreEqual(colorSettings.QuarterColors.Count, 6);
        }

        [TestMethod]
        [DeploymentItem(@"Data\color_settings.json")]
        public void ReadCustomColorsTest()
        {
            string fileNmae = "color_settings.json";

            var colorSettings = ColorReader.Read(fileNmae);
            Assert.IsNotNull(colorSettings);
            Assert.IsNotNull(colorSettings.LineColor);
            Assert.AreEqual(colorSettings.LineColor, Color.FromRgb(255, 0, 0));
            Assert.IsNotNull(colorSettings.HeadingColor);
            Assert.AreEqual(colorSettings.HeadingColor, Color.FromRgb(0, 255, 0));
            Assert.IsNotNull(colorSettings.TeamColor);
            Assert.AreEqual(colorSettings.TeamColor, Color.FromRgb(0, 0, 255));
            Assert.IsNotNull(colorSettings.QuarterColors);
            Assert.AreEqual(colorSettings.QuarterColors.Count, 6);
        }
    }
}
