using System;

namespace CommonTypes
{
    [Serializable] /* Marshal by Value; Note: attributes are not inherited */
    public abstract class Command
    {
        private string _name;
        public Command(string name) { _name = name; }

    }

    namespace Commands {
 
        [Serializable]
        public class Start : Command
        {
            private string _operatorId;
            public Start(string opID) : base("Start")
            {
                _operatorId = opID;
            }
        }

        [Serializable]
        public class Interval : Command
        {
            private string operatoId;
            private int xMs;
            public Interval(string opID, int miliseconds) : base("Interval")
            {
                operatoId = opID;
                xMs = miliseconds;
            }
        }

        [Serializable]
        public class Status : Command
        {
            public Status() : base("Status") { }
        }

        [Serializable]
        public class Crash : Command
        {
            private string _proccessID;
            public Crash(string pid) : base("Crash")
            {
                _proccessID = pid;
            }
        }

        [Serializable]
        public class Freeze : Command
        {
            private string _proccessID;
            public Freeze(string pid) : base("Freeze")
            {
                _proccessID = pid;
            }
        }

        [Serializable]
        public class UnFreeze : Command
        {
            private string _proccessID;
            public UnFreeze(string pid) : base("UnFreeze")
            {
                _proccessID = pid;
            }
        }

        [Serializable]
        public class Wait : Command
        {
            private int _xMS;
            public Wait(int xMS) : base("Wait")
            {
                _xMS = xMS;
            }
        }

        /* TODO add more commands */

    }
}
