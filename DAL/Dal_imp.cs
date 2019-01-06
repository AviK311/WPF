using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BE;
using DS;

namespace DAL
{
    public class Dal_imp : Idal
	{
		public Test GetTest(string id)
		{
			Test toReturn = DataSource.testList.FirstOrDefault(test => test.TestNumber == id);
			if (toReturn == null)
				throw new InvalidOperationException("That test doesn't exist");
			return new Test(toReturn);
		}

		public Tester GetTester(string id)
		{
			Tester toReturn = DataSource.testerList.FirstOrDefault(tester => tester.ID == id);
			if (toReturn == null)
				throw new InvalidOperationException("That tester doesn't exist");
			return new Tester(toReturn);
		}
		public Trainee GetTrainee(string id)
		{
			Trainee toReturn = DataSource.traineeList.FirstOrDefault(trainee => trainee.ID == id);
			if (toReturn == null)
				throw new InvalidOperationException("That trainee doesn't exist");
			return new Trainee(toReturn);
		}

		public IEnumerable<Tester> GetTesters()
		{
			return new List<Tester>(DataSource.testerList);
		}

		public IEnumerable<Test> GetTests()
		{
			return new List<Test>(DataSource.testList);
		}

		

		public IEnumerable<Trainee> GetTrainees()
		{
			return new List<Trainee>(DataSource.traineeList);
		}

        public IEnumerable<Messages> GetMessages()
        {
            return new List<Messages>(DataSource.MessagesList);
        }

        public IEnumerable<Admin> GetAdmins()
		{
			return new List<Admin>(DataSource.adminList);
		}

		void Idal.AddTest(Test test)
		{
			DataSource.testList.Add(test);
		}

		void Idal.AddTester(Tester tester)
		{
			DataSource.testerList.Add(tester);
		}

		void Idal.AddTrainee(Trainee trainee)
		{
			DataSource.traineeList.Add(trainee);
		}
		void Idal.AddAdmin(Admin admin)
		{
			DataSource.adminList.Add(admin);
		}

        void Idal.AddMessage(Messages message)
        {
            DataSource.MessagesList.Add(message);
        }

        void Idal.RemoveTest(Test toRemove)
		{
			DataSource.testList.Remove(toRemove);
		}

		void Idal.RemoveTester(Tester toRemove)
		{
			DataSource.testerList.Remove(toRemove);
		}

		void Idal.RemoveTrainee(Trainee toRemove)
		{
			DataSource.traineeList.Remove(toRemove);
		}
		void Idal.RemoveAdmin(Admin toRemove)
		{
			DataSource.adminList.Remove(toRemove);
		}

		public Admin GetAdmin(string id)
		{
			Admin toReturn = DataSource.adminList.FirstOrDefault(admin => admin.ID == id);
			if (toReturn == null)
				throw new InvalidOperationException("That Admin doesn't exist");
			return new Admin(toReturn);
			
		}

		public bool CheckPassword(string id, string password)
		{
			return DataSource.PasswordList.CheckPassword(id, password);
		}

		public void AddUpdatePassword(string id, string password)
		{
			DataSource.PasswordList.AddUpdatePassword(id, password);
		}

		public void RemovePassword(string id)
		{
			DataSource.PasswordList.RemovePassword(id);
		}
       

        public void RemoveMessage(int num)
		{
			DataSource.MessagesList.RemoveAll(T => T.MessageNumber == num);
		}

		




		//void Idal.UpdateTest(Test newData)
		//{
		//	int index = DataSource.testList.FindIndex(T => newData.TestNumber == T.TestNumber);
		//	DataSource.testList[index] = newData;
		//}

		//void Idal.UpdateTester( Tester newData)
		//{
		//	int index = DataSource.testerList.FindIndex(T => newData.ID == T.ID);
		//	DataSource.testerList[index] = newData;
		//}

		//void Idal.UpdateTrainee(Trainee newData)
		//{
		//	int index = DataSource.traineeList.FindIndex(T => newData.ID == T.ID);
		//	DataSource.traineeList[index] = newData;
		//}
	}
}
