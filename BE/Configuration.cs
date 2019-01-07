using System.Text.RegularExpressions;

namespace BE
{
    public class Configuration
    {
		
		public static int TestCode { get; set; }
		public static int MessageCode { get; set; }
        public static uint TimeBetweenTests { get; } = 7;
        public static uint MinAgeOfTrainee { get; } = 18;
        public static uint MinNumOfClasses { get; } = 20;
        public static uint MinAgeOfTester { get; } = 40;
		public static double MinPassGrade { get; } = 0.6;
		public static Regex EmailRegex { get; } = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
		public static Regex PhoneRegex { get; } = new Regex(@"\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})");
		public static string SystemEmail { get; } = "CsharpProject5779@gmail.com";
		public static MailClient MailSender { get; set; } = new MailClient();
		public Configuration()
        {

        }
		
	}
}
