using System;
using System.Reflection;

namespace BE
{
    public class Tester:Person
	{
		public Tester(Tester other)
		{
			foreach (FieldInfo field in other.GetType().GetFields())
				field.SetValue(this, field.GetValue(other));
			foreach (PropertyInfo property in other.GetType().GetProperties())
				property.SetValue(this, property.GetValue(other));
		}
		public Tester() {
			schedule = new Schedule();
		}
		public Schedule schedule;
		public uint MaxDistance { get; set; }
		public uint ExpYears { get; set; }
		public uint MaxWeeklyTests { get; set; }
		public CarType testingCarType { get; set; }
		public override string ToString()
		{
			return string.Format("Name: {0}, ID: {1}, CarType: {2}\n",
									Name, ID, Func.InsertSpacesBeforeUpper(testingCarType.ToString()));
		}
		public string ToLongString()
		{
			
			string toReturn = string.Format("Name: {0}\n", Name);
			toReturn += string.Format("Gender: {0}\n", Sex);
			toReturn += string.Format("ID: {0}\n", ID);
			toReturn += string.Format("CarType: {0}\n", Func.InsertSpacesBeforeUpper(testingCarType.ToString()));
			toReturn += string.Format("Phone Number: {0}\n", PhoneNumber);
			toReturn += string.Format("Address: {0}\n", Address);
			toReturn += string.Format("Years Of Experience: {0}\n", ExpYears);
			toReturn += string.Format("Availability:\n {0}\n", schedule);

			return toReturn;
								
		}
	}
}
