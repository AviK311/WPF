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
	/// Interaction logic for TesterView.xaml
	/// </summary>
	public partial class TesterView : Window
	{

        IBL bl = BL.FactoryBL.GetBL();
        Tester tester;
		List<CheckBox> sunCheckboxes = new List<CheckBox>();
		List<CheckBox> monCheckboxes = new List<CheckBox>();
		List<CheckBox> tueCheckboxes = new List<CheckBox>();
		List<CheckBox> wedCheckboxes = new List<CheckBox>();
		List<CheckBox> thursCheckboxes = new List<CheckBox>();
        List<Tester> testerList;       
        //public TesterView(Tester tester1)

        public TesterView(Tester tester1, List<Tester> list)
		{			
			InitializeComponent();
			SaveButton.Visibility = Visibility.Hidden;
			
			if (GlobalSettings.User is Tester)
			{
				RightButton.Visibility = Visibility.Hidden;
				LeftButton.Visibility = Visibility.Hidden;
			}
			else if(GlobalSettings.User is Trainee)
			{
					EditButton.IsEnabled = false;
					TesterDeleteButton.IsEnabled = false;
					CancelButton.IsEnabled = false;
			}
			testerList = Functions.TrueCopyTesters(list);
			tester = new Tester(tester1);
			DataContext = tester;
    
            

            sexComboBox.ItemsSource = Enum.GetValues(typeof(Gender));
			testingCarTypeComboBox.ItemsSource = Enum.GetValues(typeof(VehicleType));
			bl = FactoryBL.GetBL();
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

			try
			{
				Functions.ValidatePerson(tester);

				bl.UpdateTester(tester);
                EditButton.Visibility = Visibility.Visible;
				SaveButton.Visibility = Visibility.Hidden;
				MessageBox.Show("Update Successful!", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			catch (InvalidOperationException exc)
			{

				MessageBox.Show(exc.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
            var resetResult = MessageBox.Show(" Are you sure you want to delete this tester?", "Delete Tester", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (resetResult == MessageBoxResult.Yes)
            {
                try
                {
                    bl.RemoveTester(tester.ID);
					bl.RemovePassword(tester.ID);
                    if (!(GlobalSettings.User is Admin))
                    {
                        LoginWindow loginWindow = new LoginWindow();
                        loginWindow.Show();
                        Close();
                    }
                    else
                    {
                        if (bl.GetTesters().Count() == 0)
                        {
                            TesterWindow testerWindow = new TesterWindow();
                            testerWindow.Show();
                            Close();
                        }
                        else
                        {
                            testerList = (List<Tester>)bl.GetTesters();
                            RightButton_Click(sender, e);
                        }
                    }
                }
                catch (InvalidOperationException exc)
                {
                    MessageBox.Show(exc.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }

            }
            //Window confirmDelete = new ConfirmDelete(sender, this, tester.ID);
			//confirmDelete.ShowDialog();
			
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{

			EditButton.Visibility = Visibility.Visible;
			SaveButton.Visibility = Visibility.Hidden;
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			Window tester; 
			if (GlobalSettings.User is Tester)
				tester = new MainWindow();
			else tester = new TesterWindow();
			tester.Show();
			Close();
		}
        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = testerList.FindIndex(T => T.Equals(tester));
            if (currentIndex + 1 == testerList.Count)
                currentIndex = -1;
            tester = testerList[currentIndex + 1];
           
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
            DataContext = tester;
        }
        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            //SaveButton.Visibility = Visibility.Hidden;
            //EditButton.Visibility = Visibility.Visible;
            //Tester t;
            //for (int i = 0; i < bl.GetTrainees().Count() - 1; i++)
            //{
            //    t = bl.GetTesters().First();
            //    bl.RemoveTester(t.ID);
            //    bl.AddTester(t);
            //}
            //tester = new Tester(bl.GetTesters().First());
            int currentIndex = testerList.FindIndex(T => T.Equals(tester));
            if (currentIndex == 0)
                currentIndex = testerList.Count;
            tester = testerList[currentIndex - 1];
            DataContext = tester;
           
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

        private void RightButton_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key < Key.D0 || e.Key > Key.D9) && (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) && e.Key != Key.Tab)
                e.Handled = true;            
        }
		private void Phone_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key != Key.OemMinus && e.Key != Key.Subtract && (e.Key < Key.D0 || e.Key > Key.D9) && (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) && e.Key != Key.Tab)
				e.Handled = true;
		}

	}
}
