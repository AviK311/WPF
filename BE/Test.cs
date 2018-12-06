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
		public string TestNumber { get; set; }
		public string TesterID { get; set; }
		public string TraineeID { get; set; }
		public DateTime TestDateTime { get; set; }
		public Address BeginLocation { get; set; }
		public bool IsDistance { get; set; }
		public bool IsReverese { get; set; }
		public bool IsMirror { get; set; }
		public bool IsSignal { get; set; }
		public ushort Grade { get; set; }
		public bool IsSigns { get; set; }
		public bool IsBreak { get; set; }

		public override string ToString()
		{
			return string.Format(
				"Test number: {0}\n" +
				"Tester ID: {1}\n" +
				"Trainee ID: {2}\n" +
				"Date and Time: {3}\n" +
				"Begin location: {4}\n" +
				"Grade: {5}\n",
				TestNumber, TesterID, TraineeID,
				TestDateTime.ToShortDateString() + TestDateTime.ToShortTimeString(),
				BeginLocation.ToString(), Grade);
		}
	}
}
