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
            trainee = new Trainee(trainee1);
			list = (List<Trainee>)bl.GetTrainees();
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
                bl.UpdateTrainee(trainee);                
                EditButton.Visibility = Visibility.Visible;
                SaveButton.Visibility = Visibility.Hidden;
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
            bl.RemoveTrainee(trainee.ID);
            TraineeWindow traineeWindow = new TraineeWindow();
            traineeWindow.Show();
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

            EditButton.Visibility = Visibility.Visible;
            SaveButton.Visibility = Visibility.Hidden;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            TraineeWindow traineeWindow = new TraineeWindow();
            traineeWindow.Show();
            Close();
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            SaveButton.Visibility = Visibility.Hidden;
            EditButton.Visibility = Visibility.Visible;
            Trainee t = bl.GetTrainees().First();
            bl.RemoveTrainee(t.ID);
            bl.AddTrainee(t);
            trainee = new Trainee(bl.GetTrainees().First());
            gearTypeComboBox.SelectedItem = trainee.carTypeStats[trainee.CurrentCarType].gearType;
            numOfLessonsTextBox.Text = trainee.carTypeStats[trainee.CurrentCarType].numOfLessons.ToString();
            schoolNameTextBox.Text = trainee.carTypeStats[trainee.CurrentCarType].schoolName;
            numOfTestTextBlock.Text = trainee.carTypeStats[trainee.CurrentCarType].numOfTest.ToString();
            passedCheckBox.IsChecked = trainee.carTypeStats[trainee.CurrentCarType].passed;
            DataContext = trainee;          
        }
        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            SaveButton.Visibility = Visibility.Hidden;
            EditButton.Visibility = Visibility.Visible;
            Trainee t;
            for (int i = 0; i < bl.GetTrainees().Count()-1; i++)
            {
                t = bl.GetTrainees().First();
                bl.RemoveTrainee(t.ID);
                bl.AddTrainee(t);
            }                    
            trainee = new Trainee(bl.GetTrainees().First());
            gearTypeComboBox.SelectedItem = trainee.carTypeStats[trainee.CurrentCarType].gearType;
            numOfLessonsTextBox.Text = trainee.carTypeStats[trainee.CurrentCarType].numOfLessons.ToString();
            schoolNameTextBox.Text = trainee.carTypeStats[trainee.CurrentCarType].schoolName;
            numOfTestTextBlock.Text = trainee.carTypeStats[trainee.CurrentCarType].numOfTest.ToString();
            passedCheckBox.IsChecked = trainee.carTypeStats[trainee.CurrentCarType].passed;
            DataContext = trainee;            
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
	}
}
