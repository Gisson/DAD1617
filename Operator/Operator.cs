﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using CommonTypes.RemoteInterfaces;
using System.Collections;
using System.Text.RegularExpressions;
using Operator.StreamOperators;
using Operator.Commands;
using System.Collections.Concurrent;

namespace Operator {
    public class Operator : IOperator, ICommandableOperator {
        string myOpId;
        string myOpURL;
        int myReplicaIndex;
        List<string> inputOpURLs;
        /// <summary> OpId, replicaURL[] </summary>
        IDictionary<String, IList<IOperatorService>> outputOps;

        private IPuppetMasterService pms;
        private const String PMS_URL = "tcp://localhost:10000/pms";


        private StreamEngine engine = null;
        List<StreamInputs.StreamInput> streamInputs = new List<StreamInputs.StreamInput>();
        StreamOperator streamOp = null;

        /* FIXME do this better */
        StreamInputs.Operator inputStreamOp = new StreamInputs.Operator();

        /// <summary> thread-safe queue of commads to be executed </summary>
        BlockingCollection<Command> cmds;
        Thread cmdThread;

        /* FIXME: we should be consistent on how to pass parameters (parse method vs constuctor arg) */
        public Operator(String opID, String opURL, int replicaIndex, String routing) {
            myOpId = opID;
            myOpURL = opURL;
            myReplicaIndex = replicaIndex;
            outputOps = new Dictionary<string, IList<IOperatorService>>();
            cmds = new BlockingCollection<Command>(new ConcurrentQueue<Command>());

            /* TODO parse routing */

            ThreadStart ts = new ThreadStart(this.processCommands);
            cmdThread = new Thread(ts);
            cmdThread.Start();
        }

        public void parseOperatorSpec(String opSpec, String[] opParams) {
            Console.WriteLine("parseOperatorSpec");
            switch (opSpec.ToUpper()) {
                case "COUNT": {
                        streamOp = new Count();
                        break;
                    }
                case "CUSTOM": {
                        streamOp = new Custom(
                            opParams[0],
                            opParams[1],
                            opParams[2]);
                        break;
                    }
                case "DUP": {
                        streamOp = new Dup();
                        break;
                    }
                case "FILTER": {
                        streamOp = new Filter(
                            Int32.Parse(opParams[0]),
                            opParams[1],
                            opParams[2]);
                        break;
                    }
                case "UNIQ": {
                        streamOp = new Uniq(Int32.Parse(opParams[0]));
                        break;
                    }
            }
        }

        public void parseInputOperatorsSpec(String[] inputOps)
        {
            inputOpURLs = new List<string>();
            foreach (String input in inputOps) {

                if (input.Equals("-"))
                {
                    streamInputs.Add(new StreamInputs.Stdin());
                } else if(input.Contains("tcp://"))
                {
                    // assume it's an URL for an operator
                    inputOpURLs.Add(input);
                    // FIXME: assuming input from all OPs goes to the same InputStream
                    if (!streamInputs.Contains(inputStreamOp))
                    {
                        streamInputs.Add(inputStreamOp);
                    }
                }
                else
                {
                    // assume it's a file
                    //FIXME TODO streamInputs.Add(new StreamInputs.File(input));
                }
            }
        }

        public void enqueue(Command c)
        {
            cmds.Add(c);
        }

        /// <summary> thread that processes queued commands </summary>
        private void processCommands()
        {
            Console.WriteLine("Command Thread starting...");
            while (!cmds.IsCompleted)
            {
                Command c = cmds.Take();
                /* TODO this is a good place to log executed commands synchronously */
                c.execute();
            }
        }

        /* *** commands *** */
        public void start() {
            // register ourselves as ouputs
            // FIXME this is not the best place to to this, but it's a way to make sure the OPs are all running
            registerInputs(inputOpURLs);
            //start processing and emitting tuples
            pms.writeIntoLog(myOpId, "start");
            /* FIXME routing is wrong */
            Routing.RoutingPolicy r;
            if(myOpId.StartsWith("-")) {
                // just a little hack for testing
                r = new Routing.Stdout();
            } else{
                r = new Routing.Primary(outputOps);
            }
            engine = new StreamEngine(streamInputs, streamOp, r);
            engine.start();
            //throw new NotImplementedException();
        }

