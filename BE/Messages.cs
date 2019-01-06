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
		public int MessageNumber { get; set; }
		public bool UserReset { get; set; } = false;
        public override string ToString()
        {          
            if (Content.Length>25)
            {
                for (int i = 25; i < Content.Length; i += 30)
                {
                    while (Content[i] != ' ')
                        i--;
                        Content = Content.Substring(0, i) + "\n" + Content.Substring(i++);                  
                }
            }
            
            string s = DateOfMessage + "\n"+UserType.ToString() +" " + Name + "\nID: " + ID + "\n" + Content+ "\n";
            return s;           
        }
		public Messages()
		{			
		}
    }
}
