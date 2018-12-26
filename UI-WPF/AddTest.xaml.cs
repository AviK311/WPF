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
    /// Interaction logic for AddTest.xaml
    /// </summary>
    public partial class AddTest : Window
    {
        BE.Test test;
        BL.IBL bl;
        public AddTest()
        {
            InitializeComponent();
            test = new BE.Test();
            this.DataContext = test;
            bl = BL.FactoryBL.GetBL();
            test.TestDateTime = DateTime.Now;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource testViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("testViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // testViewSource.Source = [generic data source]
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            test.BeginLocation = new Address(city: City.Text, street: Street.Text, buildingNumber: Number.Text);           
            bl = FactoryBL.GetBL();
            try
            {
                bl.AddTest(test);
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
