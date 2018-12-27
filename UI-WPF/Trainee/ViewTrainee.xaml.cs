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
        public ViewTrainee(Trainee trainee1)
        {           
            InitializeComponent();                 
            //bl = BL.FactoryBL.GetBL();
            SaveButton.Visibility = Visibility.Hidden;
            trainee = new Trainee(trainee1);
            DataContext = trainee;            
            this.keyComboBox.ItemsSource = Enum.GetValues(typeof(BE.VehicleType));
            this.sexComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gender));
            this.gearTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.GearType));
            this.currentCarTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.VehicleType));
            gearTypeComboBox.SelectedItem = trainee.carTypeStats[trainee.CurrentCarType].gearType;
            numOfLessonsTextBox.Text = trainee.carTypeStats[trainee.CurrentCarType].numOfLessons.ToString();
            schoolNameTextBox.Text = trainee.carTypeStats[trainee.CurrentCarType].schoolName;
            numOfTestTextBlock.Text = trainee.carTypeStats[trainee.CurrentCarType].numOfTest.ToString();
            passedCheckBox.IsChecked = trainee.carTypeStats[trainee.CurrentCarType].passed;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            trainee.Address = new Address(city: cityTextBox.Text, street: streetTextBox.Text, buildingNumber: buildingNumberTextBox.Text);
            trainee.Name = new Name(firstNameTextBox.Text, lastNameTextBox.Text);
            trainee.carTypeStats[trainee.CurrentCarType].gearType = (GearType)gearTypeComboBox.SelectedItem;
            trainee.carTypeStats[trainee.CurrentCarType].numOfLessons = Convert.ToInt32(numOfLessonsTextBox.Text);
            trainee.carTypeStats[trainee.CurrentCarType].schoolName = schoolNameTextBox.Text;
            try
            {
                bl.UpdateTrainee(trainee);
                trainee = new BE.Trainee();                
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
    }
}
