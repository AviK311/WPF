using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using System.Threading;
using BE;
using BL;


namespace UI_WPF
{
    /// <summary>
    /// Interaction logic for TestAdd.xaml
    /// </summary>
    public partial class TestAdd : Window
    {
		IBL bl;
		List<string> testers, trainees;
		Test test;
		DateTime PreviousTime;
        Tester tester;
        bool distance = true;
        bool calculating = false;

        private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			TimeSpan ts = new TimeSpan((int)Hour.SelectedItem, 0, 0);
			test.TestDateTime = test.TestDateTime.Date + ts;
			test.BeginLocation = new Address(city: City.Text, street: Street.Text, buildingNumber: Number.Text);
            //Tester t = bl.GetTesters().FirstOrDefault(tester => tester.ID == Convert.ToString(testerIDComboBox));
            ////testingCarTypeTextBlock.Text = Convert.ToString(t.testingCarType);
            if (calculating == true)
                MessageBox.Show("please wait, the system is calculating", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            else
            {
                if (distance == false)
                    MessageBox.Show("The location is too far for the tester", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                {
                    try
                    {
                        bl.AddTest(test);
                        MessageBox.Show("Adding Successful!", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                        TestWindow testWindow = new TestWindow();
                        testWindow.Show();
                        Close();

                    }
                    catch (InvalidOperationException exc)
                    {
                        MessageBox.Show(exc.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }
        }

		private void Hour_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			TimeSpan ts = new TimeSpan((int)Hour.SelectedItem, 0, 0);
			test.TestDateTime = test.TestDateTime.Date + ts;
			if (test.TestDateTime > DateTime.Now) {
				
			propertiesGrid.Visibility = Visibility.Hidden;
			foreach (var i in propertiesGrid.Children.OfType<CheckBox>()) 
				i.IsChecked = false;
			}
			else propertiesGrid.Visibility = Visibility.Visible;
		}

       
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Window testWindow = new TestWindow();
            testWindow.Show();
            Close();
        }

        private void VerifyRange(Address address,Tester tester)
        {
            //tester = bl.GetTesters().FirstOrDefault(T => T.ID == (string)testerIDComboBox.SelectedValue);
            //BE.Address address = new BE.Address(city: City.Text, street: Street.Text, buildingNumber: Number.Text);     
            //if (address != null && tester != null)
            //{
            calculating = true;
            distance = bl.TestersInRange(tester, address);
            calculating = false;
            //if (bl.TestersInRange(tester, address) == false)
            //    distance = false;
            //else
            //    distance = true;
            //}
        }

        private void CheckingValidAddress(object sender, SelectionChangedEventArgs e)
        {
            tester = bl.GetTesters().FirstOrDefault(T => T.ID == (string)testerIDComboBox.SelectedValue);
            BE.Address address = new BE.Address(city: City.Text, street: Street.Text, buildingNumber: Number.Text);
            if (address != null && tester != null)
            {
                Thread thread = new Thread(() => VerifyRange(address, tester));
                thread.Start();
            }
        }

        private void CheckingValidAddress2(object sender, TextChangedEventArgs e)
        {
			CheckingValidAddress(null, null);

		}

        public TestAdd()
        {
            InitializeComponent();
			propertiesGrid.Visibility = Visibility.Hidden;
			bl = FactoryBL.GetBL();
			test = new Test();
			test.TestDateTime = DateTime.Now;
			PreviousTime = test.TestDateTime;
			DataContext = test;
			int[] arr = { 9, 10, 11, 12, 13, 14 };
			Hour.ItemsSource = arr;
			Hour.SelectedIndex = 0;
			testers = new List<string>();
			trainees = new List<string>();
			if (GlobalSettings.User is Trainee)
				trainees.Add(GlobalSettings.User.ID);
			else foreach (var item in bl.GetTrainees())
					trainees.Add(item.ID);
			if (GlobalSettings.User is Tester)
				testers.Add(GlobalSettings.User.ID);
			else foreach (var item in bl.GetTesters())
					testers.Add(item.ID);
			testerIDComboBox.ItemsSource = testers;
			testerIDComboBox.SelectedIndex = 0;
			traineeIDComboBox.ItemsSource = trainees;
			traineeIDComboBox.SelectedIndex = 1;



		}

		
	}
}
