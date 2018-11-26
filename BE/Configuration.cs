namespace BE
{
    public class Configuration
    {
		private static int testCode = 0;
		public static int TestCode { get => testCode; set => testCode = value; }
        public static uint TimeBetweenTests { get; } = 7;
        public static uint MinAgeOfTrainee { get; } = 18;
        public static uint MinNumOfClasses { get; } = 20;
        public static uint MinAgeOfTester { get; } = 40;
		

		public Configuration()
        {

        }
    }
}
