using System;
using System.Collections.Generic;
using System.Text;

namespace BE
{ 
	public struct Address
	{   
        //try3
		string city, street, apptNumber;

		public override string ToString()
		{
			return string.Format("{0} {1}, {2}", apptNumber, street, city);
		}
	}
}
