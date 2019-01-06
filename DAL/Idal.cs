using System;
using BE;
using System.Collections.Generic;

namespace DAL
{
    public interface Idal
	{
		void AddTest(Test test);
		Test GetTest(string id);
		void RemoveTest(Test toRemove);
		//void UpdateTest(Test newData);
		
		void AddTrainee(Trainee trainee);
		Trainee GetTrainee(string id);
		void RemoveTrainee(Trainee toRemove);
		//void UpdateTrainee(Trainee newData);

		void AddTester(Tester tester);
		Tester GetTester(string id);
		void RemoveTester(Tester toRemove);
		//void UpdateTester(Tester newData);

		void AddAdmin(Admin admin);
		Admin GetAdmin(string id);
		void RemoveAdmin(Admin toRemove);

		bool CheckPassword(string id, string password);
		void AddUpdatePassword(string id, string password);
		void RemovePassword(string id);
       

        void AddMessage(Messages message);
		void RemoveMessage(int num);

		void Initialize();
        IEnumerable<Test> GetTests();
		IEnumerable<Admin> GetAdmins();
		IEnumerable<Tester> GetTesters();
		IEnumerable<Trainee> GetTrainees();
        IEnumerable<Messages> GetMessages();


    }
}
