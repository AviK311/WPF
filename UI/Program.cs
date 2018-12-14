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
			{
				Console.WriteLine("There are no tests");
				return;
			}
			else foreach (var item in bl.GetTests())
					Console.WriteLine(item);
			bool sorted = false;
			int choice;
			Console.WriteLine("Choose one:");
			Console.WriteLine("0 - Show tests according to trainees");
			Console.WriteLine("1 - Show tests of specific trainee");
			Console.WriteLine("2 - Exit");
			choice = Convert.ToInt32(Console.ReadLine());
			switch (choice)
			{
				case 0:
					Console.WriteLine("Should the list be sorted according to trainee ID? 0 - no, 1 - yes");
					sorted = Convert.ToBoolean(Convert.ToInt32(Console.ReadLine()));
					foreach (var group in bl.TestGroupsAccordingToTrainee(sorted))
						foreach (var test in group)
							Console.WriteLine(test);
					break;
				case 1:
					Console.WriteLine("Please enter the Trainee ID");
					Trainee trainee;
					try { trainee = bl.GetTrainee(Console.ReadLine());
						var list = bl.TestGroupsAccordingToTrainee().FirstOrDefault(T => T.Key.Equals(trainee));
						if (list == null) throw new InvalidOperationException("That trainee has no tests");
						foreach (var test in list)
							Console.WriteLine(test);
					}
					catch (Exception e) { Console.WriteLine(e.Message); }
					break;
				default: return;
			}

			
		}
		static void ViewAllTesters()
		{
			IBL bl = FactoryBL.GetBL();
			if (bl.GetTesters().Count() == 0)
			{
				Console.WriteLine("There are no testers");
				return;
			}
			else foreach (var item in bl.GetTesters())
					Console.WriteLine(item);
			int choice;
			Console.WriteLine("Choose one:");
			Console.WriteLine("0 - Show testers that test on a specific car type");
			Console.WriteLine("1 - Exit");
			choice = Convert.ToInt32(Console.ReadLine());
			switch (choice)
			{
				case 0:
					Console.WriteLine("Which car type would you like to see?");
					foreach (var c in (CarType[])Enum.GetValues(typeof(CarType)))
						Console.WriteLine((int)c + " - " +  Func.InsertSpacesBeforeUpper(c.ToString()));
					try
					{
						CarType vehic = (CarType)Convert.ToInt32(Console.ReadLine());
						if (!Enum.IsDefined(typeof(CarType), vehic))
							throw new InvalidOperationException("That car type is out of range");
						var list = bl.TesterGroupsAccordingToCarType().FirstOrDefault(T => T.Key == vehic);
						if (list == null)
							throw new InvalidOperationException("No testers teach on that car type");
						foreach (var tester in list)
							Console.WriteLine(tester);
					}
					catch (Exception e) { Console.WriteLine(e.Message); }
					break;
				default: return;
			}


		}
		static void ViewAllTrainees()
		{
			IBL bl = FactoryBL.GetBL();
			if (bl.GetTrainees().Count() == 0)
			{
				Console.WriteLine("There are no trainees");
				return;
			}
			else foreach (var item in bl.GetTrainees())
					Console.WriteLine(item);
			int choice;
			Console.WriteLine("Choose one:");
			Console.WriteLine("0-  Show trainees according to the amount of tests they are signed up for");
			Console.WriteLine("1 - show trainees who've signed up for x tests");
			Console.WriteLine("2 - Show trainees who learn in a specific school");
			Console.WriteLine("3 - Show trainees who learn with a specific teacher");
			Console.WriteLine("4 - Exit");

			choice = Convert.ToInt32(Console.ReadLine());
			switch (choice)
			{
				case 0:
					Console.WriteLine("Which car type?");
					foreach (var c in (CarType[])Enum.GetValues(typeof(CarType)))
						Console.WriteLine((int)c + " - " + Func.InsertSpacesBeforeUpper(c.ToString()));
					try
					{
						CarType vehic = (CarType)Convert.ToInt32(Console.ReadLine());
						if (!Enum.IsDefined(typeof(CarType), vehic))
							throw new InvalidOperationException("That car type is out of range");
						bool sorted = false;
						Console.WriteLine("Should the list be sorted according to the number of tests? 0 - no, 1 - yes");
						sorted = Convert.ToBoolean(Convert.ToInt32(Console.ReadLine()));
						foreach (var group in bl.TraineesGroupsAccordingToTestsNum(vehic,sorted))
							foreach (var trainee in group)
								Console.WriteLine(trainee + " " + trainee.carTypeStats[vehic].numOfTest);
					}
					catch (Exception e) { Console.WriteLine(e.Message); }
					break;

				case 1:
					Console.WriteLine("Which car type?");
					foreach (var c in (CarType[])Enum.GetValues(typeof(CarType)))
						Console.WriteLine((int)c + " - " + Func.InsertSpacesBeforeUpper(c.ToString()));
					try
					{
						CarType vehic = (CarType)Convert.ToInt32(Console.ReadLine());
						if (!Enum.IsDefined(typeof(CarType), vehic))
							throw new InvalidOperationException("That car type is out of range");
						Console.WriteLine("Choose x:");
						int x = Convert.ToInt16(Console.ReadLine());
						var list = bl.TraineesGroupsAccordingToTestsNum(vehic).FirstOrDefault(T => T.Key == x);
						if (list == null)
							throw new InvalidOperationException("No trainees signed up for "+ x + " test/s");
						foreach (var trainee in list)
							Console.WriteLine(trainee);
					}
					catch (Exception e) { Console.WriteLine(e.Message); }
					break;

				case 2:
					Console.WriteLine("Which car type does the school teach?");
					foreach (var c in (CarType[])Enum.GetValues(typeof(CarType)))
						Console.WriteLine((int)c + " - " + Func.InsertSpacesBeforeUpper(c.ToString()));
					try
					{
						CarType vehic = (CarType)Convert.ToInt32(Console.ReadLine());
						if (!Enum.IsDefined(typeof(CarType), vehic))
							throw new InvalidOperationException("That car type is out of range");
						Console.WriteLine("What is the name of the school?");
						string schoolName = Console.ReadLine();
						var list = bl.TraineesGroupsAccordingToSchoolName(vehic).FirstOrDefault(T => T.Key == schoolName);
						if (list == null)
							throw new InvalidOperationException("No trainees learn in " + schoolName);
						foreach (var trainee in list)
							Console.WriteLine(trainee);
					}
					catch (Exception e) { Console.WriteLine(e.Message); }
					break;
				case 3:
					Console.WriteLine("Which car type does the teacher teach?");
					foreach (var c in (CarType[])Enum.GetValues(typeof(CarType)))
						Console.WriteLine((int)c + " - " + Func.InsertSpacesBeforeUpper(c.ToString()));
					try
					{
						CarType vehic = (CarType)Convert.ToInt32(Console.ReadLine());
						if (!Enum.IsDefined(typeof(CarType), vehic))
							throw new InvalidOperationException("That car type is out of range");
						Console.WriteLine("What is the name of the teacher? enter in two lines");
						Name teacherName = new Name(Console.ReadLine(), Console.ReadLine());
						var list = bl.TraineesGroupsAccordingToTeacherName(vehic).FirstOrDefault(T => T.Key.Equals(teacherName));
						if (list == null)
							throw new InvalidOperationException("No trainees learn with " + teacherName);
						foreach (var trainee in list)
							Console.WriteLine(trainee);
					}
					catch (Exception e) { Console.WriteLine(e.Message); }
					break;
				default: return;
			}
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
				Console.WriteLine("Choose one:");
				for (int i = 0; i < actionList.Count; i++)
					Console.WriteLine(i + " - " + Func.InsertSpacesBeforeUpper(actionList[i].Method.Name));
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
				Sex = Gender.Male,
				PhoneNumber = "123",
				BirthDay = new DateTime(1993, 11, 3),
				Address = new Address("bet shemesh", "nahal maor", "19"),
				CurrentCarType = CarType.LargeTruck,
			};
			Trainee b = new Trainee
			{
				ID = "456",
				Name = new Name("Sapir", "Barabi"),
				Sex = Gender.Female,
				PhoneNumber = "456",
				BirthDay = new DateTime(1993, 5, 14),
				Address = new Address("Kiryat Ata", "David Remez", "1a"),
				CurrentCarType = CarType.LargeTruck,

			};

			Tester c = new Tester
			{
				ID = "123456",
				Name = new Name("Dan", "internatonal"),
				Sex = Gender.Male,
				PhoneNumber = "224",
				BirthDay = new DateTime(1969, 2, 1),
				Address = new Address("jeru", "vaad", "21"),
				MaxDistance = 60,
				MaxWeeklyTests = 9,
				ExpYears = 6,
				testingCarType = CarType.PrivateCar,
				schedule = new Schedule()
			};
			IBL bl = FactoryBL.GetBL();
			bl.AddTester(c);
			bl.AddTrainee(a);
			bl.AddTrainee(b);
		}
	}
}
