using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
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
    /// Interaction logic for TesterWindow.xaml
    /// </summary>
    public partial class TesterWindow : Window
    {
        //private ObservableCollection<Tester> trainees = new ObservableCollection<Tester>();
        IBL bl;

        public TesterWindow()
        {
            InitializeComponent();
            bl = FactoryBL.GetBL();
			DataContext = bl.GetTesters();
			if (!(GlobalSettings.User is Admin))
				Add.Visibility = Visibility.Hidden;

			
		}
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Window tester = new TesterAdd();
            tester.Show();
			Close();
        }

		private void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (listBox.SelectedItem != null)
			{
				TesterView testerView = new TesterView((Tester)listBox.SelectedItem);
				testerView.Show();
				Close();				
			}
		}
        private void button_back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
        private void sort_Click(object sender, RoutedEventArgs e)
        {
            DataContext = bl.GetTesters().OrderBy(N => N.Name.last).OrderBy(N => N.Name.first);
        }

        private void All_RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            DataContext = bl.GetTesters();
        }


        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

            var button = sender as RadioButton;
            VehicleType vehicleType = new VehicleType();
            foreach (var item in (VehicleType[])Enum.GetValues(typeof(VehicleType)))
                if (item.ToString() == button.Name)
                {
                    vehicleType = item;
                    break;
                }
            DataContext = bl.GetTesters().Where(c => c.testingCarType == vehicleType);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = bl.GetTesters().Where(d => d.ID.Contains(ID_textBox.Text));
        }
    }
}
