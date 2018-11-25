using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public interface IBL
    {
        void AddTest(BE.Test test);
        void RemoveTest(string id);
        void UpdateTest(string id, BE.Test newData);

        void AddTrainee(BE.Trainee trainee);
        void RemoveTrainee(string id);
        void UpdateTrainee(string id, BE.Trainee newData);

        void AddTester(BE.Tester tester);
        void RemoveTester(string id);
        void UpdateTester(string id, BE.Tester newData);

        IEnumerable<BE.Test> GetTests();
        IEnumerable<BE.Tester> GetTesters();
        IEnumerable<BE.Trainee> GetTrainees();

        List<BE.Tester> TestersInRange(BE.Address address);//with GoogleMaps
        List<BE.Tester> AvailableTesters(DateTime date);
        List<BE.Test> AppropriateTests(Func<BE.Test, bool> match);
        int TestsNum(BE.Trainee trainee);
        bool ProperToLicense(BE.Trainee trainee);
        IEnumerable<DateTime> PlannedTests();

        //Groups
        IEnumerable<IGrouping<string, BE.Trainee>> 
            TraineesGroupsAccordingSchoolName(bool inOrder);
        IEnumerable<IGrouping<BE.Name, BE.Trainee>>
            TraineesGroupsAccordingTeacherName(bool inOrder);
        IEnumerable<IGrouping<int, BE.Trainee>>
            TraineesGroupsAccordingTestsNum(bool inOrder);
        IEnumerable<IGrouping<BE.CarType, BE.Tester>>
            TesterGroupsAccordingCarType(bool inOrder);
    }
}
