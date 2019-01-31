using System;
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
using System.Threading;
using BE;
using BL;

namespace UI_WPF
{
	/// <summary>
	/// Interaction logic for TesterAdd.xaml
	/// </summary>
	public partial class TesterAdd : Window
	{

       


        Tester tester;
		IBL bl;
		List<CheckBox> sunCheckboxes = new List<CheckBox>();
		List<CheckBox> monCheckboxes = new List<CheckBox>();
		List<CheckBox> tueCheckboxes = new List<CheckBox>();
		List<CheckBox> wedCheckboxes = new List<CheckBox>();
		List<CheckBox> thursCheckboxes = new List<CheckBox>();
		public TesterAdd()
		{
			
			InitializeComponent();
			tester = new Tester();
			DataContext = tester;
			tester.BirthDay = DateTime.Now.AddYears(-(int)Configuration.MinAgeOfTester);
			tester.BirthDay = tester.BirthDay.AddDays(-1);
			sexComboBox.ItemsSource = Enum.GetValues(typeof(Gender));
			testingCarTypeComboBox.ItemsSource = Enum.GetValues(typeof(VehicleType));

			Functions.AddTemplateList(sunCheckboxes, Sun9, Sun10, Sun11, Sun12, Sun13, Sun14);
			Functions.AddTemplateList(monCheckboxes, Mon9, Mon10, Mon11, Mon12, Mon13, Mon14);
			Functions.AddTemplateList(tueCheckboxes, Tue9, Tue10, Tue11, Tue12, Tue13, Tue14);
			Functions.AddTemplateList(wedCheckboxes, Wed9, Wed10, Wed11, Wed12, Wed13, Wed14);
			Functions.AddTemplateList(thursCheckboxes, Thurs9, Thurs10, Thurs11, Thurs12, Thurs13, Thurs14);
            image2.Visibility = Visibility.Collapsed;
			InfoBlock.Text = "Add Tester";
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			tester.Address = new Address(city: City.Text, street: Street.Text, buildingNumber: Number.Text);
			tester.Name = new Name(FirstName.Text, LastName.Text);
			Schedule sched = new Schedule();
			foreach (var item in sunCheckboxes)
				sched[DayOfWeek.Sunday][sunCheckboxes.IndexOf(item) + 9] = (bool)item.IsChecked;
			foreach (var item in monCheckboxes)
				sched[DayOfWeek.Monday][monCheckboxes.IndexOf(item) + 9] = (bool)item.IsChecked;
			foreach (var item in tueCheckboxes)
				sched[DayOfWeek.Tuesday][tueCheckboxes.IndexOf(item) + 9] = (bool)item.IsChecked;
			foreach (var item in wedCheckboxes)
				sched[DayOfWeek.Wednesday][wedCheckboxes.IndexOf(item) + 9] = (bool)item.IsChecked;
			foreach (var item in thursCheckboxes)
				sched[DayOfWeek.Thursday][thursCheckboxes.IndexOf(item) + 9] = (bool)item.IsChecked;
			tester.schedule = sched;
			bl = FactoryBL.GetBL();
			

			try
			{
				Functions.ValidatePerson(tester);
				bl.AddTester(tester);
                AutoClosingMessageBox.Show("Adding Successful!", "Alert", 3500, MessageBoxButton.OK, MessageBoxImage.Information);
               
                //MessageBox.Show("Adding Successful!", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                TesterWindow testerWindow = new TesterWindow();
				testerWindow.Show();
				Close();
				
			}catch (InvalidOperationException exc) { 
			
				MessageBox.Show(exc.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		
		}
       

        private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			Window testerWindow = new TesterWindow();
			testerWindow.Show();
			Close();
		}

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key < Key.D0 || e.Key > Key.D9)
            //    e.Handled = true;
            if ((e.Key < Key.D0 || e.Key > Key.D9) && (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) && e.Key != Key.Tab)
            {
                e.Handled = true;
            }
        }
        private void Phone_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key != Key.OemMinus && e.Key != Key.Subtract && (e.Key < Key.D0 || e.Key > Key.D9) && (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) && e.Key != Key.Tab)
				e.Handled = true;
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

		
		
		private void CheckBoxEnterEvent(object sender, MouseEventArgs e)
		{
			string SenderName = (sender as CheckBox).Name.ToLower();
			DayOfWeek day = DayOfWeek.Friday;
			int hour = 0;
			if (SenderName.Contains("sun")) day = DayOfWeek.Sunday;
			if (SenderName.Contains("mon")) day = DayOfWeek.Monday;
			if (SenderName.Contains("tue")) day = DayOfWeek.Tuesday;
			if (SenderName.Contains("wed")) day = DayOfWeek.Wednesday;
			if (SenderName.Contains("thurs")) day = DayOfWeek.Thursday;
			if (SenderName.Contains("9")) hour = 9;
			if (SenderName.Contains("10")) hour = 10;
			if (SenderName.Contains("11")) hour = 11;
			if (SenderName.Contains("12")) hour = 12;
			if (SenderName.Contains("13")) hour = 13;
			if (SenderName.Contains("14")) hour = 14;

			InfoBlock.Text = string.Format("Check this box if the tester is available on {0} at {1}:00", day, hour);
		}
		private void MouseLeave(object sender, MouseEventArgs e)
		{
			InfoBlock.Text = "Add Tester";
		}
 
        private void emailTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                string s = emailTextBox.Text + "gmail.com";
                if (emailTextBox.Text.Last() == '@' && e.Key != Key.Delete && e.Key != Key.Back && !emailTextBox.Text.Contains("@gmail.com"))
                    emailTextBox.Text = s;
            }
            catch { }
        }
    }
}
