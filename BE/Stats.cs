using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
	public class Stats
	{
		public Stats()
		{
			numOfLessons = 0;
			numOfTest = 0;
			teacherName = new Name();
		}

		public Stats(Stats other)
		{
			foreach(PropertyInfo property in typeof(Stats).GetProperties())
				property.SetValue(this, property.GetValue(other));
		}

		public Name teacherName { get; set; }
		public string schoolName { get; set; }
		public GearType gearType { get; set; }
		public int numOfLessons { get; set; }
		public int numOfTest { get; set; }
		public bool passed { get; set; }
		
	}
}
