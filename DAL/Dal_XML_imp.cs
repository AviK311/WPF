﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;

namespace DAL
{
	class Dal_XML_imp : Idal
	{
		public void AddAdmin(Admin admin)
		{
			DataSource.adminList.Add(admin);
			XMLHandler.GetXMLHandler().SaveToXML(DataSource.adminList, XMLHandler.GetXMLHandler().AdminPath);
		}

		public void AddMessage(Messages message)
		{
			DataSource.MessagesList.Add(message);
			XMLHandler.GetXMLHandler().SaveToXML(DataSource.MessagesList, XMLHandler.GetXMLHandler().MessagePath);

		}

		public void AddTest(Test test)
		{
			DataSource.testList.Add(test);
			XMLHandler.GetXMLHandler().SaveToXML(DataSource.testList, XMLHandler.GetXMLHandler().TestPath);
		}

		public void AddTester(Tester tester)
		{
			/////for tamar
		}

		public void AddTrainee(Trainee trainee)
		{
			XMLHandler.GetXMLHandler().AddTrainee(trainee);
		}

		public void AddUpdatePassword(string id, string password)
		{
			DataSource.PasswordList.AddUpdatePassword(id, password);
			XMLHandler.GetXMLHandler().SaveToXML(DataSource.PasswordList, XMLHandler.GetXMLHandler().PasswordPath);
		}

		public bool CheckPassword(string id, string password)
		{
			return DataSource.PasswordList.CheckPassword(id, password);
		}

		public Admin GetAdmin(string id)
		{
			Admin toReturn = DataSource.adminList.FirstOrDefault(admin => admin.ID == id);
			if (toReturn == null)
				throw new InvalidOperationException("That Admin doesn't exist");
			return new Admin(toReturn);
		}

		public IEnumerable<Admin> GetAdmins()
		{
			return new List<Admin>(DataSource.adminList);
		}

		public IEnumerable<Messages> GetMessages()
		{
			return new List<Messages>(DataSource.MessagesList);
		}

		

		public Test GetTest(string id)
		{
			Test toReturn = DataSource.testList.FirstOrDefault(test => test.TestNumber == id);
			if (toReturn == null)
				throw new InvalidOperationException("That test doesn't exist");
			return new Test(toReturn);
		}

		public Tester GetTester(string id)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Tester> GetTesters()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Test> GetTests()
		{
			return new List<Test>(DataSource.testList);
		}

		public Trainee GetTrainee(string id)
		{
			return XMLHandler.GetXMLHandler().GetTrainee(id);
		}

		public IEnumerable<Trainee> GetTrainees()
		{
			return XMLHandler.GetXMLHandler().GetTrainees();
		}

		public void Initialize()
		{
			DataSource.MessagesList = XMLHandler.GetXMLHandler().LoadFromXML<List<Messages>>(XMLHandler.GetXMLHandler().MessagePath);
			DataSource.PasswordList = XMLHandler.GetXMLHandler().LoadFromXML<PasswordList>(XMLHandler.GetXMLHandler().PasswordPath);
			DataSource.testList = XMLHandler.GetXMLHandler().LoadFromXML<List<Test>>(XMLHandler.GetXMLHandler().TestPath);
			DataSource.adminList = XMLHandler.GetXMLHandler().LoadFromXML<List<Admin>>(XMLHandler.GetXMLHandler().AdminPath);
		}

		public void RemoveAdmin(Admin toRemove)
		{
			DataSource.adminList.Remove(toRemove);
			XMLHandler.GetXMLHandler().SaveToXML(DataSource.adminList, XMLHandler.GetXMLHandler().AdminPath);
		}

		public void RemoveMessage(int num)
		{
			DataSource.MessagesList.RemoveAll(T => T.MessageNumber == num);
			XMLHandler.GetXMLHandler().SaveToXML(DataSource.MessagesList, XMLHandler.GetXMLHandler().MessagePath);


		}

		public void RemovePassword(string id)
		{
			DataSource.PasswordList.RemovePassword(id);
			XMLHandler.GetXMLHandler().SaveToXML(DataSource.PasswordList, XMLHandler.GetXMLHandler().PasswordPath);
		}

		public void RemoveTest(Test toRemove)
		{
			DataSource.testList.Remove(toRemove);
			XMLHandler.GetXMLHandler().SaveToXML(DataSource.testList, XMLHandler.GetXMLHandler().TestPath);

		}

		public void RemoveTester(Tester toRemove)
		{
			throw new NotImplementedException();
		}

		public void RemoveTrainee(Trainee toRemove)
		{
			XMLHandler.GetXMLHandler().RemoveTrainee(toRemove.ID);
		}
	}
}
