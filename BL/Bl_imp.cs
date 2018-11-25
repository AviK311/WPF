using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace BL
{
    class Bl_imp : IBL
    {
		DAL.Idal dal;
		public Bl_imp()
		{
			dal = DAL.FactoryDal.GetDAL();
		}

		public void AddTest(Test test)
        {

			var tests = from test1 in dal.GetTests()
						where test1.TraineeID == test.TraineeID
						select test1;
			if (tests.Any(T => (T.TestDateTime - DateTime.Now).TotalDays < Configuration.TimeBetweenTests))
				throw new InvalidOperationException(string.Format("The trainee must wait {0} days before he can redo the test", Configuration.TimeBetweenTests));
			var testTrainee = (from trainee in dal.GetTrainees()
							  where trainee.ID == test.TraineeID
							  select trainee).First();
			var testTester = (from tester in dal.GetTesters()
							  where tester.ID == test.TesterID
							  select tester).First();
			
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
			if (!testTester.schedule[day][time] || tests_by_tester.Any(T=>T.TestDateTime==test.TestDateTime))
			{
				//code to suggest another tester
			}
			

			dal.AddTest(test);
        }

         void IBL.AddTester(Tester tester) 
        {
			if (tester.getAge < Configuration.MinAgeOfTester)
				throw new InvalidOperationException("The tester is younger than " + Configuration.MinAgeOfTester);
			dal.AddTester(tester);
        }

        public void AddTrainee(Trainee trainee)
        {
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
            throw new NotImplementedException();
        }

        public void RemoveTester(string id)
        {
            throw new NotImplementedException();
        }

        public void RemoveTrainee(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGrouping<CarType, Tester>> TesterGroupsAccordingToCarType(bool inOrder)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public IEnumerable<IGrouping<Name, Trainee>> TraineesGroupsAccordingToTeacherName(bool inOrder)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGrouping<int, Trainee>> TraineesGroupsAccordingToTestsNum(bool inOrder)
        {
            throw new NotImplementedException();
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

		public IEnumerable<IGrouping<string, Test>> TestGroupsAccordingToStudentID(bool inOrder)
		{

			throw new NotImplementedException();
		}
	}
}
