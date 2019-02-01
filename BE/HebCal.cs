using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;


namespace BE
{
	public class HebCal
	{
		public static HebCal HolidayChecker = new HebCal();
		HebrewCalendar cal = new HebrewCalendar();
		CultureInfo culture;
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

			culture = CultureInfo.CreateSpecificCulture("he-IL");
						
		}
		/// <summary>
		/// checks if a given day falls out on a hebrew holiday.
		/// if it is, holiday will hold the specific holiday
		/// </summary>
		/// <param name="Day">the day to check</param>
		/// <param name="holiday">the returning holiday</param>
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
		/// <summary>
		/// Returns a string containing the IL culture format of the date
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public string GetHebrewDate(DateTime date)
		{
			culture.DateTimeFormat.Calendar = cal;
			Thread.CurrentThread.CurrentCulture = culture;
			string toReturn = date.ToShortDateString();
			culture.DateTimeFormat.Calendar = culture.Calendar;
			return toReturn;

		}
       

    }
}
