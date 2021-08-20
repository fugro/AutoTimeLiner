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
        public void CreateAndSaveDefaultImage()
        {
            var Projects = new List<Project>
            {
                new Project("Build Product", "Ongoing", "01/01/2021"),
                new Project("Test Product", "Not Started", "06/01/2021")
            };

            Input input = new Input(null, "Your Team:", "01/01/2021", Projects, 4);

            var imageStream = RoadmapImage.MakeImage(input, Settings.Default);
            var bytes = imageStream.ToArray();
            WriteBytesToTimestampedFile("default", bytes);
        }

        [TestMethod]
        public void CreateAndSaveImage()
        {
            var Projects = new List<Project>
            {
                new Project("Task 1 - Description", "Status", "12/22/2020"),
                new Project("Task 3 - Description", "Status", "1/25/2021"),
                new Project("Task 5 - Description", "Status", "12/28/2020"),
                new Project("Task 1 - Description", "Status", "3/15/2021"),
                new Project("Task 3 - Description", "Status", "4/26/2021"),
                new Project("Task 5 - Description", "Status", "6/21/2021")
            };

            Input input = new Input(null, "Enter Team Name Here:", "12/15/2020", Projects, 4);

            var imageStream = RoadmapImage.MakeImage(input, Settings.Default);
            var bytes = imageStream.ToArray();
            WriteBytesToTimestampedFile("test", bytes);
        }

        [TestMethod]
        public void CreateAndSaveImageWithCustomTitle()
        {
            var Projects = new List<Project>
            {
                new Project("Build Product", "Ongoing", "01/01/2021"),
                new Project("Test Product", "Not Started", "06/01/2021")
            };

            Input input = new Input("Your Title", "Your Team:", "01/01/2021", Projects, 4);

            var imageStream = RoadmapImage.MakeImage(input, Settings.Default);
            var bytes = imageStream.ToArray();
            WriteBytesToTimestampedFile("test-customTitle", bytes);
        }


        [TestMethod]
        public void CreateAndSaveImageWithSixQuarters()
        {
            var Projects = new List<Project>
            {
                new Project("Task 1 - Description", "Status", "12/22/2020"),
                new Project("Task 3 - Description", "Status", "1/25/2021"),
                new Project("Task 5 - Description", "Status", "12/28/2020"),
                new Project("Task 1 - Description", "Status", "3/15/2021"),
                new Project("Task 3 - Description", "Status", "4/26/2021"),
                new Project("Task 5 - Description", "Status", "6/21/2021"),
                new Project("Task 7 - Description", "Status", "9/20/2021"),
                new Project("Task 5 - Description", "Status", "8/2/2021"),
                new Project("Task 7 - Description", "Status", "11/5/2021"),
                new Project("Task 9 - Description", "Status", "1/15/2022"),
                new Project("Task 7 - Description", "Status", "12/18/2021"),
                new Project("Task 9 - Description", "Status", "3/10/2022")
            };

            Input input = new Input(null, "Enter Team Name Here:", "12/15/2020", Projects, 6);

            var imageStream = RoadmapImage.MakeImage(input, Settings.Default);
            var bytes = imageStream.ToArray();
            WriteBytesToTimestampedFile("test-sixQuarters", bytes);
        }

        [TestMethod]
        public void CreateAndSaveImageWithTwoQuarters()
        {
            var Projects = new List<Project>
            {
                new Project("Task 1 - Description", "Status", "12/22/2020"),
                new Project("Task 3 - Description", "Status", "1/25/2021"),
                new Project("Task 5 - Description", "Status", "12/28/2020"),
                new Project("Task 1 - Description", "Status", "3/15/2021"),
                new Project("Task 3 - Description", "Status", "4/26/2021"),
                new Project("Task 5 - Description", "Status", "6/21/2021")
            };

            Input input = new Input(null, "Enter Team Name Here:", "12/15/2020", Projects, 2);

            var imageStream = RoadmapImage.MakeImage(input, Settings.Default);
            var bytes = imageStream.ToArray();
            WriteBytesToTimestampedFile("test-twoQuarters", bytes);
        }

        [TestMethod]
        public void CreateAndSaveImageWithPreviousQuarter()
        {
            var Projects = new List<Project>
            {
                new Project("Task 1 - Description", "Status", "2/22/2020"),
                new Project("Task 3 - Description", "Status", "1/25/2021"),
                new Project("Task 5 - Description", "Status", "12/28/2020"),
                new Project("Task 1 - Description", "Status", "3/15/2021"),
                new Project("Task 3 - Description", "Status", "4/26/2021"),
                new Project("Task 5 - Description", "Status", "6/21/2021"),
            };

            Input input = new Input(null, "Enter Team Name Here:", "12/15/2020", Projects, 4);

            var imageStream = RoadmapImage.MakeImage(input, Settings.Default);
            var bytes = imageStream.ToArray();
            WriteBytesToTimestampedFile("test-previousQuarter", bytes);
        }

        [TestMethod]
        public void CreateAndSaveImageWithNextQuarter()
        {
            var Projects = new List<Project>
            {
                new Project("Task 1 - Description", "Status", "12/22/2020"),
                new Project("Task 3 - Description", "Status", "1/25/2021"),
                new Project("Task 5 - Description", "Status", "12/28/2020"),
                new Project("Task 1 - Description", "Status", "3/15/2021"),
                new Project("Task 3 - Description", "Status", "4/26/2021"),
                new Project("Task 5 - Description", "Status", "6/21/2021"),
                new Project("Task 5 - Description", "Status", "10/21/2021"),
            };

            Input input = new Input(null, "Enter Team Name Here:", "12/15/2020", Projects, 4);

            var imageStream = RoadmapImage.MakeImage(input, Settings.Default);
            var bytes = imageStream.ToArray();
            WriteBytesToTimestampedFile("test-nextQuarter", bytes);
        }

        [TestMethod]
        public void CreateAndSaveImageWitPreviousAndNextQuarter()
        {
            var Projects = new List<Project>
            {
                new Project("Task 1 - Description", "Status", "10/21/2021"),
                new Project("Task 3 - Description", "Status", "1/25/2021"),
                new Project("Task 5 - Description", "Status", "12/28/2020"),
                new Project("Task 1 - Description", "Status", "3/15/2021"),
                new Project("Task 3 - Description", "Status", "4/26/2021"),
                new Project("Task 5 - Description", "Status", "6/21/2021"),
                new Project("Task 5 - Description", "Status", "2/22/2020"),
            };

            Input input = new Input(null, "Enter Team Name Here:", "12/15/2020", Projects, 4);

            var imageStream = RoadmapImage.MakeImage(input, Settings.Default);
            var bytes = imageStream.ToArray();
            WriteBytesToTimestampedFile("test-previous-nextQuarter", bytes);
        }

        [TestMethod]
        public void CreateAndSaveImageWithOutsideStartDateAndQuarters()
        {
            var Projects = new List<Project>
            {
                new Project("Task 1 - Description", "Status", "6/21/2021"),
                new Project("Task 3 - Description", "Status", "1/25/2021"),
                new Project("Task 5 - Description", "Status", "12/28/2020"),
                new Project("Task 1 - Description", "Status", "3/15/2021"),
                new Project("Task 3 - Description", "Status", "4/26/2021"),
                new Project("Task 5 - Description", "Status", "2/22/2020"),
            };

            Input input = new Input(null, "Enter Team Name Here:", "12/15/2021", Projects, 4);

            var imageStream = RoadmapImage.MakeImage(input, Settings.Default);
            var bytes = imageStream.ToArray();
            WriteBytesToTimestampedFile("test-outsideStartDateAndQuarters", bytes);
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

            Input input = new Input(null, "Enter Team Name Here:", "12/15/2020", Projects, 4);

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

            Input input = new Input(null, "Enter Team Name Here:", "12/15/2020", Projects, 4);

            var imageStream = RoadmapImage.MakeImage(input, Settings.Default);
            var bytes = imageStream.ToArray();
            WriteBytesToTimestampedFile("test-missingDate", bytes);
        }

        [TestMethod]
        public void CreateAndSaveImageWithProjectsTruncatedOnRightSide()
        {
            var Projects = new List<Project>
            {
                new Project("Task 1, This description should be too long to be viewed", "Status", "1/1/2021"),
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
                new Project("Task 35, This description should be too long to be viewed", "Status", "12/10/2021"),
                new Project("Task 36", "Status", "12/20/2021"),
                new Project("Task 37, This is the", "description that will", "12/31/2021"),
            };

            Input input = new Input(null, "Truncated Placard Test:", "01/01/2021", Projects, 4);

            var imageStream = RoadmapImage.MakeImage(input, Settings.Default);            
            var bytes = imageStream.ToArray();
            WriteBytesToTimestampedFile("test-truncated", bytes);
        }

        [TestMethod]
        public void CreateAndSaveImageWithProjectNameAndTwoQuarters()
        {
            var Projects = new List<Project>
            {
                new Project("Task 1, This description should be too long to be viewed", "Status", "1/1/2021"),
                new Project("Task 2, This is a little long", "The status is unknown", "1/10/2021"),
                new Project("Task 3, This is a little long", "Status", "1/20/2021"),
                new Project("Task 4, This description is a medium long", "The status is unknown", "2/1/2021"),
                new Project("Task 5, This description should be too long to be viewed", "Status", "2/10/2021"),
                new Project("Task 6, This description is a medium long", "Status", "2/20/2021"),
                new Project("Task 7, This is a little long", "The status is unknown", "3/1/2021"),
                new Project("Task 8, This description should be too long to be viewed", "Status", "3/10/2021"),
                new Project("Task 9, This description is a medium long", "Status", "3/20/2021"),
                new Project("Task 11, This is a little long, ", "Status", "4/10/2021"),
                new Project("Task 12, This description is a medium long", "The status is unknown", "4/20/2021"),
                new Project("Task 13, This description should be too long to be viewed", "Status", "5/1/2021"),
                new Project("Task 14, This description is a medium long", "The status is unknown", "5/10/2021"),
                new Project("Task 15, This is a little long", "Status", "5/20/2021"),
                new Project("Task 16, This description is a medium long", "Status", "6/1/2021"),
                new Project("Task 17, This description is a medium long", "The status is unknown", "6/10/2021"),
                new Project("Task 18, This is a little long", "The status is unknown", "6/20/2021"),
                new Project("Task 19, This description should be too long to be viewed", "Status", "7/1/2021"),
                new Project("Task 20, This is a little long", "The status is unknown", "7/10/2021"),
                new Project("Task 21, This description should be too long to be viewed", "Status", "7/20/2021"),
                new Project("Task 22, This description is a medium long", "Status", "8/1/2021"),
                new Project("Task 23, This description should be too long to be viewed", "Status", "8/10/2021"),
                new Project("Task 24, This is a little long", "Status", "8/20/2021"),
                new Project("Task 25, This description is a medium long", "Status", "9/1/2021"),
                new Project("Task 26, This is a little long", "The status is unknown", "9/10/2021"),
                new Project("Task 27, This description should be too long to be viewed", "Status", "9/20/2021"),
                new Project("Task 28, This is a little long", "Status", "10/1/2021"),
                new Project("Task 29, This description is a medium long", "The status is unknown", "10/10/2021"),
                new Project("Task 30, This description is a medium long", "Status", "10/20/2021"),
                new Project("Task 31, This is a little long", "Status", "11/1/2021"),
                new Project("Task 33, This description is a medium long", "Status", "11/20/2021"),
                new Project("Task 34, This is a little long", "The status is unknown", "12/1/2021"),
                new Project("Task 35, This description should be too long to be viewed", "Status", "12/10/2021"),
                new Project("Task 36, This is a little long", "The status is unknown", "12/20/2021"),
                new Project("Task 37, This is the", "description that will", "12/31/2021"),
            };

            Input input = new Input(null, "Long Name Placard Test:", "01/01/2021", Projects, 2);

            var imageStream = RoadmapImage.MakeImage(input, Settings.Default);
            var bytes = imageStream.ToArray();
            WriteBytesToTimestampedFile("test-longName-twoQuarters", bytes);
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