        public void interval(int milliseconds) {
            //sleep between consecutive events
            pms.writeIntoLog(myOpId, "interval");
            //throw new NotImplementedException();
        }

        public void freeze() {
            //stops processing tuples
            pms.writeIntoLog(myOpId, "freeze");
            //throw new NotImplementedException();
        }

        public void unfreeze() {
            pms.writeIntoLog(myOpId, "unfreeze");
            //continues processing tuples
            //throw new NotImplementedException();
        }

        public void crash() {
            pms.writeIntoLog(myOpId, "crash");
            //rip process, you will be missed
            //throw new NotImplementedException();
        }

        public void status() {
            pms.writeIntoLog(myOpId, "status");
            //throw new NotImplementedException();
        }



        public void receiveTuple(IList<string> tuple) {
            /* FIXME add a parameter for the opid? */
            inputStreamOp.putTuple(tuple);
            //throw new NotImplementedException();
        }
        /// <summary>
        /// make ourself an output of our input OPs
        /// </summary>
        /// <param name=inputOpsURLs>OPs which will send us tuples</param>
        private void registerInputs(List<string> inputOpsURLs) {
            foreach (String url in inputOpsURLs) {
                IOperatorService inputOp = getOperatorServiceByURL(url);
                /* FIXME we should probably start a new thread here, since not all OPs will be up yet */
                Console.WriteLine("Subscribing to " + url);
                inputOp.registerOutputOperator(myOpId, myOpURL, myReplicaIndex);
            }
        }

        public void registerOutputOperator(string opId, string opURL, int replicaIndex)
        {
            Console.WriteLine(opId + " subscribed to " + myOpId);
            IOperatorService service = getOperatorServiceByURL(opURL);
            lock (outputOps) {
                IList<IOperatorService> replicas;
                if (!outputOps.TryGetValue(opId, out replicas)) {
                    replicas = new List<IOperatorService>();
                }
                replicas.Add(service);
            }
        }

        private IOperatorService getOperatorServiceByURL(string URL) {
            return (IOperatorService)Activator.GetObject(
                    typeof(IOperatorService), URL);
        }

        public void launchService() {
            Match match = Regex.Match(myOpURL, @"^tcp://[\w\.]+:(\d{4,5})/(\w+)$");
            System.Console.WriteLine(match.Groups[1].Value);
            int port = Int32.Parse(match.Groups[1].Value);
            String serviceName = match.Groups[2].Value;

            IDictionary props = new Hashtable();
            props["port"] = port;
            props["name"] = serviceName;
            TcpChannel channel = new TcpChannel(props, null, null);
            ChannelServices.RegisterChannel(channel, false);
            /* register our operator */
            OperatorService op = new OperatorService(this, this);
            RemotingServices.Marshal(op, serviceName, typeof(OperatorService));
        }

        private class PuppetMasterMock : IPuppetMasterService {
            public void writeIntoLog(string opID, string logMessage)
            {
                Console.WriteLine("PMMOCK: " + opID + " " + logMessage);
            }
        }

        public void connectToPuppetMaster()
        {
            if (myOpId.EndsWith("_")) {
                // just a little hack for testing
                pms = new PuppetMasterMock();
            } else {
                pms = (IPuppetMasterService)Activator.GetObject(
                    typeof(IPuppetMasterService),
                    PMS_URL);
            }
        }
    }


    class Program {
        //args syntax: OpID opURL replicaIndex inputOps Routing OpSpec OpParams;
        static void Main(string[] args) {
            Console.Write("args: ");
            foreach( string a in args)
            {
                Console.Write(a + " ");
            }
            Console.WriteLine();
            try {
                String opID = args[0];
                String opURL = args[1];
                int replicaIndex = Int32.Parse(args[2]);
                String[] inputOps = args[3].Split(',');
                String routing = args[4];
                String opSpec = args[5];
                String[] opParams = (args.Length > 6 ? args[6].Split(',') : null);


                Operator op = new Operator(opID, opURL, replicaIndex, routing);
                /* \/ FIXME TODO move these to the construct 'a la' ES ? */
                op.parseOperatorSpec(opSpec, opParams);
                op.parseInputOperatorsSpec(inputOps);
                op.connectToPuppetMaster();

                op.launchService();
                // FIXME testing
                op.start();
            }
            catch (Exception e) { Console.WriteLine(e); }

            //Console.ReadLine();
        }
    }
}
