using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
	public class Functions
	{
		public static string InsertSpacesBeforeUpper(string str)
		{
			for (int i = 1; i < str.Length; i++)
				if (char.IsUpper(str[i]))
					str = str.Substring(0, i) + ' ' + str.Substring(i++);
			return str;
		}
		public static bool DatesAreInTheSameWeek(DateTime date1, DateTime date2)
		{
			var d1 = date1.AddDays(-1 * (int)date1.DayOfWeek);
			var d2 = date2.AddDays(-1 * (int)date2.DayOfWeek);
			return d1 == d2;
		}
		public static List<DayOfWeek> DaysOfWeekFromString(string days)
		{
			List<DayOfWeek> toReturn = new List<DayOfWeek>();
			Schedule sched = new Schedule();
			foreach (var s in sched.week)
				if (days.Contains(((int)s.Key + 1).ToString()))
					toReturn.Add(s.Key);
			return toReturn; 	
		}
	}
}
