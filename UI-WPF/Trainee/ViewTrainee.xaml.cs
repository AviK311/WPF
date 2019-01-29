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
    /// Interaction logic for ViewTrainee.xaml
    /// </summary>
    public partial class ViewTrainee : Window
    {
        IBL bl = BL.FactoryBL.GetBL();
        Trainee trainee;        
		List<Trainee> traineelist;
        public ViewTrainee(Trainee trainee1, List<Trainee> list )
        {           
            InitializeComponent();                 
            //bl = BL.FactoryBL.GetBL();
            SaveButton.Visibility = Visibility.Hidden;
			if (GlobalSettings.User is Trainee)
			{
				RightButton.Visibility = Visibility.Hidden;
				LeftButton.Visibility = Visibility.Hidden;
			}
			else if (GlobalSettings.User is Tester)
			{
				
					EditButton.IsEnabled = false;
					TraineeDeleteButton.IsEnabled = false;
					CancelButton.IsEnabled = false;
			}
			traineelist = Functions.TrueCopyTrainee(list);
			trainee =new Trainee(trainee1);
			DataContext = trainee;
            this.cartype.ItemsSource = Enum.GetValues(typeof(BE.VehicleType));
            this.sexComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gender));
            this.gearTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.GearType));
            this.currentCarTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.VehicleType));
			StatsGrid.DataContext = trainee.carTypeStats[trainee.CurrentCarType];
			cartype.SelectedItem = trainee.CurrentCarType;
			teacherLast.Text = trainee.carTypeStats[(VehicleType)cartype.SelectedIndex].teacherName.last;
			teacherFirst.Text = trainee.carTypeStats[(VehicleType)cartype.SelectedIndex].teacherName.first;
			InfoBlock.Text = "View Trainee";

		}

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            trainee.Address = new Address(city: cityTextBox.Text, street: streetTextBox.Text, buildingNumber: buildingNumberTextBox.Text);
            trainee.Name = new Name(firstNameTextBox.Text, lastNameTextBox.Text);
			
			try
            {
				Functions.ValidatePerson(trainee);

				bl.UpdateTrainee(trainee);                
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
            var resetResult = MessageBox.Show(" Are you sure you want to delete this trainee?", "Delete Trainee", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (resetResult == MessageBoxResult.Yes)
            {
                try
                {
                    bl.RemoveTrainee(trainee.ID);
					bl.RemovePassword(trainee.ID);
					if (!(GlobalSettings.User is Admin))
                    {
                        LoginWindow loginWindow = new LoginWindow();
                        loginWindow.Show();
                        Close();
                    }
                    else
                    {
                        if (bl.GetTrainees().Count() == 0)
                        {
                            TraineeWindow Window = new TraineeWindow();
                            Window.Show();
                            Close();
                        }
                        else
                        {
                            traineelist = (List<Trainee>)bl.GetTrainees();
                            RightButton_Click(sender, e);
                        }
                    }
                }
                catch (InvalidOperationException exc)
                {
                    MessageBox.Show(exc.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }

            //Window confirmDelete = new ConfirmDelete(sender, this, trainee.ID);
            //confirmDelete.ShowDialog();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
			
            EditButton.Visibility = Visibility.Visible;
            SaveButton.Visibility = Visibility.Hidden;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
			Window trainee;
			if (GlobalSettings.User is Trainee)
				trainee = new MainWindow();
			else trainee = new TraineeWindow();
			trainee.Show();
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

        
        private void image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            image.Visibility = Visibility.Collapsed;
            image2.Visibility = Visibility.Visible;
        }
        private void image2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            image2.Visibility = Visibility.Collapsed;
            image.Visibility = Visibility.Visible;
        }
		private void RightArrow(object sender, MouseEventArgs e)
		{
			var button = sender as Button;
			if (button.IsEnabled)
			InfoBlock.Text = "Click to go to the next Trainee on the list";
		}
		private void LeftArrow(object sender, MouseEventArgs e)
		{
			var button = sender as Button;
			if (button.IsEnabled)
				InfoBlock.Text = "Click to go to the previous Trainee on the list";
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
			if (button.IsEnabled) InfoBlock.Text = "Click to delete the Trainee";
		}
		private void SaveButtonEnterEvent(object sender, MouseEventArgs e)
		{
			var button = sender as Button;
			if (button.IsEnabled) InfoBlock.Text = "Click to update changes";
		}
		private void BackButtonEnterEvent(object sender, MouseEventArgs e)
		{
			InfoBlock.Text = "Click to navigate back to " + (GlobalSettings.User is Trainee ? "Main Page" : "Trainee list page");
		}
		private void CarTypeEnterEvent(object sender, MouseEventArgs e)
		{

			InfoBlock.Text = "Choose to view/edit the vehicle learning stats.";
		}
		private void MouseLeave(object sender, MouseEventArgs e)
		{
			InfoBlock.Text = "View Trainee";
		}
        private void emailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string s = emailTextBox.Text + "gmail.com";
                if (emailTextBox.Text.Last() == '@')
                    emailTextBox.Text = s;
            }
            catch { }
        }
    }
}
