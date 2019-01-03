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
				if (tester.Email != null)
				{
					Match match = GlobalSettings.EmailRegex.Match(tester.Email);
					if (!match.Success)
						throw new InvalidOperationException("The email address is invalid");
				}
				else throw new InvalidOperationException("The email address is invalid");
				bl.AddTester(tester);
				MessageBox.Show("Adding Successful!", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (e.Key < Key.D0 || e.Key > Key.D9)
                e.Handled = true;
            //if ((e.Key < Key.D0 || e.Key > Key.D9) && (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) && e.Key != Key.Tab)
            //{
            //    e.Handled = true;
            //}
        }       
    }
}
