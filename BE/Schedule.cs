using System;
using System.Collections.Generic;
using System.Text;

namespace BE
{  
	public class Day
	{
		public bool[] hours = new bool[6];
		public bool this[int index]
		{
			get => hours[index - 9];
			set => hours[index - 9] = value;
		}
	}
	public class Schedule
	{
		Dictionary<string, Day> week;

		public Schedule()
		{
			Day day1 = new Day(),
				day2 = new Day(),
				day3 = new Day(),
				day4 = new Day(),
				day5 = new Day();
			week.Add("sunday", day1);
			week.Add("monday", day2);
			week.Add("tuesday", day3);
			week.Add("wednesday", day4);
			week.Add("thursday", day5);
		}
		public Day this[string index]
		{
			get => week[index];
			set => week[index] = value;
		}
	}
}
