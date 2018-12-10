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
			return new Test(DataSource.testList.FirstOrDefault(test => test.TestNumber == id));
		}

		public Tester GetTester(string id)
		{
			return new Tester(DataSource.testerList.FirstOrDefault(tester => tester.ID == id));
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
			return new Trainee(DataSource.traineeList.FirstOrDefault(trainee => trainee.ID == id));
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

		void Idal.UpdateTest(Test newData)
		{
			int index = DataSource.testList.FindIndex(T => newData.TestNumber == T.TestNumber);
			DataSource.testList[index] = newData;
		}

		void Idal.UpdateTester( Tester newData)
		{
			int index = DataSource.testerList.FindIndex(T => newData.ID == T.ID);
			DataSource.testerList[index] = newData;
		}

		void Idal.UpdateTrainee(Trainee newData)
		{
			int index = DataSource.traineeList.FindIndex(T => newData.ID == T.ID);
			DataSource.traineeList[index] = newData;
		}
	}
}
