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
    /// Interaction logic for TraineeWindow.xaml
    /// </summary>
    public partial class TraineeWindow : Window
    {
        //private ObservableCollection<Trainee> trainees=new ObservableCollection<Trainee>();
        IBL bl;        
        public TraineeWindow()
        {
            InitializeComponent();
            bl = FactoryBL.GetBL();
			if (!(GlobalSettings.User is Admin))
				Add.Visibility = Visibility.Hidden;
			DataContext = bl.GetTrainees();


        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Window trainee = new AddTrainee1();
            trainee.Show();
            Close();
        }      

        
        private void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                ViewTrainee traineeView = new ViewTrainee((Trainee)listBox.SelectedItem);
                traineeView.Show();
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
            DataContext = bl.GetTrainees().OrderBy(N=>N.Name.last).OrderBy(N => N.Name.first);
        }

        private void All_RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            DataContext = bl.GetTrainees();
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
			DataContext = bl.GetTrainees().Where(c => c.CurrentCarType == vehicleType);
		}
       

    }
}
