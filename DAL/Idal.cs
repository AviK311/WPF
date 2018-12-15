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


		IEnumerable<Test> GetTests();
		IEnumerable<Tester> GetTesters();
		IEnumerable<Trainee> GetTrainees();

	}
}
