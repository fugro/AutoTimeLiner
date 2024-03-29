﻿using System.Text.Json.Serialization;

namespace RoadmapLambda
{
    /// <summary>
    /// Input DTO for the AWS Lambda function that creates the image.
    /// </summary>
    public class FunctionInput
    {
        public bool Debug { get; set; }

        [JsonPropertyName("bg_color_hex")]
        public string BackgroundColor { get; set; }

        public string Title { get; set; }

        public string Team { get; set; }

        [JsonPropertyName("start_date")]
        public string StartDate { get; set; }

        public int Quarters { get; set; }

        public IEnumerable<FunctionProject> Projects { get; set; }

        /// <summary>
        /// Creates an input object consumable by the Roadmap image generator.
        /// </summary>
        public RoadmapLogic.Input ToRoadmapInput()
        {
            return new RoadmapLogic.Input(Team, StartDate, Projects.Select(p => p.ToRoadmapProject()), Quarters, Debug, Title, BackgroundColor);
        }
    }

    public class FunctionProject
    {
        public string Name { get; set; }

        public string Label { get; set; }

        public string Date { get; set; }

        /// <summary>
        /// Creates a project object consumable by the Roadmap image generator.
        /// </summary>
        public RoadmapLogic.Project ToRoadmapProject()
        {
            return new RoadmapLogic.Project(Name, Label, Date);
        }
    }
}
