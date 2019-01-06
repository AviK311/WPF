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
		public void SaveToXML<T>(T source, string path)
		{
			FileStream file = new FileStream(path, FileMode.Create);
			XmlSerializer xmlSerializer = new XmlSerializer(source.GetType());
			xmlSerializer.Serialize(file, source);
			file.Close();
		}




	}

}
