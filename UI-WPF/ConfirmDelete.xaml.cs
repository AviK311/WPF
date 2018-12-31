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
	/// Interaction logic for ConfirmDelete.xaml
	/// </summary>
	public partial class ConfirmDelete : Window
	{
		IBL bl = FactoryBL.GetBL();
		Window toClose;
		Button senderButton;
		string toDelete;
		public ConfirmDelete(object sender, Window window, string ID)
		{
			
			InitializeComponent();
			toClose = window;
			senderButton = sender as Button;
			toDelete = ID;
		}

		private void ConfirmButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Window toReturn = new Window();
				if (!bl.CheckPassword(GlobalSettings.User.ID, ConfirmDeletePassword.Password))
					throw new InvalidOperationException("The password is incorrect");
				if (senderButton.Name.Contains("Tester"))
				{
					bl.RemoveTester(toDelete);
					if (GlobalSettings.User.ID == toDelete)
						toReturn = new LoginWindow();
					else toReturn = new TesterWindow();
					
				}
				if (senderButton.Name.Contains("Trainee"))
				{
					bl.RemoveTrainee(toDelete);
					if (GlobalSettings.User.ID == toDelete)
						toReturn = new LoginWindow();
					else toReturn = new TraineeWindow();
				}
				if (senderButton.Name.Contains("Admin"))
				{
					bl.RemoveTrainee(toDelete);
					if (GlobalSettings.User.ID == toDelete)
						toReturn = new LoginWindow();
					//else toReturn = new ();
				}
				if (senderButton.Name.Contains("Test") && !senderButton.Name.Contains("Tester"))
				{
					bl.RemoveTest(toDelete);
				}
					
				else bl.RemovePassword(toDelete);
				toReturn.Show();
				toClose.Close();
				MessageBox.Show("Deletion Successful!", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
				Close();
			}
			catch (InvalidOperationException exc)
			{
				MessageBox.Show(exc.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
			//	


		}
	}
}
