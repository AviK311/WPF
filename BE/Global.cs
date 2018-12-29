using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
	public class Global
	{
		public static ClearanceLevel appClearanceLevel = ClearanceLevel.Trainee;
		public static Person user;
		public static bool alreadyLoggedIn;
		public static bool IsUpdate { get; set; } = false;
		
	}
}
