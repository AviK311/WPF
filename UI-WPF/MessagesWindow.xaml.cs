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
    /// Interaction logic for MessagesWindow.xaml
    /// </summary>
    public partial class MessagesWindow : Window
    {
        IBL bl;
        public MessagesWindow()
        {
            InitializeComponent();
            bl = FactoryBL.GetBL();            
            DataContext = bl.GetMessages();
        }

		private void Back_Click(object sender, RoutedEventArgs e)
		{
			Window main = new MainWindow();
			main.Show();
			Close();

		}

		private void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (listBox.SelectedItem != null)
			{
				if (((Messages)listBox.SelectedItem).UserReset)
				{
					var resetResult = MessageBox.Show("Click yes to reset the user's password.", "Password Reset", MessageBoxButton.YesNo, MessageBoxImage.Information);
					if (resetResult == MessageBoxResult.Yes)
					{
						var id = ((Messages)listBox.SelectedItem).ID;
						try
						{
							if (bl.GetTesters().Any(T => T.ID == id))
							{
								var p = bl.GetTester(id);
								p.AwaitingAdminReset = false;
								p.FirstLogIn = true;
								bl.UpdatePerson(p);
							}
							else if (bl.GetTrainees().Any(T => T.ID == id))
							{
								var p = bl.GetTrainee(id);
								p.AwaitingAdminReset = false;
								p.FirstLogIn = true;
								bl.UpdatePerson(p);
							}
							else if (bl.GetAdmins().Any(A => A.ID == id))
							{
								var p = bl.GetAdmin(id);
								p.AwaitingAdminReset = false;
								p.FirstLogIn = true;
								bl.UpdatePerson(p);
							}
							else throw new InvalidOperationException("That user ID does not exist in the system");
							bl.RemoveMessage(((Messages)listBox.SelectedItem).MessageNumber);
							DataContext = bl.GetMessages();
						}
						catch (InvalidOperationException exc)
						{
							MessageBox.Show(exc.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
							bl.RemoveMessage(((Messages)listBox.SelectedItem).MessageNumber);
							DataContext = bl.GetMessages();
						}
					}
				}
				else
				{

					var result = MessageBox.Show("Are you sure you want to delete this message?", "Alert", MessageBoxButton.YesNo, MessageBoxImage.Warning);
					if (result == MessageBoxResult.Yes)
					{
						bl.RemoveMessage(((Messages)listBox.SelectedItem).MessageNumber);
						DataContext = bl.GetMessages();
					}
				}
			}
		}
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

            var button = sender as RadioButton;
            if (button.Name== "All")
            {
                DataContext = bl.GetMessages();
            }
            if (button.Name == "Tester")
            {
                DataContext = bl.GetMessages().Where(c => c.UserType == UserType.Tester);
            }
            else
            {
                DataContext = bl.GetMessages().Where(c => c.UserType == UserType.Trainee);
            }
        }
    }
}
