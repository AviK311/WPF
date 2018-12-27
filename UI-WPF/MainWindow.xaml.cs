﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI_WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		LoginWindow toReturn;
		public MainWindow()
		{
			InitializeComponent();
		}

        private void button_trainee_Click(object sender, RoutedEventArgs e)
        {
            Window trainee = new TraineeWindow();
            trainee.Show();
            Close();
        }

        private void button_tester_Click(object sender, RoutedEventArgs e)
        {
            Window tester = new TesterWindow();
            tester.ShowDialog();
        }

        private void button_test_Click(object sender, RoutedEventArgs e)
        {
            Window test = new TestWindow();
            test.ShowDialog();
        }

		private void LogOut_Click(object sender, RoutedEventArgs e)
		{
			Window loginWindow = new LoginWindow();
			loginWindow.Show();
			Close();
		}
	}
}
