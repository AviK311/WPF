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
            
            this.sexComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gender));
            this.gearTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.GearType));
            this.currentCarTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.VehicleType));
        }       

        

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			trainee.Address = new Address(city: cityTextBox.Text, street: streetTextBox.Text, buildingNumber: buildingNumberTextBox.Text);
			trainee.Name = new Name(firstNameTextBox.Text, lastNameTextBox.Text);
			try
			{
				if (password.Password != confirmPassword.Password)
					throw new InvalidOperationException("The passwords do not match!");
				if (password.Password == "")
					throw new InvalidOperationException("Please enter a password!");
				bl.AddTrainee(trainee);
				trainee = new BE.Trainee();
				bl.AddPassword(trainee.ID, password.Password);
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
	}
}
