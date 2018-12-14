using System;
using System.Collections.Generic;
using System.Reflection;

namespace BE
{
    public class Trainee:Person
	{
		public Dictionary<CarType, Stats> carTypeStats;
		public Trainee(Trainee other)
		{
			foreach (FieldInfo field in other.GetType().GetFields())
				field.SetValue(this, field.GetValue(other));
			foreach (PropertyInfo property in other.GetType().GetProperties())
				property.SetValue(this, property.GetValue(other));
		}
		public Trainee()
		{
			carTypeStats = new Dictionary<CarType, Stats>();
			foreach (var c in (CarType[])Enum.GetValues(typeof(CarType)))
			{
				carTypeStats.Add(c, new Stats());
			}
		}

		public CarType CurrentCarType { get; set; }

		public override string ToString()
		{
			return string.Format("Name: {0}, ID: {1}, CarType: {2}",
									Name, ID, Func.InsertSpacesBeforeUpper(CurrentCarType.ToString()));
		}
		public string ToLongString()
		{
			string toReturn = string.Format("Name: {0}\n", Name);
			toReturn += string.Format("Gender: {0}\n", Sex);
			toReturn += string.Format("ID: {0}\n", ID);
			toReturn += string.Format("CarType: {0}\n", Func.InsertSpacesBeforeUpper(CurrentCarType.ToString()));
			toReturn += string.Format("PhoneNumber: {0}\n", PhoneNumber);
			toReturn += string.Format("Address: {0}\n", Address);
			string passedCars = "Tests Passed: ";
			foreach (var item in carTypeStats)
				if (item.Value.passed)
					passedCars += string.Format("{0}, ", item.Key);
			if (passedCars == "Tests Passed: ")
				passedCars += "none";
			toReturn += passedCars + "\n";
			return toReturn;

		}
	}
}
