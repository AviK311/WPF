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
			var removeTest = DataSource.testList.FirstOrDefault(test => test.TestNumber == id);
			DataSource.testList.Remove(removeTest);

		}

		void Idal.RemoveTester(string id)
		{
			var removeTester = DataSource.testerList.FirstOrDefault(tester => tester.ID == id);
			DataSource.testerList.Remove(removeTester);
		}

		void Idal.RemoveTrainee(string id)
		{
			var removeTrainee = DataSource.traineeList.FirstOrDefault(trainee => trainee.ID == id);
			DataSource.traineeList.Remove(removeTrainee);
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
