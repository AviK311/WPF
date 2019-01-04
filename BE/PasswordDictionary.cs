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

		public bool CheckPassword(string id, string password)
		{
			//return PasswordDB.Any(pair => pair.Key == id && pair.Value == password);
			return PasswordDB[id] == password;
		}
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
        public string GetPassword(string id)
        {
            return PasswordDB[id];
        }
	}
}
