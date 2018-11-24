using System;
using System.Collections.Generic;
using System.Text;
using BE;
using System.Linq;
using DS;

namespace DAL
{
	class Dal_imp : Idal
	{
		public IEnumerable<Tester> GetTesters()
		{
			return DataSource.testerList;
		}

		public IEnumerable<Test> GetTests()
		{
			return DataSource.testList;
		}

		public IEnumerable<Trainee> GetTrainees()
		{
			return DataSource.traineeList;
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

		

		void Idal.RemoveTest(string id)
		{
			var removeTest = from test in DataSource.testList
							 where test.TestNumber == id
							 select test;
			DataSource.testList.Remove((Test)removeTest);

		}

		void Idal.RemoveTester(string id)
		{
			var removeTester = from tester in DataSource.testerList
							   where tester.ID == id
							   select tester;
			DataSource.testerList.Remove((Tester)removeTester);
		}

		void Idal.RemoveTrainee(string id)
		{
			var removeTrainee = from trainee in DataSource.traineeList
								where trainee.ID == id
								select trainee;
			DataSource.traineeList.Remove((Trainee)removeTrainee);
		}

		void Idal.UpdateTest(string id, Test newData)
		{
			throw new NotImplementedException();
		}

		void Idal.UpdateTester(string id, Tester newData)
		{
			throw new NotImplementedException();
		}

		void Idal.UpdateTrainee(string id, Trainee newData)
		{
			throw new NotImplementedException();
		}
	}
}
