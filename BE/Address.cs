using System;
using System.Collections.Generic;
using System.Text;

namespace BE
{ 
	public struct Address
	{   
        //try3
		public string city, street, buildingNumber;

		public override string ToString()
		{
			return string.Format("{0} {1}, {2}", buildingNumber, street, city);
		}
	}
}
