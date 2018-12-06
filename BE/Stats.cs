using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
	public class Stats
	{
		public Name teacherName { get; set; }
		public string schoolName { get; set; }
		public GearType gearType { get; set; }
		public int numOfLessons { get; set; }
		public int numOfTest { get; set; }
		public bool passed { get; set; }
	}
}
