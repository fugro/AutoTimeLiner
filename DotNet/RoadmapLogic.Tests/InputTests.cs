using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace RoadmapLogic.Tests
{
    [TestClass]
    public class InputTests
    {
        [TestMethod]
        public void ValidWhenDatesAreValid()
        {
            var input = new Input(
                "Team",
                "2021-01-01",
                new List<Project>
                {
                    new Project("Name", "Label", "2021-01-01")
                });
            Assert.IsTrue(input.IsValid);
        }

        [TestMethod]
        public void InvalidWhenStartDateIsInvalid()
        {
            var input = new Input(
                "Team",
                "invalidDate",
                new List<Project>());
            Assert.IsFalse(input.IsValid);
        }

        [TestMethod]
        public void InvalidWhenAnyProjectIsInvalid()
        {
            var input = new Input(
                "Team",
                "2021-01-01",
                new List<Project>
                {
                    new Project("Name", "Label", "invalidDate")
                });
            Assert.IsFalse(input.IsValid);
        }
    }
}
