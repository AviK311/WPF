using System;
using System.Collections.Generic;
using System.Text;

namespace BE
{
    class Configuration
    {
        protected static int testCode = 0;
        public int TestCode { get => testCode; set => testCode = value; }
        public static uint TimeBetweenTests { get; } = 7;
        public static uint MinAgaOfTrainee { get; } = 18;
        public static uint MinNumOfClasses { get; } = 20;
        public static uint MinAgaOfTester { get; } = 40;
    }
}
