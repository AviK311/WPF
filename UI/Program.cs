using System;
using BE;

namespace UI
{
    class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("What is your user type? 0 - Admin, 1 - Tester, 2 - Trainee");
			var user = (UserType)Convert.ToInt32(Console.ReadLine());
			Console.WriteLine("you are a {0}", user.ToString());
		}
	}
}
