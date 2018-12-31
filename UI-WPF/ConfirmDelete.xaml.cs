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
using BE;
using BL;

namespace UI_WPF
{
	/// <summary>
	/// Interaction logic for ConfirmDelete.xaml
	/// </summary>
	public partial class ConfirmDelete : Window
	{
		IBL bl = FactoryBL.GetBL();
		Window toReturn;
		Button senderButton;
		string toDelete;
		public ConfirmDelete(object sender, Window window, string ID)
		{
			
			InitializeComponent();
			toReturn = window;
			senderButton = sender as Button;
		}

		private void ConfirmButton_Click(object sender, RoutedEventArgs e)
		{
			if (senderButton.Name.Contains("Tester"))

		}
	}
}
