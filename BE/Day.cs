using System;
using System.Collections.Generic;
using System.Text;

namespace BE
{  
	public class Day
	{
		public bool[] hours = new bool[8];
		public bool this[int index]
		{
			get => hours[index - 9];
		}
	}
}
