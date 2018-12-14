using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
	public class Func
	{
		public static string InsertSpacesBeforeUpper(string str)
		{
			for (int i = 1; i < str.Length; i++)
				if (char.IsUpper(str[i]))
					str = str.Substring(0, i) + ' ' + str.Substring(i++);
			return str;
		}
	}
}
