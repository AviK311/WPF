﻿using System;
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
		/// <summary>
		/// This function Adds all items in args into the list
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="args"></param>
		public static void AddTemplateList<T>(List<T> list, params T[] args)
		{
			foreach (var item in args)
				list.Add(item);
		}
		/// <summary>
		/// this function checks if a string contains a valid email
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		public static bool ValidateEmail(string email)
		{
			if (email == null || email == "")
				return true;
			Match match = Configuration.EmailRegex.Match(email);
			return match.Success;
		}
		/// <summary>
		/// this function checks if string contains a valid phone number
		/// </summary>
		/// <param name="phone"></param>
		/// <returns></returns>
		public static bool ValidatePhone(string phone)
		{
			if (phone == null || phone == "")
				return true;
			Match match = Configuration.PhoneRegex.Match(phone);
			return match.Success;
		}
		/// <summary>
		/// this function checks if an ID string contains 9 number chars
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		public static bool ValidateID(string ID)
		{
			return (ID != null && ID != "" && ID.Length == 9 && ID.All(c => char.IsNumber(c)));
		}
		/// <summary>
		/// checks if the ID is a valid ID is israel
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
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
		/// <summary>
		/// checks that first and last name are not empty
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static bool ValidateName(Name name)
		{
			var FirstValidate = name.first != "" && name.first != null;
			var LastValidate = name.last != "" && name.last != null;
			return FirstValidate && LastValidate;
		}

		/// <summary>
		/// Validates ID, Name, Email, and Phone number of p
		/// </summary>
		/// <param name="p"></param>
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
		/// <summary>
		/// sends an email to p using the MailClient class. 
		/// the function uses a thread
		/// </summary>
		/// <param name="p">The person who will receive the email</param>
		/// <param name="subject">subject of the email</param>
		/// <param name="content">content of the email</param>
		public static void SendEmail(Person p, string subject, string content)
		{
			if (p.Email != null && p.Email != "")
			{
				MailMessage mail = new MailMessage(Configuration.SystemEmail, p.Email, subject, content);
			
				Thread thread = new Thread(() => Configuration.MailSender.send(mail));
				thread.Start();
			}
		}
		/// <summary>
		/// randomizes a password from letters and numbers
		/// </summary>
		/// <returns></returns>
		public static string CreateNewRandomPassword()
		{
			Random random = new Random();
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			return new string(Enumerable.Repeat(chars, 8)
			  .Select(s => s[random.Next(s.Length)]).ToArray());
		}
		/// <summary>
		/// Checks if a day is a holiday, using the HebCal class.
		/// if it is, the specific holiday is returned with holiday
		/// </summary>
		/// <param name="Day">day to check</param>
		/// <param name="holiday">if is a holiday, it will be returned here</param>
		/// <returns></returns>
		public static bool IsHoliday(DateTime Day, out Holiday? holiday)
		{
			return HebCal.HolidayChecker.IsHoliday(Day, out holiday);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="Day">day to convert</param>
		/// <returns>the hebrew representation of the date</returns>
		public static string GetHebrewDate(DateTime Day)
		{
			return HebCal.HolidayChecker.GetHebrewDate(Day);
		}
		/// <summary>
		/// Full copy constructor, using copy constructors
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public static List<Test> TrueCopyTests(List<Test> other)
		{
			if (other == null) return null;
			List<Test> tests = new List<Test>();
			foreach (var t in other)
				tests.Add(new Test(t));
			return tests;
		}
		/// <summary>
		/// Full copy constructor, using copy constructors
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public static List<Tester> TrueCopyTesters(List<Tester> other)
		{
			if (other == null) return null;
			List<Tester> testers = new List<Tester>();
			foreach (var t in other)
				testers.Add(new Tester(t));
			return testers;
		}
		/// <summary>
		/// Full copy constructor, using copy constructors
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public static List<Trainee> TrueCopyTrainee(List<Trainee> other)
		{
			if (other == null) return null;
			List<Trainee> trainees = new List<Trainee>();
			foreach (var t in other)
				trainees.Add(new Trainee(t));
			return trainees;
		}
		/// <summary>
		/// Full copy constructor, using copy constructors
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public static List<Admin> TrueCopyAdmin(List<Admin> other)
		{
			if (other == null) return null;
			List<Admin> admins = new List<Admin>();
			foreach (var a in other)
				admins.Add(new Admin(a));
			return admins;
		}
		/// <summary>
		/// returns false if all the address fields are empty
		/// </summary>
		/// <param name="ad">address to check</param>
		/// <returns></returns>
		public static bool IsAddress(Address ad)
		{
			if (ad.buildingNumber == "" || ad.buildingNumber == null)
				if (ad.city == "" || ad.city == null)
					if (ad.street == "" || ad.street == null)
						return false;
			return true;
		}

		

	}
		



}

