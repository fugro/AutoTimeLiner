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

        [TestMethod]
        [Ignore("Run manually to generate a .txt file of Base64 data from an image file.")]
        public void FileToBase64()
        {
            // Fill this path with the image you want to convert to Base64.
            var imageFile = @"C:\Path\To\image.png";

            var txtFile = Path.GetFileNameWithoutExtension(imageFile) + ".txt";

            var pathUpToFilename = imageFile.Substring(0, imageFile.LastIndexOf(Path.DirectorySeparatorChar));

            var saveTo = Path.Combine(pathUpToFilename, txtFile);

            File.WriteAllText(saveTo, new Base64Converter().ToBase64(imageFile));
        }
    }
}
