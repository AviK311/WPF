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
        private ObservableCollection<Trainee> trainees=new ObservableCollection<Trainee>();
        IBL bl;
        Trainee trainee;
        public TraineeWindow()
        {
            InitializeComponent();
            bl = FactoryBL.GetBL();
			
           
			DataContext = bl.GetTrainees();
			//trainees = (ObservableCollection<Trainee>)bl.GetTrainees();
		}

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Window trainee = new AddTrainee1();
            trainee.Show();
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            Window trainee = new ViewTrainee();
            trainee.Show();
        }

        private void listBox1_GotMouseCapture(object sender, MouseEventArgs e)
        {
            trainee = (Trainee)sender;
        }
    }
}
