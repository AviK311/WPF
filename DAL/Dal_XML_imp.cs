using System;
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
			throw new NotImplementedException();
		}

		public void AddMessage(Messages message)
		{
			throw new NotImplementedException();
		}

		public void AddTest(Test test)
		{
			throw new NotImplementedException();
		}

		public void AddTester(Tester tester)
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

		public IEnumerable<Admin> GetAdmins()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Messages> GetMessages()
		{
			throw new NotImplementedException();
		}

		public string GetPassword(string id)
		{
			throw new NotImplementedException();
		}

		public Test GetTest(string id)
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
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
		}

		public void RemoveAdmin(Admin toRemove)
		{
			throw new NotImplementedException();
		}

		public void RemoveMessage(int num)
		{
			throw new NotImplementedException();
		}

		public void RemovePassword(string id)
		{
			DataSource.PasswordList.RemovePassword(id);
			XMLHandler.GetXMLHandler().SaveToXML(DataSource.PasswordList, XMLHandler.GetXMLHandler().PasswordPath);
		}

		public void RemoveTest(Test toRemove)
		{
			throw new NotImplementedException();
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
