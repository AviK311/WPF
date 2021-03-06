﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
	public class Admin : Person
	{
		public Admin(Name name)
		{
			Name = name;
		}
		public Admin()
		{
			Name = new Name();
		}
		public Admin(Admin other)
		{
			notifications = new List<Notification>(other.notifications);
			foreach (PropertyInfo property in other.GetType().GetProperties())
				if (property.CanWrite) property.SetValue(this, property.GetValue(other));
			
		}
	}
}
