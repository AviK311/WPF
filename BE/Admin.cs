using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
	public class Admin : Person
	{
		public Admin(Name name, string password)
		{
			Name = name;
			Password = password;
			clearanceLevel = ClearanceLevel.Admin;
		}
		public Admin(Admin other)
		{
			clearanceLevel = ClearanceLevel.Admin;
			foreach (PropertyInfo property in other.GetType().GetProperties())
				property.SetValue(this, property.GetValue(other));
			Password = other.Password;
		}
	}
}
