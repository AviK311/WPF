using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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
		/// <summary>
		/// checks if the given password is the password of the user with the given id
		/// </summary>
		/// <param name="id"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public bool CheckPassword(string id, string password)
		{
			return passwords.Any(P => P.id == id && Encrypt.DecryptString(P.password) == password);
		}
		/// <summary>
		/// if the user already has a password, his password will be changed. otherwise, he will receive a new password.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="password"></param>
		public void AddUpdatePassword(string id, string password)
		{
			var existing = passwords.FirstOrDefault(P => P.id == id);
			if (existing == null)
				passwords.Add(new Password(id, Encrypt.EncryptString(password)));
			else existing.password = Encrypt.EncryptString(password);
		}
		/// <summary>
		/// removes a user's password from the password db.
		/// </summary>
		/// <param name="id"></param>
		public void RemovePassword(string id)
		{
			passwords.Remove(passwords.FirstOrDefault(P => P.id == id));
		}
		/// <summary>
		/// The password list is a private field, and cannot be serialized by external classes.
		/// It has its own serialize function.
		/// </summary>
		/// <param name="file"></param>
		public void Serialize(FileStream file)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(passwords.GetType());
			xmlSerializer.Serialize(file, passwords);
		}
		/// <summary>
		/// The password list is a private field, and cannot be deserialized by external classes.
		/// It has its own deserialize function.
		/// </summary>
		/// <param name="file"></param>
		public PasswordList Deserialize(FileStream file)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Password>));
			List<Password> result = (List<Password>)xmlSerializer.Deserialize(file);
			return new PasswordList() { passwords = result };
		}


	}
}
