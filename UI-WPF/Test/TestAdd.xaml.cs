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
        bool IsCloseEnough = true;
        bool IsStillCalculating = false;
		Thread PropellerThread;
		
		private void PropellerFunction()
		{
			while (true)
				if (IsStillCalculating)
					image_MouseEnter();

		}

        private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			TimeSpan ts = new TimeSpan((int)Hour.SelectedItem, 0, 0);
			test.TestDateTime = test.TestDateTime.Date + ts;
			test.BeginLocation = new Address(city: City.Text, street: Street.Text, buildingNumber: Number.Text);
			if (IsStillCalculating == true)
				MessageBox.Show("please wait, the system is calculating", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
			else
			{
				if (IsCloseEnough == false)
					MessageBox.Show("The location is too far for the tester", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
				else
				{
					try
					{
						bl.AddTest(test);
						MessageBox.Show("Adding Successful!", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
						TestWindow testWindow = new TestWindow();
						testWindow.Show();
						PropellerThread.Abort();
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
				if (!(GlobalSettings.User is Tester)) testerIDComboBox.ItemsSource = bl.AvailableTesters(LastValidTime, "");

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
			PropellerThread.Abort();
            Close();
        }

        private void VerifyRange(Address address,Tester tester)
        {
            IsStillCalculating = true;
            IsCloseEnough = bl.IsTesterCloseEnough(tester, address);
            IsStillCalculating = false;
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
			else IsCloseEnough = true;

		}

		private void traineeIDComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try { TraineeName.Text = bl.GetTrainee(test.TraineeID).Name.ToString(); } catch { }
		}
        
		private void AddressOrTester(object sender, MouseEventArgs e)
		{
			InfoBlock.Text ="When you choose an address and a Tester, the system will check if the tester lives close enough.";
		}
		private void Date(object sender, MouseEventArgs e)
		{
			if (!(GlobalSettings.User is Tester)) InfoBlock.Text = "When you choose a date and time, the Tester list will update to the testers available on that date.\n";
			if (GlobalSettings.User is Trainee)
				InfoBlock.Text += "A trainee cannot choose a time that has already passed.";
			else
				InfoBlock.Text += "Choosing a time that has passed enables grading of the test";
		}
		private void MouseLeave(object sender, MouseEventArgs e)
		{
			InfoBlock.Text = "Add Test: Once you choose a trainee and a tester, you can save.";
		}

		public TestAdd()
        {
            InitializeComponent();
			propertiesGrid.Visibility = Visibility.Hidden;          
            image2.Visibility = Visibility.Collapsed;
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
			PropellerThread = new Thread(PropellerFunction);
            PropellerThread.Start();


        }
        private void image_MouseEnter(object sender, MouseEventArgs e)
        {

            if (image.Visibility == Visibility.Visible)
            {
                image.Visibility = Visibility.Collapsed;
                Thread.Sleep(40);

                image2.Visibility = Visibility.Visible;
            }
            else
            {
                image2.Visibility = Visibility.Collapsed;
                Thread.Sleep(40);
                image.Visibility = Visibility.Visible;
            }
        }
      
        private void image_MouseEnter()
        {
			this.Dispatcher.Invoke((Action)(() =>
			{//this refer to form in WPF application 
				
			}));
			if (image.Visibility == Visibility.Visible)
                    {
				this.Dispatcher.Invoke((Action)(() =>
				{
					image.Visibility = Visibility.Collapsed;
					image2.Visibility = Visibility.Visible;

				}));
				
                    }
                    else
                    {
				this.Dispatcher.Invoke((Action)(() =>
				{
					image2.Visibility = Visibility.Collapsed;
					image.Visibility = Visibility.Visible;
					

				}));
			}
                                     
        }



    }
}
