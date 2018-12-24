using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace UI_WPF
{
	public class Global
	{
		public static ClearanceLevel appClearanceLevel = ClearanceLevel.Trainee;
		public static Person user;

		public static bool IsUpdate { get; set; } = false;
	}
}
