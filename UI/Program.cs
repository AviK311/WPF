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
        BL.IBL bl = FactoryBL.GetBL();
		static void AddTest()
		{
			Test toAdd = new Test();
			Console.WriteLine("What is the Trainee ID?");
			toAdd.TraineeID = Console.ReadLine();
			Console.WriteLine("What is the Tester ID?");
			toAdd.TesterID = Console.ReadLine();
			Console.WriteLine("When is the Test?");
			DateTime date;
			DateTime.TryParse(Console.ReadLine(), out date);
			toAdd.TestDateTime = date;
			Console.WriteLine("In what location will the test begin? type on seperate lines.");
			toAdd.BeginLocation = new Address(Console.ReadLine(), Console.ReadLine(), Console.ReadLine());
            Console.WriteLine("What is grade?");
            toAdd.Grade = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("if trinne keep Distance? 0 - false, 1 - true ");
            toAdd.IsDistance = Convert.ToBoolean(Convert.ToInt32(Console.ReadLine()));
            Console.WriteLine("if trinne succeed in Reverese? 0 - false, 1 - true ");
            toAdd.IsReverese = Convert.ToBoolean(Convert.ToInt32(Console.ReadLine()));
            Console.WriteLine("if trinne looked at the Mirror? 0 - false, 1 - true ");
            toAdd.IsMirror = Convert.ToBoolean(Convert.ToInt32(Console.ReadLine()));
            Console.WriteLine("if trinne signaled? 0 - false, 1 - true ");
            toAdd.IsSignal = Convert.ToBoolean(Convert.ToInt32(Console.ReadLine()));

            Console.WriteLine("if trinne followed the Signs? 0 - false, 1 - true ");
            toAdd.IsSigns = Convert.ToBoolean(Convert.ToInt32(Console.ReadLine()));
           // Console.WriteLine("if trinne Succeeded in the test? 0 - false, 1 - true ");
           
			BL.IBL bl = FactoryBL.GetBL();
			try { bl.AddTest(toAdd);  }
			catch (Exception e)	{
				Console.WriteLine(e.Message);
			}
		}
        static void RemoveTest()
        {
            Console.WriteLine("Enter the Test Number");  
            BL.IBL bl = FactoryBL.GetBL();
            try {bl.RemoveTest(Console.ReadLine()); }
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
        }
  
        static void AddTrainee()
        {
            Trainee toAdd = new Trainee();
			Console.WriteLine("What is the ID?");
            toAdd.ID = Console.ReadLine();			
		    Console.WriteLine("What is the Gender? 0 - Male, 1 - Female");
			toAdd.Sex = (BE.Gender)Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("What is the date of birth");
			DateTime date;
			DateTime.TryParse(Console.ReadLine(), out date);
			toAdd.TestDateTime = date;
            Console.WriteLine("What is the first name?");
            toAdd.FirstName = Console.ReadLine();
            Console.WriteLine("What is the last name?");
            toAdd.LastName = Console.ReadLine();
            Console.WriteLine("What is the PhoneNumber?");
            toAdd.PhoneNumber=Console.ReadLine();
			Console.WriteLine("What is the Address? type on seperate lines.");
            toAdd.Address = new Address(Console.ReadLine(), Console.ReadLine(), Console.ReadLine());
            Console.WriteLine("What type is the  vehicle? 0 - motorCycle, 1 -  privateCar, 2 - smallTruck, 3 - largeTruck");
            toAdd.currentCarType = (BE.CarType)Convert.ToInt32(Console.ReadLine());

			BL.IBL bl = FactoryBL.GetBL();
			try { bl.AddTest(toAdd);  }
			catch (Exception e)	{
				Console.WriteLine(e.Message);
			}
        }
        static void RemoveTrainee()
        {
            Console.WriteLine("Enter the Trainee ID");  
            BL.IBL bl = FactoryBL.GetBL();
            try {bl.RemoveTrainee(Console.ReadLine()); }
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
        }

		static void AddTester()
		{
			Tester toAdd = new Tester();
			Console.WriteLine("What is the ID?");
			toAdd.ID = Console.ReadLine();
			Console.WriteLine("What is the Gender? 0 - Male, 1 - Female");
			toAdd.Sex = (BE.Gender)Convert.ToInt32(Console.ReadLine());
			
			BL.IBL bl = FactoryBL.GetBL();
			try { bl.AddTester(toAdd); }
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
        static void RemoveTester()
        {
            Console.WriteLine("Enter the Tester ID");  
            BL.IBL bl = FactoryBL.GetBL();
            try {bl.RemoveTester(Console.ReadLine()); }
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
        }

		static void Main(string[] args)
		{

			int choice = 0;
			while (choice >= 0 && choice <= 11)
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
				try
				{
					choice = Convert.ToInt32(Console.ReadLine());
					if (choice < 0 || choice > 11)
						throw new InvalidOperationException("That choice is Invalid");
				}
				catch (Exception e) { Console.WriteLine(e.Message + ". Exiting..."); choice = 12; }
				switch (choice)
				{
					case 0: AddTest(); break;
					default: break;
				}
			}

		}
	}
}
