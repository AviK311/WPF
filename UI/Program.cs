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
	//class Program
	//{
	//		static void Main(string[] args)
	//	{
	//		List<int> mylist = new List<int>();
	//		mylist.Add(1);
	//		mylist.Add(2);
	//		mylist.Add(3);

	//		mylist.Add(4);

	//		mylist.Add(5);

	//		mylist.Add(1);

	//		mylist.Add(1);
	//		mylist.Remove(1);


	//	}


	//}
	class Program
	{
		static void UpdateTrainee()
		{
			Console.WriteLine("Please enter the Trainee ID");
			IBL bl = FactoryBL.GetBL();
			Trainee toUpdate;
			try
			{
				toUpdate = bl.GetTrainee(Console.ReadLine());
				Console.WriteLine(toUpdate.ToLongString());
			}
			catch (Exception e) { Console.WriteLine(e.Message); return; }
			Console.WriteLine("You will be prompted to update fields. Press enter to ignore.");
			Console.WriteLine("What is the Phone Number?");
			toUpdate.PhoneNumber = Console.ReadLine();
			Console.WriteLine("What is the Address? type on seperate lines: city, street name, building number.");
			toUpdate.Address = new Address(Console.ReadLine(), Console.ReadLine(), Console.ReadLine());

			Console.WriteLine("What type is the vehicle the trainee is learning on?");
			PrintVehicleTypes();
			int v = (int)toUpdate.CurrentCarType;
			int.TryParse(Console.ReadLine(), out v);
			toUpdate.CurrentCarType = (BE.VehicleType)v;
			Console.WriteLine("What gear type is the trainee learning on?");
			int g = (int)toUpdate.carTypeStats[toUpdate.CurrentCarType].gearType;
			int.TryParse(Console.ReadLine(), out g);
			toUpdate.carTypeStats[toUpdate.CurrentCarType].gearType = (GearType)g;




		}
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
					try
					{
						trainee = bl.GetTrainee(Console.ReadLine());
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
					PrintVehicleTypes();
					try
					{
						VehicleType vehic = (VehicleType)Convert.ToInt32(Console.ReadLine());
						if (!Enum.IsDefined(typeof(VehicleType), vehic))
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
					PrintVehicleTypes();
					try
					{
						VehicleType vehic = (VehicleType)Convert.ToInt32(Console.ReadLine());
						if (!Enum.IsDefined(typeof(VehicleType), vehic))
							throw new InvalidOperationException("That car type is out of range");
						bool sorted = false;
						Console.WriteLine("Should the list be sorted according to the number of tests? 0 - no, 1 - yes");
						sorted = Convert.ToBoolean(Convert.ToInt32(Console.ReadLine()));
						foreach (var group in bl.TraineesGroupsAccordingToTestsNum(vehic, sorted))
							foreach (var trainee in group)
								Console.WriteLine(trainee + " " + trainee.carTypeStats[vehic].numOfTest);
					}
					catch (Exception e) { Console.WriteLine(e.Message); }
					break;

				case 1:
					Console.WriteLine("Which car type?");
					PrintVehicleTypes();
					try
					{
						VehicleType vehic = (VehicleType)Convert.ToInt32(Console.ReadLine());
						if (!Enum.IsDefined(typeof(VehicleType), vehic))
							throw new InvalidOperationException("That car type is out of range");
						Console.WriteLine("Choose x:");
						int x = Convert.ToInt32(Console.ReadLine());
						var list = bl.TraineesGroupsAccordingToTestsNum(vehic).FirstOrDefault(T => T.Key == x);
						if (list == null)
							throw new InvalidOperationException("No trainees signed up for " + x + " test/s");
						foreach (var trainee in list)
							Console.WriteLine(trainee);
					}
					catch (Exception e) { Console.WriteLine(e.Message); }
					break;

				case 2:
					Console.WriteLine("Which car type does the school teach?");
					PrintVehicleTypes();
					try
					{
						VehicleType vehic = (VehicleType)Convert.ToInt32(Console.ReadLine());
						if (!Enum.IsDefined(typeof(VehicleType), vehic))
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
					PrintVehicleTypes();
					try
					{
						VehicleType vehic = (VehicleType)Convert.ToInt32(Console.ReadLine());
						if (!Enum.IsDefined(typeof(VehicleType), vehic))
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

		private static void PrintVehicleTypes()
		{
			foreach (var c in (VehicleType[])Enum.GetValues(typeof(VehicleType)))
				Console.WriteLine((int)c + " - " + Functions.InsertSpacesBeforeUpper(c.ToString()));
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
			Console.WriteLine("In what location will the test begin? type on seperate lines: city, street name, building number.");
			toAdd.BeginLocation = new Address(Console.ReadLine(), Console.ReadLine(), Console.ReadLine());
			BL.IBL bl = FactoryBL.GetBL();
			try { bl.AddTest(toAdd); }
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				if (e.Message != "The tester is unavailable")
					return;
				Console.WriteLine("Choose one:");
				Console.WriteLine("0 - choose another available time with same tester");
				Console.WriteLine("1 - choose another tester with the same time");
				int choice = Convert.ToInt32(Console.ReadLine());
				switch (choice)
				{

					case 0:
						var availableDates = bl.otherAvailableTestTimes(bl.GetTester(toAdd.TesterID), toAdd.TestDateTime);
						if (availableDates.Count() == 0)
						{
							Console.WriteLine("The tester is not available during {0}-{1}.",
								toAdd.TestDateTime.AddDays(-2).ToShortDateString(),
								toAdd.TestDateTime.AddDays(2).ToShortDateString());
							break;
						}
						Console.WriteLine("These are the available times for the tester:");
						foreach (var d in availableDates)
							Console.WriteLine("{0}, {1}:00", d.ToShortDateString(), d.Hour);
						break;
					case 1:
						var availableTesters = bl.AvailableTesters(toAdd.TestDateTime);
						if (availableTesters.Count() == 0)
						{
							Console.WriteLine("There are no available testers for the chosen time.");
							break;
						}
						Console.WriteLine("These are the available testers for the chosen time:");
						foreach (var tester in availableTesters)
							Console.WriteLine(tester);
						break;
					default: return;
				}


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
			int s;
			int.TryParse(Console.ReadLine(), out s);
			toAdd.Sex = (BE.Gender)s; 
			Console.WriteLine("What is the date of birth?");
			DateTime date;
			DateTime.TryParse(Console.ReadLine(), out date);
			toAdd.BirthDay = date;
			Console.WriteLine("What is the name? type on seperate lines");
			toAdd.Name = new Name(Console.ReadLine(), Console.ReadLine());
			Console.WriteLine("What is the Phone Number?");
			toAdd.PhoneNumber = Console.ReadLine();
			Console.WriteLine("What is the Address? type on seperate lines: city, street name, building number.");
			toAdd.Address = new Address(Console.ReadLine(), Console.ReadLine(), Console.ReadLine());
			Console.WriteLine("What type is the vehicle the trainee is learning on?");
			PrintVehicleTypes();
			int v;
			int.TryParse(Console.ReadLine(), out v);
			toAdd.CurrentCarType = (BE.VehicleType)v;
			Console.WriteLine("What gear type is the trainee learning on?");
			int g;
			int.TryParse(Console.ReadLine(), out g);
			toAdd.carTypeStats[toAdd.CurrentCarType].gearType = (GearType)g;
			
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
			int s;
			int.TryParse(Console.ReadLine(), out s);
			toAdd.Sex = (BE.Gender)s;
			Console.WriteLine("What is the date of birth?");
			DateTime date;
			DateTime.TryParse(Console.ReadLine(), out date);
			toAdd.BirthDay = date;
			Console.WriteLine("What is the name? type on seperate lines");
			toAdd.Name = new Name(Console.ReadLine(), Console.ReadLine());
			Console.WriteLine("What is the Phone Number?");
			toAdd.PhoneNumber = Console.ReadLine();
			Console.WriteLine("What is the Address? type on seperate lines: city, street name, building number.");
			toAdd.Address = new Address(Console.ReadLine(), Console.ReadLine(), Console.ReadLine());
			Console.WriteLine("What type is the  vehicle?");
			PrintVehicleTypes();
			int v;
			int.TryParse(Console.ReadLine(), out v);
			toAdd.testingCarType = (BE.VehicleType)v;
			Schedule schedule = new Schedule();
			Console.WriteLine("On which days is the tester busy?");
			Console.WriteLine("1 - sunday, etc");
			string busyDays = Console.ReadLine();
			foreach (var day in Functions.DaysOfWeekFromString(busyDays))
			{
				Console.WriteLine("During what hours is the tester busy on {0}?\n9-14", day);
				string hoursString = Console.ReadLine();
				for (int hour = 9; hour < 15; hour++)
					schedule[day][hour] = !hoursString.Contains(hour.ToString());
			}
			toAdd.schedule = schedule;
			Console.WriteLine("How many years of experience does the tester have?");
			UInt32 expYears;
			while(!UInt32.TryParse(Console.ReadLine(), out expYears));
			toAdd.ExpYears = expYears;
			Console.WriteLine("What is the maximum amount of weekly tests the tester can perform?");
			UInt32 maxTests;
			while (!UInt32.TryParse(Console.ReadLine(), out maxTests)) ;
			toAdd.MaxWeeklyTests = maxTests;
			Console.WriteLine("What is the maximum distance the tester can go for a test?");
			UInt32 maxDist;
			while (!UInt32.TryParse(Console.ReadLine(), out maxDist)) ;
			toAdd.MaxDistance = maxDist;

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
			//actionList.Add(UpdateTest);
			actionList.Add(AddTrainee);
			actionList.Add(RemoveTrainee);
			actionList.Add(UpdateTrainee);
			actionList.Add(AddTester);
			actionList.Add(RemoveTester);
			//actionList.Add(UpdateTester);
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
					Console.WriteLine(i + " - " + Functions.InsertSpacesBeforeUpper(actionList[i].Method.Name));
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
				CurrentCarType = VehicleType.LargeTruck,
			};
			Trainee b = new Trainee
			{
				ID = "456",
				Name = new Name("Sapir", "Barabi"),
				Sex = Gender.Female,
				PhoneNumber = "456",
				BirthDay = new DateTime(1993, 5, 14),
				Address = new Address("Kiryat Ata", "David Remez", "1a"),
				CurrentCarType = VehicleType.LargeTruck,

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
				testingCarType = VehicleType.PrivateCar,
				schedule = new Schedule()
			};
			IBL bl = FactoryBL.GetBL();
			bl.AddTester(c);
			bl.AddTrainee(a);
			bl.AddTrainee(b);
		}
	}
}
