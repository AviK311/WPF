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
        public void AddTest(Test test)
        {
            
            throw new NotImplementedException();
        }

         void IBL.AddTester(Tester tester) 
        {
            string e = tester.ID;
           
            throw new NotImplementedException();
        }

        public void AddTrainee(Trainee trainee)
        {
            
            throw new NotImplementedException();
        }

        public List<Test> AppropriateTests(Func<Test, bool> match)
        {
            throw new NotImplementedException();
        }

        public List<Tester> AvailableTesters(DateTime date)
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

        public IEnumerable<IGrouping<CarType, Tester>> TesterGroupsAccordingCarType(bool inOrder)
        {
            throw new NotImplementedException();
        }

        public List<Tester> TestersInRange(Address address)
        {
            throw new NotImplementedException();
        }

        public int TestsNum(Trainee trainee)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGrouping<string, Trainee>> TraineesGroupsAccordingSchoolName(bool inOrder)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGrouping<Name, Trainee>> TraineesGroupsAccordingTeacherName(bool inOrder)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGrouping<int, Trainee>> TraineesGroupsAccordingTestsNum(bool inOrder)
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
    }
}
