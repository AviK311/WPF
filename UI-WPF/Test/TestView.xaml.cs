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
using System.Threading;
using BE;
using BL;

namespace UI_WPF
{
	/// <summary>
	/// Interaction logic for TestView.xaml
	/// </summary>
	public partial class TestView : Window
	{
		Test test;
        List<string> testers, trainees;
        IBL bl = BL.FactoryBL.GetBL();
        List<Test> list;
		readonly DateTime OriginalTime;
        Tester tester;
        bool distance = true;
        bool calculating = false;
        public TestView(Test test1)
		{
			testers = new List<string>();
			trainees = new List<string>();
			InitializeComponent();
            list = (List<Test>)bl.GetTests();
			test = list.First(T => T.TestNumber == test1.TestNumber);
			OriginalTime = test.TestDateTime;
			SaveButton.Visibility = Visibility.Hidden;            
            propertiesGrid.Visibility = Visibility.Hidden;
            bl = FactoryBL.GetBL();
            DataContext = test;
            int[] arr = { 9, 10, 11, 12, 13, 14 };
            Hour.ItemsSource = arr;
            Hour.SelectedIndex = test1.TestDateTime.Hour-9;
			if (GlobalSettings.User is Trainee)
			{
				trainees.Add(GlobalSettings.User.ID);
				list = (from item in list
						where item.TraineeID == GlobalSettings.User.ID
						select item).ToList();
				if (test.TestDateTime < DateTime.Now)
					EditButton.IsEnabled = false;
				TestDeleteButton.IsEnabled = false;

			}
			else foreach (var item in bl.GetTrainees())
					trainees.Add(item.ID);
			if (GlobalSettings.User is Tester)
			{
				testers.Add(GlobalSettings.User.ID);
				list = (from item in list
						where item.TesterID == GlobalSettings.User.ID
						select item).ToList();
			}
			else foreach (var item in bl.GetTesters())
					testers.Add(item.ID);
            traineeIDComboBox.ItemsSource = trainees;
			testerIDComboBox.ItemsSource = testers;
            
            
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            test.BeginLocation = new Address(city: City.Text, street: Street.Text, buildingNumber: Number.Text);
            if (distance == false)
                MessageBox.Show("The location is too far for the tester", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            else
            {
                try
                {
                    bl.UpdateTest(test);
                    EditButton.Visibility = Visibility.Visible;
                    SaveButton.Visibility = Visibility.Hidden;
                    MessageBox.Show("Update Successful!", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (InvalidOperationException exc)
                {
                    MessageBox.Show(exc.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
		}
		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			EditButton.Visibility = Visibility.Hidden;
			SaveButton.Visibility = Visibility.Visible;
		}
		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
            var resetResult = MessageBox.Show(" Are you sure you want to delete this test?", "Delete Test", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (resetResult == MessageBoxResult.Yes)
            {
                try
                {
                    bl.RemoveTest(test.TestNumber);
                    if (bl.GetTests().Count() == 0)
                    {
                        TestWindow testWindow = new TestWindow();
                        testWindow.Show();
                        Close();
                    }
                    else
                    {
                        list = (List<Test>)bl.GetTests();
                        RightButton_Click(sender, e);
                    }
               }
                catch (InvalidOperationException exc)
                {
                    MessageBox.Show(exc.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            //Window confirmDelete = new ConfirmDelete(sender, this, test.TestNumber);
            //confirmDelete.ShowDialog();
        }

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{

			EditButton.Visibility = Visibility.Visible;
			SaveButton.Visibility = Visibility.Hidden;
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			TestWindow testWindow = new TestWindow();
			testWindow.Show();
			Close();
		}
        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = list.FindIndex(T => T.TestNumber == test.TestNumber);
            if (currentIndex + 1 == list.Count)
                currentIndex = -1;
            test = list[currentIndex + 1];
            DataContext = test;
			if (GlobalSettings.User is Trainee && test.TestDateTime < DateTime.Now)
				EditButton.IsEnabled = false;
			else EditButton.IsEnabled = true;


		}
        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = list.FindIndex(T => T.TestNumber == test.TestNumber);
            if (currentIndex == 0)
                currentIndex = list.Count;
            test = list[currentIndex - 1];

            DataContext = test;
			if (GlobalSettings.User is Trainee && test.TestDateTime < DateTime.Now)
				EditButton.IsEnabled = false;
			else EditButton.IsEnabled = true;
		}

        private void TestersInRange(Address address, Tester tester)
        {
            calculating = true;
            distance = bl.TestersInRange(tester, address);
            calculating = false;
 
        }
        private void City_TextChanged(object sender, TextChangedEventArgs e)
        {
            tester = bl.GetTesters().FirstOrDefault(T => T.ID == (string)testerIDComboBox.SelectedValue);
            BE.Address address = new BE.Address(city: City.Text, street: Street.Text, buildingNumber: Number.Text);
            if (address != null && tester != null)
            {
                Thread thread = new Thread(() => TestersInRange(address, tester));
                thread.Start();
            }
        }

        private void testerIDComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tester = bl.GetTesters().FirstOrDefault(T => T.ID == (string)testerIDComboBox.SelectedValue);
            BE.Address address = new BE.Address(city: City.Text, street: Street.Text, buildingNumber: Number.Text);
            if (address != null && tester != null)
            {
                Thread thread = new Thread(() => TestersInRange(address, tester));
                thread.Start();
            }
        }

        private void Hour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TimeSpan ts = new TimeSpan((int)Hour.SelectedItem, 0, 0);
			test.TestDateTime = test.TestDateTime.Date + ts;
            if (test.TestDateTime > DateTime.Now)
            {
                propertiesGrid.Visibility = Visibility.Hidden;
                foreach (var i in propertiesGrid.Children.OfType<CheckBox>())
                    i.IsChecked = false;
            }
            else propertiesGrid.Visibility = Visibility.Visible;
			//testers.Clear();
			//foreach (var item in bl.AvailableTesters(test.TestDateTime))
			//	testers.Add(item.ID);
			//if (test.TestDateTime == OriginalTime)
			//	testers.Add(test.TesterID);
			//testerIDComboBox.ItemsSource = testers;
		}

    }
}