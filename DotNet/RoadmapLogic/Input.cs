using System.Collections.Generic;

namespace RoadmapLogic
{
    public class Input
    {
        public Input(string team, string startDate, IEnumerable<Project> projects)
        {
            Team = team;
            StartDate = startDate;
            Projects = projects;
        }

        public string Team { get; }

        public string StartDate { get; }

        public IEnumerable<Project> Projects { get; }
    }
}
