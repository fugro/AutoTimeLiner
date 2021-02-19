using System;
using System.Collections.Generic;
using System.Globalization;

namespace RoadmapLogic
{
    public class Quarter
    {
        public int Year { get; }
        public int Index { get; set; }

        public Quarter(int year, int index)
        {
            Year = year;
            Index = index;
        }

        public static bool GetQuarters(string startDay, out List<Quarter> quarters)
        {
            quarters = new List<Quarter>();
            string format = "M/d/yyyy";
            if (DateTime.TryParseExact(startDay, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                int year = date.Year;
                int month = date.Month;
                int quarter;
                if (month < 4)
                {
                    quarter = 1;
                }
                else if (month < 7)
                {
                    quarter = 2;
                }
                else if (month < 10)
                {
                    quarter = 3;
                }
                else
                {
                    quarter = 4;
                }
                quarters.Add(new Quarter(year, quarter));

                for (int i = 1; i < 4; i++)
                {
                    if (quarter == 4)
                    {
                        year++;
                        quarter = 1;
                    }
                    else
                    {
                        quarter++;
                    }
                    quarters.Add(new Quarter(year, quarter));
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
