using BE;
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
        public void AddTest(Test test)
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
            if (otherTests.Any(T => (T.TestDateTime - DateTime.Now).TotalDays < Configuration.TimeBetweenTests))
                throw new InvalidOperationException(string.Format("The trainee must wait {0} days before he can redo the test", Configuration.TimeBetweenTests));
            
            if (testTrainee.CurrentCarType != testTester.testingCarType)
                throw new InvalidOperationException("The tester does not teach on the car type that the trainee learned with");
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
            if (!testTester.schedule[day][time] || tests_by_tester.Any(T => T.TestDateTime == test.TestDateTime))
            {
                //code to suggest another tester
            }
            var tests_by_tester_same_week = from test1 in tests_by_tester
                                            where DatesAreInTheSameWeek(test.TestDateTime, test1.TestDateTime)
                                            select test1;
            if (tests_by_tester_same_week.Count() >= testTester.MaxWeeklyTests)
                throw new InvalidOperationException("The tester has signed up for too many tests");
			
			Configuration.TestCode++;
			dal.AddTest(test);
        }

        void IBL.AddTester(Tester tester)
        {
			if (dal.GetTesters().Any(T => T.ID == tester.ID))
				throw new InvalidOperationException("A tester with that ID already exists");
			if (tester.GetAge() < Configuration.MinAgeOfTester)
                throw new InvalidOperationException("The tester is younger than " + Configuration.MinAgeOfTester);
            dal.AddTester(tester);
        }

        public void AddTrainee(Trainee trainee)
        {
			if (dal.GetTrainees().Any(T => T.ID == trainee.ID))
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

        public IEnumerable<IGrouping<CarType, Tester>> TesterGroupsAccordingToCarType(bool inOrder)
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

        public IEnumerable<IGrouping<string, Trainee>> TraineesGroupsAccordingToSchoolName(CarType c, bool inOrder)
        {
			var toReturn = from trainee in dal.GetTrainees()
						   group trainee by trainee.carTypeStats[c].schoolName;
			if (inOrder) toReturn.OrderBy(item => item.Key);
			return toReturn;
		}

        public IEnumerable<IGrouping<Name, Trainee>> TraineesGroupsAccordingToTeacherName(CarType c, bool inOrder)
        {
			var toReturn = from trainee in dal.GetTrainees()
						   group trainee by trainee.carTypeStats[c].teacherName;
			if (inOrder) toReturn.OrderBy(item => item.Key);
			return toReturn;
		}

        public IEnumerable<IGrouping<int, Trainee>> TraineesGroupsAccordingToTestsNum(CarType c, bool inOrder)
        {
			var toReturn = from trainee in dal.GetTrainees()
                          group trainee by trainee.carTypeStats[c].numOfTest;
			if (inOrder) toReturn.OrderBy(item => item.Key);
			return toReturn;
		}

        public void UpdateTest(Test newData)
        {
			dal.UpdateTest(newData);
        }

        public void UpdateTester(Tester newData)
        {
			dal.UpdateTester(newData);
		}

        public void UpdateTrainee(Trainee newData)
        {
			dal.UpdateTrainee(newData);
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
        private bool DatesAreInTheSameWeek(DateTime date1, DateTime date2)
        {
            //var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            //var d1 = date1.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date1));
            //var d2 = date2.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date2));
            var d1 = date1.AddDays(-1 * (int)date1.DayOfWeek);
            var d2 = date2.AddDays(-1 * (int)date2.DayOfWeek);
            return d1 == d2;
        }

	}

}


