using System.Collections.Generic;
using System.Linq;

namespace RoadmapLogic
{
    public class Project
    {
        public Project(string name, string label, string date)
        {
            Name = name;
            Label = label;
            Date = date;
        }

        public string Name { get; }

        public string Label { get; }

        public string Date { get; }

        public List<string> ToList()
        {
            return new List<string>() { Name, Label, Date };
        }

        public static List<Project> SortProjects(IEnumerable<Project> projects, Quarter startQuarter)
        {
            projects = projects.OrderBy(p => Calculations.GetJulianDay(p.Date)).ToList();

            int startJulianDay = 1;
            switch (startQuarter.Index)
            {
                case 2:
                    startJulianDay = Calculations.GetJulianDay($"04/01/{startQuarter.Year}");
                    break;
                case 3:
                    startJulianDay = Calculations.GetJulianDay($"07/01/{startQuarter.Year}");
                    break;
                case 4:
                    startJulianDay = Calculations.GetJulianDay($"10/1/{startQuarter.Year}");
                    break;
            }

            var list = new List<Project>();
            var list2 = new List<Project>();

            foreach (var project in projects)
            {
                if (Calculations.GetJulianDay(project.Date) >= startJulianDay)
                {
                    list.Add(project);
                }
                else
                {
                    list2.Add(project);
                }
            }
            list.AddRange(list2);

            return list;
        }
    }
}
