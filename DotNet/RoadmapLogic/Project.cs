using System;
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
            IsValid = DateConverter.ConvertToDate(date, out DateTime pDate);
            Date = pDate;
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
            return projects.OrderBy(p => Calculations.GetJulianDay(p.Date, startQuarter.Year)).ToList();
        }
    }
}
