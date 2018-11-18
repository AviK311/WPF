using System;
using System.Collections.Generic;
using System.Text;

namespace BE
{
	public struct Address
	{
		string city, street, apptNumber;

		public override string ToString()
		{
			return string.Format("{0} {1}, {2}", apptNumber, street, city);
		}
	}
}
