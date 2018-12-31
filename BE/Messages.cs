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
        public string content { get; set; }
        public DateTime dateOfMessage { get; set; }
        public override string ToString()
        {
            string s = "\n" + dateOfMessage+ "\nName: " + Name + " ID: " + ID + "\n" + content;
            return s;           
        }
    }
}
