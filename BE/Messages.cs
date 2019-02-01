using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
	/// <summary>
	/// messages that a non-admin can leave for the admins. The messages are sent to all admins, and upon deleting one, it is deleted for all admins.
	/// </summary>
    public class Messages
    {
		
        public Name Name { get; set; }
        public string ID { get; set; }
        public string Content { get; set; }
        public DateTime DateOfMessage { get; set; }
		public UserType UserType { get; set; }
		public int MessageNumber { get; set; }
		public bool UserReset { get; set; } = false;
        public override string ToString()
        {
            string c= Content;
            if (c.Length > 25)
            {
                for (int i = 25; i < c.Length; i += 30)
                {
                    while (c[i] != ' ')
                        i--;
                    c = c.Substring(0, i) + "\n" + c.Substring(i++);
                }
            }

            string s = DateOfMessage + "\n"+UserType.ToString() +" " + Name + "\nID: " + ID + "\n" + c+ "\n";
            return s;           
        }
		public Messages()
		{			
		}
    }
}
