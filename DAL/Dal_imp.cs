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
			return new List<Tester>(DataSource.testerList);
		}

		public IEnumerable<Test> GetTests()
		{
			return new List<Test>(DataSource.testList);
		}

		public Trainee GetTrainee(string id)
		{
			return DataSource.traineeList.FirstOrDefault(trainee => trainee.ID == id);
		}

		public IEnumerable<Trainee> GetTrainees()
		{
			return new List<Trainee>(DataSource.traineeList);
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
