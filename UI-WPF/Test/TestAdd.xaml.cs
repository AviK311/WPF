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
		DateTime LastValidTime;
        Tester tester;
        bool distance = true;
        bool calculating = false;
		bool PromptDistance = false;

        private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			TimeSpan ts = new TimeSpan((int)Hour.SelectedItem, 0, 0);
			test.TestDateTime = test.TestDateTime.Date + ts;
			test.BeginLocation = new Address(city: City.Text, street: Street.Text, buildingNumber: Number.Text);
			//Tester t = bl.GetTesters().FirstOrDefault(tester => tester.ID == Convert.ToString(testerIDComboBox));
			////testingCarTypeTextBlock.Text = Convert.ToString(t.testingCarType);
			PromptDistance = calculating;

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
			try
			{
				if (GlobalSettings.User is Trainee && test.TestDateTime < DateTime.Now)
					throw new InvalidOperationException("A Trainee cannot appoint a test retroactively");
				if (test.TestDateTime.DayOfWeek > (DayOfWeek)4)
					throw new InvalidOperationException("You cannot appoint a test on a " + test.TestDateTime.DayOfWeek);
				TimeSpan ts = new TimeSpan((int)Hour.SelectedItem, 0, 0);
				test.TestDateTime = test.TestDateTime.Date + ts;
				if (test.TestDateTime > DateTime.Now)
				{

					propertiesGrid.Visibility = Visibility.Hidden;
					foreach (var i in propertiesGrid.Children.OfType<CheckBox>())
						i.IsChecked = false;
				}
				else propertiesGrid.Visibility = Visibility.Visible;
				LastValidTime = test.TestDateTime;
				HebDate.Text = Functions.GetHebrewDate(LastValidTime);
				testerIDComboBox.ItemsSource = bl.AvailableTesters(LastValidTime, "");

			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				test.TestDateTime = LastValidTime;
				testDateTimeDatePicker.SelectedDate = LastValidTime;
			}
		}

       
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Window testWindow = new TestWindow();
            testWindow.Show();
            Close();
        }

        private void VerifyRange(Address address,Tester tester)
        {
			string FinishedMessage = "The system finished calculating.\n";
            calculating = true;
            distance = bl.TesterIsInRange(tester, address);
            calculating = false;
			FinishedMessage += distance ? "The tester lives close enough to the designated begin address" : "The tester lives too far away from the designated begin address";
		//	if (PromptDistance) MessageBox.Show(FinishedMessage, "Alert", MessageBoxButton.OK,MessageBoxImage.Information);
			PromptDistance = false;
        }

        private void CheckingValidAddress(object sender, SelectionChangedEventArgs e)
        {
			try
			{
				TesterName.Text = bl.GetTester(test.TesterID).Name.ToString();
					}
			catch { }
			CheckingValidAddress2(null, null);
		}

        private void CheckingValidAddress2(object sender, TextChangedEventArgs e)
       {
			
			tester = bl.GetTesters().FirstOrDefault(T => T.ID == (string)testerIDComboBox.SelectedValue);
			BE.Address address = new BE.Address(city: City.Text, street: Street.Text, buildingNumber: Number.Text);
			if (Functions.IsAddress(address) && tester != null)
			{
				Thread thread = new Thread(() => VerifyRange(address, tester));
				thread.Start();
			}
			else distance = true;

		}

		private void traineeIDComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try { TraineeName.Text = bl.GetTrainee(test.TraineeID).Name.ToString(); } catch { }
		}

        private void image_MouseEnter(object sender, MouseEventArgs e)
        {
            ScaleTransform scale = new ScaleTransform(1.2, 1.2);
            image.RenderTransform = scale;
        }

        private void image_MouseLeave(object sender, MouseEventArgs e)
        {
            image.RenderTransform = null;
        }

        public TestAdd()
        {
            InitializeComponent();
			propertiesGrid.Visibility = Visibility.Hidden;
			bl = FactoryBL.GetBL();
			test = new Test();
			var closestValidDate = DateTime.Now.AddDays(1);
			if (closestValidDate.DayOfWeek > (DayOfWeek)4)
				closestValidDate = closestValidDate.AddDays(7 - (int)closestValidDate.DayOfWeek);
			test.TestDateTime = closestValidDate;
			LastValidTime = test.TestDateTime;
			DataContext = test;
			TestNumber.Text = bl.GetTestCode().ToString().PadLeft(8,'0') ;
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
			InfoBlock.Text = "Add Test: Once you choose a trainee and a tester, you can save.";


		}

		
	}
}
