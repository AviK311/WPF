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

namespace UI_WPF
{
    /// <summary>
    /// Interaction logic for AddTrainee1.xaml
    /// </summary>
    public partial class AddTrainee1 : Window
    {
        BE.Trainee trainee;
        BL.IBL bl;
        public AddTrainee1()
        {
            InitializeComponent();
            trainee = new BE.Trainee();
            this.DataContext = trainee;
            bl = BL.FactoryBL.GetBL();
            this.keyComboBox.ItemsSource = Enum.GetValues(typeof(BE.VehicleType));
            this.sexComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gender));
            this.gearTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.GearType));
            this.currentCarTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.VehicleType));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource traineeViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("traineeViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // traineeViewSource.Source = [generic data source]
        }       
    }
}
