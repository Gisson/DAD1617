using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTypes.RemoteInterfaces;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Collections.Concurrent;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using CommonTypes;

/* ********************** HOW THIS WORKS ***************************
 * Main is the starting point.
 * It creates a form, and starts the Remote Puppet Master
 * As soon as we choose the config file from the UI by
 * pressing "Browse...", all text is copied to the form window.
 * Meanwhile, all commands are parsed and **STORED** in a queue - DON'T RUN ANYTHING YET.
 * If the "Run" button is pressed, all commands are executed asynchronously,
 * with the exception of the WAIT command.
 * If the "Step" button is pressed, only one thread runs while
 * all others are blocked until further orders
 */


namespace PuppetMaster {
    using Replicas = IDictionary<String, IOperatorService>;

    public sealed class PuppetMaster {
        //constants
        private const string SERVICE_NAME = "pms";
        private const int PORT = 10000;
        private const String PCS_URL = "tcp://localhost:10001/pcs";

        public static IProcessCreationService PCS = null;
        //command queue to store instructions and execute later
        public static ConcurrentQueue<ICommand> Commands = new ConcurrentQueue<ICommand>();
        //table with key=op-name|value=replicaList
        public static IDictionary<String, Replicas> OperatorTable = new Dictionary<String, Replicas>();

        //singleton stuff
        private static readonly PuppetMaster instance = new PuppetMaster();
        static PuppetMaster() { }
        private PuppetMaster() { }

        
        //START HERE

        [STAThread]
        public static void Main(string[] args)
        {
            if (args.Contains("-d") || args.Contains("--debug"))
            {
                Logger.debug = true;
                Logger.debugWriteLine("debug activated");
            }
            // createUI();
            Thread tInit = new Thread(() => init());
            tInit.IsBackground = true; // close thread if application exits
            tInit.Start();

            Thread tPrompt = new Thread(() => commandPrompt());
            tPrompt.IsBackground = true; // close thread if application exits
            tPrompt.Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DADStormForm());
            Logger.debugWriteLine("PuppetMaster main thread exited");
        }



        public static void init() {
            //Hosts connection with Remote Puppet Master
            TcpChannel channel = new TcpChannel(PORT);
            ChannelServices.RegisterChannel(channel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(PuppetMasterService),
                SERVICE_NAME,
                WellKnownObjectMode.Singleton);
            //connect to PCS// NEVER returns null
            PCS = (IProcessCreationService)Activator.GetObject(
                typeof(IProcessCreationService),
                PCS_URL);
            bool connected = false;
            while(!connected)
            {
                try
                {
                    PCS.ping(); // throws System.Net.Sockets.Exception if not connected
                    connected = true;
                    Logger.debugWriteLine("connected");
                }
                catch (System.Net.Sockets.SocketException)
                {
                    Thread.Sleep(10);
                }
            }

            //Console.ReadLine();
        }

        /// <summary>
        /// reads commands from stdin and executes them
        /// </summary>
        public static void commandPrompt()
        {
            String line;
            while((line = Console.ReadLine()) != null)
            {
                Logger.debugWriteLine("read: " + line);
                if(Parser.executeLine(line, false) != LineSyntax.VALID)
                {
                    Logger.errorWriteLine("Syntax error");
                }
            }
        }


        //get specific operator replica
        public static IOperatorService getOperator(String opId, int replicaIndex) {
            Replicas replicas;
            OperatorTable.TryGetValue(opId, out replicas);
            //FIXME
            return replicas.Values.ElementAt(replicaIndex);
        }

        //get all replicas from an operator
        public static IList<IOperatorService> getOperatorReplicas(String opId) {
            Replicas replicas;
            OperatorTable.TryGetValue(opId, out replicas);
            return replicas.Values.ToList();
        }

        //get all replicas from all operators
        public static IList<IOperatorService> getAllOperators() {
            IList<IOperatorService> opList = new List<IOperatorService>();
            foreach (Replicas replicas in OperatorTable.Values) {
                opList = opList.Concat(replicas.Values).ToList();
            }
            return opList;
        }

        //create a new operator and replicas
        public static void addOperator(String opID, int repFact, String[] addresses, String configArgs) {
            Replicas opReplicas = new Dictionary<String, IOperatorService>();
            for (int i = 0; i < repFact; i++)
            {
                //String opURL = addresses[i] + i + opID; //small hack to avoid already registered service names
                String opURL = addresses[i]; // the hack is not needed

                // OpID opURL replicaIndex inputOps Routing OpSpec OpParams
                String args = opID + " " + opURL + " " + i + " " + configArgs;

                bool pcsOK = false;
                while (!pcsOK) { 
                    try
                    {
                        PCS.createOperator(args);
                        pcsOK = true;
                    }
                    catch (System.Net.Sockets.SocketException)
                    {
                        Console.WriteLine("Waiting for PCS");
                        Thread.Sleep(100);
                    }
                }
                IOperatorService op = (IOperatorService)Activator.GetObject(typeof(IOperatorService), opURL);

                opReplicas.Add(new KeyValuePair<String, IOperatorService>(opURL, op));
            }
            //add created replicas to operator table, associating them with the new OP
            OperatorTable.Add(new KeyValuePair<String, Replicas>(opID, opReplicas));
        }

        //logging
        public static void receiveLog(string opID, string logMessage) {
            Console.WriteLine(opID+": "+logMessage);
        }



        //asynchronous command execution
        //stepHandle autoblocks any other threads untill the current one finishes
    /*    static EventWaitHandle stepHandle = new AutoResetEvent(false);

        public static void executeInstructions(bool step) {
            ICommand command;
            //loop until queue is empty
            if (Commands.TryDequeue(out command)) {
                //step instruction
                if (step) { //start new blocked thread
                    new Thread(() => stepHandle.WaitOne()).Start();
                    stepHandle.Set();  //wake up the thread
                }
                //run all instructions
                else {  //launch new thread
                    new Thread(() => executeInstructions(false)).Start();
                }
                command.execute();
            }
        }*/

        /*go to source directory*/
        public static string getInputDir() {
            string dir = Directory.GetCurrentDirectory();
            DirectoryInfo dirInfo = null;
            //go 3 directories up
            for (int i = 0; i < 3; i++) {
                dirInfo = Directory.GetParent(dir);
                dir = dirInfo.FullName;
            }
            return dirInfo.GetDirectories("InputFiles").First().FullName + "\\";
        }


        public static void executeInstructions(bool step) {
            ICommand command;
            //loop until queue is empty
            if (Commands.TryDequeue(out command)) {
                command.execute();
                if (!step) {
                    executeInstructions(false);
                }
            }
        }

        public static void clearCommands() {
            Commands = new ConcurrentQueue<ICommand>();
        }
    }
}
