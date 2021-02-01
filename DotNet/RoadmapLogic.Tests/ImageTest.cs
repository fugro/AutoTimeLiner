using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace RoadmapLogic.Tests
{
    [TestClass]
    public class ImageTest
    {
        [TestMethod]
        public void CreateAndSaveImage()
        {
            var imageStream = RoadmapImage.MakeImage(new Input { Team = "Fugro" });

            var bytes = imageStream.ToArray();

            File.WriteAllBytes($"test-{DateTime.Now:yyyy-MM-dd-hh-mm-ss-fff}.png", bytes);
        }
    }
}
