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
        private ObservableCollection<Tester> trainees = new ObservableCollection<Tester>();
        IBL bl;

        public TesterWindow()
        {
            InitializeComponent();
            bl = FactoryBL.GetBL();
            DataContext = trainees;            
            Tester c = new Tester
            {
                ID = "123456",
                Name = new Name("Dan", "internatonal"),
                Sex = Gender.Male,
                PhoneNumber = "224",
                BirthDay = new DateTime(1969, 2, 1),
                Address = new Address("jeru", "vaad", "21"),
                MaxDistance = 60,
                MaxWeeklyTests = 9,
                ExpYears = 6,
                testingCarType = VehicleType.PrivateCar,
                schedule = new Schedule()
            };
            trainees.Add(c);           
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Window tester = new TesterAdd();
            tester.Show();
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            //Window tester = new TesterView();
            //tester.Show();
        }
    }
}
