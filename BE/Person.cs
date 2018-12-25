using System;

namespace BE
{

    public struct Name
	{

		public string first { get; set; }
        public string last { get; set; }

        public Name(string first, string last)
		{
			this.first = first;
			this.last = last;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Name))
			{
				return false;
			}

			var name = (Name)obj;
			return first == name.first &&
				   last == name.last;
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", first, last);
		}
		
	}
	public class Person
	{
		public ClearanceLevel clearanceLevel;
		protected string Password;
		protected string id;
		public DateTime BirthDay { get; set; }
		public Name Name { get; set; }
		public string PhoneNumber { get; set; }
		public Address Address { get; set; }
		public Gender Sex { get; set; }
		public string ID { get => id?.PadLeft(8,'0'); set => id = value; }

		public override bool Equals(object obj)
		{
			var person = obj as Person;
			return person != null &&
				   ID == person.ID;
		}
		public bool CheckPassword(string attempt)
		{
			return attempt == Password;
		}
		public ushort GetAge()
		{
			DateTime now = DateTime.Today;
			ushort age = (ushort)(now.Year - BirthDay.Year);
			if (now < BirthDay.AddYears(age)) age--;
			return age;
		}
	}
}
