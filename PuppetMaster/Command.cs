using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTypes.RemoteInterfaces;
using System.IO;
using System.Threading;
using CommonTypes;


namespace PuppetMaster {
    using Replicas = IDictionary<String, IOperatorService>;

    //command interface
    public interface ICommand {
        void execute();
    }

    //start
    public class Start : ICommand {
        private String OpID;

        public Start(String opID) {
            OpID = opID;
        }

        public void execute() {
            foreach (IOperatorService op in PuppetMaster.getOperatorReplicas(OpID)) {
                new Thread(() => op.forceStart()).Start();

            }
            Logger.debugWriteLine("Start " + OpID);
        }
    }

    //interval
    public class Interval : ICommand {
        private String OpID;
        private int Milisec;

        public Interval(String opID, int milisec) {
            OpID = opID;
            Milisec = milisec;
        }

        public void execute() {
            foreach (IOperatorService op in PuppetMaster.getOperatorReplicas(OpID)) {
                new Thread(() => op.forceInterval(Milisec)).Start();
            }
            Logger.debugWriteLine("Interval " + OpID + " " + Milisec);
        }
    }

    //status
    public class Status : ICommand {
        public Status() { }

        public void execute() {
            foreach (IOperatorService op in PuppetMaster.getAllOperators()) {
                new Thread(() => op.getStatus()).Start();
            }
            Logger.debugWriteLine("Status");
        }
    }

    //crash
    public class Crash : ICommand {
        private String OpID;
        private int ReplicaIndex;

        public Crash(String opID, int rep) {
            OpID = opID;
            ReplicaIndex = rep;
        }

        public void execute() {
            new Thread(() => PuppetMaster.getOperator(OpID, ReplicaIndex).forceCrash()).Start();
        }
    }

    //freeze
    public class Freeze : ICommand {
        private String OpID;
        private int ReplicaIndex;

        public Freeze(String opID, int rep) {
            OpID = opID;
            ReplicaIndex = rep;
        }

        public void execute() {
            new Thread(() => PuppetMaster.getOperator(OpID, ReplicaIndex).forceFreeze()).Start();
        }
    }

    //unfreeze
    public class Unfreeze : ICommand {
        private String OpID;
        private int ReplicaIndex;

        public Unfreeze(String opID, int rep) {
            OpID = opID;
            ReplicaIndex = rep;
        }

        public void execute() {
            new Thread(() => PuppetMaster.getOperator(OpID, ReplicaIndex).forceUnfreeze()).Start();
        }
    }

    //wait
    public class Wait : ICommand {
        private int Millisec;

        public Wait(int millisec) {
            Millisec = millisec;
        }

        public void execute() {
            Thread.Sleep(Millisec);
            PuppetMaster.receiveLog("", "wait");
        }
    }

    //log
    public class Log : ICommand {
        private String LoggingLevel;

        public Log(String loggingLevel) {
            LoggingLevel = loggingLevel;
        }

        public void execute() {
            //set logginglevel
            throw new NotImplementedException("Log is not implemented");
        }
    }

    //semantics
    public class SetSemantics : ICommand {
        private String Semantics;

        public SetSemantics(String semantics) {
            Semantics = semantics;
        }

        public void execute() {
            //set semantics
            throw new NotImplementedException("SetSemantics is not implemented");
        }
    }

    //create 
    public class ConfigureOperator : ICommand {
        private String OpID, Routing, OpSpec;
        private String[] InputOps, Addresses;
        private int RepFact;

        public ConfigureOperator(String opID, String[] inputOps, int repFact, String routing, String[] addresses, String[] opSpec) {
            Console.WriteLine("new String.Join: opID="+ opID
                + " inputOps=" + String.Join(";", inputOps)
                + " repFact=" + repFact
                + " routing=" + routing
                + " addresses=" + String.Join(";", addresses)
                + " opSpec=" + String.Join(";", opSpec)
                );
            OpID = opID;
            InputOps = inputOps;
            RepFact = repFact;
            Routing = routing;
            Addresses = addresses;
            OpSpec = String.Join(" ", opSpec);
        }

        /*TODO: MOVE THIS FROM HERE, TOO MUCH PARSY*/
        public void execute() {
            Replicas inputOpReplicas = new Dictionary<String, IOperatorService>();
            List<String> fullInputOPs = new List<String>();

            //get urls
            foreach (String inputOpID in InputOps) {
                /* only the operator cares if it's a file or not
                //is it a file?
                if (File.Exists(PuppetMaster.getInputDir() + inputOpID)) {
                    if (inputFiles != "") inputFiles += ",";
                    inputFiles += inputOpID;
                }
                else
                */
                { //get all replicas from input OP
                    // FIXME? assuming that the input OPs have already been previously added
                    if (PuppetMaster.OperatorTable.TryGetValue(inputOpID, out inputOpReplicas)) {
                        //put together urls and separate by commas
                        foreach (String url in inputOpReplicas.Keys.ToArray())
                        {
                            fullInputOPs.Add(url);
                        }
                    } else
                    {
                        // assume it's a file. not really relevant at this point since only the operator will try to open it
                        fullInputOPs.Add(inputOpID);
                    }
                }
            }
            // inputOps Routing OpSpec OpParams
            String configArgs = String.Join(",", fullInputOPs) + " " + Routing + " " + OpSpec;
            PuppetMaster.addOperator(OpID, RepFact, Addresses, configArgs);
        }
    }


}
