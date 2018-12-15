using System;
using System.Collections.Generic;
using System.Reflection;

namespace BE
{
    public class Trainee:Person
	{
		public Dictionary<VehicleType, Stats> carTypeStats;
		public Trainee(Trainee other)
		{
			foreach (PropertyInfo property in other.GetType().GetProperties())
				property.SetValue(this, property.GetValue(other));
			carTypeStats = new Dictionary<VehicleType, Stats>();
			foreach (var item in other.carTypeStats)
				carTypeStats.Add(item.Key, new Stats(item.Value));
		}
		public Trainee()
		{
			carTypeStats = new Dictionary<VehicleType, Stats>();
			foreach (var c in (VehicleType[])Enum.GetValues(typeof(VehicleType)))
			{
				carTypeStats.Add(c, new Stats());
			}
		}

		public VehicleType CurrentCarType { get; set; }

		public override string ToString()
		{
			return string.Format("Name: {0}, ID: {1}, CarType: {2}",
									Name, ID, Functions.InsertSpacesBeforeUpper(CurrentCarType.ToString()));
		}
		public string ToLongString()
		{
			string toReturn = string.Format("Name: {0}\n", Name);
			toReturn += string.Format("Gender: {0}\n", Sex);
			toReturn += string.Format("ID: {0}\n", ID);
			toReturn += string.Format("CarType: {0}\n", Functions.InsertSpacesBeforeUpper(CurrentCarType.ToString()));
			toReturn += string.Format("Phone Number: {0}\n", PhoneNumber);
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
