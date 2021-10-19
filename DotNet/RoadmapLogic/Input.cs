using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoadmapLogic
{
    public class Input
    {
        [JsonConstructor]
        public Input(
            string team,
            [JsonProperty("start_date")]string startDate,
            IEnumerable<Project> projects,
            int? quarters = 4,
            string title = null
            )
        {
            Team = team;
            if (IsValid = DateConverter.ConvertToDate(startDate, out DateTime date))
            {
                Title = title;
                StartDate = date;
                Projects = projects;
                Quarters = quarters;

                if (projects.Any(p => !p.IsValid))
                {
                    IsValid = false;
                }
            }
            else
            {
                IsValid = false;
            }
        }

        public bool IsValid { get; }

        public string Title { get; }

        public string Team { get; }

        public DateTime StartDate { get; }

        public IEnumerable<Project> Projects { get; }

        public int? Quarters { get; }
    }
}
