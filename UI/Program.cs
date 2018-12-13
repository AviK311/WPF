using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using BL;
using System.Reflection;
using System.Text.RegularExpressions;

namespace UI
{
	/// <summary>
	/// for practice
	/// </summary>
	//class Program
	//{
	//	static void Main(string[] args)
	//	{
	//		Test t = new Test();
	//		Console.Write("Tests Passed: ".Length);
	//		Console.WriteLine(t.ToLongString());
	//	}
	//}
	class Program
	{ 
		

		static void ViewTest()
		{
			Console.WriteLine("Please enter the Test Number");
			IBL bl = FactoryBL.GetBL();
			try { Console.WriteLine(bl.GetTest(Console.ReadLine()).ToLongString()); }
			catch (Exception e) { Console.WriteLine(e.Message); }
		}
		static void ViewTrainee()
		{
			Console.WriteLine("Please enter the Trainee ID");
			IBL bl = FactoryBL.GetBL();
			try { Console.WriteLine(bl.GetTrainee(Console.ReadLine()).ToLongString()); }
			catch (Exception e) { Console.WriteLine(e.Message); }
		}
		static void ViewTester()
		{
			Console.WriteLine("Please enter the Tester ID");
			IBL bl = FactoryBL.GetBL();
			try { Console.WriteLine(bl.GetTester(Console.ReadLine()).ToLongString()); }
			catch (Exception e) { Console.WriteLine(e.Message); }
		}
		static void ViewAllTests()
		{
			IBL bl = FactoryBL.GetBL();
			if (bl.GetTests().Count() == 0)
				Console.WriteLine("There are no tests");
			else foreach (var item in bl.GetTests())
				Console.WriteLine(item);
		}
		static void ViewAllTesters()
		{
			IBL bl = FactoryBL.GetBL();
			if (bl.GetTesters().Count() == 0)
				Console.WriteLine("There are no testers");
			else foreach (var item in bl.GetTesters())
				Console.WriteLine(item);
		}
		static void ViewAllTrainees()
		{
			IBL bl = FactoryBL.GetBL();
			if (bl.GetTrainees().Count() == 0)
				Console.WriteLine("There are no trainees");
			else foreach (var item in bl.GetTrainees())
				Console.WriteLine(item);
		}

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
			BL.IBL bl = FactoryBL.GetBL();
			try { bl.AddTest(toAdd); }
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
		static void RemoveTest()
		{
			Console.WriteLine("Enter the Test Number");
			BL.IBL bl = FactoryBL.GetBL();
			try { bl.RemoveTest(Console.ReadLine()); }
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
			Console.WriteLine("What is the date of birth?");
			DateTime date;
			DateTime.TryParse(Console.ReadLine(), out date);
			toAdd.BirthDay = date;
			Console.WriteLine("What is the name? type on seperate lines");
			toAdd.Name = new Name(Console.ReadLine(), Console.ReadLine());
			Console.WriteLine("What is the PhoneNumber?");
			toAdd.PhoneNumber = Console.ReadLine();
			Console.WriteLine("What is the Address? type on seperate lines.");
			toAdd.Address = new Address(Console.ReadLine(), Console.ReadLine(), Console.ReadLine());
			Console.WriteLine("What type is the  vehicle? 0 - motorCycle, 1 -  privateCar, 2 - smallTruck, 3 - largeTruck");
			toAdd.CurrentCarType = (BE.CarType)Convert.ToInt32(Console.ReadLine());

			BL.IBL bl = FactoryBL.GetBL();
			try { bl.AddTrainee(toAdd); }
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
		static void RemoveTrainee()
		{
			Console.WriteLine("Enter the Trainee ID");
			BL.IBL bl = FactoryBL.GetBL();
			try { bl.RemoveTrainee(Console.ReadLine()); }
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
			Console.WriteLine("What is the date of birth?");
			DateTime date;
			DateTime.TryParse(Console.ReadLine(), out date);
			toAdd.BirthDay = date;
			Console.WriteLine("What is the name? type on seperate lines");
			toAdd.Name = new Name(Console.ReadLine(), Console.ReadLine());
			Console.WriteLine("What is the PhoneNumber?");
			toAdd.PhoneNumber = Console.ReadLine();
			Console.WriteLine("What is the Address? type on seperate lines.");
			toAdd.Address = new Address(Console.ReadLine(), Console.ReadLine(), Console.ReadLine());
			Console.WriteLine("What type is the  vehicle? 0 - motorCycle, 1 -  privateCar, 2 - smallTruck, 3 - largeTruck");
			toAdd.testingCarType = (BE.CarType)Convert.ToInt32(Console.ReadLine());
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
			try { bl.RemoveTester(Console.ReadLine()); }
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
		static string InsertSpacesBeforeUpper(string str)
		{
			for(int i = 1; i< str.Length; i++)
				if (char.IsUpper(str[i]))
					str = str.Substring(0, i) + ' ' + str.Substring(i++);
			return str;
		}
		public delegate void mainFunctions();
		static void Main(string[] args)
		{
			init();
			
			List<Action> actionList = new List<Action>();
			actionList.Add(AddTest);
			actionList.Add(RemoveTest);
			//actionList.Add(updateTest);
			actionList.Add(AddTrainee);
			actionList.Add(RemoveTrainee);
			//actionList.Add(updateTrainee);
			actionList.Add(AddTester);
			actionList.Add(RemoveTester);
			//actionList.Add(updateTester);
			actionList.Add(ViewAllTests);
			actionList.Add(ViewAllTrainees);
			actionList.Add(ViewAllTesters);
			actionList.Add(ViewTest);
			actionList.Add(ViewTrainee);
			actionList.Add(ViewTester);
			int choice = 0;
			while (choice >= 0 && choice < actionList.Count)
			{
				Console.WriteLine("choose one:");
				for (int i = 0; i < actionList.Count; i++)
					Console.WriteLine(i + " - " + InsertSpacesBeforeUpper(actionList[i].Method.Name));
				try
				{
					choice = Convert.ToInt32(Console.ReadLine());
					if (choice < 0 || choice > actionList.Count)
						throw new InvalidOperationException("That choice is Invalid");
				}
				catch (Exception e) { Console.WriteLine(e.Message + ". Exiting..."); choice = actionList.Count; }
				if (choice != actionList.Count) actionList[choice]();
			}

		}
		static void init()
		{
			Trainee a = new Trainee
			{
				ID = "123",
				Name = new Name("Avi", "Levi"),
				Sex = Gender.male,
				PhoneNumber = "123",
				BirthDay = new DateTime(1993, 11, 3),
				Address = new Address("bet shemesh", "nahal maor", "19"),
				CurrentCarType = CarType.largeTruck,
			};
			Trainee b = new Trainee
			{
				ID = "456",
				Name = new Name("Sapir", "Barabi"),
				Sex = Gender.female,
				PhoneNumber = "456",
				BirthDay = new DateTime(1993, 5, 14),
				Address = new Address("Kiryat Ata", "David Remez", "1a"),
				CurrentCarType = CarType.largeTruck,

			};

			Tester c = new Tester
			{
				ID = "123456",
				Name = new Name("Dan", "internatonal"),
				Sex = Gender.male,
				PhoneNumber = "224",
				BirthDay = new DateTime(1969, 2, 1),
				Address = new Address("jeru", "vaad", "21"),
				MaxDistance = 60,
				MaxWeeklyTests = 9,
				ExpYears = 6,
				testingCarType = CarType.privateCar,
				schedule = new Schedule()
			};
			IBL bl = FactoryBL.GetBL();
			bl.AddTester(c);
			bl.AddTrainee(a);
			bl.AddTrainee(b);
		}
	}
}
