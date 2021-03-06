﻿using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Xml;
using System.Threading;

namespace BL
{
	/// <summary>
	/// 
	/// </summary>
    public class Bl_imp : IBL
    {
        DAL.Idal dal;
        public Bl_imp()
        {
            dal = DAL.FactoryDal.GetDAL();
        }
		/// <summary>
		/// function that checks the logic of a test that the user requested to add or update to the database.
		/// 
		/// </summary>
		/// <param name="test">the test the user added or updated</param>
		/// <param name="update">if the function was called for updating, this will be true</param>
        public void AddTest(Test test, bool update = false)
        {
			string ErrorString = "";
			
            if (!update)test.TestNumber = dal.GetTestCode().ToString().PadLeft(8, '0');
			var testTrainee = dal.GetTrainees().FirstOrDefault(T => T.ID == test.TraineeID);
			var testTester = dal.GetTesters().FirstOrDefault(T => T.ID == test.TesterID);
			if (testTrainee == null) throw new InvalidOperationException("The trainee does not exist" + "\n");
			if (testTester == null) throw new InvalidOperationException("The tester does not exist" + "\n");

            // Thread thread = new Thread(() =>TestersInRange(testTester, test.BeginLocation));
            //thread.Start();
            Test test2 = dal.GetTests().FirstOrDefault(t => t.TraineeID == test.TraineeID && t.TestDateTime.AddDays(Configuration.TimeBetweenTests) > test.TestDateTime);
            //var otherTests = TestGroupsAccordingToTrainee(false).FirstOrDefault(item => item.Key.ID == test.TraineeID);
            if (test2!=null&&update==false)
				ErrorString += string.Format("The trainee must wait {0} days before he can appoint the test", Configuration.TimeBetweenTests) + "\n";

			if (testTrainee.CurrentCarType != testTester.testingCarType)
				ErrorString+="The tester does not teach on the vehicle type that the trainee learned with" +"\n";
			else test.TestingCarType = testTrainee.CurrentCarType;

			if (testTrainee.carTypeStats[testTrainee.CurrentCarType].numOfLessons < 20)
				ErrorString += "The trainee is not yet ready for a test" + "\n";

			if (!update && testTrainee.carTypeStats[testTrainee.CurrentCarType].passed)
				ErrorString += "The student has already passed a test on that vehicle" + "\n";
			if (Functions.IsHoliday(test.TestDateTime, out Holiday? holiday))
				ErrorString += "The chosen date falls out on " + Functions.InsertSpacesBeforeUpper(holiday.ToString()) + "\n";
			DayOfWeek day = test.TestDateTime.DayOfWeek;
            int time = test.TestDateTime.Hour;
			
            var tests_by_tester = from test1 in dal.GetTests()
                                  where test1.TesterID == testTester.ID
                                  select test1;
			///if the tester is unavailable
			if (!testTester.schedule[day,time] || tests_by_tester.Any(T => T.TestDateTime.Date == test.TestDateTime.Date && T.TestDateTime.Hour == test.TestDateTime.Hour&&T.TestNumber!=test.TestNumber))
				ErrorString += "The tester is unavailable" + "\n";
            var tests_by_tester_same_week = from test1 in tests_by_tester
                                            where Functions.DatesAreInTheSameWeek(test.TestDateTime, test1.TestDateTime)
                                            select test1;
			if (tests_by_tester_same_week.Count() >= testTester.MaxWeeklyTests)
				ErrorString += "The tester has signed up for too many tests";
			//if(TestersInRange(test.BeginLocation).FirstOrDefault(T => T.ID == test.TesterID) == null)
			//if (TestersInRange(testTester, test.BeginLocation) == false)
			//    throw new InvalidOperationException("The location is too far for the tester");
			if (ErrorString.Length != 0)
				throw new InvalidOperationException(ErrorString); 
            if (test.TestDateTime < DateTime.Now)
			{
				if (test.testProperties.passed)
				{
					testTrainee.carTypeStats[testTrainee.CurrentCarType].passed = true;
					
					string message = string.Format("Congradulations! you've passed the test on {0}, at {1}!", test.TestDateTime.ToShortDateString(), test.TestDateTime.ToShortTimeString());
					testTrainee.AddNotification(message, MessageIcon.Information);
					Functions.SendEmail(testTrainee, "You Passed the Test!!!", message);
				}
				else
				{
					string message = string.Format("We're sorry, but you failed the test on {0}, at {1}", test.TestDateTime.ToShortDateString(), test.TestDateTime.ToShortTimeString());
					testTrainee.AddNotification(message, MessageIcon.Error);
					Functions.SendEmail(testTrainee, "You failed :(", message);
				}
			}
			if (!update)
			{
				testTrainee.carTypeStats[testTrainee.CurrentCarType].numOfTest++;
				string testerMessage = string.Format("A test was appointed to you with the trainee {0} at {1}, {2}. See test for details.", testTrainee.Name, test.TestDateTime.ToShortDateString(), test.TestDateTime.ToShortTimeString());
				string traineeMessage = string.Format("A test was appointed to you with the tester {0} at {1}, {2}. See test for details.", testTester.Name, test.TestDateTime.ToShortDateString(), test.TestDateTime.ToShortTimeString());
				testTrainee.AddNotification(traineeMessage, MessageIcon.Warning);
				testTester.AddNotification(testerMessage, MessageIcon.Warning);
				if (test.TestDateTime > DateTime.Now) Functions.SendEmail(testTrainee, "New Test Appointment", traineeMessage);
				Functions.SendEmail(testTester, "New Test Appointment", testerMessage);
			}
			else
			{
				string message = string.Format("Your test on {0} at {1} has been updated. See for details.", test.TestDateTime.ToShortDateString(), test.TestDateTime.ToShortTimeString());
				if (!testTrainee.Equals(GlobalSettings.User))
				{
					testTrainee.AddNotification(message, MessageIcon.Information);
					if (test.TestDateTime > DateTime.Now) Functions.SendEmail(testTrainee, "Test Update", message);
				}
				if (!testTester.Equals(GlobalSettings.User))
				{
					testTester.AddNotification(message, MessageIcon.Information);
					if (test.TestDateTime > DateTime.Now)Functions.SendEmail(testTester, "Test Update", message);
				}
			}
			UpdateTrainee(testTrainee);
			UpdateTester(testTester);
			if (!update) dal.AddTestCode();
			else RemoveTest(test.TestNumber);
			test.TestingCarType = testTester.testingCarType;
			
			dal.AddTest(test);
        }
		/// <summary>
		/// the function checks the logic of a tester before adding or updating him to the database
		/// </summary>
		/// <param name="tester"></param>
		/// <param name="update"></param>
        public void AddTester(Tester tester, bool update = false)
        {
			if (!update && dal.GetTesters().Any(T => T.Equals(tester)))
				throw new InvalidOperationException("A tester with that ID already exists");
            if (!update && dal.GetTrainees().Any(T => T.Equals(tester)))
                throw new InvalidOperationException("A trainee with that ID exists");
            if (tester.Age< Configuration.MinAgeOfTester)
                throw new InvalidOperationException("The tester is younger than " + Configuration.MinAgeOfTester);
            dal.AddTester(tester);
        }
		/// <summary>
		/// check trainee logic before adding hi to database
		/// </summary>
		/// <param name="trainee"></param>
		/// <param name="update"></param>
        public void AddTrainee(Trainee trainee, bool update = false)
        {
			if (trainee.Age < Configuration.MinAgeOfTrainee)
				throw new InvalidOperationException("The trainee is younger than " + Configuration.MinAgeOfTrainee);
			if (!update &&dal.GetTrainees().Any(T => T.Equals(trainee)))
				throw new InvalidOperationException("A trainee with that ID already exists");
            if (!update && dal.GetTesters().Any(T => T.Equals(trainee)))
                throw new InvalidOperationException("A tester with that ID exists");
            dal.AddTrainee(trainee);
        }

