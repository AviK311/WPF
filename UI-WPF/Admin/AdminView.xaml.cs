using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView : Window
    {
        IBL bl = BL.FactoryBL.GetBL();
        Admin admin;        
		List<Admin> adminList;
        public AdminView(Admin admin1, List<Admin> list )
        {           
            InitializeComponent();
			SaveButton.Visibility = Visibility.Hidden;
			adminList = Functions.TrueCopyAdmin(list);
			admin =new Admin(admin1);
			DataContext = admin;
            sexComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gender));
			InfoBlock.Text = "View Admin";

		}

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

			try
            {
				Functions.ValidatePerson(admin);
				bl.UpdateAdmin(admin);                
                EditButton.Visibility = Visibility.Visible;
                SaveButton.Visibility = Visibility.Hidden;
				MessageBox.Show("Update Successful!", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                //adminList[adminList.FindIndex(t => t.ID == admin.ID)] = admin;
            }
            catch (InvalidOperationException exc)
            {
                MessageBox.Show(exc.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditButton.Visibility = Visibility.Hidden;
            SaveButton.Visibility = Visibility.Visible;
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var resetResult = MessageBox.Show(" Are you sure you want to delete this admin?", "Delete Admin", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (resetResult == MessageBoxResult.Yes)
            {
                try
                {
                    bl.RemoveAdmin(admin.ID);
					bl.RemovePassword(admin.ID);
					if (GlobalSettings.User.Equals(admin))
                    {
                        LoginWindow loginWindow = new LoginWindow();
                        loginWindow.Show();
                        Close();
                    }
                    else
                    {
                        if (bl.GetAdmins().Count() == 0)
                        {
                            AdminWindow Window = new AdminWindow();
                            Window.Show();
                            Close();
                        }
                        else
                        {
                            adminList = (List<Admin>)bl.GetAdmins();
                            RightButton_Click(sender, e);
                        }
                    }
                }
                catch (InvalidOperationException exc)
                {
                    MessageBox.Show(exc.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
			
            EditButton.Visibility = Visibility.Visible;
            SaveButton.Visibility = Visibility.Hidden;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
			Window win = new AdminWindow();
			win.Show();
            Close();
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = adminList.FindIndex(T => T.Equals(admin));
            if (currentIndex + 1 == adminList.Count)
                currentIndex = -1;
            admin = adminList[currentIndex + 1];
            DataContext = admin;                     
        }
        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = adminList.FindIndex(T => T.Equals(admin));
            if (currentIndex == 0)
                currentIndex = adminList.Count;
            admin = adminList[currentIndex - 1];
            DataContext = admin;           
        }
       
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {            
            if ((e.Key < Key.D0 || e.Key > Key.D9) && (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) && e.Key != Key.Tab)
            {
                e.Handled = true;
            }
        }
		private void Phone_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key!= Key.OemMinus && e.Key != Key.Subtract &&(e.Key < Key.D0 || e.Key > Key.D9) && (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) && e.Key != Key.Tab)
				e.Handled = true;
		}
		private void RightArrow(object sender, MouseEventArgs e)
		{
			var button = sender as Button;
			//if (button.IsEnabled)
			InfoBlock.Text = "Click to go to the next Admin on the list";
		}
		private void LeftArrow(object sender, MouseEventArgs e)
		{
			var button = sender as Button;
			if (button.IsEnabled)
				InfoBlock.Text = "Click to go to the previous Admin on the list";
		}
		
		private void EditButtonEnterEvent(object sender, MouseEventArgs e)
		{
			var button = sender as Button;
			if (button.IsEnabled) InfoBlock.Text = "Click to Enter Edit Mode";
		}
		private void CancelButtonEnterEvent(object sender, MouseEventArgs e)
		{
			var button = sender as Button;
			if (button.IsEnabled) InfoBlock.Text = "Click to exit Edit Mode";
		}
		private void DeleteButtonEnterEvent(object sender, MouseEventArgs e)
		{
			var button = sender as Button;
			if (button.IsEnabled) InfoBlock.Text = "Click to delete the Admin";
		}
		private void SaveButtonEnterEvent(object sender, MouseEventArgs e)
		{
			var button = sender as Button;
			if (button.IsEnabled) InfoBlock.Text = "Click to update changes";
		}
		private void BackButtonEnterEvent(object sender, MouseEventArgs e)
		{
			var button = sender as Button;
			if (button.IsEnabled) InfoBlock.Text = "Click to navigate back to Admin List page";
		}
		private void MouseLeave(object sender, MouseEventArgs e)
		{
			InfoBlock.Text = "View Admin";
		}

	}
}
