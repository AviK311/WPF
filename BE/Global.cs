using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BE
{
	public class GlobalSettings
	{
		public static UserType AppClearanceLevel { get; set; } = UserType.Trainee;
		public static Person User { get; set; }
		public static bool AlreadyLoggedIn { get; set; }
		public static MailClient MailSender { get; set; }
		public static Regex EmailRegex;
		public static string SystemEmail { get; } = "CsharpProject5779@gmail.com";


	}
}
