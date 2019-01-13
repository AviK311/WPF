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
            
			adminList = Functions.TrueCopyAdmin(list);
			admin =new Admin(admin1);
			DataContext = admin;
            sexComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gender))

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
			int currentIndex = traineelist.FindIndex(T => T.Equals(trainee));
			if (currentIndex + 1 == traineelist.Count)
				currentIndex = -1;
			trainee = traineelist[currentIndex + 1];
			DataContext = trainee;
			StatsGrid.DataContext = trainee.carTypeStats[trainee.CurrentCarType];
			cartype.SelectedItem = trainee.CurrentCarType;
			teacherLast.Text = trainee.carTypeStats[(VehicleType)cartype.SelectedIndex].teacherName.last;
			teacherFirst.Text = trainee.carTypeStats[(VehicleType)cartype.SelectedIndex].teacherName.first;		
		}
        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
			int currentIndex = traineelist.FindIndex(T => T.Equals(trainee));
			if (currentIndex == 0)
				currentIndex = traineelist.Count;
			trainee = traineelist[currentIndex - 1];
			DataContext = trainee;
			StatsGrid.DataContext = trainee.carTypeStats[trainee.CurrentCarType];
			cartype.SelectedItem = trainee.CurrentCarType;
			teacherLast.Text = trainee.carTypeStats[(VehicleType)cartype.SelectedIndex].teacherName.last;
			teacherFirst.Text = trainee.carTypeStats[(VehicleType)cartype.SelectedIndex].teacherName.first;			
        }

		private void cartype_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			StatsGrid.DataContext = trainee.carTypeStats[(VehicleType)cartype.SelectedIndex];
			teacherLast.Text = trainee.carTypeStats[(VehicleType)cartype.SelectedIndex].teacherName.last;
			teacherFirst.Text= trainee.carTypeStats[(VehicleType)cartype.SelectedIndex].teacherName.first;
		}

		private void teacherLast_TextChanged(object sender, TextChangedEventArgs e)
		{
			trainee.carTypeStats[(VehicleType)cartype.SelectedIndex].teacherName.last = teacherLast.Text;

		}

		private void teacherFirst_TextChanged(object sender, TextChangedEventArgs e)
		{
			trainee.carTypeStats[(VehicleType)cartype.SelectedIndex].teacherName.first = teacherFirst.Text;

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

	}
}
