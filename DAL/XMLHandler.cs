using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using BE;
using DS;

namespace DAL
{
	public class XMLHandler
	{
		XElement TraineeRoot, TesterRoot, ConfigRoot;
		public string TraineePath = @"TraineeXML.xml",
			TesterPath = @"TesterXML.xml",
			AdminPath = @"AdminXML.xml",
			TestPath = @"TestXML.xml",
			PasswordPath = @"PasswordXML.xml",
			MessagePath = @"MessageXML.xml",
			ConfigPath = @"ConfigXML.xml";
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
				if (!File.Exists(ConfigPath))
					CreateConfigFile();
				else LoadConfigData();
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
			SaveToXML(DataSource.MessagesList, MessagePath);
		}
		private void LoadMessageFile()
		{
			DataSource.MessagesList = LoadFromXML<List<Messages>>(MessagePath);
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
		private void CreateConfigFile()
		{
			ConfigRoot = new XElement("Config");
			ConfigRoot.Add(new XElement("TestCode", 0), new XElement("MessageCode", 0));
			ConfigRoot.Save(ConfigPath);
		}
		private void LoadConfigData()
		{
			try
			{
				ConfigRoot = XElement.Load(ConfigPath);
			}
			catch
			{
				throw new InvalidOperationException("Error Loading Configuration File");
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
			SaveToXML(DataSource.PasswordList, PasswordPath);
		}
		private void LoadPasswordFile()
		{
			DataSource.PasswordList = LoadFromXMLPasswords();
		}

		private void CreateAdminFile()
		{
			SaveToXML(DataSource.adminList, AdminPath);
		}
		private void LoadAdminFile()
		{
			DataSource.adminList = LoadFromXML<List<Admin>>(AdminPath);
		}


		private void CreateTestFile()
		{
			SaveToXML(DataSource.testList, TestPath);
		}
		private void LoadTestFile()
		{
			DataSource.testList = LoadFromXML<List<Test>>(TestPath);
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
			List<XElement> Notifications = new List<XElement>();
			foreach(var item in trainee.notifications)
			{
				XElement Icon = new XElement("Icon", item.Icon);
				XElement Time = new XElement("Time", item.time);
				XElement Message = new XElement("Message", item.message);
				Notifications.Add(new XElement("Notification", Icon, Message, Time));
			}
			XElement Trainee = new XElement("Trainee", ID, Name, sex, phone, email,
				BirthDay, Address, CheckEmail, AwaitingAdminReset, firstLogin, CurrentCarType,
				Stats, new XElement("Notifications", Notifications));
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
				var traineeElement = (from trainee in TraineeRoot.Elements()
									  where trainee.Element("id").Value == id
									  select trainee).FirstOrDefault();
				toReturn.notifications = (from Notification in traineeElement.Elements("Notifications").Elements()
										  select new Notification
										  {
											  Icon = (from icon in (MessageIcon[])Enum.GetValues(typeof(MessageIcon))
													  where icon.ToString() == Notification.Element("Icon").Value
													  select icon).FirstOrDefault(),
											  message = Notification.Element("Message").Value,
											  time = DateTime.Parse(Notification.Element("Time").Value)
										  }).ToList();
			}
			catch
			{
				toReturn = null;
			}
			return toReturn;
		}
		public List<Trainee> GetTrainees()
		{
			return (from trainee in TraineeRoot.Elements()
					where GetTrainee(trainee.Element("id").Value)!=null
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
            List<XElement> Days = new List<XElement>();
            foreach (var item in tester.schedule.week)
            {
				string HourString = "";
				for (int i = 9; i < 15; i++)
					if (item.Value[i])
						HourString += i.ToString() + ",";
				if (HourString.Length>0) HourString = HourString.Substring(0, HourString.Length - 1);
				Days.Add(new XElement(item.Key.ToString(), HourString));
            }
			List<XElement> Notifications = new List<XElement>();
			foreach (var item in tester.notifications)
			{
				XElement Icon = new XElement("Icon", item.Icon);
				XElement Time = new XElement("Time", item.time);
				XElement Message = new XElement("Message", item.message);
				Notifications.Add(new XElement("Notification", Icon, Message, Time));
			}
			XElement Schedule = new XElement("Schedule", Days);
            XElement Tester = new XElement("Tester", ID, Name, sex, phone, email,
                BirthDay, Address, CheckEmail, AwaitingAdminReset, firstLogin, testingCarType, MaxDistance,
                ExpYears, MaxWeeklyTests, Schedule, new XElement("Notifications",Notifications));
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
                                ExpYears = uint.Parse(tester.Element("ExpYears").Value),
								MaxWeeklyTests = uint.Parse(tester.Element("MaxWeeklyTests").Value),
							}).FirstOrDefault();

                foreach (var item in toReturn.schedule.week)
                {
					var DayElement = TesterRoot.Elements().FirstOrDefault(T => T.Element("id").Value == id).Element("Schedule").Element(item.Key.ToString());
					var stringArray = DayElement.Value.Split(',');
					foreach (var hour in stringArray)
					{
						int h;
						if (int.TryParse(hour, out h))
							toReturn.schedule[item.Key,h] = true;
					}
				}
				var testerElement = (from tester in TesterRoot.Elements()
									  where tester.Element("id").Value == id
									  select tester).FirstOrDefault();
				toReturn.notifications = (from Notification in testerElement.Elements("Notification").Elements()
										  select new Notification
										  {
											  Icon = (from icon in (MessageIcon[])Enum.GetValues(typeof(MessageIcon))
													  where icon.ToString() == Notification.Element("Icon").Value
													  select icon).FirstOrDefault(),
											  message = Notification.Element("Message").Value,
											  time = DateTime.Parse(Notification.Element("Time").Value)
										  }).ToList();
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
					where GetTester(tester.Element("id").Value)!=null
					select GetTester(tester.Element("id").Value)).ToList();
        }
        public void RemoveTester(string id)
        {
            var toRemove = (from tester in TesterRoot.Elements()
                            where tester.Element("id").Value == id
                            select tester).FirstOrDefault();
            toRemove.Remove();
            TesterRoot.Save(TesterPath);
        }

		#endregion
		public int GetTestCode()
		{
			return int.Parse(ConfigRoot.Element("TestCode").Value);
		}
		public void AddToTestCode()
		{
			ConfigRoot.Element("TestCode").Value = (GetTestCode() + 1).ToString();
			ConfigRoot.Save(ConfigPath);
		}
		public int GetMessageCode()
		{
			return int.Parse(ConfigRoot.Element("MessageCode").Value);
		}
		public void AddToMessageCode()
		{
			ConfigRoot.Element("MessageCode").Value = (GetMessageCode() + 1).ToString();
			ConfigRoot.Save(ConfigPath);
		}


		public void SaveToXML<T>(T source, string path)
        {
            FileStream file = new FileStream(path, FileMode.Create);
			if (source is PasswordList)
				DataSource.PasswordList.Serialize(file);
			else
			{
				XmlSerializer xmlSerializer = new XmlSerializer(source.GetType());
				xmlSerializer.Serialize(file, source);
			}
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
		public PasswordList LoadFromXMLPasswords()
		{
			FileStream file = new FileStream(PasswordPath, FileMode.OpenOrCreate);
			var result = DataSource.PasswordList.Deserialize(file);
			file.Close();
			return result;
		}
    }

}
