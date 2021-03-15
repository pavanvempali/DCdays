using System;
using Xunit;
using DCDays;
using System.Collections;
using System.Collections.Generic;

namespace DCDaysTests
{
	public class DCDaysTest
	{
	
		/// <summary>
		/// For any two given dates calculate the number of Working days
		/// </summary>
		[Fact]
		public void Workingday_Should_Exclude_Weekends()
		{
			var dc = new DayCounter();
			var dates = new List<StartEndDates>
			{
				new StartEndDates(new DateTime(2013, 10, 7), new DateTime(2013,10,9), 1),
				new StartEndDates(new DateTime(2013, 10, 5), new DateTime(2013, 10, 14),5),
				new StartEndDates(new DateTime(2013, 10, 7), new DateTime(2014, 1, 1), 61),
				new StartEndDates(new DateTime(2013, 10, 7), new DateTime(2013, 10, 5),0),
			};

			foreach(StartEndDates date in dates)
			{
				int result = dc.WeekdaysBetweenTwoDates(date.startDate, date.endDate);
				Assert.Equal(date.difValue, result);
			}
		}

		/// <summary>
		/// For any two given dates calculate the number of Business days
		/// This excludes weekends and the fixed list of holidays with full date
		/// </summary>
		[Fact]
		public void BusinessDay_Should_Exclude_Weekends_And_Holidays()
		{
			var dc = new DayCounter();
			var dates = new List<StartEndDates>
			{
				new StartEndDates(new DateTime(2013, 10, 7), new DateTime(2013,10,9), 1),
				new StartEndDates(new DateTime(2013, 12, 24), new DateTime(2013, 12, 27),0),
				new StartEndDates(new DateTime(2013, 10, 7), new DateTime(2014, 1, 1), 59)
			};
			foreach (StartEndDates date in dates)
			{
				int result = dc.BusinessDaysBetweenTwoDates(date.startDate, date.endDate, GetHolidays(), new List<DynamicHolidayDate>());
				Assert.Equal(date.difValue, result);
			}
		}

		/// <summary>
		/// For any two given dates calculate the number of Business days
		/// This excludes weekends, fixed list of holidays with full date and dynamic holidays like 2nd Monday of June etc.,
		/// </summary>
		[Fact]
		public void BusinessDay_Should_Exclude_Weekends_And_Holidays_And_DynamicHolidays()
		{
			var dc = new DayCounter();
			var dates = new List<StartEndDates>
			{
				new StartEndDates(new DateTime(2013, 10, 7), new DateTime(2013,10,9), 1),
				new StartEndDates(new DateTime(2013, 12, 24), new DateTime(2013, 12, 27),0),
				new StartEndDates(new DateTime(2013, 10, 7), new DateTime(2014, 1, 1), 59),
				new StartEndDates(new DateTime(2013, 4, 1), new DateTime(2013, 4, 30), 18)
			};
			foreach (StartEndDates date in dates)
			{
				int result = dc.BusinessDaysBetweenTwoDates(date.startDate, date.endDate, GetHolidays(), GetDynamicHolidays());
				Assert.Equal(date.difValue, result);
			}
		}

		/// <summary>
		/// When a holiday falls on a weekend move it to Monday and this day should be a working day 
		/// otherwise move it by 1 day until it is a working day
		/// </summary>
		[Fact]
		public void CheckFor_Rules_Application_When_A_Holiday_FallsOn_A_Weekend()
		{
			var dc = new DayCounter();
			IEnumerable<Holiday> newHoliday= dc.ApplyHolidayRules(GetHolidaysOnWeekends());
			Assert.Collection(newHoliday,
			   item => Assert.Equal(new DateTime(2015, 12, 28), ((Holiday)item).holidayDate),
			   item => Assert.Equal(new DateTime(2016, 12, 27), ((Holiday)item).holidayDate),
			   item => Assert.Equal(new DateTime(2016, 12, 26), ((Holiday)item).holidayDate)
		   );
		}

		/// When a holiday falls on a weekend move it to Monday and this day should be a working day 
		/// otherwise move it by 1 day until it is a working day
		/// </summary>
		[Fact]
		public void CheckFor_Rules_Application_When_A_Dynamic_Holiday_FallsOn_A_Weekend()
		{
			var dc = new DayCounter();
			IEnumerable<Holiday> holidayList = GetHolidays();
			IEnumerable<Holiday> newHoliday = dc.ApplyDynamicHolidayRules(ref holidayList, new DateTime(2014, 4, 1), new DateTime(2014, 4, 30), GetDynamicHolidays());
			Assert.Collection(newHoliday,
			   item => Assert.Equal(new DateTime(2014, 4, 7), ((Holiday)item).holidayDate),
			   item => Assert.Equal(new DateTime(2014, 4, 21), ((Holiday)item).holidayDate)
		   );
		}
		/// <summary>
		/// Get holidays on weekends
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<Holiday> GetHolidaysOnWeekends()
		{
			var holidays = new List<Holiday>
			{
				new Holiday(new DateTime(2015,12,26), "Christmas Eve"),
				new Holiday(new DateTime(2016,12,25), "Christmas"),
				new Holiday(new DateTime(2016,12,26), "Christmas Eve")
			};
			return holidays;
		}



		/// <summary>
		/// List of holidays
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<Holiday> GetHolidays()
		{
			var holidays = new List<Holiday>
			{
				new Holiday(new DateTime(2013,12, 25 ), "Christmas"),
				new Holiday(new DateTime(2013,12, 26 ), "Christmas Eve"),
				new Holiday(new DateTime(2013,1, 1 ), "New Year")

			};
			return holidays;
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

	public class StartEndDates
	{
		public DateTime startDate { get; set; }
		public DateTime endDate { get; set; }
		public int difValue { get; set; }

		public StartEndDates(DateTime start, DateTime end, int diffValue)
		{
			startDate = start;
			endDate = end;
			difValue = diffValue;
		}
	}
}
