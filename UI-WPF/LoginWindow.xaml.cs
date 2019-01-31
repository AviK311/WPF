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
using System.Windows.Media.Animation;
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
			//Configuration.MailSender = new MailClient();
			
			InitializeComponent();
			
			bl = FactoryBL.GetBL();			
			InfoBlock.Text = "Login: Enter ID and password to enter.\nFloat above a button for an explanation.";

		}
		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			string id = IdInput.Text;
			string password = PasswordInput.Password;            
            try
			{
				IBL bl = FactoryBL.GetBL();
				if (bl.GetTesters().Any(T => T.ID == id))
					GlobalSettings.User = bl.GetTester(id);
				else if (bl.GetTrainees().Any(T => T.ID == id))
					GlobalSettings.User = bl.GetTrainee(id);
				
				else if (bl.GetAdmins().Any(A => A.ID == id))
					GlobalSettings.User = bl.GetAdmin(id);

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
	
					MainWindow maino = new MainWindow();
					ShowNotifications(GlobalSettings.User);
					maino.Show();

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
					GlobalSettings.User = bl.GetTester(id);
				else if (bl.GetTrainees().Any(T => T.ID == id))	
					GlobalSettings.User = bl.GetTrainee(id);
				else if (bl.GetAdmins().Any(A => A.ID == id))
					GlobalSettings.User = bl.GetAdmin(id);
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
						Functions.SendEmail(GlobalSettings.User, "Password Reset", "Hello "+ GlobalSettings.User.Name.first+" "+ GlobalSettings.User.Name.last + "!!\nYour new password is: " + NewPassword+"\nYou can change your password in the main window.");
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

		

		private void MouseLeave(object sender, MouseEventArgs e)
		{
			InfoBlock.Text = "Login: Enter ID and password to enter.\nFloat above a button for an explanation.";
		}

		private void Forgot_MouseEnter(object sender, MouseEventArgs e)
		{
			InfoBlock.Text = "If your Email Address is in the system, you will receive an email with a new randomized password. "+
				"Otherwise, the administrators will receive a message requesting a password reset. Once one of them accepts, you may " +
				"try logging in again";
		}

		private void LoginButton_MouseEnter(object sender, MouseEventArgs e)
		{
			InfoBlock.Text = "After you've entered the correct ID and password, click here to log in.\n" +
				"If this is your first time, you will be prompted to choose a password for yourself.";
		}
        private void image1_MouseEnter(object sender, MouseEventArgs e)
        {

            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 360;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.RepeatBehavior = RepeatBehavior.Forever;
            RotateTransform rt = new RotateTransform();
            image.RenderTransform = rt;
            rt.BeginAnimation(RotateTransform.AngleProperty, da);

        }

        private void image1_MouseLeave(object sender, MouseEventArgs e)
        {
            image.RenderTransform = null;
        }


    } }
