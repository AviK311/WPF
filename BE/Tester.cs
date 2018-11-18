using System;
using System.Collections.Generic;
using System.Text;

namespace BE
{
	class Tester:Person
	{
		


		private uint expYears;
		private uint maxWeeklyTests;
		
		private uint maxDistance;


		public Day[] Schedule = new Day[5];
		public uint MaxDistance { get => maxDistance; set => maxDistance = value; }
		public uint ExpYears { get => expYears; set => expYears = value; }
		public uint MaxWeeklyTests { get => maxWeeklyTests; set => maxWeeklyTests = value; }
		
	}
}
