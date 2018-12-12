using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
	public struct TestProperties
	{
		public bool IsDistance { get; set; }
		public bool IsReverese { get; set; }
		public bool IsMirror { get; set; }
		public bool IsSignal { get; set; }
		public bool IsSigns { get; set; }
		public bool IsBreak { get; set; }

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
