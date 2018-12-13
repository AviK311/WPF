using System;

namespace BE
{

    public struct Name
	{

		private readonly string first, last;

		public Name(string first, string last)
		{
			this.first = first;
			this.last = last;
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", first, last);
		}
		
	}
	public class Person
	{
		
		public string ID { get; set; }
		
		public DateTime BirthDay { get; set; }
		public Name Name { get; set; }
		public string PhoneNumber { get; set; }
		public Address Address { get; set; }
		public Gender Sex { get; set; }

		public ushort GetAge()
		{
			DateTime now = DateTime.Today;
			ushort age = (ushort)(now.Year - BirthDay.Year);
			if (now < BirthDay.AddYears(age)) age--;
			return age;
		}
	}
}
