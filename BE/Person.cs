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
		
		public string ID { get => iD; set => iD = value; }
		public string FirstName { get => firstName; set => firstName = value; }
		public string LastName { get => lastName; set => lastName = value; }
		public DateTime BirthDay { get => birthDay; set => birthDay = value; }
		public Name Name { get => name; set => name = value; }
		public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
		public Address Address { get => address; set => address = value; }
		public sex Sex { get => sex; set => sex = value; }
		public CarType CarType { get => carType; set => carType = value; }

		protected CarType carType;
		protected Address address;
		protected sex sex;
		protected string phoneNumber;
		protected DateTime birthDay;
		protected Name name;
		protected string iD;
		protected string firstName;
		protected string lastName;

	}
}
