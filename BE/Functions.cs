using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
		public static void AddTemplateList<T>(List<T> list, params T[] args)
		{
			foreach (var item in args)
				list.Add(item);
		}
		public static bool ValidateEmail(string email)
		{
			if (email == null || email == "")
				return true;
			Match match = Configuration.EmailRegex.Match(email);
			return match.Success;
		}
		public static bool ValidatePhone(string phone)
		{
			if (phone == null || phone == "")
				return true;
			Match match = Configuration.PhoneRegex.Match(phone);
			return match.Success;
		}
		public static bool ValidateID(string ID)
		{
			return (ID != null && ID != "" && ID.Length == 9);
		}
		public static bool ValidateLastDigit(string ID)
		{ 
			int[] NumArray = new int[9];
			int sum = 0;
			for (int i = 0; i < 9; i++)
			{
				NumArray[i] = (Convert.ToInt32(ID[i]) - '0') * (i % 2 + 1);
				if (NumArray[i] >= 10)
					NumArray[i] = NumArray[i] / 10 + NumArray[i] % 10;
				sum += NumArray[i];
			}
			return sum % 10 == 0;
		}

		public static bool ValidateName(Name name)
		{
			var FirstValidate = name.first != "" && name.first != null;
			var LastValidate = name.last != "" && name.last != null;
			return FirstValidate && LastValidate;
		}
		public static void ValidatePerson(Person p)
		{
			if (!ValidateID(p.ID))
				throw new InvalidOperationException("The ID must be 9 digits long!");
			if (!ValidateLastDigit(p.ID))
				throw new InvalidOperationException("The ID is invalid!");
			if (!ValidateName(p.Name))
				throw new InvalidOperationException("Please enter first and last name!");
			if (!ValidateEmail(p.Email))
				throw new InvalidOperationException("The email address is invalid");
			if (!ValidatePhone(p.PhoneNumber))
				throw new InvalidOperationException("The phone number is invalid");
		}
		public static void SendEmail(Person p, string subject, string content)
		{
			if (p.Email != null && p.Email != "")
			{
				MailMessage mail = new MailMessage(Configuration.SystemEmail, p.Email, subject, content);
				Thread thread = new Thread(() => Configuration.MailSender.send(mail));
				thread.Start();
			}
		}
		
		public static string CreateNewRandomPassword()
		{
			Random random = new Random();
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			return new string(Enumerable.Repeat(chars, 8)
			  .Select(s => s[random.Next(s.Length)]).ToArray());
		}
		public static bool IsHoliday(DateTime Day, out Holiday? holiday)
		{
			return HebCal.HolidayChecker.IsHoliday(Day, out holiday);
		}
		
		public static List<Test> TrueCopyTests(List<Test> other)
		{
			if (other == null) return null;
			List<Test> tests = new List<Test>();
			foreach (var t in other)
				tests.Add(new Test(t));
			return tests;
		}
		public static List<Tester> TrueCopyTesters(List<Tester> other)
		{
			if (other == null) return null;
			List<Tester> testers = new List<Tester>();
			foreach (var t in other)
				testers.Add(new Tester(t));
			return testers;
		}
		public static List<Trainee> TrueCopyTrainee(List<Trainee> other)
		{
			if (other == null) return null;
			List<Trainee> trainees = new List<Trainee>();
			foreach (var t in other)
				trainees.Add(new Trainee(t));
			return trainees;
		}

	}
		



}

