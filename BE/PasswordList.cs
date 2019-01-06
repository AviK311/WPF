﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
	public class Password
	{
		public string id, password;

		public Password(string id, string password)
		{
			this.id = id;
			this.password = password;
		}
		public Password() { }
	}
	public class PasswordList
	{
		private List<Password> passwords;
		public PasswordList() { passwords = new List<Password>(); }
		public bool CheckPassword(string id, string password)
		{
			return passwords.Any(P => P.id == id && P.password == password);
		}
		public void AddUpdatePassword(string id, string password)
		{
			var existing = passwords.FirstOrDefault(P => P.id == id);
			if (existing == null)
				passwords.Add(new Password(id, password));
			else existing.password = password;
		}
		public void RemovePassword(string id)
		{
			passwords.Remove(passwords.FirstOrDefault(P => P.id == id));
		}
	}
}