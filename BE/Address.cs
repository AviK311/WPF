namespace BE
{
    public class Address
    {
        public string city {get; set;}
        public string street { get; set; }
        public string buildingNumber { get; set; }

        public Address(string city, string street, string buildingNumber)
		{
            this.city = city;
			this.street = street;
			this.buildingNumber = buildingNumber;
		}
		public Address(Address other)
		{
			this.city = other.city;
			this.street = other.street;
			this.buildingNumber = other.buildingNumber;
		}
		public Address()
		{
			this.city = "";
			this.street = "";
			this.buildingNumber = "";
		}

		public override string ToString()
		{
			return string.Format("{0} {1}, {2}", buildingNumber, street, city);
		}
	}
}
