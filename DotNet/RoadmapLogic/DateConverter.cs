using System;
using System.Globalization;

namespace RoadmapLogic
{
    public static class DateConverter
    {
        private const string Ymd = "yyyy MM dd";
        private const string Dmy = "dd MMM yyyy";
        private const string Mdy = "MM dd yyyy";

        public static bool ConvertToDate(string input, out DateTime date)
        {
            date = DateTime.Now;

            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            input = input.Replace('/', ' ').Replace('-', ' ').Trim();

            var parts = input.Split(' ');

            for (int i = 0; i < parts.Length; i++)
            {
                if (int.TryParse(parts[i], out _) && parts[i].Length == 1)
                {
                    parts[i] = $"0{parts[i]}";
                }
            }

            input = string.Join(" ", parts);

            if (parts[0].Length == 4 && DateTime.TryParseExact(input, Ymd, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                date = result;
                return true;
            }
            else if (DateTime.TryParseExact(input, Mdy, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                date = result;
                return true;
            }
            else if (DateTime.TryParseExact(input, Dmy, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                date = result;
                return true;
            }
            else if (DateTime.TryParseExact(input, Ymd, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                date = result;
                return true;
            }
            
            return false;            
        }
    }
}
