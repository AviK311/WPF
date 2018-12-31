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
		}

        private void button_trainee_Click(object sender, RoutedEventArgs e)
        {
			Window trainee;
			if (GlobalSettings.User is Trainee)
				trainee = new ViewTrainee((Trainee)GlobalSettings.User);
			else 
              trainee = new TraineeWindow();
            trainee.Show();
            Close();
        }

        private void button_tester_Click(object sender, RoutedEventArgs e)
        {
			Window tester;
			if (GlobalSettings.User is Tester)
				tester = new TesterView((Tester)GlobalSettings.User);
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
    }
}
