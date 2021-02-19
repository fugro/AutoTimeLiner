using System.Collections.Generic;

namespace RoadmapLogic
{
    public class Input
    {
        public Input(string team, string startDate, List<Project> projects)
        {
            Team = team;
            StartDate = startDate;
            Projects = projects;
        }
        public string Team { get; }

        public string StartDate { get; }

        public List<Project> Projects { get; }
    }
}
