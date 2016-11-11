using System;
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
    public class Operator : IOperator {
        string myOpId;
        string myOpURL;
        int myReplicaIndex;
        /// <summary> OpId, replicaURL[] </summary>
        IDictionary<String, IList<IOperatorService>> outputOps;

        private IPuppetMasterService pms;
        private const String PMS_URL = "tcp://localhost:10000/pms";


        private StreamEngine engine = null;

        BlockingCollection<Command> cmds;
        Thread cmdThread;

        public Operator(String opID, String opURL, int replicaIndex, String routing) {
            myOpId = opID;
            myOpURL = opURL;
            myReplicaIndex = replicaIndex;
            outputOps = new Dictionary<string, IList<IOperatorService>>();
            cmds = new BlockingCollection<Command>(new ConcurrentQueue<Command>());

            ThreadStart ts = new ThreadStart(this.processCommands);
            cmdThread = new Thread(ts);
            cmdThread.Start();
        }

        public void parseOperatorSpec(String opSpec, String[] opParams) {
            Console.WriteLine("hello");
            List<StreamInputs.StreamInput> inputs = new List<StreamInputs.StreamInput>();
            inputs.Add(new StreamInputs.Stdin());
            StreamOperator streamOp = null;
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
            engine = new StreamEngine(inputs, streamOp, new Routing.Stdout());
            //FIXME just testing
            //engine.start();
            //Console.ReadLine();
            ThreadStart ts = new ThreadStart(this.processCommands);
            cmdThread = new Thread(ts);
            cmdThread.Start();
        }

        public void enqueue(Command c)
        {
            cmds.Add(c);
        }

        /// <summary> thread that processes queued commands </summary>
        private void processCommands()
        {
            while (!cmds.IsCompleted)
            {
                Command c = cmds.Take();
                /* TODO this is a good place to log executed commands synchronously */
                c.execute();
            }
        }

        /* *** commands *** */
        public void start() {
            //start processing and emitting tuples

            pms.writeIntoLog(myOpId, "start");
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
            throw new NotImplementedException();
        }
        /// <summary>
        /// make ourself an output of our input OPs
        /// </summary>
        /// <param name=inputOpsURLs>OPs which will send us tuples</param>
        private void registerInputs(String[] inputOpsURLs) {
            foreach (String url in inputOpsURLs) {
                IOperatorService inputOp = getOperatorServiceByURL(url);
                /* FIXME we should probably start a new thread here, since not all OPs will be up yet */
                inputOp.registerOutputOperator(myOpId, myOpURL, myReplicaIndex);
            }
        }

        public void registerOutputOperator(string opId, string opURL, int replicaIndex) {
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
            OperatorService op = new OperatorService(this);
            RemotingServices.Marshal(op, serviceName, typeof(OperatorService));
        }

        public void connectToPuppetMaster() {
            pms = (IPuppetMasterService)Activator.GetObject(
        typeof(IPuppetMasterService),
        PMS_URL);
        }
    }


    class Program {
        //args syntax: OpID opURL replicaIndex inputFiles inputOpURLs Routing OpSpec OpParams;
        static void Main(string[] args) {
            try {
                String opID = args[0];
                String opURL = args[1];
                int replicaIndex = Int32.Parse(args[2]);
                String[] inputFiles = args[3].Split(',');
                String[] inputOpsURLs = args[4].Split(',');
                String routing = args[5];
                String opSpec = args[6];
                String[] opParams = (args.Length > 7 ? args[7].Split(',') : null);


                Operator op = new Operator(opID, opURL, replicaIndex, routing);
                op.parseOperatorSpec(opSpec, opParams);
                op.launchService();
                //process.RequestInput(inputOpsURLs);
                op.connectToPuppetMaster();
            }
            catch (Exception e) { Console.WriteLine(e); }

            Console.ReadLine();
        }
    }
}
