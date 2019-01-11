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
			testlist = bl.GetTests().ToList();
			if (GlobalSettings.User is Trainee)
				DataContext = (from item in testlist
							   where item.TraineeID == GlobalSettings.User.ID
							   select item).ToList();
			else if(GlobalSettings.User is Tester)
				DataContext = (from item in testlist
							   where item.TesterID == GlobalSettings.User.ID
							   select item).ToList();
			else DataContext = testlist;
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
    }
}
