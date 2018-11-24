using System;
using System.Collections.Generic;
using System.Text;

namespace BE
{  
	class Trainee:Person
	{ 
		private GearType gearType;
		private string schoolName;
		private Name teacherName;
		private uint numOfClasses;
		public GearType GearType { get => gearType; set => gearType = value; }
		public string SchoolName { get => schoolName; set => schoolName = value; }
		public Name TeacherName { get => teacherName; set => teacherName = value; }
		public uint NumOfClasses { get => numOfClasses; set => numOfClasses = value; }
	}
}
