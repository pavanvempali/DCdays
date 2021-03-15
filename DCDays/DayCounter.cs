using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DCDays
{
	public class DayCounter
	{
     
        /// <summary>
        /// Funtion returns weekdays between any two given dates.
        /// </summary>
        /// <param name="firstDate"></param>
        /// <param name="secondDate"></param>
        /// <returns></returns>
        public int WeekdaysBetweenTwoDates(DateTime firstDate, DateTime secondDate)
		{
            //return 0 when firstDate = secondDate and when firstDate is greater than secondDate
            if (firstDate == secondDate || firstDate > secondDate) return 0;
      
            int totalWorkingDays=0;
            try
            {
                //exlude firstDate and startDate, and weekends
                for (var current = firstDate.AddDays(1); current < secondDate; current = current.AddDays(1))
                {
                    if (current.DayOfWeek == DayOfWeek.Saturday || current.DayOfWeek == DayOfWeek.Sunday)
                    {
                        // exclude weekends
                    }
                    else totalWorkingDays++;
                }
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
            return totalWorkingDays;
        }

        /// <summary>
        /// Returns number of businees days between any given dates
        /// </summary>
        /// <param name="firstDate"></param>
        /// <param name="secondDate"></param>
        /// <param name="publicHolidays"></param>
        /// <returns></returns>
        public int BusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate,IEnumerable<Holiday> holidays, IEnumerable<DynamicHolidayDate> dHolidays)
        {
            //return 0 when firstDate = secondDate and when firstDate is greater than secondDate
            if (firstDate == secondDate || firstDate > secondDate) return 0;
            int totoalBusinessDays = 0;
            try
            {
                //Apply holiday rules and get the updated list
                IEnumerable<Holiday> fullHolidays = ApplyHolidayRules(holidays);
                //Prepare a new list along with fullHolidays list for fixed day holidays like queen's birthday on 2nd Monday of June
                IEnumerable<Holiday> fullWithDynamicHolidays;

                fullWithDynamicHolidays = ApplyDynamicHolidayRules(ref fullHolidays, firstDate, secondDate, dHolidays);
                for (var current = firstDate.AddDays(1); current < secondDate; current = current.AddDays(1))
                {
                    if (current.DayOfWeek == DayOfWeek.Saturday
                        || current.DayOfWeek == DayOfWeek.Sunday
                        || (fullWithDynamicHolidays != null && fullWithDynamicHolidays.Where(d => d.holidayDate == current.Date).FirstOrDefault() != null)
                        )
                    {
                        // exclude weekends and holidays
                    }
                    else totoalBusinessDays++;
                }
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
            return totoalBusinessDays;
        }

        /// <summary>
        /// Apply holiday rules on given dates in the list. Incase of any holiday on Saturday or Sunday it gets moved to Monday
        /// </summary>
        /// <param name="publicHolidays"></param>
        /// <returns>Updated enumerable holiday list</returns>
       public IEnumerable<Holiday> ApplyHolidayRules(IEnumerable<Holiday> publicHolidays)
        {
                List<Holiday> updatedHolidays= new List<Holiday>();
                DateTime temp=new DateTime();
                try
                {
                    foreach (Holiday ph in publicHolidays)
                    {
                        if (ph.holidayDate.DayOfWeek == DayOfWeek.Saturday) temp = ph.holidayDate.AddDays(2);
                        else if (ph.holidayDate.DayOfWeek == DayOfWeek.Sunday) temp = ph.holidayDate.AddDays(1);

                        var obj = ((Holiday)publicHolidays.Where(h => h.holidayDate == temp).FirstOrDefault());
                        if (obj != null) temp = temp.AddDays(1);

                        if (temp == new DateTime(0001, 1, 1)) temp = ph.holidayDate;

                        Holiday updatedHoliday = new Holiday(temp, ph.holidayDescription);
                        updatedHolidays.Add(updatedHoliday);
                        temp = new DateTime();
                    }
                }
                catch(Exception ex)
                {
                    throw ex.InnerException;
                }
                if (updatedHolidays.Count > 0) return updatedHolidays;
            return publicHolidays;   
        }

        /// <summary>
        /// Apply holiday rules on specific days in a month and on a specific day
        /// </summary>
        /// <param name="publicHolidays">Pass the current holiday list</param>
        /// <param name="current">first date</param>
        /// <returns></returns>
        public IEnumerable<Holiday> ApplyDynamicHolidayRules(ref IEnumerable<Holiday> publicHolidays, DateTime? firstDate, DateTime? secondDate, IEnumerable<DynamicHolidayDate> dHolidays)
        {
            List<Holiday> tempList = publicHolidays.ToList();
            try
            {
                if (firstDate.HasValue && secondDate.HasValue)
                {
                    IEnumerable<Holiday> dynamicPublicHoliday = GetDynamicHolidays(firstDate.Value, secondDate.Value, dHolidays);
                    foreach (Holiday h in dynamicPublicHoliday)
                    {
                        if (h.holidayDate.DayOfWeek == DayOfWeek.Saturday) h.holidayDate = h.holidayDate.AddDays(2);
                        if (h.holidayDate.DayOfWeek == DayOfWeek.Sunday) h.holidayDate = h.holidayDate.AddDays(1);
                        tempList.Add(new Holiday(h.holidayDate, h.holidayDescription));
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
            return tempList;
        }

        /// <summary>
        /// Retrieve dynamic holidays for given dates. For example holidays like queen's birthday on 2nd Monday of June
        /// </summary>
        /// <param name="current">start date</param>
        /// <param name="last">end date</param>
        /// <returns>Returns well formed dates from the given dates like 2nd Monday of June</returns>
        public IEnumerable<Holiday> GetDynamicHolidays(DateTime? current, DateTime? last, IEnumerable<DynamicHolidayDate> dHolidays)
        {
           var lst = new List<Holiday>();
            try
            {
                int count = last.Value.Year - current.Value.Year;
                if (current.HasValue)
                {
                    DateTime dynamicDate;
                    foreach (DynamicHolidayDate dhd in dHolidays)
                    {
                        if (DynamicHoliday.FindDynamicHoliday(current.Value, dhd) != null)
                        {
                            dynamicDate = (DateTime)DynamicHoliday.FindDynamicHoliday(current.Value, dhd);

                            //if the start and end dates are from different year then replicate the dynamic holidays for subsequent years
                            for (int y = 0; count > 0 && y < count - 1; y++)
                            {
                                dynamicDate = dynamicDate.AddYears(1);
                                lst.Add(new Holiday(dynamicDate, dhd.holidayDescription));
                            }
                            lst.Add(new Holiday(dynamicDate, dhd.holidayDescription));
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
         return lst;
        }
    }
}
