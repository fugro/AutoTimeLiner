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

            Input input = new Input("Enter TeamName Here:", "12/15/2020", Projects, 4);

            var imageStream = RoadmapImage.MakeImage(input, Settings.Default);
            var bytes = imageStream.ToArray();
            WriteBytesToTimestampedFile("test", bytes);
        }

        [TestMethod]
        public void CreateAndSaveImageWithEmptyValues()
        {
            var Projects = new List<Project>
            {
                new Project(string.Empty, "Status", "2/22/2020"),
                new Project("Task 3 - Description", string.Empty, "1/25/2021"),
                new Project(string.Empty, string.Empty, "4/26/2021"),
            };

            Input input = new Input("Enter TeamName Here:", "12/15/2020", Projects, 4);

            var imageStream = RoadmapImage.MakeImage(input, Settings.Default);
            var bytes = imageStream.ToArray();
            WriteBytesToTimestampedFile("test-missingNameAndValue", bytes);
        }

        [TestMethod]
        public void CreateAndSaveImageWithEmptyDateValuesShouldReturnError()
        {
            var Projects = new List<Project>
            {
                new Project(string.Empty, "Status", "2/22/2020"),
                new Project("Task 3 - Description", string.Empty, "1/25/2021"),
                new Project("Task 5 - Description", "Status", string.Empty),
                new Project("Task 1 - Description", string.Empty, string.Empty),
                new Project(string.Empty, string.Empty, "4/26/2021"),
                new Project(string.Empty, "Status", string.Empty)
            };

            Input input = new Input("Enter TeamName Here:", "12/15/2020", Projects, 4);

            var imageStream = RoadmapImage.MakeImage(input, Settings.Default);
            var bytes = imageStream.ToArray();
            WriteBytesToTimestampedFile("test-missingDate", bytes);
        }

        [TestMethod]
        public void CreateAndSaveImageWithProjectsTruncatedOnRightSide()
        {
            var Projects = new List<Project>
            {
                new Project("Task 1, This description should be too tion should be to", "Status", "1/1/2021"),
                new Project("Task 2", "Status", "1/10/2021"),
                new Project("Task 3", "Status", "1/20/2021"),
                new Project("Task 4", "Status", "2/1/2021"),
                new Project("Task 5", "Status", "2/10/2021"),
                new Project("Task 6", "Status", "2/20/2021"),
                new Project("Task 7, this is a little long", "Status", "3/1/2021"),
                new Project("Task 8", "Status", "3/10/2021"),
                new Project("Task 9", "Status", "3/20/2021"),
                new Project("Task 10", "Status", "4/1/2021"),
                new Project("Task 11", "Status", "4/10/2021"),
                new Project("Task 12", "Status", "4/20/2021"),
                new Project("Task 13", "Status", "5/1/2021"),
                new Project("Task 14", "Status", "5/10/2021"),
                new Project("Task 15", "Status", "5/20/2021"),
                new Project("Task 16", "Status", "6/1/2021"),
                new Project("Task 17", "Status", "6/10/2021"),
                new Project("Task 18", "Status", "6/20/2021"),
                new Project("Task 19", "Status", "7/1/2021"),
                new Project("Task 20", "Status", "7/10/2021"),
                new Project("Task 21", "Status", "7/20/2021"),
                new Project("Task 22", "Status", "8/1/2021"),
                new Project("Task 23", "Status", "8/10/2021"),
                new Project("Task 24", "Status", "8/20/2021"),
                new Project("Task 25", "Status", "9/1/2021"),
                new Project("Task 26", "Status", "9/10/2021"),
                new Project("Task 27", "Status", "9/20/2021"),
                new Project("Task 28", "Status", "10/1/2021"),
                new Project("Task 29", "Status", "10/10/2021"),
                new Project("Task 30", "Status", "10/20/2021"),
                new Project("Task 31", "Status", "11/1/2021"),
                new Project("Task 32", "Status", "11/10/2021"),
                new Project("Task 33", "Status", "11/20/2021"),
                new Project("Task 34", "Status", "12/1/2021"),
                new Project("Task 35", "Status", "12/10/2021"),
                new Project("Task 36", "Status", "12/20/2021"),
                new Project("Task 37, This is the", "description that will", "12/31/2021"),
            };

            Input input = new Input("Truncated Placard Test:", "01/01/2021", Projects, 4);

            var imageStream = RoadmapImage.MakeImage(input, Settings.Default);            
            var bytes = imageStream.ToArray();
            WriteBytesToTimestampedFile("test-truncated", bytes);
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

        private void WriteBytesToTimestampedFile(string filenamePart, byte[] bytes)
        {
            File.WriteAllBytes($"{filenamePart }-{DateTime.Now:yyyy-MM-dd-hh-mm-ss-fff}.png", bytes);
        }
    }
}
