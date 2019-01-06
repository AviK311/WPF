using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BE
{
	public class Notification
	{
		public Notification(MessageIcon icon, string message)
		{
			Icon = icon;
			this.time = DateTime.Now;
			this.message = message;
		}
		public Notification() { }

		public MessageIcon Icon { get; set; }
		public DateTime time { get; set; }
		public string message { get; set; }
		
	}
}
