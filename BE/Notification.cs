using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BE
{/// <summary>
/// Each user has a list of notifications. when he logs in, he immediately receives his notifications, and then they are deleted.
/// </summary>
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
