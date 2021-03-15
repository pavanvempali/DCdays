using System;
using System.Collections.Generic;
using System.Linq;

namespace DCDays
{
    public static class Program
    {
        static void Main(string[] args)
        {
            DateTime start, stop;

            Console.WriteLine("Please enter a start date for example '2014-4-1'");
            var first=Console.ReadLine();
            Console.WriteLine("Please enter a end date for example  '2014-4-31'");
            var second= Console.ReadLine();

            DateTime.TryParse(first, out start);
            DateTime.TryParse(second, out stop);

            if (start >= stop)
            {
                Console.WriteLine("Enter valid start and end dates, and end date should be greater than start date. Program terminating.....");
                return;
            }

            DayCounter objDayCounter = new DayCounter();
            int totalWorkingDays = objDayCounter.WeekdaysBetweenTwoDates(start, stop);
            int totalBusinessDays = objDayCounter.BusinessDaysBetweenTwoDates(start, stop,GetHolidays(), GetDynamicHolidays());

            if(totalWorkingDays == 0)
            {
                Console.WriteLine("The total working days are 0");
            }
            if(totalBusinessDays==0)
            {
                Console.WriteLine("The total business days are 0");
            }
            Console.WriteLine("There are {0} working days and {1} business days", totalWorkingDays, totalBusinessDays);
           }

        /// <summary>
        /// List of holidays
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Holiday> GetHolidays()
        {
            var lst = new List<Holiday>
            {
                new Holiday(new DateTime(2013,12, 25 ), "Christmas"),
                new Holiday(new DateTime(2013,12, 26 ), "Christmas Eve"),
                new Holiday(new DateTime(2013,1, 1 ), "New Year"),
                new Holiday(new DateTime(2013,4, 25 ), "Anzac Day"),
            };
            return lst;
        }

        /// <summary>
        /// List of dynamic holidays
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DynamicHolidayDate> GetDynamicHolidays()
        {
            var dlst = new List<DynamicHolidayDate>
            {
                new DynamicHolidayDate(DayOfWeek.Monday,2, 4, "Queen Birthday"),
                new DynamicHolidayDate(DayOfWeek.Saturday,3,4, "The Big Festival")
            };
            return dlst;
        }

    }
}
