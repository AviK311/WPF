﻿using System;
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
    /// Interaction logic for AddTrainee1.xaml
    /// </summary>
    public partial class AddTrainee1 : Window
    {
        BE.Trainee trainee;
        BL.IBL bl;
        public AddTrainee1()
        {
            InitializeComponent();
            trainee = new BE.Trainee();
            this.DataContext = trainee;
            bl = BL.FactoryBL.GetBL();
            trainee.BirthDay = DateTime.Now.AddYears(-(int)Configuration.MinAgeOfTrainee);
            trainee.BirthDay = trainee.BirthDay.AddDays(-1);
            
            sexComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gender));
            gearTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.GearType));
            currentCarTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.VehicleType));
        }       

        

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
            trainee.Address = new Address(city: cityTextBox.Text, street: streetTextBox.Text, buildingNumber: buildingNumberTextBox.Text);
            trainee.Name = new Name(firstNameTextBox.Text, lastNameTextBox.Text);
            //trainee.carTypeStats[(VehicleType)currentCarTypeComboBox.SelectedIndex].numOfLessons = Convert.ToInt32(numOfLessonsTextBox.Text);
            trainee.carTypeStats[(VehicleType)currentCarTypeComboBox.SelectedIndex].schoolName = schoolNameTextBox.Text;
            trainee.carTypeStats[(VehicleType)currentCarTypeComboBox.SelectedIndex].gearType = (GearType)gearTypeComboBox.SelectedIndex;
            trainee.carTypeStats[(VehicleType)currentCarTypeComboBox.SelectedIndex].teacherName.first = teacherFirst.Text;
            trainee.carTypeStats[(VehicleType)currentCarTypeComboBox.SelectedIndex].teacherName.first = teacherFirst.Text;
			Match match = GlobalSettings.EmailRegex.Match(trainee.Email);

			try
			{
				if (!match.Success)
					throw new InvalidOperationException("The email address is invalid");				
				bl.AddTrainee(trainee);
				MessageBox.Show("Adding Successful!", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
				TraineeWindow traineeWindow = new TraineeWindow();
				traineeWindow.Show();
				Close();
			}
			catch (InvalidOperationException exc)
			{
				MessageBox.Show(exc.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			Window traineeWindow = new TraineeWindow();
			traineeWindow.Show();
			Close();
		}

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key < Key.D0 || e.Key > Key.D9)
                e.Handled = true;
        }
       
    }
}
