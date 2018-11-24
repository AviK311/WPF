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
			return DataSource.testerList;
		}

		public IEnumerable<Trainee> GetTrainees()
		{
			return DataSource.testerList;
		}

		void Idal.AddTest(Test test)
		{
			DataSource.testList.Add(test);
		}

		void Idal.AddTester(Tester tester)
		{
			dataSource.testerList.Add(tester);

		}

		void Idal.AddTrainee(Trainee trainee)
		{
			dataSource.traineeList.Add(trainee);
		}

		

		void Idal.RemoveTest(string id)
		{
			var removeTest = from test in dataSource.testList
							 where test.TestNumber == id
							 select test;
			dataSource.testList.Remove()

		}

		void Idal.RemoveTester(string id)
		{
			throw new NotImplementedException();
		}

		void Idal.RemoveTrainee(string id)
		{
			throw new NotImplementedException();
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
