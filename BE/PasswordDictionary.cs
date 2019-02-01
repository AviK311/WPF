using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
	public class PasswordDictionary
	{
		private Dictionary<string, string> PasswordDB;

		public PasswordDictionary()
		{
			PasswordDB = new Dictionary<string, string>();
		}
		/// <summary>
		/// checks if the password is the correct password of the user with the given id
		/// </summary>
		/// <param name="id"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public bool CheckPassword(string id, string password)
		{
			//return PasswordDB.Any(pair => pair.Key == id && pair.Value == password);
			return PasswordDB[id] == password;
		}
		/// <summary>
		/// </summary>
		/// <param name="id"></param>
		/// <param name="password"></param>
		public void AddUpdatePassword(string id, string password)
		{
			if (PasswordDB.Any(pair => pair.Key == id))
				PasswordDB[id] = password;
			else
				PasswordDB.Add(id, password);
		}
		public void RemovePassword(string id)
		{
			PasswordDB.Remove(id);
		}
        
	}
}
