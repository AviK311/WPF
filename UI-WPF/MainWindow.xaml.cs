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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BE;
using BL;

namespace UI_WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			contentTextBox.Visibility = Visibility.Hidden;
			MessageButton.Visibility = Visibility.Hidden;
			if (GlobalSettings.User is Admin)
				messageLabel.Visibility = Visibility.Hidden;
			else button_Messages.Visibility = Visibility.Hidden;
		}

        private void button_trainee_Click(object sender, RoutedEventArgs e)
        {
			Window trainee;
			if (GlobalSettings.User is Trainee)
				trainee = new ViewTrainee((Trainee)GlobalSettings.User, null);
			else 
              trainee = new TraineeWindow();
            trainee.Show();
            Close();
        }

        private void button_tester_Click(object sender, RoutedEventArgs e)
        {
			Window tester;
			if (GlobalSettings.User is Tester)
				tester = new TesterView((Tester)GlobalSettings.User, null);
			else tester = new TesterWindow();
            tester.Show();
            Close();
        }

        private void button_test_Click(object sender, RoutedEventArgs e)
        {
            Window test = new TestWindow();
            test.Show();
            Close();
        }

		private void LogOut_Click(object sender, RoutedEventArgs e)
		{
			Window loginWindow = new LoginWindow();
			loginWindow.Show();
			Close();
		}

		private void changePassword_Click(object sender, RoutedEventArgs e)
		{
			ChangePassword changePassword = new ChangePassword();
			changePassword.Show();
		}

        private void button_actions_Click(object sender, RoutedEventArgs e)
        {
            Window messages = new MessagesWindow();
            if (GlobalSettings.User is Admin)
            {
                messages.Show();
                Close();
            }
        }

		private void Label_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (contentTextBox.Visibility == Visibility.Visible)
			{
				contentTextBox.Visibility = Visibility.Hidden;
				MessageButton.Visibility = Visibility.Hidden;
			}else
			{
				contentTextBox.Visibility = Visibility.Visible;
				MessageButton.Visibility = Visibility.Visible;
			}
		}

		

		private void MessageButton_Click(object sender, RoutedEventArgs e)
		{
			IBL bl = FactoryBL.GetBL();
			Messages message = new Messages
			{
				ID = GlobalSettings.User.ID,
				Name = GlobalSettings.User.Name,
				DateOfMessage = DateTime.Now,
				Content = contentTextBox.Text,

			};

			message.UserType = GlobalSettings.User is Tester ? UserType.Tester : UserType.Trainee;
			bl.AddMessage(message);
			MessageBox.Show("Message Sent!", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
			contentTextBox.Text = "";
		}

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Window informationWindow = new InformationWindow();
            informationWindow.Show();
        }

        private void contentTextBox_KeyDown(object sender, KeyEventArgs e)
        {
           if (e.Key == Key.Enter)
            {
                MessageButton_Click(this, new RoutedEventArgs());
            }
        }
    }
}
