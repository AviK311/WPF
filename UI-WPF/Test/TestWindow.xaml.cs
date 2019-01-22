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
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        IBL bl;
		List<Test> testlist;
        public TestWindow()
        {
            InitializeComponent();
            bl = FactoryBL.GetBL();
			
			if (GlobalSettings.User is Trainee)
				testlist = (from item in bl.GetTests()
							   where item.TraineeID == GlobalSettings.User.ID
							   select item).ToList();
			else if(GlobalSettings.User is Tester)
				testlist = (from item in bl.GetTests()
							   where item.TesterID == GlobalSettings.User.ID
							   select item).ToList();
			else testlist = bl.GetTests().ToList();
			DataContext = testlist;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Window test = new TestAdd();
            test.Show();
            Close();
        }
        private void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                TestView testerView = new TestView((Test)listBox.SelectedItem, testlist);
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

        private void All_RadioButton_Checked(object sender, RoutedEventArgs e)
        {
			DataContext = testlist;
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
            DataContext =testlist.Where(c => c.TestingCarType == vehicleType);
        }

        private void image_MouseEnter(object sender, MouseEventArgs e)
        {
            ScaleTransform scale = new ScaleTransform(1.1, 1.1);
            image.RenderTransform = scale;
        }

        private void image_MouseLeave(object sender, MouseEventArgs e)
        {
            image.RenderTransform = null;
        }
    }
}
