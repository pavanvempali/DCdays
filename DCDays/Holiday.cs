using System;

namespace DCDays
{
   	public class Holiday:IHoliday
     {
       public DateTime holidayDate { get; set; }
       public  string holidayDescription { get; set; }
       
        public Holiday(DateTime holidayDate, string holidayDescription)
        {
            this.holidayDate = holidayDate;
            this.holidayDescription = holidayDescription;
        }
            
        public void setHoliday(DateTime newholidayDate)
        {
            this.holidayDate = newholidayDate;
        }
     }
}