using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Messages
    {
		private static int messageNumber { get; set; } = 0;
        public Name Name { get; set; }
        public string ID { get; set; }
        public string Content { get; set; }
        public DateTime DateOfMessage { get; set; }
		public UserType UserType { get; set; }
		public int MessageNumber { get; set; }
		public bool UserReset { get; set; } = false;
        public override string ToString()
        {
            string s = DateOfMessage + "\n"+UserType.ToString() +" " + Name + "\nID: " + ID + "\n" + Content;
            return s;           
        }
		public Messages()
		{
			MessageNumber = messageNumber;
			messageNumber++;
		}
    }
}
