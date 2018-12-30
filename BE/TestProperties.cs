using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
	public class TestProperties
	{
		public TestProperties()
		{
			foreach (PropertyInfo p in typeof(TestProperties).GetProperties())
				p.SetValue(this, null);
		}
		public TestProperties(TestProperties other)
		{
			foreach (PropertyInfo p in typeof(TestProperties).GetProperties())
				p.SetValue(this, p.GetValue(other));
		}
		
		public bool KeepingDistance { get; set; }
		public bool DrivingInReverse { get; set; }
		public bool MirrorUsage { get; set; }
		public bool Signaling { get; set; }
		public bool SignsReading { get; set; }
		public bool Breaking { get; set; }

		public bool passed()
		{
			var info = GetType().GetProperties();
			int passedTests = 0;
			foreach (var item in info)
				if (item.Name.Contains("Is") && (bool)item.GetValue(this)) passedTests++;
			double grade = (double)passedTests / info.Length;
			return grade >= Configuration.MinPassGrade;
		}
	}
}
