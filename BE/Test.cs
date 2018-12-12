using System;
using System.Reflection;

namespace BE
{

    public class Test
	{
		public Test(Test other)
		{
			foreach (PropertyInfo property in other.GetType().GetProperties())
				property.SetValue(this, property.GetValue(other));
			
		}
		public Test() { }
		public string TestNumber { get; set; }
		public string TesterID { get; set; }
		public string TraineeID { get; set; }
		public DateTime TestDateTime { get; set; }
		public Address BeginLocation { get; set; }

		public TestProperties testProperties;

		public override string ToString()
		{
			return string.Format(
				"Test number: {0}\n" +
				"Tester ID: {1}\n" +
				"Trainee ID: {2}\n" +
				"Date and Time: {3}\n" +
				"Begin location: {4}\n" +
				"Passed: {5}\n",
				TestNumber, TesterID, TraineeID,
				TestDateTime.ToShortDateString() + TestDateTime.Hour,
				BeginLocation.ToString(), testProperties.passed());
		}
	}
}
