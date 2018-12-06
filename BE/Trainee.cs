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
			foreach (PropertyInfo property in other.GetType().GetProperties())
				property.SetValue(this, property.GetValue(other));
		}
		public Trainee()
		{
			foreach (var c in (CarType[])Enum.GetValues(typeof(CarType)))
				carTypeStats.Add(c, new Stats());
		}
		public CarType currentCarType;
	}
}
