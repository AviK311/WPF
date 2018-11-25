using System;
using System.Collections.Generic;


namespace DAL
{
	public interface Idal
	{
		void AddTest(BE.Test test);
		BE.Test GetTest(string id);
		void RemoveTest(string id);
		void UpdateTest(string id, BE.Test newData);
		
		void AddTrainee(BE.Trainee trainee);
		BE.Trainee GetTrainee(string id);
		void RemoveTrainee(string id);
		void UpdateTrainee(string id, BE.Trainee newData);

		void AddTester(BE.Tester tester);
		BE.Tester getTester(string id);
		void RemoveTester(string id);
		void UpdateTester(string id, BE.Tester newData);


		IEnumerable<BE.Test> GetTests();
		IEnumerable<BE.Tester> GetTesters();
		IEnumerable<BE.Trainee> GetTrainees();

	}
}
