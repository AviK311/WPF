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
	/// Interaction logic for FirstPassword.xaml
	/// </summary>
	public partial class FirstPassword : Window
	{
		IBL bl;
		public FirstPassword()
		{
			InitializeComponent();
			bl = FactoryBL.GetBL();
		}

		

		private void AcceptButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (password.Password != confirmPassword.Password)
					throw new InvalidOperationException("The passwords do not match!");
				if (password.Password == "")
					throw new InvalidOperationException("Please enter a password!");
				bl.AddUpdatePassword(GlobalSettings.User.ID, password.Password);
				GlobalSettings.User.FirstLogIn = false;
				bl.UpdatePerson(GlobalSettings.User);
				MainWindow main = new MainWindow();
				LoginWindow.ShowNotifications(GlobalSettings.User);
				MessageBox.Show("Password successfully set!", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
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
