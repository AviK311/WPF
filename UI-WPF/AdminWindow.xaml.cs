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
		private void button_Click(object sender, RoutedEventArgs e)
		{
			DataContext =list.Where(d => d.ID.Contains(ID_textBox.Text) || d.Name.ToString().Contains(ID_textBox.Text));
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
	}
}
