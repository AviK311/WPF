using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Messages
    {
        public Name Name { get; set; }
        public string ID { get; set; }
        public string Content { get; set; }
        public DateTime DateOfMessage { get; set; }
		public UserType UserType { get; set; }
        public override string ToString()
        {
            string s = DateOfMessage + "\n"+UserType.ToString() +" " + Name + "\nID: " + ID + "\n" + Content;
            return s;           
        }
    }
}
