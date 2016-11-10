using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuppetMaster {

    //command interface
    public interface ICommand {
        void execute();
    }

    //start
    public class Start : ICommand {
        private String OperatorID;

        public Start(String opID) {
            OperatorID = opID;
        }

        public void execute() {
            Console.WriteLine("Start " + OperatorID);
        }
    }

    //interval
    public class Interval : ICommand {
        private String OperatorID;
        private int Milisec;

        public Interval(String opID, int milisec) {
            OperatorID = opID;
            Milisec = milisec;
        }

        public void execute() {
            Console.WriteLine("Interval " + OperatorID + " " + Milisec);
        }
    }

    //status
    public class Status : ICommand {
        public Status() { }

        public void execute() {
            Console.WriteLine("Status");
        }
    }

    //crash
    public class Crash : ICommand {
        private String OperatorID;
        private int ReplicaIndex;

        public Crash(String opID, int rep) {
            OperatorID = opID;
            ReplicaIndex = rep;
        }

        public void execute() {
            Console.WriteLine("Crash " + OperatorID + " " + ReplicaIndex);
        }
    }

    //freeze
    public class Freeze : ICommand {
        private String OperatorID;
        private int ReplicaIndex;

        public Freeze(String opID, int rep) {
            OperatorID = opID;
            ReplicaIndex = rep;
        }

        public void execute() {
            Console.WriteLine("Freeze " + OperatorID + " " + ReplicaIndex);
        }
    }

    //unfreeze
    public class Unfreeze : ICommand {
        private String OperatorID;
        private int ReplicaIndex;

        public Unfreeze(String opID, int rep) {
            OperatorID = opID;
            ReplicaIndex = rep;
        }

        public void execute() {
            Console.WriteLine("Unfreeze " + OperatorID + " " + ReplicaIndex);
        }
    }

    //wait
    public class Wait : ICommand {
        private int Millisec;

        public Wait(int millisec) {
            Millisec = millisec;
        }

        public void execute() {
            Console.WriteLine("Wait " + Millisec);
        }
    }

    //create 
    public class ConfigureOperator : ICommand {

        public void execute() {
            Console.WriteLine("OP CONFIG");
        }


    }

    /* TODO add more commands */


}
