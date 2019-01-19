using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;


namespace BE
{
	public class HebCal
	{
		public static HebCal HolidayChecker = new HebCal();
		HebrewCalendar cal = new HebrewCalendar();
		
		Dictionary<string, Holiday> Holidays;
		/// <summary>
		/// initializes the static instance with the known holidays
		/// </summary>
		private HebCal()
		{
			Holidays = new Dictionary<string, Holiday>();
			Holidays.Add("12,29", Holiday.ErevRoshHashana);
			for (int i = 1; i < 3; i++)
				Holidays.Add("1," + i.ToString(), Holiday.RoshHashana);
			Holidays.Add("1,9", Holiday.ErevYomKippur);
			Holidays.Add("1,10",Holiday.YomKippur);
			Holidays.Add("1,14", Holiday.ErevSukkot);
			for (int i=15; i<22; i++)
				Holidays.Add("1,"+i.ToString(),Holiday.Sukkot);
			Holidays.Add("1,22", Holiday.ShminiAtzeret);
			Holidays.Add("7,14", Holiday.ErevPesach);
			for (int i = 15; i < 22; i++)
				Holidays.Add("7,"+i.ToString(), Holiday.Pesach);
			Holidays.Add("9,5", Holiday.ErevShavuot);
			Holidays.Add("9,6", Holiday.Shavuot);
		}
		/// <summary>
		/// checks if a given day falls out on a hebrew holiday.
		/// if it is, holiday will hold the specific holiday
		/// </summary>
		/// <param name="Day"></param>
		/// <param name="holiday"></param>
		/// <returns></returns>
		public bool IsHoliday(DateTime Day, out Holiday? holiday)
		{
			holiday = null;
			int month = cal.IsLeapYear(cal.GetYear(Day)) && cal.GetMonth(Day) > 7 ? cal.GetMonth(Day) - 1 : cal.GetMonth(Day);
			string Date = string.Format("{0},{1}", month, cal.GetDayOfMonth(Day));
			if (!Holidays.Any(T=>T.Key == Date))
				return false;
			holiday = Holidays[Date];
			return true;
		}
		public string GetHebrewDate(DateTime date)
		{
			string toReturn = "";
			int HebDay = cal.GetDayOfMonth(date);
			int HebYear = cal.GetYear(date);
			int hebMonth = cal.GetMonth(date);
			if (cal.IsLeapYear(HebYear))
			{
				if (hebMonth == 6) toReturn += "Adar A";
				else if (hebMonth == 7) toReturn += "Adar B";
				else if (hebMonth > 7) toReturn += (HebMonth)(hebMonth - 1);
				else toReturn += (HebMonth)(hebMonth);
			}
			else toReturn += (HebMonth)(hebMonth);
			toReturn += string.Format(" {0}, {1}", HebDay, HebYear);
			return toReturn;
		}
	}
}
