using System;
using System.Collections.Generic;
using System.Linq;
using BE;
using DAL;

namespace BL
{
    public interface IBL
    {
        void AddTest(BE.Test test);
		BE.Test GetTest(string id);
        void RemoveTest(string id);
        void UpdateTest(string id, BE.Test newData);

        void AddTrainee(BE.Trainee trainee);
		BE.Trainee GetTrainee(string id);
		void RemoveTrainee(string id);
        void UpdateTrainee(string id, BE.Trainee newData);

        void AddTester(BE.Tester tester);
		BE.Tester GetTester(string id);
        void RemoveTester(string id);
        void UpdateTester(string id, BE.Tester newData);

        IEnumerable<BE.Test> GetTests();
        IEnumerable<BE.Tester> GetTesters();
        IEnumerable<BE.Trainee> GetTrainees();

		IEnumerable<BE.Tester> TestersInRange(BE.Address address);//with GoogleMaps
		IEnumerable<BE.Tester> AvailableTesters(DateTime date);
		IEnumerable<BE.Test> AppropriateTests(Func<BE.Test, bool> match);
        int TestsNum(BE.Trainee trainee);
        bool ProperToLicense(BE.Trainee trainee);
        IEnumerable<DateTime> PlannedTests();

        //Groups
        IEnumerable<IGrouping<string, BE.Trainee>> 
            TraineesGroupsAccordingToSchoolName(bool inOrder = false);
        IEnumerable<IGrouping<BE.Name, BE.Trainee>>
            TraineesGroupsAccordingToTeacherName(bool inOrder = false);
        IEnumerable<IGrouping<int, BE.Trainee>>
            TraineesGroupsAccordingToTestsNum(bool inOrder = false);
        IEnumerable<IGrouping<BE.CarType, BE.Tester>>
            TesterGroupsAccordingToCarType(bool inOrder = false);
		IEnumerable<IGrouping<string, BE.Test>>
			TestGroupsAccordingToStudentID(bool inOrder = false);
    }

    public class Bl_imp : IBL
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
            if (!testTester.schedule[day][time] || tests_by_tester.Any(T => T.TestDateTime == test.TestDateTime))
            {
                //code to suggest another tester
            }
            var tests_by_tester_same_week = from test1 in tests_by_tester
                                            where DatesAreInTheSameWeek(test.TestDateTime, test1.TestDateTime)
                                            select test1;
            if (tests_by_tester_same_week.Count() > testTester.MaxWeeklyTests)
                throw new InvalidOperationException("The tester has signed up for too many tests");

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
