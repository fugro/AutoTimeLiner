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
    }
}
