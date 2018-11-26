using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BE;
using DS;

namespace DAL
{
    public class Dal_imp : Idal
	{
		public Test GetTest(string id)
		{
			return DataSource.testList.FirstOrDefault(test => test.TestNumber == id);
		}

		public Tester getTester(string id)
		{
			return DataSource.testerList.FirstOrDefault(tester => tester.ID == id);
		}

		public IEnumerable<Tester> GetTesters()
		{
			return DataSource.testerList;
		}

		public IEnumerable<Test> GetTests()
		{
			return DataSource.testList;
		}

		public Trainee GetTrainee(string id)
		{
			return DataSource.traineeList.FirstOrDefault(trainee => trainee.ID == id);
		}

		public IEnumerable<Trainee> GetTrainees()
		{
			return DataSource.traineeList;
		}

		void Idal.AddTest(Test test)
		{
			test.TestNumber = Configuration.TestCode.ToString().PadLeft(8, '0');
			if (DataSource.testList.Any(T => T.TestNumber == test.TestNumber))
				throw new InvalidOperationException("A test with that ID already exists");
			Configuration.TestCode++;
			DataSource.testList.Add(test);
		}

		void Idal.AddTester(Tester tester)
		{
			if (DataSource.testerList.Any(T => T.ID == tester.ID))
				throw new InvalidOperationException("A tester with that ID already exists");
			DataSource.testerList.Add(tester);
		}

		void Idal.AddTrainee(Trainee trainee)
		{
			if (DataSource.traineeList.Any(T => T.ID==trainee.ID))
				throw new InvalidOperationException("A trainee with that ID already exists");
			DataSource.traineeList.Add(trainee);
		}

		void Idal.RemoveTest(string id)
		{
			var removeTest = DataSource.testList.FirstOrDefault(test => test.TestNumber == id);
			if (removeTest != null)
				DataSource.testList.Remove(removeTest);
			else throw new InvalidOperationException("A Test with that ID doesn't exist");
		}

		void Idal.RemoveTester(string id)
		{
			var removeTester = DataSource.testerList.FirstOrDefault(tester => tester.ID == id);
			if (removeTester != null)
				DataSource.testerList.Remove(removeTester);
			else throw new InvalidOperationException("A Trainee with that ID doesn't exist");
		}

		void Idal.RemoveTrainee(string id)
		{
			var removeTrainee = DataSource.traineeList.FirstOrDefault(trainee => trainee.ID == id);
			if (removeTrainee != null)
				DataSource.traineeList.Remove(removeTrainee);
			else throw new InvalidOperationException("A Trainee with that ID doesn't exist");
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
