using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BL;
using BE;

namespace UI_WPF
{
	/// <summary>
	/// Interaction logic for LoginWindow.xaml
	/// </summary>
	public partial class LoginWindow : Window
	{
		public LoginWindow()
		{
			InitializeComponent();
			IBL bl = FactoryBL.GetBL();
			if (Global.alreadyLoggedIn == false) { 
				Admin avi = new Admin(new Name("Avi", "Koenigsberg"));
				avi.ID = "31122370";
				bl.AddAdmin(avi);
				bl.AddPassword(avi.ID, "5678");
				Admin tamar = new Admin(new Name("Tamar", "Gold"));
				tamar.ID = "207623224";
				bl.AddAdmin(tamar);
				bl.AddPassword(tamar.ID, "1234");
				Test d = new Test
				{
					TesterID = "00123456",
					TraineeID = "00000123",
					TestDateTime = new DateTime(2018, 12, 26, 10, 0, 0),
					BeginLocation = new Address("jeru", "vaad", "21"),
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
					testingCarType = VehicleType.LargeTruck,
					schedule = new Schedule()
				};
				bl.AddTester(c);
				
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
				a.carTypeStats[VehicleType.LargeTruck] = new Stats { gearType = GearType.Manual, numOfLessons = 21, numOfTest = 0, schoolName = " www", passed = false };

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
				a.notifications.Add(new Notification(MessageIcon.Error, "what do you want?"));
				bl.AddTrainee(a);
				bl.AddTrainee(b);
				bl.AddTest(d);
				bl.AddPassword(a.ID, "a");
				bl.AddPassword(b.ID, "b");
				bl.AddPassword(c.ID, "c");
			}
			//this needs to be erased. it's here only for debugging reasons
			Global.alreadyLoggedIn = true;


		}
		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			string id = IdInput.Text;
			string password = PasswordInput.Password;
			try
			{
				IBL bl = FactoryBL.GetBL();
				if (bl.GetTesters().Any(T => T.ID == id))
				{
					Global.user = bl.GetTester(id);
					Global.appClearanceLevel = ClearanceLevel.Tester;
				}
				else if (bl.GetTrainees().Any(T => T.ID == id))
				{
					Global.user = bl.GetTrainee(id);
					Global.appClearanceLevel = ClearanceLevel.Trainee;
				}
				else if (bl.GetAdmins().Any(A => A.ID == id))
				{
					Global.user = bl.GetAdmin(id);
					Global.appClearanceLevel = ClearanceLevel.Admin;
				}
				else throw new InvalidOperationException("That user ID does not exist in the system");
				if (!bl.CheckPassword(Global.user.ID, password))
					throw new InvalidOperationException("Wrong password!");
				MainWindow main = new MainWindow();
				//TesterAdd main = new TesterAdd();
				foreach (var item in Global.user.notifications)
				{
					MessageBoxImage messageBoxImage = MessageBoxImage.Asterisk;
					switch (item.Icon)
					{
						case MessageIcon.Error:
							messageBoxImage = MessageBoxImage.Error;
							break;
						case MessageIcon.Information:
							messageBoxImage = MessageBoxImage.Information;
							break;
						case MessageIcon.Warning:
							messageBoxImage = MessageBoxImage.Warning;
							break;
						default: break;
					}
					MessageBox.Show(item.message, item.time.ToShortDateString() + " "+ item.time.ToShortTimeString(), MessageBoxButton.OK, messageBoxImage);
				}
				Global.user.notifications.Clear();
				switch (Global.appClearanceLevel)
				{
					case ClearanceLevel.Admin:
						///// code to update admin;
						break;
					case ClearanceLevel.Tester:
						bl.UpdateTester((Tester)Global.user);
						break;
					case ClearanceLevel.Trainee:
						bl.UpdateTrainee((Trainee)Global.user);
						break;
				}
				main.Show();
               
                Close();
			}
			catch (InvalidOperationException exc)
			{
				MessageBox.Show(exc.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}

		}
	}
}
