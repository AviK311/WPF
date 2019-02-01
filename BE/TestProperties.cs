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
				if (p.CanWrite)p.SetValue(this, false);
		}
		public TestProperties(TestProperties other)
		{
			foreach (PropertyInfo p in typeof(TestProperties).GetProperties())
				if (p.CanWrite) p.SetValue(this, p.GetValue(other));
		}
		
		public bool IsKeepingDistance { get; set; }
		public bool IsDrivingInReverse { get; set; }
		public bool IsMirrorUsage { get; set; }
		public bool IsSignaling { get; set; }
		public bool IsSignsReading { get; set; }
		public bool IsBreaking { get; set; }
		/// <summary>
		/// get only property that determines whether the test was passed
		/// </summary>
		public bool passed
		{
			get
			{
				var info = GetType().GetProperties();
				int passedTests = 0;
				foreach (var item in info)
					if (item.CanWrite&& (bool)item.GetValue(this)) passedTests++;	
				double grade = (double)passedTests / info.Length;
				return grade >= Configuration.MinPassGrade;
			}
		}
	}
}
