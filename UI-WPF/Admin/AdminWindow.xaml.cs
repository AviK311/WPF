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
using BL;
using BE;

namespace UI_WPF
{
	/// <summary>
	/// Interaction logic for AdminWindow.xaml
	/// </summary>
	public partial class AdminWindow : Window
	{
		IBL bl;
		List<Admin> list;
		public AdminWindow()
		{
			InitializeComponent();
			bl = FactoryBL.GetBL();
			list = bl.GetAdmins().ToList();
			DataContext = list;
		}
		private void button_back_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = new MainWindow();
			mainWindow.Show();
			Close();
		}
		private void ID_textBox_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			DataContext = bl.GetTesters().Where(d => d.ID.Contains(ID_textBox.Text) || d.Name.ToString().ToLower().Contains(ID_textBox.Text.ToLower()));
		}

		private void Add_Click(object sender, RoutedEventArgs e)
		{
			Window adminAdd = new AddAdmin();
			adminAdd.Show();
			Close();
		}
		private void sort_Click(object sender, RoutedEventArgs e)
		{
			DataContext = bl.GetAdmins().OrderBy(N => N.Name.last).OrderBy(N => N.Name.first);
		}

		private void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (listBox.SelectedItem != null)
			{
				AdminView adminView = new AdminView((Admin)listBox.SelectedItem, list);
				adminView.Show();
				Close();
			}
		}
        private void image_MouseEnter(object sender, MouseEventArgs e)
        {
            image.Visibility = Visibility.Collapsed;
            System.Threading.Thread.Sleep(200);
            image2.Visibility = Visibility.Visible;
        }

        private void image2_MouseEnter(object sender, MouseEventArgs e)
        {
            image2.Visibility = Visibility.Collapsed;
            System.Threading.Thread.Sleep(200);
            image.Visibility = Visibility.Visible;
        }
    }
}
