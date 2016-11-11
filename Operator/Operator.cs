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

namespace Operator {
    class Operator : IOperator {
        string myOpId;
        string myOpURL;
        int myReplicaIndex;
        /// <summary> OpId, replicaURL[] </summary>
        IDictionary<String, IList<IOperatorService>> outputOps;

        private IPuppetMasterService pms;
        private const String PMS_URL = "tcp://localhost:10000/pms";


        private StreamEngine engine;

        public Operator(String opID, String opURL, int replicaIndex, String routing, String opSpec, String[] opParams) {
            pms = (IPuppetMasterService)Activator.GetObject(
                    typeof(IPuppetMasterService),
                    PMS_URL);

            myOpId = opID;
            myOpURL = opURL;
            myReplicaIndex = replicaIndex;
            outputOps = new Dictionary<string, IList<IOperatorService>>();
            //routing
            //opspec
            //opparams

            // FIXME just for testing
            List<StreamInputs.StreamInput> inputs = new List<StreamInputs.StreamInput>();
            inputs.Add(new StreamInputs.Stdin());
            engine = new StreamEngine(inputs, new StreamOperators.Uniq(0), new Routing.Stdout());

            //FIXME just testing
            //engine.start();
            //Console.ReadLine();
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


        /// <summary>
        /// make ourself an output of our input OPs
        /// </summary>
        /// <param name="inputOpsURLs">OPs which will send us tuples</param>
        private void registerInputs(String[] inputOpsURLs) {
            foreach (String url in inputOpsURLs) {
                IOperatorService inputOp = getOperatorServiceByURL(url);
                /* FIXME we should probably start a new thread here, since not all OPs will be up yet */
                inputOp.registerOutputOperator(myOpId, myOpURL, myReplicaIndex);
            }
        }

        public void registerOutputOperator(string opId, string opURL, int replicaIndex)
        {
            IOperatorService service = getOperatorServiceByURL(opURL);
            lock (outputOps)
            {
                IList<IOperatorService> replicas;
                if (!outputOps.TryGetValue(opId, out replicas))
                {
                    replicas = new List<IOperatorService>();
                }
                replicas.Add(service);
            }
        }

        private IOperatorService getOperatorServiceByURL(string URL)
        {
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


        public void receiveTuple(IList<string> tuple)
        {
            throw new NotImplementedException();
        }
    }


    class Program {
        static int port;

        static void printUsage() {
            // the PCS will tell us which port to listen on
            String usage = "Usage: Operator.exe PORT" + Environment.NewLine
                         + Environment.NewLine
                         + "       PORT: port to listen on";
            System.Console.WriteLine(usage);
        }

        static bool parseArgs(string[] args) {
            if (args.Length >= 1) {
                /* parse the Operator port number */
                string portArg = args[0];
                if (!Int32.TryParse(portArg, out port) || port < 10002 || port > 65535) {
                    System.Console.WriteLine("Invalid port '" + portArg + "'. Enter a port in the range 10002-65535.");
                    return false;
                }
            }
            else {
                return false;
            }
            return true;
        }

        //args syntax: OpID opURL replicaIndex inputFiles inputOpURLs Routing OpSpec OpParams;
        static void Main(string[] args) {
            /*   if (!parseArgs(args)) {
                   printUsage();
                   return;
               }*/
            System.Console.WriteLine(String.Join(" ", args));
            //PERFECTLY TESTED: READY TO USE
            String opID = args[0];
            String opURL = args[1];
            int replicaIndex = Int32.Parse(args[2]);
            String[] inputFiles = args[3].Split(',');
            String[] inputOpsURLs = args[4].Split(',');
            String routing = args[5];
            String opSpec = args[6];
            String[] opParams = (args.Length > 7 ? args[7].Split(',') : null);


            Operator process = new Operator(opID, opURL, replicaIndex, routing, opSpec, opParams);
            process.launchService();
            //process.RequestInput(inputOpsURLs);
            //process.ConnectToPuppetMaster();

            //System.Console.WriteLine("Press <enter> to terminate Operator...");
            //System.Console.ReadLine(); /* can't use StreamInputs.Stdin with this line here */

            Console.ReadLine();
        }
    }
}
