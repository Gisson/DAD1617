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
    class Operator {
        /// <summary> port where the OperatorServices are exposed </summary>
        

        public Operator() {
        }


        /* *** commands *** */
        void Start() {
            //start processing and emitting tuples
            throw new NotImplementedException();
        }

        void Interval(int milliseconds) {
            //sleep between consecutive events
            throw new NotImplementedException();
        }

        void Freeze() {
            //stops processing tuples
            throw new NotImplementedException();
        }

        void Unfreeze() {
            //continues processing tuples
            throw new NotImplementedException();
        }

        void Crash() {
            //rip process, you will be missed
            throw new NotImplementedException();
        }


        public void LaunchService(String operatorURL) {
            Match match = Regex.Match(operatorURL, @"^tcp://[\w\.]+:(\d{4,5})/(\w+)$");
            int port = Int32.Parse(match.Groups[1].Value);
            String serviceName = match.Groups[2].Value;

            TcpChannel channel = new TcpChannel(port);
            ChannelServices.RegisterChannel(channel, false);
            /* register our operator */
            OperatorService op = new OperatorService();
            RemotingServices.Marshal(op, serviceName, typeof(OperatorService));
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

        static void Main(string[] args) {
         /*   if (!parseArgs(args)) {
                printUsage();
                return;
            }*/
            Operator process = new Operator();
            process.LaunchService(args[0]);
            //process.ConnectToPuppetMaster();

            //System.Console.WriteLine("Press <enter> to terminate Operator...");
            //System.Console.ReadLine(); /* can't use StreamInputs.Stdin with this line here */

            System.Console.WriteLine("Operator reached end of main().");
            Console.ReadLine();
        }
    }
}
