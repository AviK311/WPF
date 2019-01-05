using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

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
			throw new NotImplementedException();
		}

		public bool CheckPassword(string id, string password)
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
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
