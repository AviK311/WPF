using BE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; 
namespace DS
{
	public class DataSource
	{
		public static List<Test> testList = new List<Test>();
		public static List<Tester> testerList = new List<Tester>();
		public static List<Trainee> traineeList = new List<Trainee>();
		public static List<Admin> adminList = new List<Admin>();
        public static PasswordDictionary passwordDictionary = new PasswordDictionary();
        public static List<Messages> messagesList = new List<Messages>();

    }
}
