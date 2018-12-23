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
				main.Show();
				this.Close();
			}
			catch (InvalidOperationException exc)
			{
				MessageBox.Show(exc.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}

		}
	}
}
