using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BE
{
	public class XMLHandler
	{
		XElement TraineeRoot, TesterRoot, AdminRoot, TestRoot, PasswordRoot, MessageRoot;
		public string TraineePath = @"TraineeXML.xml",
			TesterPath = @"TesterXML.xml",
			AdminPath = @"AdminXML.xml",
			TestPath = @"TestXML.xml",
			PasswordPath = @"PasswordXML.xml",
			MessagePath = @"MessageXML.xml";
		public static XMLHandler Handler = null;
		public static XMLHandler GetXMLHandler()
		{
			if (Handler == null)
				Handler = new XMLHandler();
			 return Handler;
		}
		private XMLHandler()
		{
			try
			{
				if (!File.Exists(TraineePath))
					CreateTraineeFile();
				else LoadTraineeData();
				if (!File.Exists(TesterPath))
					CreateTesterFile();
				else LoadTesterFile();
				if (!File.Exists(TestPath))
					CreateTestFile();
				else LoadTestFile();
				if (!File.Exists(AdminPath))
					CreateAdminFile();
				else LoadAdminFile();
				if (!File.Exists(PasswordPath))
					CreatePasswordFile();
				else LoadPasswordFile();
				if (!File.Exists(MessagePath))
					CreateMessageFile();
				else LoadMessageFile();
			}
			catch { }

		}

		private void CreateMessageFile()
		{
			MessageRoot = new XElement("Messages");
			MessageRoot.Save(MessagePath);
		}
		private void LoadMessageFile()
		{
			try
			{
				MessageRoot = XElement.Load(MessagePath);
			}
			catch
			{
				throw new InvalidOperationException("Error Loading Message File");
			}
		}
		private void CreateTraineeFile()
		{
			TraineeRoot = new XElement("Trainees");
			TraineeRoot.Save(TraineePath);
		}
		private void LoadTraineeData()
		{
			try
			{
				TraineeRoot = XElement.Load(TraineePath);
			}
			catch
			{
				throw new InvalidOperationException("Error Loading Trainee File");
			}
		}
		private void CreateTesterFile()
		{
			TesterRoot = new XElement("Testers");
			TesterRoot.Save(TesterPath);
		}
		private void LoadTesterFile()
		{
			try
			{
				TesterRoot = XElement.Load(TesterPath);
			}
			catch
			{
				throw new InvalidOperationException("Error Loading Tester File");
			}
		}
		private void CreatePasswordFile()
		{
			PasswordRoot = new XElement("Passwords");
			PasswordRoot.Save(PasswordPath);
		}
		private void LoadPasswordFile()
		{
			try
			{
				PasswordRoot = XElement.Load(PasswordPath);
			}
			catch
			{
				throw new InvalidOperationException("Error Loading Password File");
			}
		}

		private void CreateAdminFile()
		{
			AdminRoot = new XElement("Admins");
			AdminRoot.Save(AdminPath);
		}
		private void LoadAdminFile()
		{
			try
			{
				AdminRoot = XElement.Load(AdminPath);
			}
			catch
			{
				throw new InvalidOperationException("Error Loading Admin File");
			}
		}


		private void CreateTestFile()
		{
			TestRoot = new XElement("Tests");
			TestRoot.Save(TestPath);
		}
		private void LoadTestFile()
		{
			try
			{
				TestRoot = XElement.Load(TestPath);
			}
			catch
			{
				throw new InvalidOperationException("Error Loading Test File");
			}
		}
        #region trainee
        public void AddTrainee(Trainee trainee)
		{
			XElement ID = new XElement("id", trainee.ID);
			
			XElement FirstName = new XElement("firstName", trainee.Name.first);
			XElement LastName = new XElement("lastName", trainee.Name.last);
			XElement Name = new XElement("name",LastName, FirstName);
			XElement Street = new XElement("street", trainee.Address.street);
			XElement BuildingNumber = new XElement("bldNumber", trainee.Address.buildingNumber);
			XElement City = new XElement("City", trainee.Address.city);
			XElement Address = new XElement("Address", BuildingNumber, Street, City);
			XElement phone = new XElement("Phone", trainee.PhoneNumber);
			XElement email = new XElement("Email", trainee.Email);
			XElement BirthDay = new XElement("BirthDay", trainee.BirthDay);
			XElement sex = new XElement("Gender", trainee.Sex);
			XElement CheckEmail = new XElement("CheckEmail", trainee.CheckEmail);
			XElement AwaitingAdminReset = new XElement("AwaitingAdminReset", trainee.AwaitingAdminReset);
			XElement firstLogin = new XElement("FirstLogin", trainee.FirstLogIn);
			XElement CurrentCarType = new XElement("VehicleType", trainee.CurrentCarType);
			List<XElement> Stats = new List<XElement>();
			foreach (var item in trainee.carTypeStats)
			{
				var stats = item.Value;
				XElement TeacherFirstName = new XElement("FirstName", stats.teacherName.first);
				XElement TeacherLastName = new XElement("LastName", stats.teacherName.last);
				XElement TeacherName = new XElement("TeacherName", TeacherFirstName, TeacherLastName);
				XElement SchoolName = new XElement("SchoolName", stats.schoolName);
				XElement GearType = new XElement("GearType", stats.gearType);
				XElement NumOfTests = new XElement("NumOfTests", stats.numOfTest);
				XElement NumOfLessons = new XElement("NumOfLessons", stats.numOfLessons);
				XElement Passed = new XElement("Passed", stats.passed);
				Stats.Add(new XElement(item.Key.ToString(), GearType, SchoolName, TeacherName, NumOfLessons, NumOfTests, Passed));
			}
			XElement Trainee = new XElement("Trainee", ID, Name, sex, phone, email,
				BirthDay, Address, CheckEmail, AwaitingAdminReset, firstLogin, CurrentCarType,
				Stats);
			TraineeRoot.Add(Trainee);
			TraineeRoot.Save(TraineePath);
		}
		public Trainee GetTrainee(string id)
		{
			Trainee toReturn;
			try
			{
				toReturn = (from trainee in TraineeRoot.Elements()
							where trainee.Element("id").Value == id
							select new Trainee()
							{
								ID = trainee.Element("id").Value,
								Email = trainee.Element("Email").Value,
								PhoneNumber = trainee.Element("Phone").Value,
								Name = new Name(trainee.Element("name").Element("firstName").Value,
												trainee.Element("name").Element("lastName").Value),
								Address = new Address(trainee.Element("Address").Element("City").Value,
								trainee.Element("Address").Element("street").Value,
								trainee.Element("Address").Element("bldNumber").Value),
								BirthDay = DateTime.Parse(trainee.Element("BirthDay").Value),
								Sex = (from sex in (Gender[])Enum.GetValues(typeof(Gender))
									   where sex.ToString() == trainee.Element("Gender").Value
									   select sex).FirstOrDefault(),
								CheckEmail = bool.Parse(trainee.Element("CheckEmail").Value),
								AwaitingAdminReset = bool.Parse(trainee.Element("AwaitingAdminReset").Value),
								FirstLogIn = bool.Parse(trainee.Element("FirstLogin").Value),
								CurrentCarType = (from type in (VehicleType[])Enum.GetValues(typeof(VehicleType))
												  where type.ToString() == trainee.Element("VehicleType").Value
												  select type).FirstOrDefault(),
							}).FirstOrDefault();

				foreach (var item in toReturn.carTypeStats)
				{
					var stats = item.Value;
					var TypeElement = TraineeRoot.Elements().FirstOrDefault(T => T.Element("id").Value == id).Element(item.Key.ToString());
					stats.gearType = (from type in (GearType[])Enum.GetValues(typeof(GearType))
									  where type.ToString() == TypeElement.Element("GearType").Value
									  select type).FirstOrDefault();
					stats.numOfLessons = int.Parse(TypeElement.Element("NumOfLessons").Value);
					stats.numOfTest = int.Parse(TypeElement.Element("NumOfTests").Value);
					stats.teacherName = new Name(TypeElement.Element("TeacherName").Element("FirstName").Value,
											TypeElement.Element("TeacherName").Element("LastName").Value);
					stats.passed = bool.Parse(TypeElement.Element("Passed").Value);
					stats.schoolName = TypeElement.Element("SchoolName").Value;
				}
			}
			catch {
				toReturn = null;
			}
			return toReturn;
		}
		public List<Trainee> GetTrainees()
		{
			return (from trainee in TraineeRoot.Elements()
					select GetTrainee(trainee.Element("id").Value)).ToList();
		}
		public void RemoveTrainee(string id)
		{
			var toRemove = (from trainee in TraineeRoot.Elements()
							where trainee.Element("id").Value == id
							select trainee).FirstOrDefault();
			toRemove.Remove();
			TraineeRoot.Save(TraineePath);
		}
#endregion
       
        #region tester
        public void AddTester(Tester tester)
        {
            XElement ID = new XElement("id", tester.ID);
            XElement FirstName = new XElement("firstName", tester.Name.first);
            XElement LastName = new XElement("lastName", tester.Name.last);
            XElement Name = new XElement("name", LastName, FirstName);
            XElement Street = new XElement("street", tester.Address.street);
            XElement BuildingNumber = new XElement("bldNumber", tester.Address.buildingNumber);
            XElement City = new XElement("City", tester.Address.city);
            XElement Address = new XElement("Address", BuildingNumber, Street, City);
            XElement phone = new XElement("Phone", tester.PhoneNumber);
            XElement email = new XElement("Email", tester.Email);
            XElement BirthDay = new XElement("BirthDay", tester.BirthDay);
            XElement sex = new XElement("Gender", tester.Sex);
            XElement CheckEmail = new XElement("CheckEmail", tester.CheckEmail);
            XElement AwaitingAdminReset = new XElement("AwaitingAdminReset", tester.AwaitingAdminReset);
            XElement firstLogin = new XElement("FirstLogin", tester.FirstLogIn);
            XElement testingCarType = new XElement("VehicleType", tester.testingCarType);
            XElement MaxDistance = new XElement("MaxDistance", tester.MaxDistance);
            XElement ExpYears = new XElement("ExpYears", tester.ExpYears);
            XElement MaxWeeklyTests = new XElement("MaxWeeklyTests", tester.MaxWeeklyTests);
            List<XElement> Day = new List<XElement>();
            foreach (var item in tester.schedule.week)
            {
                var day = item.Value;
                XElement _9 = new XElement("9", day.hours[0]);
                XElement _10 = new XElement("10", day.hours[1]);
                XElement _11 = new XElement("11", day.hours[2]);
                XElement _12 = new XElement("12", day.hours[3]);
                XElement _13 = new XElement("13", day.hours[4]);
                XElement _14 = new XElement("14", day.hours[5]);
                XElement _15 = new XElement("15", day.hours[6]);

                Day.Add(new XElement(item.Key.ToString(), _9, _10, _11, _12, _13, _14, _15));
            }
            XElement Scheduale = new XElement("Scheduale", Day);
            XElement Tester = new XElement("Tester", ID, Name, sex, phone, email,
                BirthDay, Address, CheckEmail, AwaitingAdminReset, firstLogin, testingCarType, MaxDistance,
                ExpYears, MaxWeeklyTests, Scheduale);
            TesterRoot.Add(Tester);
            TesterRoot.Save(TesterPath);
        }

        public Tester GetTester(string id)
        {
            Tester toReturn;
            try
            {
                toReturn = (from tester in TesterRoot.Elements()
                            where tester.Element("id").Value == id
                            select new Tester()
                            {
                                ID = tester.Element("id").Value,
                                Email = tester.Element("Email").Value,
                                PhoneNumber = tester.Element("Phone").Value,
                                Name = new Name(tester .Element("name").Element("firstName").Value,
                                                tester.Element("name").Element("lastName").Value),
                                Address = new Address(tester.Element("Address").Element("City").Value,
                                tester.Element("Address").Element("street").Value,
                                tester.Element("Address").Element("bldNumber").Value),
                                BirthDay = DateTime.Parse(tester.Element("BirthDay").Value),
                                Sex = (from sex in (Gender[])Enum.GetValues(typeof(Gender))
                                       where sex.ToString() == tester.Element("Gender").Value
                                       select sex).FirstOrDefault(),
                                CheckEmail = bool.Parse(tester.Element("CheckEmail").Value),
                                AwaitingAdminReset = bool.Parse(tester.Element("AwaitingAdminReset").Value),
                                FirstLogIn = bool.Parse(tester.Element("FirstLogin").Value),
                                testingCarType = (from type in (VehicleType[])Enum.GetValues(typeof(VehicleType))
                                                  where type.ToString() == tester.Element("VehicleType").Value
                                                  select type).FirstOrDefault(),
                                MaxDistance= uint.Parse(tester.Element("MaxDistance").Value),
                                ExpYears = uint.Parse(tester.Element("ExpYears").Value)
                            }).FirstOrDefault();

                foreach (var item in toReturn.schedule.week)
                {
                    var day = item.Value;
                    var TypeElement = TesterRoot.Elements().FirstOrDefault(T => T.Element("id").Value == id).Element(item.Key.ToString());
                    day.hours[0] = bool.Parse(TypeElement.Element("Scheduale").Element("Day").Element("_9").Value);
                    day.hours[1] = bool.Parse(TypeElement.Element("Scheduale").Element("Day").Element("_10").Value);
                    day.hours[2] = bool.Parse(TypeElement.Element("Scheduale").Element("Day").Element("_11").Value);
                    day.hours[3] = bool.Parse(TypeElement.Element("Scheduale").Element("Day").Element("_12").Value);
                    day.hours[4] = bool.Parse(TypeElement.Element("Scheduale").Element("Day").Element("_13").Value);
                    day.hours[5] = bool.Parse(TypeElement.Element("Scheduale").Element("Day").Element("_14").Value);
                    day.hours[6] = bool.Parse(TypeElement.Element("Scheduale").Element("Day").Element("_15").Value);
                }
            }
            catch
            {
                toReturn = null;
            }
            return toReturn;
        }
        public List<Tester> GetTesters()
        {
            return (from tester in TesterRoot.Elements()
                    select GetTester(tester.Element("id").Value)).ToList();
        }
        public void RemoveTester(string id)
        {
            var toRemove = (from tester in TesterRoot.Elements()
                            where tester.Element("id").Value == id
                            select tester).FirstOrDefault();
            toRemove.Remove();
            TesterRoot.Save(TraineePath);
        }
        #endregion



        public void SaveToXML<T>(T source, string path)
        {
            FileStream file = new FileStream(path, FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(source.GetType());
            xmlSerializer.Serialize(file, source);
            file.Close();
        }
        public T LoadFromXML<T>(string path)
        {
            FileStream file = new FileStream(path, FileMode.OpenOrCreate);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            T result = (T)xmlSerializer.Deserialize(file);
            file.Close();
            return result;
        }
    }

}
