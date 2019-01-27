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
        List<Test> testList;
		DateTime LastValidTime;
        Tester tester;
        bool distance = true;
        bool calculating = false;
		bool PromptDistance = false;
        public TestView(Test test1, List<Test> list)
		{            
			testers = new List<string>();
			trainees = new List<string>();
			InitializeComponent();
            image2.Visibility = Visibility.Collapsed;
            testList = Functions.TrueCopyTests(list);
			test = testList.First(T => T.TestNumber == test1.TestNumber);
			LastValidTime = test.TestDateTime;
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
				if (test.TestDateTime < DateTime.Now)
					EditButton.IsEnabled = false;
				TestDeleteButton.IsEnabled = false;

			}
			else foreach (var item in bl.GetTrainees())
					trainees.Add(item.ID);
			if (GlobalSettings.User is Tester)
			{
				testers.Add(GlobalSettings.User.ID);
			}
			else foreach (var item in bl.GetTesters())
					testers.Add(item.ID);
            traineeIDComboBox.ItemsSource = trainees;
			testerIDComboBox.ItemsSource = testers;
			InfoBlock.Text = "View Test";

		}

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            test.BeginLocation = new Address(city: City.Text, street: Street.Text, buildingNumber: Number.Text);
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
                        testList = (List<Test>)bl.GetTests();
                        RightButton_Click(sender, e);
                    }
               }
                catch (InvalidOperationException exc)
                {
                    MessageBox.Show(exc.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            
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
            int currentIndex = testList.FindIndex(T => T.TestNumber == test.TestNumber);
            if (currentIndex + 1 == testList.Count)
                currentIndex = -1;
            test = testList[currentIndex + 1];
            DataContext = test;
			LastValidTime = test.TestDateTime;
			if (GlobalSettings.User is Trainee && test.TestDateTime < DateTime.Now)
				EditButton.IsEnabled = false;
			else EditButton.IsEnabled = true;

		}
        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = testList.FindIndex(T => T.TestNumber == test.TestNumber);
            if (currentIndex == 0)
                currentIndex = testList.Count;
            test = testList[currentIndex - 1];
            DataContext = test;
			LastValidTime = test.TestDateTime;
			if (GlobalSettings.User is Trainee && test.TestDateTime < DateTime.Now)
				EditButton.IsEnabled = false;
			else EditButton.IsEnabled = true;
		}

        private void VerifyAddress(Address address, Tester tester)
        {
			string FinishedMessage = "The system finished calculating.\n";
			calculating = true;
			distance = bl.TesterIsInRange(tester, address);
			calculating = false;
			FinishedMessage += distance ? "The tester lives close enough to the designated begin address" : "The tester lives too far away from the designated begin address";
			//if (PromptDistance)MessageBox.Show(FinishedMessage, "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
			PromptDistance = false;

		}
        private void CheckingValidAddress(object sender, TextChangedEventArgs e)
        {
			try { TesterName.Text = bl.GetTester(test.TesterID).Name.ToString(); } catch { }
			CheckingValidAddress2(null, null);

		}

		private void CheckingValidAddress2(object sender, SelectionChangedEventArgs e)
        {
			tester = bl.GetTesters().FirstOrDefault(T => T.ID == (string)testerIDComboBox.SelectedValue);
			BE.Address address = new BE.Address(city: City.Text, street: Street.Text, buildingNumber: Number.Text);
			if (Functions.IsAddress(address) && tester != null)
			{
				Thread thread = new Thread(() => VerifyAddress(address, tester));
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
            image.Visibility = Visibility.Collapsed;
            image2.Visibility = Visibility.Visible;
        }

        private void image2_MouseEnter(object sender, MouseEventArgs e)
        {
            image2.Visibility = Visibility.Collapsed;
            image.Visibility = Visibility.Visible;
        }
        //private void image_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    ScaleTransform scale = new ScaleTransform(1.1, 1.1);
        //    image.RenderTransform = scale;
        //}

        //private void image_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    image.RenderTransform = null;
        //}

        private void Hour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			try
			{
				if (GlobalSettings.User is Trainee && test.TestDateTime < DateTime.Now && LastValidTime > DateTime.Now)
					throw new InvalidOperationException("A Trainee cannot change a test to a past date");
					
				if (test.TestDateTime.DayOfWeek > (DayOfWeek)4)
					throw new InvalidOperationException("You cannot appoint a test on a " + test.TestDateTime.DayOfWeek);				TimeSpan ts = new TimeSpan((int)Hour.SelectedItem, 0, 0);
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
				//testerIDComboBox.ItemsSource = bl.AvailableTesters(LastValidTime, test.TestNumber);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				test.TestDateTime = LastValidTime;
				testDateTimeDatePicker.SelectedDate = LastValidTime;
			}
			
		}
		private void RightArrow(object sender, MouseEventArgs e)
		{
			var button = sender as Button;
			//if (button.IsEnabled)
				InfoBlock.Text = "Click to go to the next Test on the list";
		}
		private void LeftArrow(object sender, MouseEventArgs e)
		{
			var button = sender as Button;
			if (button.IsEnabled)
				InfoBlock.Text = "Click to go to the previous Test on the list";
		}
		private void AddressOrTester(object sender, MouseEventArgs e)
		{
			InfoBlock.Text = "When you choose an address and a Tester, the system will check if the tester lives close enough.";
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
			if (button.IsEnabled) InfoBlock.Text = "Click to delete the test";
		}
		private void SaveButtonEnterEvent(object sender, MouseEventArgs e)
		{
			var button = sender as Button;
			if (button.IsEnabled) InfoBlock.Text = "Click to update changes";
		}
		private void BackButtonEnterEvent(object sender, MouseEventArgs e)
		{
			var button = sender as Button;
			if (button.IsEnabled) InfoBlock.Text = "Click to navigate back to Test List page";
		}
		private void MouseLeave(object sender, MouseEventArgs e)
		{
			InfoBlock.Text = "View Test";
		}
		private void Date(object sender, MouseEventArgs e)
		{
			if (!(GlobalSettings.User is Tester)) InfoBlock.Text = "When you choose a date and time, the Tester list will update to the testers available on that date.\n";
			if (GlobalSettings.User is Trainee)
				InfoBlock.Text += "A trainee cannot choose a time that has already passed.";
			else
				InfoBlock.Text += "Choosing a time that has passed enables grading of the test";
		}

	}
}