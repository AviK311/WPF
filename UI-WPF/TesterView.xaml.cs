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
	/// Interaction logic for TesterView.xaml
	/// </summary>
	public partial class TesterView : Window
	{
		
		IBL bl;
		Tester tester;
		List<CheckBox> sunCheckboxes = new List<CheckBox>();
		List<CheckBox> monCheckboxes = new List<CheckBox>();
		List<CheckBox> tueCheckboxes = new List<CheckBox>();
		List<CheckBox> wedCheckboxes = new List<CheckBox>();
		List<CheckBox> thursCheckboxes = new List<CheckBox>();
        //public TesterView(Tester tester1)

        public TesterView(Tester tester1)
		{
			Global.IsUpdate = false;
			
			InitializeComponent();
			SaveButton.Visibility = Visibility.Hidden;
			tester = tester1;
			DataContext = tester1;
			sexComboBox.ItemsSource = Enum.GetValues(typeof(Gender));
			testingCarTypeComboBox.ItemsSource = Enum.GetValues(typeof(VehicleType));
			
			Functions.AddTemplateList(sunCheckboxes, Sun9, Sun10, Sun11, Sun12, Sun13, Sun14);
			Functions.AddTemplateList(monCheckboxes, Mon9, Mon10, Mon11, Mon12, Mon13, Mon14);
			Functions.AddTemplateList(tueCheckboxes, Tue9, Tue10, Tue11, Tue12, Tue13, Tue14);
			Functions.AddTemplateList(wedCheckboxes, Wed9, Wed10, Wed11, Wed12, Wed13, Wed14);
			Functions.AddTemplateList(thursCheckboxes, Thurs9, Thurs10, Thurs11, Thurs12, Thurs13, Thurs14);
			foreach (var item in sunCheckboxes)
				item.IsChecked = tester.schedule[DayOfWeek.Sunday][sunCheckboxes.IndexOf(item) + 9];
			foreach (var item in monCheckboxes)
				item.IsChecked = tester.schedule[DayOfWeek.Monday][monCheckboxes.IndexOf(item) + 9];
			foreach (var item in tueCheckboxes)
				item.IsChecked = tester.schedule[DayOfWeek.Tuesday][tueCheckboxes.IndexOf(item) + 9];
			foreach (var item in wedCheckboxes)
				item.IsChecked = tester.schedule[DayOfWeek.Wednesday][wedCheckboxes.IndexOf(item) + 9];
			foreach (var item in thursCheckboxes)
				item.IsChecked = tester.schedule[DayOfWeek.Thursday][thursCheckboxes.IndexOf(item) + 9];
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			EditButton.Visibility = Visibility.Hidden;
			SaveButton.Visibility = Visibility.Visible;
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			foreach (var item in sunCheckboxes)
				tester.schedule[DayOfWeek.Sunday][sunCheckboxes.IndexOf(item) + 9] = (bool)item.IsChecked;
			foreach (var item in monCheckboxes)
				tester.schedule[DayOfWeek.Monday][monCheckboxes.IndexOf(item) + 9] = (bool)item.IsChecked;
			foreach (var item in tueCheckboxes)
				tester.schedule[DayOfWeek.Tuesday][tueCheckboxes.IndexOf(item) + 9] = (bool)item.IsChecked;
			foreach (var item in wedCheckboxes)
				tester.schedule[DayOfWeek.Wednesday][wedCheckboxes.IndexOf(item) + 9] = (bool)item.IsChecked;
			foreach (var item in thursCheckboxes)
				tester.schedule[DayOfWeek.Thursday][thursCheckboxes.IndexOf(item) + 9] = (bool)item.IsChecked;
			bl = FactoryBL.GetBL();
			try
			{
				bl.UpdateTester(tester);
			}
			catch (InvalidOperationException exc)
			{

				MessageBox.Show(exc.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}
	}
}
