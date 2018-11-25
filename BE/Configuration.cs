using System;
using System.Collections.Generic;
using System.Text;

namespace BE
{
    class Configuration
    {
        public static uint TimeBetweenTests { get; } = 7;
        public static uint MinAgaOfTrainee { get; } = 18;
        public static uint MinNumOfClasses { get; } = 20;
        public static uint MinAgaOfTester { get; } = 40;
    }
}
