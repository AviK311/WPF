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
		List<Trainee> list;
        public ViewTrainee(Trainee trainee1)
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
			list = (List<Trainee>)bl.GetTrainees();
			trainee = list.First(T=>T.Equals(trainee1));
			DataContext = trainee;
            this.cartype.ItemsSource = Enum.GetValues(typeof(BE.VehicleType));
            this.sexComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gender));
            this.gearTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.GearType));
            this.currentCarTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.VehicleType));
			StatsGrid.DataContext = trainee.carTypeStats[trainee.CurrentCarType];
			cartype.SelectedItem = trainee.CurrentCarType;
			teacherLast.Text = trainee.carTypeStats[(VehicleType)cartype.SelectedIndex].teacherName.last;
			teacherFirst.Text = trainee.carTypeStats[(VehicleType)cartype.SelectedIndex].teacherName.first;

		}

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            trainee.Address = new Address(city: cityTextBox.Text, street: streetTextBox.Text, buildingNumber: buildingNumberTextBox.Text);
            trainee.Name = new Name(firstNameTextBox.Text, lastNameTextBox.Text);
			
			try
            {
				if (trainee.Email != null)
				{
					Match match = GlobalSettings.EmailRegex.Match(trainee.Email);
					if (!match.Success)
						throw new InvalidOperationException("The email address is invalid");
				}
				else throw new InvalidOperationException("The email address is invalid");
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
                    if (bl.GetTrainees().Count() == 0)
                    {
                        TraineeWindow Window = new TraineeWindow();
                        Window.Show();
                        Close();
                    }
                    else
                    {
                        list = (List<Trainee>)bl.GetTrainees();
                        RightButton_Click(sender, e);
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
			int currentIndex = list.FindIndex(T => T.Equals(trainee));
			if (currentIndex + 1 == list.Count)
				currentIndex = -1;
			trainee = list[currentIndex + 1];
			DataContext = trainee;
			StatsGrid.DataContext = trainee.carTypeStats[trainee.CurrentCarType];
			cartype.SelectedItem = trainee.CurrentCarType;
			teacherLast.Text = trainee.carTypeStats[(VehicleType)cartype.SelectedIndex].teacherName.last;
			teacherFirst.Text = trainee.carTypeStats[(VehicleType)cartype.SelectedIndex].teacherName.first;		
		}
        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
			int currentIndex = list.FindIndex(T => T.Equals(trainee));
			if (currentIndex == 0)
				currentIndex = list.Count;
			trainee = list[currentIndex - 1];
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
            if (e.Key < Key.D0 || e.Key > Key.D9)
                e.Handled = true;
        }   
       
    }
}
