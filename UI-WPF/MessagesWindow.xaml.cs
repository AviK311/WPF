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
                var result = MessageBox.Show("Are you sure you want to delete this message?", "Alert", MessageBoxButton.YesNo,  MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    bl.RemoveMessage(((Messages)listBox.SelectedItem).MessageNumber);
                    DataContext = bl.GetMessages();
                }              
			}
		}
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

            var button = sender as RadioButton;
            if (button.Name== "Trainee")
            {
                DataContext = bl.GetMessages().Where(c => c.UserType== UserType.Trainee);
            }
            if (button.Name == "Tester")
            {
                DataContext = bl.GetMessages().Where(c => c.UserType == UserType.Tester);
            }
            else
            {
                DataContext = bl.GetMessages();
            }
        }
    }
}
