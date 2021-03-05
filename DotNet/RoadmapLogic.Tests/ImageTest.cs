using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace RoadmapLogic.Tests
{
    [TestClass]
    public class ImageTest
    {
        [TestMethod]
        public void CreateAndSaveImage()
        {
            var Projects = new List<Project>
            {
                new Project("Task 1 - Description", "Status", "2/22/2020"),
                new Project("Task 3 - Description", "Status", "1/25/2021"),
                new Project("Task 5 - Description", "Status", "12/28/2020"),
                new Project("Task 1 - Description", "Status", "3/15/2021"),
                new Project("Task 3 - Description", "Status", "4/26/2021"),
                new Project("Task 5 - Description", "Status", "6/21/2021")
            };

            Input input = new Input("Enter TeamName Here:", "12/15/2020", Projects);

            var imageStream = RoadmapImage.MakeImage(input, Settings.Default);

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
