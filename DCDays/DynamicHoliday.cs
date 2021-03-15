using System;
using System.Collections.Generic;
using System.Text;

namespace DCDays
{
     public class DynamicHoliday : IHoliday
    {
        public DateTime holidayDate { get; set; }
        public string holidayDescription { get; set; }

        public DynamicHoliday(DateTime holidayDate, string holidayDescription)
        {
            this.holidayDate = holidayDate;
            this.holidayDescription = holidayDescription;
        }

        public void setHoliday(DateTime newholidayDate)
        {
            this.holidayDate = newholidayDate;
        }
  
        /// <summary>
        /// Create a datetime from the dynamic date
        /// </summary>
        /// <param name="current">start date</param>
        /// <param name="last">end date</param>
        /// <param name="dynamicHoliday">dynamic holiday structure</param>
        /// <returns>date formed by processing dynamic date</returns>
        public static object FindDynamicHoliday(DateTime current, DynamicHolidayDate dynamicHoliday)
        {
            object finalDate = null;
            int month = current.Month;
            if (current == null) return new DateTime(1900, 1, 1);
            else
            {
                try
                {
                    int days = DateTime.DaysInMonth(current.Year, month);
                    if (current.Month == dynamicHoliday.month)
                    {
                        return CreateDynamicHolidaysBetweenYears(current, days, dynamicHoliday);
                    }
                }
                catch(Exception ex)
                {
                    throw ex.InnerException;
                }
                return finalDate;
            }
        }

        /// <summary>
        /// Create dates from dynamic days
        /// </summary>
        /// <param name="current"></param>
        /// <param name="days"></param>
        /// <param name="dynamicHoliday"></param>
        /// <returns></returns>
        public static object CreateDynamicHolidaysBetweenYears(DateTime current, int days, DynamicHolidayDate dynamicHoliday)
        {
            int month = current.Month;
            int year = current.Year;
            DateTime dynamicholidayDate;
            int dayOfWeekCounter = 0;
            object finalDate = null;
            try
            {
                for (int day = 1; day <= days; day++)
                {
                    dynamicholidayDate = new DateTime(year, month, day);
                    if (dynamicholidayDate.DayOfWeek == dynamicHoliday.dayOfWeek)
                    {
                        dayOfWeekCounter++;
                        if (dayOfWeekCounter == dynamicHoliday.dayPostion)
                        {
                            finalDate = dynamicholidayDate;
                            return finalDate;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
            return finalDate;
        }
    }

    /// <summary>
    /// Structure for dynamic holiday, for example queen's birthday on 2nd Monday of June
    /// </summary>
    public struct DynamicHolidayDate
    {
        public DayOfWeek dayOfWeek;
        public int dayPostion;
        public int month;
        public string holidayDescription;

        public DynamicHolidayDate(DayOfWeek dayOfWeek, int dayPostion, int month, string holidayDescription)
        {
            this.month = month;
            this.dayOfWeek = dayOfWeek;
            this.dayPostion = dayPostion;
            this.holidayDescription = holidayDescription;
        }
    }

}
