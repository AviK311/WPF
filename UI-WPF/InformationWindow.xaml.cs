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
    /// Interaction logic for InformationWindow.xaml
    /// </summary>
    public partial class InformationWindow : Window
    {
        public InformationWindow()
        {
            InitializeComponent();
            a1.Visibility = Visibility.Collapsed;
            a2.Visibility = Visibility.Collapsed;
            a3.Visibility = Visibility.Collapsed;
            a4.Visibility = Visibility.Collapsed;
        }
        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var question = sender as Label;
            switch (question.Name)
            {
                case "q1":
                    if (a1.Visibility == Visibility.Visible)
                        a1.Visibility = Visibility.Collapsed;
                    else
                        a1.Visibility = Visibility.Visible;
                    break;
                case "q2":
                    if (a2.Visibility == Visibility.Visible)
                        a2.Visibility = Visibility.Collapsed;
                    else
                        a2.Visibility = Visibility.Visible;
                    break;
                case "q3":
                    if (a3.Visibility == Visibility.Visible)
                        a3.Visibility = Visibility.Collapsed;
                    else
                        a3.Visibility = Visibility.Visible;
                    break;
                case "q4":
                    if (a4.Visibility == Visibility.Visible)
                        a4.Visibility = Visibility.Collapsed;
                    else
                        a4.Visibility = Visibility.Visible;
                    break;
                default: break;
            }                         
        }
    }
}
