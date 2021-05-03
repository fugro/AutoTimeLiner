using System;
using System.Collections.Generic;
using System.Linq;

namespace RoadmapLogic
{
    public class Input
    {
        public Input(
            string team,
            string startDate,
            IEnumerable<Project> projects,
            int? numberOfQuarters = 4)
        {
            Team = team;
            if (IsValid = DateConverter.ConvertToDate(startDate, out DateTime date))
            {
                StartDate = date;
                Projects = projects;
                Quarters = numberOfQuarters;

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

        public bool IsValid { get; private set; }

        public string Team { get; }

        public DateTime StartDate { get; }

        public IEnumerable<Project> Projects { get; }

        public int? Quarters { get; }
    }
}
