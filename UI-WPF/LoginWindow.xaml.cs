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
			Name name = new Name("Avi", "Koenigsberg");
			Admin avi = new Admin(name, "J0gging9");           
			avi.ID = "31122370";            
			bl.AddAdmin(avi);
            name = new Name("Tamar", "Gold");
			Admin tamar = new Admin(name, "1234");
            tamar.ID="207623224";
            bl.AddAdmin(tamar);

        }
		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			string id = IdInput.Text;
			string password = PasswordInput.Password;
			try
			{
				IBL bl = FactoryBL.GetBL();
				if (bl.GetTesters().Any(T => T.ID == id))
					Global.user = bl.GetTester(id);
				else if (bl.GetTrainees().Any(T => T.ID == id))
					Global.user = bl.GetTrainee(id);
				else if (bl.GetAdmins().Any(A => A.ID == id))
					Global.user = bl.GetAdmin(id);
				else throw new InvalidOperationException("That user ID does not exist in the system");
				if (!Global.user.CheckPassword(password))
					throw new InvalidOperationException("Wrong password!");
				MainWindow main = new MainWindow();
				//TesterAdd main = new TesterAdd();
				main.Show();
                Test d = new Test
                {
                    TesterID = "123456",
                    TraineeID = "123",                  
                    TestDateTime = new DateTime(2018, 12, 26),
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
					testingCarType = VehicleType.PrivateCar,
					schedule = new Schedule()
				};
				bl.AddTester(c);
                Dictionary<VehicleType, Stats> s = new Dictionary<VehicleType, Stats>();
                s.Add(VehicleType.LargeTruck, new Stats { gearType = GearType.Manual, numOfLessons = 21, numOfTest = 0, schoolName = " www", passed = false });
                Trainee a = new Trainee
                {
                    ID = "123",
                    Name = new Name("Avi", "Levi"),
                    Sex = Gender.Male,
                    PhoneNumber = "123",
                    BirthDay = new DateTime(1993, 11, 3),
                    Address = new Address("bet shemesh", "nahal maor", "19"),
                    CurrentCarType = VehicleType.LargeTruck,
                    carTypeStats = s
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

				bl.AddTrainee(a);
				bl.AddTrainee(b);
                //bl.AddTest(d);
                Close();
			}
			catch (InvalidOperationException exc)
			{
				MessageBox.Show(exc.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}

		}
	}
}
