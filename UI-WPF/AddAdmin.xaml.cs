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
	/// Interaction logic for AddAdmin.xaml
	/// </summary>
	public partial class AddAdmin : Window
	{
		IBL bl;
		Admin admin;
		public AddAdmin()
		{
			
			InitializeComponent();
			admin = new Admin();
			bl = FactoryBL.GetBL();
			DataContext = admin;
			sexComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gender));
			admin.BirthDay = DateTime.Now.AddYears(-25);
			admin.BirthDay = admin.BirthDay.AddDays(-1);
		}
		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			//admin.Name = new Name(firstNameTextBox.Text, lastNameTextBox.Text);
			try
			{
				Functions.ValidatePerson(admin);
				bl.AddAdmin(admin);
				MessageBox.Show("Adding Successful!", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
				AdminWindow adminWindow = new AdminWindow();
				adminWindow.Show();
				Close();
			}
			catch (InvalidOperationException exc)
			{
				MessageBox.Show(exc.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			Window adminWindow = new AdminWindow();
			adminWindow.Show();
			Close();
		}

		private void TextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if ((e.Key < Key.D0 || e.Key > Key.D9) && (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) && e.Key != Key.Tab)
				e.Handled = true;
		}
		private void Phone_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key != Key.OemMinus && e.Key != Key.Subtract && (e.Key < Key.D0 || e.Key > Key.D9) && (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) && e.Key != Key.Tab)
				e.Handled = true;
		}


	}
}
