﻿using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Bl_imp : IBL
    {
        DAL.Idal dal;
        public Bl_imp()
        {
            dal = DAL.FactoryDal.GetDAL();
        }
        public void AddTest(Test test, bool update = false)
        {
			test.TestNumber = Configuration.TestCode.ToString().PadLeft(8, '0');
			var testTrainee = (from trainee in dal.GetTrainees()
							   where trainee.ID == test.TraineeID
							   select trainee).First();
			var testTester = (from tester in dal.GetTesters()
							  where tester.ID == test.TesterID
							  select tester).First();
			if (testTrainee == null) throw new InvalidOperationException("The trainee does not exist");
			if (testTester == null) throw new InvalidOperationException("The tester does not exist");

			var otherTests = TestGroupsAccordingToTrainee(false).FirstOrDefault(item => item.Key.ID == test.TraineeID);
            if (otherTests != null &&otherTests.Any(T => (T.TestDateTime - DateTime.Now).TotalDays < Configuration.TimeBetweenTests))
                throw new InvalidOperationException(string.Format("The trainee must wait {0} days before he can redo the test", Configuration.TimeBetweenTests));
            
            if (testTrainee.CurrentCarType != testTester.testingCarType)
                throw new InvalidOperationException("The tester does not teach on the vehicle type that the trainee learned with");
            if (testTrainee.carTypeStats[testTrainee.CurrentCarType].numOfLessons < 20)
                throw new InvalidOperationException("The trainee is not yet ready for a test");
            if (testTrainee.carTypeStats[testTrainee.CurrentCarType].passed)
                throw new InvalidOperationException("The student has already passed a test on that vehicle");
            DayOfWeek day = test.TestDateTime.DayOfWeek;
            int time = test.TestDateTime.Hour;
            var tests_by_tester = from test1 in dal.GetTests()
                                  where test1.TesterID == testTester.ID
                                  select test1;
			///if the tester is unavailable
			if (!testTester.schedule[day][time] || tests_by_tester.Any(T => T.TestDateTime.Date == test.TestDateTime.Date && T.TestDateTime.Hour == test.TestDateTime.Hour))
				throw new InvalidOperationException("The tester is unavailable");
            var tests_by_tester_same_week = from test1 in tests_by_tester
                                            where Functions.DatesAreInTheSameWeek(test.TestDateTime, test1.TestDateTime)
                                            select test1;
            if (tests_by_tester_same_week.Count() >= testTester.MaxWeeklyTests)
                throw new InvalidOperationException("The tester has signed up for too many tests");
			if (test.TestDateTime < DateTime.Now && test.testProperties.passed())
				testTrainee.carTypeStats[testTrainee.CurrentCarType].passed = true;
			if(!update)
				testTrainee.carTypeStats[testTrainee.CurrentCarType].numOfTest++;
			UpdateTrainee(testTrainee);
			Configuration.TestCode++;
			dal.AddTest(test);
        }

        public void AddTester(Tester tester, bool update = false)
        {
			if (!update && dal.GetTesters().Any(T => T.Equals(tester)))
				throw new InvalidOperationException("A tester with that ID already exists");
			if (tester.GetAge() < Configuration.MinAgeOfTester)
                throw new InvalidOperationException("The tester is younger than " + Configuration.MinAgeOfTester);
            dal.AddTester(tester);
        }

        public void AddTrainee(Trainee trainee, bool update = false)
        {
			if (!update &&dal.GetTrainees().Any(T => T.Equals(trainee)))
				throw new InvalidOperationException("A trainee with that ID already exists");
			dal.AddTrainee(trainee);
        }

        public IEnumerable<Test> AppropriateTests(Func<Test, bool> match)
        {
            //add code here
            throw new NotImplementedException();
        }

        public IEnumerable<Tester> AvailableTesters(DateTime date)
        {
            return from  tester in dal.GetTesters()
                          let tests = from test in dal.GetTests()
                                      where test.TesterID==tester.ID
                                      select test
                          where tester.schedule[date.DayOfWeek][date.Hour]  
                                && !tests.Any(T=>(T.TestDateTime-date).Days==0 && date.Hour==T.TestDateTime.Hour)
                                && tester.MaxWeeklyTests<tests.Count()
                          select tester;
        }
		public IEnumerable<DateTime> otherAvailableTestTimes(Tester tester, DateTime date)
		{
			List<DateTime> available = new List<DateTime>();
			for (var j = date.AddDays(-2); j <= date.AddDays(2); j.AddDays(1))
				for (int hour = 9; hour < 15; hour++)
				{
					DateTime availableDate = new DateTime(j.Year, j.Month, j.Day, hour, 0, 0);
					if (tester.schedule[j.DayOfWeek][hour] &&
						AvailableTesters(availableDate).Any(T => T.Equals(tester)))
						available.Add(availableDate);
				}
			return available;
			
		}

		public IEnumerable<Tester> GetTesters()
        {
            return dal.GetTesters();
        }

        public IEnumerable<Test> GetTests()
        {
            return dal.GetTests();
        }

        public IEnumerable<Trainee> GetTrainees()
        {
            return dal.GetTrainees();
        }

        public IEnumerable<DateTime> PlannedTests()
        {
			return from tests in dal.GetTests()
				   select tests.TestDateTime;
        }

        public bool ProperToLicense(Trainee trainee)
        {
			return trainee.carTypeStats[trainee.CurrentCarType].passed;
        }

        public void RemoveTest(string id)
        {
			var removeTest = dal.GetTests().FirstOrDefault(test => test.TestNumber == id);
			if (removeTest != null)
				dal.RemoveTest(removeTest);
			else throw new InvalidOperationException("A Test with that ID doesn't exist");
        }

        public void RemoveTester(string id)
        {
			var removeTester = dal.GetTesters().FirstOrDefault(tester => tester.ID == id);
			if (removeTester != null)
				dal.RemoveTester(removeTester);
			else throw new InvalidOperationException("A Trainee with that ID doesn't exist");
        }

        public void RemoveTrainee(string id)
        {
			var removeTrainee = dal.GetTrainees().FirstOrDefault(trainee => trainee.ID == id);
			if (removeTrainee != null)
				dal.RemoveTrainee(removeTrainee);
			else throw new InvalidOperationException("A Trainee with that ID doesn't exist");
        }

        public IEnumerable<IGrouping<VehicleType, Tester>> TesterGroupsAccordingToCarType(bool inOrder)
        {
			var toReturn = from tester in dal.GetTesters()
						   group tester by tester.testingCarType;
			if (inOrder) toReturn.OrderBy(item => item.Key.ToString());
			return toReturn;
        }

        public IEnumerable<Tester> TestersInRange(Address address)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGrouping<string, Trainee>> TraineesGroupsAccordingToSchoolName(VehicleType c, bool inOrder)
        {
			var toReturn = from trainee in dal.GetTrainees()
						   group trainee by trainee.carTypeStats[c].schoolName;
			if (inOrder) toReturn.OrderBy(item => item.Key);
			return toReturn;
		}

        public IEnumerable<IGrouping<Name, Trainee>> TraineesGroupsAccordingToTeacherName(VehicleType c, bool inOrder)
        {
			var toReturn = from trainee in dal.GetTrainees()
						   group trainee by trainee.carTypeStats[c].teacherName;
			if (inOrder) toReturn.OrderBy(item => item.Key);
			return toReturn;
		}

        public IEnumerable<IGrouping<int, Trainee>> TraineesGroupsAccordingToTestsNum(VehicleType c, bool inOrder)
        {
			var toReturn = from trainee in dal.GetTrainees()
                          group trainee by trainee.carTypeStats[c].numOfTest;
			if (inOrder) toReturn.OrderBy(item => item.Key);
			return toReturn;
		}

        public void UpdateTest(Test newData)
        {
			AddTest(newData);
			RemoveTest(newData.TestNumber);
        }

        public void UpdateTester(Tester newData)
        {
			AddTester(tester: newData, update: true);
			RemoveTester(newData.ID);
		}

        public void UpdateTrainee(Trainee newData)
        {
			AddTrainee(trainee: newData, update: true);
			RemoveTrainee(newData.ID);
		}

        public Test GetTest(string id)
        {
			return dal.GetTest(id);
        }

        public Trainee GetTrainee(string id)
        {
			return dal.GetTrainee(id);
		}

        public Tester GetTester(string id)
        {
			return dal.GetTester(id);
        }

        public IEnumerable<IGrouping<Trainee, Test>> TestGroupsAccordingToTrainee(bool inOrder)
        {
			var toReturn = from test in dal.GetTests()
						   let trainee = (from trainee in dal.GetTrainees()
										  where trainee.ID == test.TraineeID
										  select trainee).FirstOrDefault()
						   group test by trainee;
			if (inOrder) toReturn.OrderBy(item => item.Key.ID);
			return toReturn;
        }
        

	}

}


