using System;
using System.Collections.Generic;
using System.Text;

namespace DCDays
{
    public interface IHoliday
    {
        DateTime holidayDate { get; set; }
        string holidayDescription { get; set; }
        void setHoliday(DateTime newholidayDate);
    }
}