        public IEnumerable<Test> AppropriateTests(Func<Test, bool> match)
        {
            //add code here
            throw new NotImplementedException();
        }
		/// <summary>
		/// returns an Ienumerable of Testers who are available on a given date
		/// 
		/// </summary>
		/// <param name="date">date to check</param>
		/// <param name="TestNumber">this is sent to exclude this test from the testing of availability</param>
		/// <returns></returns>
        public IEnumerable<string> AvailableTesters(DateTime date, string TestNumber)
        {
			
			List<string> toReturn = new List<string>();
			var testList = dal.GetTests();
			foreach (var tester in dal.GetTesters())
			{
				if (!tester.schedule[date.DayOfWeek, date.Hour])
					continue;
				if (testList.Any(T =>
				T.TesterID == tester.ID &&
				T.TestNumber != TestNumber &&
				T.TestDateTime.ToShortDateString() == date.ToShortDateString() &&
				T.TestDateTime.Hour == date.Hour))
					continue;
				toReturn.Add(tester.ID);

			}
			return toReturn;
		}
		public IEnumerable<DateTime> otherAvailableTestTimes(Tester tester, DateTime date)
		{
			List<DateTime> available = new List<DateTime>();
			for (var j = date.AddDays(-2); j <= date.AddDays(2); j.AddDays(1))
				for (int hour = 9; hour < 15; hour++)
				{
					DateTime availableDate = new DateTime(j.Year, j.Month, j.Day, hour, 0, 0);
					if (tester.schedule[j.DayOfWeek,hour] &&
						AvailableTesters(availableDate, "").Any(T => T.Equals(tester)))
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

        public IEnumerable<Messages> GetMessages()
        {
            return dal.GetMessages();
        }

        public IEnumerable<Admin> GetAdmins()
		{
			return dal.GetAdmins();
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
			var testList = GetTests();
			foreach (var test in testList)
				if (test.TesterID == id)
					RemoveTest(test.TestNumber);
		}

        public void RemoveTrainee(string id)
        {
			var removeTrainee = dal.GetTrainees().FirstOrDefault(trainee => trainee.ID == id);
			if (removeTrainee != null)
				dal.RemoveTrainee(removeTrainee);
			else throw new InvalidOperationException("A Trainee with that ID doesn't exist");
			var testList = GetTests();
			foreach (var test in testList)
				if (test.TraineeID == id)
					RemoveTest(test.TestNumber);
		}
		public void RemoveAdmin(string id)
		{
			var removeAdmin = dal.GetAdmins().FirstOrDefault(admin => admin.ID == id);
			if (removeAdmin != null)
				dal.RemoveAdmin(removeAdmin);
			else throw new InvalidOperationException("An Admin with that ID doesn't exist");
		}

		public IEnumerable<IGrouping<VehicleType, Tester>> TesterGroupsAccordingToCarType(bool inOrder)
        {
			var toReturn = from tester in dal.GetTesters()
						   group tester by tester.testingCarType;
			if (inOrder) toReturn.OrderBy(item => item.Key.ToString());
			return toReturn;
        }
		/// <summary>
		/// function that takes a tester and a designated test begin address and returns
		/// true if the test is close enough
		/// </summary>
		/// <param name="tester"></param>
		/// <param name="address"></param>
		/// <returns></returns>
        public bool IsTesterCloseEnough(Tester tester,Address address)
        {          
                string origin = tester.Address.street + " " + tester.Address.buildingNumber + " st." + tester.Address.city;  //"pisga 45 st. jerusalem"; //
                string destination = address.street + " " + address.buildingNumber + " st." + address.city; //"gilgal 78 st. ramat-gan"; 
                    if ((address.street==""&&address.city==""&&address.buildingNumber=="" )||(tester.Address.street==""&&tester.Address.city==""&&tester.Address.buildingNumber==""))
                        return true;
                string KEY = @"Bem5PJyvuuUAhHz9K2qM88vC9QEHrMgx";
                string url = @"https://www.mapquestapi.com/directions/v2/route" +
                 @"?key=" + KEY +
                 @"&from=" + origin +
                 @"&to=" + destination +
                 @"&outFormat=xml" +
                 @"&ambiguities=ignore&routeType=fastest&doReverseGeocode=false" +
                 @"&enhancedNarrative=false&avoidTimedConditions=false";
                //request from MapQuest service the distance between the 2 addresses
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader sreader = new StreamReader(dataStream);
                string responsereader = sreader.ReadToEnd();
                response.Close();
                //the response is given in an XML format//
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(responsereader);

                if (xmldoc.GetElementsByTagName("statusCode")[0].ChildNodes[0].InnerText == "0")
                //we have the expected answer
                {
                    //display the returned distance
                    XmlNodeList distance = xmldoc.GetElementsByTagName("distance");
                    double distInMiles = Convert.ToDouble(distance[0].ChildNodes[0].InnerText);
                    Console.WriteLine("Distance In KM: " + distInMiles * 1.609344);
                if (distInMiles * 1.609344 > tester.MaxDistance && tester.MaxDistance > 3)
                    return false;
                }
            return true;
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
		public IEnumerable<IGrouping<Tester, Test>> TestGroupsAccordingToTester(bool inOrder)
		{
			var toReturn = from test in dal.GetTests()
						   group test by dal.GetTester(test.TesterID);
			if (inOrder) toReturn.OrderBy(item => item.Key);
			return toReturn;
		}

		public void UpdateTest(Test newData)
        {
			AddTest(newData, update: true);
				
        }

        public void UpdateTester(Tester newData)
        {
			AddTester(tester: newData, update: true);
			dal.RemoveTester(newData);
		}

        public void UpdateTrainee(Trainee newData)
        {
			AddTrainee(trainee: newData, update: true);
			dal.RemoveTrainee(newData);
		}
		public void UpdateAdmin(Admin newData)
		{
			AddAdmin(admin: newData, update: true);
			RemoveAdmin(newData.ID);
		}
		public void UpdatePerson(Person newData)
		{
			if (newData is Trainee)
				UpdateTrainee((Trainee)newData);
			else if (newData is Tester)
				UpdateTester((Tester)newData);
			else
				UpdateAdmin((Admin)newData);
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

        public void AddMessage(Messages message)
        {
			message.MessageNumber = dal.GetMessageCode();
            dal.AddMessage(message);
			dal.AddMessageCode();
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

		public void AddAdmin(Admin admin, bool update = false)
		{
			if (!update && dal.GetAdmins().Any(A => A.Equals(admin)))
				throw new InvalidOperationException("An Admin with that ID already exists");
			dal.AddAdmin(admin);
		}

		public Admin GetAdmin(string id)
		{
			return dal.GetAdmin(id);
		}

		public bool CheckPassword(string id, string password)
		{
			return dal.CheckPassword(id, password);
		}

		public void AddUpdatePassword(string id, string password)
		{

			dal.AddUpdatePassword(id, password);
		}

		public void RemovePassword(string id)
		{
			dal.RemovePassword(id);
		}

        

        public void RemoveMessage(int num)
		{
			dal.RemoveMessage(num);
		}

		public int GetTestCode()
		{
			return dal.GetTestCode();
		}
	}

}


