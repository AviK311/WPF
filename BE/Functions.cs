using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
	public class Functions
	{
		/// <summary>
		/// This function returns str with spaces before uppercases,
		/// for usage of toString
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string InsertSpacesBeforeUpper(string str)
		{
			for (int i = 1; i < str.Length; i++)
				if (char.IsUpper(str[i]))
					str = str.Substring(0, i) + ' ' + str.Substring(i++);
			return str;
		}
		/// <summary>
		/// Checks whether date1 an date2 are in the same week
		/// </summary>
		/// <param name="date1"></param>
		/// <param name="date2"></param>
		/// <returns></returns>
		public static bool DatesAreInTheSameWeek(DateTime date1, DateTime date2)
		{
			var d1 = date1.AddDays(-1 * (int)date1.DayOfWeek);
			var d2 = date2.AddDays(-1 * (int)date2.DayOfWeek);
			return d1.ToShortDateString() == d2.ToShortDateString();
		}
		/// <summary>
		/// Receives a string of numbers and returns a list
		/// of the days represented by the numbers in the string.
		/// example: 14534 will return
		/// sunday, tuesday, wednesday, thursday.
		/// </summary>
		/// <param name="days"></param>
		/// <returns></returns>
		public static List<DayOfWeek> DaysOfWeekFromString(string days)
		{
			List<DayOfWeek> toReturn = new List<DayOfWeek>();
			Schedule sched = new Schedule();
			foreach (var s in sched.week)
				if (days.Contains(((int)s.Key + 1).ToString()))
					toReturn.Add(s.Key);
			return toReturn; 	
		}

		public static void init()
		{
			Trainee a = new Trainee
			{
				ID = "123",
				Name = new Name("Avi", "Levi"),
				Sex = Gender.Male,
				PhoneNumber = "123",
				BirthDay = new DateTime(1993, 11, 3),
				Address = new Address("bet shemesh", "nahal maor", "19"),
				CurrentCarType = VehicleType.LargeTruck,
			};
			Trainee b = new Trainee
			{
				ID = "456",
				Name = new Name("Sapir", "Barabi"),
				Sex = Gender.Female,
				PhoneNumber = "456",
				BirthDay = new DateTime(1993, 5, 14),
				Address = new Address("Kiryat Ata", "David Remez", "1a"),
				CurrentCarType = VehicleType.LargeTruck,

			};

			Tester c = new Tester
			{
				ID = "123456",
				Name = new Name("Dan", "internatonal"),
				Sex = Gender.Male,
				PhoneNumber = "224",
				BirthDay = new DateTime(1969, 2, 1),
				Address = new Address("jeru", "vaad", "21"),
				MaxDistance = 60,
				MaxWeeklyTests = 9,
				ExpYears = 6,
				testingCarType = VehicleType.PrivateCar,
				schedule = new Schedule()
			};
			IBL bl = FactoryBL.GetBL();
			bl.AddTester(c);
			bl.AddTrainee(a);
			bl.AddTrainee(b);
		}

	}
}
