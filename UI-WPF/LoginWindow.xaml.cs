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
using System.Net.Mail;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace UI_WPF
{
	/// <summary>
	/// Interaction logic for LoginWindow.xaml
	/// </summary>
	public partial class LoginWindow : Window
	{
		static IBL bl;
		public static void ShowNotifications(Person p)
		{
			if (p.notifications.Count == 0)
				return;
			foreach (var item in p.notifications)
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
				MessageBox.Show(item.message, item.time.ToShortDateString() + " " + item.time.ToShortTimeString(), MessageBoxButton.OK, messageBoxImage);
			}
			p.notifications.Clear();
			bl.UpdatePerson(p);
			//switch (Global.appClearanceLevel)
			//{
			//	case ClearanceLevel.Admin:
			//		///// code to update admin;
			//		break;
			//	case ClearanceLevel.Tester:
			//		bl.UpdateTester((Tester)Global.user);
			//		break;
			//	case ClearanceLevel.Trainee:
			//		bl.UpdateTrainee((Trainee)Global.user);
			//		break;
			//}
		}


		public LoginWindow()
		{
			Configuration.MailSender = new MailClient();
			InitializeComponent();
			XMLHandler Handler = XMLHandler.GetXMLHandler();
			bl = FactoryBL.GetBL();
			

		
			if (GlobalSettings.AlreadyLoggedIn == false) {
				Admin Vizen = new Admin(new Name("Dr.", "Vizen"));
				Vizen.ID = "11111111";
				bl.AddAdmin(Vizen);
				//MessageBox.Show("Welcome, Dr. Vizen!\n" +
				//	"To begin, click login. the system will allow you to choose a password.\n" +
				//	"For your convenience, there are 3 other users in the system:\n" +
				//	"Tester id: 00123456, password: \"c\"\n" +
				//	"Trainee id: 00000123, password: \"a\" \n" +
				//	"Trainee id: 00000456, password:  \"b\" \n");
				Admin tamar = new Admin(new Name("Tamar", "Gold"));
				tamar.ID = "207623224";
				tamar.FirstLogIn = false;
				bl.AddAdmin(tamar);
				bl.AddUpdatePassword(tamar.ID, "1234");
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
					schedule = new Schedule(),
					FirstLogIn = false,
				};
				bl.AddTester(c);
				//Tester e = new Tester
				//{
				//    ID = "123456",
				//    Name = new Name("ron", "Cohen"),
				//    Sex = Gender.Male,
				//    PhoneNumber = "224",
				//    BirthDay = new DateTime(1969, 2, 1),
				//    Address = new Address("Tel Aviv", "Hahagana", "28"),
				//    MaxDistance = 60,
				//    MaxWeeklyTests = 9,
				//    ExpYears = 6,
				//    testingCarType = VehicleType.PrivateCar,
				//    schedule = new Schedule(),
				//    FirstLogIn = false,
				//};
				//bl.AddTester(e);

				Trainee a = new Trainee
				{
					ID = "123",
					Name = new Name("Avi", "Levi"),
					Sex = Gender.Male,
					PhoneNumber = "123",
					BirthDay = new DateTime(1993, 11, 3),
					Address = new Address("bet shemesh", "nahal maor", "19"),
					CurrentCarType = VehicleType.LargeTruck,
					FirstLogIn = false,

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
				bl.AddUpdatePassword(a.ID, "a");
				bl.AddUpdatePassword(b.ID, "b");
				bl.AddUpdatePassword(c.ID, "c");
			
			}
			//this needs to be erased. it's here only for debugging reasons
			GlobalSettings.AlreadyLoggedIn = true;


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
					GlobalSettings.User = bl.GetTester(id);
					GlobalSettings.AppClearanceLevel = UserType.Tester;
				}
				else if (bl.GetTrainees().Any(T => T.ID == id))
				{
					GlobalSettings.User = bl.GetTrainee(id);
					GlobalSettings.AppClearanceLevel = UserType.Trainee;
				}
				else if (bl.GetAdmins().Any(A => A.ID == id))
				{
					GlobalSettings.User = bl.GetAdmin(id);
					GlobalSettings.AppClearanceLevel = UserType.Admin;
				}
				else throw new InvalidOperationException("That user ID does not exist in the system");
				if (GlobalSettings.User.FirstLogIn)
				{
					FirstPassword firstPassword = new FirstPassword(this);
					firstPassword.Show();
				}
				else
				{
					if (!bl.CheckPassword(GlobalSettings.User.ID, password))
						throw new InvalidOperationException(password == "" ? "This is not your first login, please enter a password!" : "Wrong password!");
					if (GlobalSettings.User.CheckEmail == true)
					{
						GlobalSettings.User.CheckEmail = false;
						bl.UpdatePerson(GlobalSettings.User);
					}
	
					MainWindow main = new MainWindow();
					ShowNotifications(GlobalSettings.User);
					main.Show();

					Close();
				}
			}
			catch (InvalidOperationException exc)
			{
				MessageBox.Show(exc.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}

		}


		private void Forgot_Click(object sender, RoutedEventArgs e)
		{
			string id = IdInput.Text;
			try
			{
				IBL bl = FactoryBL.GetBL();
				if (bl.GetTesters().Any(T => T.ID == id))
				{
					GlobalSettings.User = bl.GetTester(id);
					GlobalSettings.AppClearanceLevel = UserType.Tester;
				}
				else if (bl.GetTrainees().Any(T => T.ID == id))
				{
					GlobalSettings.User = bl.GetTrainee(id);
					GlobalSettings.AppClearanceLevel = UserType.Trainee;
				}
				else if (bl.GetAdmins().Any(A => A.ID == id))
				{
					GlobalSettings.User = bl.GetAdmin(id);
					GlobalSettings.AppClearanceLevel = UserType.Admin;
				}
				else throw new InvalidOperationException("That user ID does not exist in the system");
				if (GlobalSettings.User.AwaitingAdminReset == true)
					throw new InvalidOperationException("The admins are processing your first request.");
				if (GlobalSettings.User.CheckEmail == true)
					throw new InvalidOperationException("Your new password has been sent to your email address");
				
				if (GlobalSettings.User.Email != null && GlobalSettings.User.Email != "")
				{
					var result = MessageBox.Show("You will receive a new password by email.\nDo you want to proceed?", "Alert",
					MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
					if (result == MessageBoxResult.Yes)
					{
						GlobalSettings.User.CheckEmail = true;
						bl.UpdatePerson(GlobalSettings.User);
						string NewPassword = Functions.CreateNewRandomPassword();
						bl.AddUpdatePassword(GlobalSettings.User.ID, NewPassword);
						Functions.SendEmail(GlobalSettings.User, "Password Reset", "Your new password is: " + NewPassword);
					}
				}
				else { 
				var result = MessageBox.Show("The administrators will receive a request to reset your password.\nDo you want to proceed?", "Alert",
					MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    GlobalSettings.User.AwaitingAdminReset = true;
                    bl.UpdatePerson(GlobalSettings.User);

                        Messages message = new Messages
                        {
                            ID = GlobalSettings.User.ID,
                            Name = GlobalSettings.User.Name,
                            DateOfMessage = DateTime.Now,
                            Content = "The user has requested a password reset.",
                            UserReset = true,
                        };
                        message.UserType = GlobalSettings.User is Tester ? UserType.Tester : GlobalSettings.User is Trainee ? UserType.Trainee : UserType.Admin;
                        bl.AddMessage(message);
                    }
                }
			}
			catch (InvalidOperationException exc)
			{
				MessageBox.Show(exc.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}

        private void PasswordInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginButton_Click(this, new RoutedEventArgs());
            }
        }
    } }
