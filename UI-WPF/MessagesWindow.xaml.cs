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
    /// Interaction logic for MessagesWindow.xaml
    /// </summary>
    public partial class MessagesWindow : Window
    {
        IBL bl;
        public MessagesWindow()
        {
            InitializeComponent();
            bl = FactoryBL.GetBL();            
            DataContext = bl.GetMessages();
        }

		private void Back_Click(object sender, RoutedEventArgs e)
		{
			Window main = new MainWindow();
			main.Show();
			Close();

		}

		private void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (listBox.SelectedItem != null)
			{
				bl.RemoveMessage(((Messages)listBox.SelectedItem).MessageNumber);
				DataContext = bl.GetMessages();
			}
		}
	}
}
