using System;
using System.Collections.Generic;

namespace BE
{
    public class Trainee:Person
	{
		private GearType gearType;
		private string schoolName;
		private Name teacherName;
		private uint numOfClasses;
		public Dictionary<CarType, bool> passedTests;

		public Trainee()
		{
			foreach (CarType c in (CarType[])Enum.GetValues(typeof(CarType)))
			{
				passedTests.Add(c, false);
			}
		}

		public GearType GearType { get => gearType; set => gearType = value; }
		public string SchoolName { get => schoolName; set => schoolName = value; }
		public Name TeacherName { get => teacherName; set => teacherName = value; }
		public uint NumOfClasses { get => numOfClasses; set => numOfClasses = value; }

	}
}
