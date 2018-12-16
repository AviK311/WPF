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
			testProperties = new TestProperties(other.testProperties);
		}
		public Test()
		{
			testProperties = new TestProperties();
		}
		public string TestNumber { get; set; }
		public string TesterID { get; set; }
		public string TraineeID { get; set; }
		public DateTime TestDateTime { get; set; }
		public Address BeginLocation { get; set; }

		public TestProperties testProperties;
		public override string ToString()
		{
			string toReturn = string.Format("Test Number: {0},", TestNumber);
			toReturn += string.Format("Tester ID: {0},", TesterID);
			toReturn += string.Format("Trainee ID: {0},", TraineeID);
			toReturn += string.Format("Date and Time: {0}, {1}:00", TestDateTime.ToShortDateString(), TestDateTime.Hour);


			return toReturn;
		}
		public string ToLongString()
		{
			return string.Format(
				"Test number: {0}\n" +
				"Tester ID: {1}\n" +
				"Trainee ID: {2}\n" +
				"Date and Time: {3}, {4}:00\n" +
				"Begin location: {5}\n" +
				"Passed: {6}\n",
				TestNumber, TesterID, TraineeID,
				TestDateTime.ToShortDateString(), TestDateTime.Hour,
				BeginLocation, testProperties.passed());
		}
	}
}
