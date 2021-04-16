using System;
using System.Collections.Generic;
using System.Linq;

namespace RoadmapLogic
{
    public class Project
    {
        public Project(string name, string label, string inputDate)
        {
            Name = name;
            Label = label;
            IsValid = DateConverter.ConvertToDate(inputDate, out DateTime date);
            Date = date;
        }

        public bool IsValid { get; private set; }

        public string Name { get; }

        public string Label { get; }

        public DateTime Date { get; }

        public List<string> ToList()
        {
            return new List<string>() { Name, Label, Date.ToString("dd MMM yyyy") };
        }

        public static List<Project> SortProjects(IEnumerable<Project> projects, Quarter startQuarter)
        {
            projects = projects.OrderBy(p => Calculations.GetJulianDay(p.Date)).ToList();

            int startJulianDay = 1;
            switch (startQuarter.Index)
            {
                case 2:
                    startJulianDay = Calculations.GetJulianDay(new DateTime(startQuarter.Year, 4, 1));
                    break;
                case 3:
                    startJulianDay = Calculations.GetJulianDay(new DateTime(startQuarter.Year, 7, 1));
                    break;
                case 4:
                    startJulianDay = Calculations.GetJulianDay(new DateTime(startQuarter.Year, 10, 1));
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
