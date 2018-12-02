using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using BL;

namespace UI
{
	class Program
	{
		static void AdminFlow()
		{
			Console.WriteLine("choose one: \n" +
						"0 - Add a Test\n" +
						"1 - Remove a Test\n" +
						"2 - Update a Test\n" +
						"3 - Add a Trainee\n" +
						"4 - Remove a Trainee\n" +
						"5 - Update a Trainee\n" +
						"6 - Add a Tester\n" +
						"7 - Remove a Trainee\n" +
						"8 - Update a Trainee\n" +
						"9 - View all Tests\n" +
						"10 - View all Trainees\n" +
						"11 - View all Testers\n" +
						"else - Exit");

		}
		static void TesterFlow()
		{
			Console.WriteLine("choose one: \n" +
						"0 - Add a Test\n" +
						"1 - Remove a Test\n" +
						"2 - Update a Test\n" +
						"3 - Add a Trainee\n" +
						"4 - View all Tests\n" +
						"5 - View all Trainees\n" +
						"else - Exit");
		}
		static void TraineeFlow()
		{
			Console.WriteLine("choose one: \n" +
						"0 - View all Tests\n" +
						"1 - View all Testers\n" +
						"else - Exit");
		}
		
		
		static void Main(string[] args)
		{
			BL.IBL bl = new Bl_imp();
			Console.WriteLine("What is your user type? 0 - Admin, 1 - Tester, 2 - Trainee");
			int userNumber = -1;
			while (userNumber < 0 || userNumber > 2) 
			try
			{
				userNumber = Convert.ToInt32(Console.ReadLine());
				if (userNumber > 2 || userNumber < 0) throw new InvalidOperationException("The choice is invalid");
			}catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			
			switch ((UserType)userNumber)
			{
				case UserType.Admin: AdminFlow(); break;
				case UserType.Tester: TesterFlow(); break;
				case UserType.Trainee: TraineeFlow();break;
			}
			
		}
	}
}
