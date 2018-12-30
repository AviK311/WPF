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
	/// Interaction logic for TestView.xaml
	/// </summary>
	public partial class TestView : Window
	{
		Test test;
        List<string> testers, trainees;
        IBL bl = BL.FactoryBL.GetBL();
        List<Test> list;
        public TestView(Test test1)
		{
			InitializeComponent();
            list = (List<Test>)bl.GetTests();
            SaveButton.Visibility = Visibility.Hidden;            
            propertiesGrid.Visibility = Visibility.Hidden;
            bl = FactoryBL.GetBL();
            test = new Test();
            test.TestDateTime = DateTime.Now;
            DataContext = test;
            int[] arr = { 9, 10, 11, 12, 13, 14 };
            Hour.ItemsSource = arr;
            Hour.SelectedIndex = test1.TestDateTime.Hour-9;
            testers = new List<string>();
            trainees = new List<string>();
            foreach (var item in bl.GetTrainees())
                trainees.Add(item.ID);
            foreach (var item in bl.GetTesters())
                testers.Add(item.ID);
            testerIDComboBox.ItemsSource = testers;
            testerIDComboBox.SelectedIndex = 0;
            traineeIDComboBox.ItemsSource = trainees;
            traineeIDComboBox.SelectedIndex = 1;
            test = new Test(test1);
            DataContext = test;
        }

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{

			System.Windows.Data.CollectionViewSource testViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("testViewSource")));
			// Load data by setting the CollectionViewSource.Source property:
			// testViewSource.Source = [generic data source]
		}
		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			test.BeginLocation = new Address(city: City.Text, street: Street.Text, buildingNumber: Number.Text);
			try
			{
				bl.UpdateTest(test);
				test = new BE.Test();
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
			bl.RemoveTest(test.TestNumber);
			TestWindow testWindow = new TestWindow();
			testWindow.Show();
			Close();
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
            int currentIndex = list.FindIndex(T => T.Equals(test));
            if (currentIndex + 1 == list.Count)
                currentIndex = -1;
            test = list[currentIndex + 1];
            DataContext = test;            
        }
        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = list.FindIndex(T => T.Equals(test));
            if (currentIndex == 0)
                currentIndex = list.Count;
            test = list[currentIndex - 1];
            DataContext = test;            
        }

        private void Hour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TimeSpan ts = new TimeSpan((int)Hour.SelectedItem, 0, 0);
            DateTime s = test.TestDateTime.Date + ts;
            if (s > DateTime.Now)
            {
                propertiesGrid.Visibility = Visibility.Hidden;
                foreach (var i in propertiesGrid.Children.OfType<CheckBox>())
                    i.IsChecked = false;
            }
            else propertiesGrid.Visibility = Visibility.Visible;
            //testers = new List<string>();
            //foreach (var item in bl.AvailableTesters(test.TestDateTime))
            //    testers.Add(item.ID);
        }

    }
}