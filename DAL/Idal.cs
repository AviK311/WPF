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
		void UpdateTest(string id, Test newData);
		
		void AddTrainee(Trainee trainee);
		Trainee GetTrainee(string id);
		void RemoveTrainee(Trainee toRemove);
		void UpdateTrainee(string id, Trainee newData);

		void AddTester(Tester tester);
		Tester getTester(string id);
		void RemoveTester(Tester toRemove);
		void UpdateTester(string id,Tester newData);


		IEnumerable<Test> GetTests();
		IEnumerable<Tester> GetTesters();
		IEnumerable<Trainee> GetTrainees();

	}
}
