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
        public TestWindow()
        {
            InitializeComponent();
            bl = FactoryBL.GetBL();
            DataContext = bl.GetTests();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Window test = new AddTest();
            test.Show();
            Close();
        }
        private void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                TestView testerView = new TestView((Test)listBox.SelectedItem);
                testerView.Show();
                Close();
            }

        }
    }
}
