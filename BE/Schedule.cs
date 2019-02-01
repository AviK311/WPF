using System;
using System.Collections;
using System.Collections.Generic;

namespace BE
{
    public class Day
	{
		public bool[] hours = new bool[6];

		public Day()
		{
			for (int i = 0; i < 6; i++)
				hours[i] = false;
		}
		public Day(Day other)
		{
			for (int i = 9; i <= 14; i++)
				this[i] = other[i];
		}

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
	/// <summary>
	/// A data structure that holds information on the availability of a tester during the week, with functions for comfortable usage
	/// </summary>
	public class Schedule
	{
		public Dictionary<DayOfWeek, Day> week;
		public Schedule(Schedule other)
		{
			week = new Dictionary<DayOfWeek, Day>();
			foreach (var day in other.week)
				this.week.Add(day.Key, new Day(day.Value));
		}
		public Schedule()
		{
			week = new Dictionary<DayOfWeek, Day>();
			week.Add(DayOfWeek.Sunday, new Day());
			week.Add(DayOfWeek.Monday, new Day());
			week.Add(DayOfWeek.Tuesday, new Day());
			week.Add(DayOfWeek.Wednesday, new Day());
			week.Add(DayOfWeek.Thursday, new Day());
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
		/// <summary>
		/// determines whether the Tester is available on the day and hour
		/// </summary>
		/// <param name="dayIndex"></param>
		/// <param name="hourIndex"></param>
		/// <returns></returns>
		public bool this[DayOfWeek dayIndex, int hourIndex]
		{
			get
			{
				if (!week.ContainsKey(dayIndex))
					throw new System.InvalidOperationException("The given day is invalid");
				return week[dayIndex][hourIndex];
			}
			set
			{
				if (!week.ContainsKey(dayIndex))
					throw new System.InvalidOperationException("The given day is invalid");
				week[dayIndex][hourIndex] = value;
			}
		}
		public override string ToString()
		{
			string toReturn = "".PadLeft(1);
			foreach(var item in week)
				toReturn +=item.Key.ToString().PadLeft(12);
			toReturn += "\n";
			for (int i = 9; i<= 14; i++)
			{
				toReturn += i.ToString().PadLeft(2);
				foreach (var item in week)
				{
					if (week[item.Key][i])
						toReturn += "available".PadLeft(12);
					else toReturn += "unavailble".PadLeft(12);
				}
					
				toReturn += "\n";
			}
			return toReturn;
		}
	}
}
