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
using BE;
using BL;

namespace UI_WPF
{
	/// <summary>
	/// Interaction logic for ChangePassword.xaml
	/// </summary>
	public partial class ChangePassword : Window
	{
		IBL bl;
		public ChangePassword()
		{
			InitializeComponent();
			bl = FactoryBL.GetBL();
		}

		private void AcceptButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (!bl.CheckPassword(GlobalSettings.User.ID, PrevPassword.Password))
					throw new InvalidOperationException("The password is incorrect");
				if (NewPassword.Password != confirmPassword.Password)
					throw new InvalidOperationException("The passwords do not match!");
				if (NewPassword.Password == "")
					throw new InvalidOperationException("Please enter a new password!");
				bl.AddUpdatePassword(GlobalSettings.User.ID, NewPassword.Password);
				MessageBox.Show("Password change successful", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
				Close();
			}
			catch (InvalidOperationException exc)
			{
				MessageBox.Show(exc.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}
	}
}
