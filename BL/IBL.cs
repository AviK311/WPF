﻿using System;
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
        void UpdateTest(BE.Test newData);

        void AddTrainee(BE.Trainee trainee, bool update = false);
		BE.Trainee GetTrainee(string id);
		void RemoveTrainee(string id);
        void UpdateTrainee(BE.Trainee newData);

        void AddTester(BE.Tester tester, bool update = false);
		BE.Tester GetTester(string id);
        void RemoveTester(string id);
        void UpdateTester(BE.Tester newData);

        IEnumerable<BE.Test> GetTests();
        IEnumerable<BE.Tester> GetTesters();
        IEnumerable<BE.Trainee> GetTrainees();

		IEnumerable<BE.Tester> TestersInRange(BE.Address address);//with GoogleMaps
		IEnumerable<BE.Tester> AvailableTesters(DateTime date);
		IEnumerable<DateTime> otherAvailableTestTimes(Tester tester, DateTime date);
		IEnumerable<BE.Test> AppropriateTests(Func<BE.Test, bool> match);
        bool ProperToLicense(BE.Trainee trainee);
        IEnumerable<DateTime> PlannedTests();

        //Groups
        IEnumerable<IGrouping<string, BE.Trainee>> 
            TraineesGroupsAccordingToSchoolName(VehicleType c, bool inOrder = false);
        IEnumerable<IGrouping<BE.Name, BE.Trainee>>
            TraineesGroupsAccordingToTeacherName(VehicleType c,bool inOrder = false);
        IEnumerable<IGrouping<int, BE.Trainee>>
            TraineesGroupsAccordingToTestsNum(VehicleType c, bool inOrder = false);
        IEnumerable<IGrouping<BE.VehicleType, BE.Tester>>
            TesterGroupsAccordingToCarType(bool inOrder = false);
		IEnumerable<IGrouping<Trainee, BE.Test>>
			TestGroupsAccordingToTrainee(bool inOrder = false);
    }
}

    