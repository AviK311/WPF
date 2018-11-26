using System;
using System.Collections.Generic;

namespace BE
{
    public class Day
	{
		public bool[] hours = new bool[6];
		public bool this[int index]
		{
			get
			{
				if (index > 14 || index < 9)
					throw new System.InvalidOperationException("The hour is invalid");
				return hours[index - 9];
			}
			set
			{
				if (index > 14 || index < 9)
					throw new System.InvalidOperationException("The hour is invalid");
				hours[index - 9] = value;
			}
		}
	}
	public class Schedule
	{
		Dictionary<DayOfWeek, Day> week;

		public Schedule()
		{
			Day day1 = new Day(),
				day2 = new Day(),
				day3 = new Day(),
				day4 = new Day(),
				day5 = new Day();
			week.Add(DayOfWeek.Sunday, day1);
			week.Add(DayOfWeek.Monday, day2);
			week.Add(DayOfWeek.Tuesday, day3);
			week.Add(DayOfWeek.Wednesday, day4);
			week.Add(DayOfWeek.Thursday, day5);
		}
		public Day this[DayOfWeek index]
		{
			get
			{ 
				if (!week.ContainsKey(index))
					throw new System.InvalidOperationException("The given day is invalid");
				return week[index];
			}
		}
	}
}
