using System.Reflection;

namespace BE
{
    public class Tester:Person
	{
		public Tester(Tester other)
		{
			foreach (PropertyInfo property in other.GetType().GetProperties())
				property.SetValue(this, property.GetValue(other));
		}
		public Tester() { }
		public Schedule schedule;
		public uint MaxDistance { get; set; }
		public uint ExpYears { get; set; }
		public uint MaxWeeklyTests { get; set; }
		public CarType testingCarType { get; set; }
	}
}
