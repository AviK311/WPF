﻿using System;
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
        //private ObservableCollection<Tester> trainees = new ObservableCollection<Tester>();
        IBL bl;

        public TesterWindow()
        {
            InitializeComponent();
            bl = FactoryBL.GetBL();
			DataContext = bl.GetTesters();
		}
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Window tester = new TesterAdd();
            tester.Show();
			Close();
        }

		private void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (listBox.SelectedItem != null)
			{
				TesterView testerView = new TesterView((Tester)listBox.SelectedItem);
				testerView.Show();
				Close();
				
			}

		}
	}
}