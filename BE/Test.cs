using System;
using System.Collections.Generic;
using System.Text;

namespace BE
{

	public class Test
	{
		private string testNumber;
		private string testerID;
		private string traineeID;
		private DateTime testDateTime;
		private Address beginLocation;
		private ushort grade;
		private bool isDistance, isReverese, isMirror, isSignal, isBreak, isSigns;

		public string TestNumber { get => testNumber; set => testNumber = value; }
		public string TesterID { get => testerID; set => testerID = value; }
		public string TraineeID { get => traineeID; set => traineeID = value; }
		public DateTime TestDateTime { get => testDateTime; set => testDateTime = value; }
		public Address BeginLocation { get => beginLocation; set => beginLocation = value; }
		public bool IsDistance { get => isDistance; set => isDistance = value; }
		public bool IsReverese { get => isReverese; set => isReverese = value; }
		public bool IsMirror { get => isMirror; set => isMirror = value; }
		public bool IsSignal { get => isSignal; set => isSignal = value; }
		public ushort Grade { get => grade; set => grade = value; }
		public bool IsSigns { get => isSigns; set => isSigns = value; }
		public bool IsBreak { get => isBreak; set => isBreak = value; }

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
