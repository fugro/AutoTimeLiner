using System;
using System.Collections.Generic;
using System.Linq;

namespace RoadmapLogic
{
    public class Input
    {
        public Input(
            string team,
            string start_date,
            IEnumerable<Project> projects,
            int? quarters = 4,
            string title = null
            )
        {
            Team = team;
            if (IsValid = DateConverter.ConvertToDate(start_date, out DateTime date))
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

        public bool IsValid { get; private set; }

        public string Title { get; }

        public string Team { get; }

        public DateTime StartDate { get; }

        public IEnumerable<Project> Projects { get; }

        public int? Quarters { get; }
    }
}
