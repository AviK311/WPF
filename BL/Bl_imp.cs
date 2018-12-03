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
            
            if (testTrainee.CarType != testTester.CarType)
                throw new InvalidOperationException("The tester does not teach on the car type that the trainee learned with");
            if (testTrainee.NumOfClasses < 20)
                throw new InvalidOperationException("The trainee is not yet ready for a test");
            if (testTrainee.passedTests[testTrainee.CarType])
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
            if (tests_by_tester_same_week.Count() > testTester.MaxWeeklyTests)
                throw new InvalidOperationException("The tester has signed up for too many tests");
			
			Configuration.TestCode++;
			dal.AddTest(test);
        }

        void IBL.AddTester(Tester tester)
        {
			if (dal.GetTesters().Any(T => T.ID == tester.ID))
				throw new InvalidOperationException("A tester with that ID already exists");
			if (tester.getAge < Configuration.MinAgeOfTester)
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
		   throw new NotImplementedException();
        }

        public IEnumerable<Tester> GetTesters()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Test> GetTests()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Trainee> GetTrainees()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DateTime> PlannedTests()
        {
            throw new NotImplementedException();
        }

        public bool ProperToLicense(Trainee trainee)
        {
            throw new NotImplementedException();
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
						   group tester by tester.CarType;
			if (inOrder) toReturn.OrderBy(item => item.Key.ToString());
			return toReturn;
        }

        public IEnumerable<Tester> TestersInRange(Address address)
        {
            throw new NotImplementedException();
        }

        public int TestsNum(Trainee trainee)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGrouping<string, Trainee>> TraineesGroupsAccordingToSchoolName(bool inOrder)
        {
			var toReturn = from trainee in dal.GetTrainees()
						   group trainee by trainee.SchoolName;
			if (inOrder) toReturn.OrderBy(item => item.Key);
			return toReturn;
		}

        public IEnumerable<IGrouping<Name, Trainee>> TraineesGroupsAccordingToTeacherName(bool inOrder)
        {
			var toReturn = from trainee in dal.GetTrainees()
						   group trainee by trainee.TeacherName;
			if (inOrder) toReturn.OrderBy(item => item.Key);
			return toReturn;
		}

        public IEnumerable<IGrouping<int, Trainee>> TraineesGroupsAccordingToTestsNum(bool inOrder)
        {
			var toReturn = from trainee in dal.GetTrainees()
						   let testsByTrainee = from test in dal.GetTests() where test.TraineeID == trainee.ID select test
						   group trainee by testsByTrainee.Count();						   
			if (inOrder) toReturn.OrderBy(item => item.Key);
			return toReturn;
		}

        public void UpdateTest(string id, Test newData)
        {
            throw new NotImplementedException();
        }

        public void UpdateTester(string id, Tester newData)
        {
            throw new NotImplementedException();
        }

        public void UpdateTrainee(string id, Trainee newData)
        {
            throw new NotImplementedException();
        }

        public Test GetTest(string id)
        {
            throw new NotImplementedException();
        }

        public Trainee GetTrainee(string id)
        {
            throw new NotImplementedException();
        }

        public Tester GetTester(string id)
        {
            throw new NotImplementedException();
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


