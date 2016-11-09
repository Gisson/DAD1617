using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;

namespace Operator
{
    class Operator
    {
        /// <summary> port where the OperatorServices are exposed </summary>
        static int port;

        static void printUsage()
        {
            // the PCS will tell us which port to listen on
            String usage = "Usage: Operator.exe PORT" + Environment.NewLine
                         + Environment.NewLine
                         + "       PORT: port to listen on";
            System.Console.WriteLine(usage);
        }

        static bool parseArgs(string[] args)
        {
            if(args.Length >= 1)
            {
                /* parse the Operator port number */
                string portArg = args[0];
                if(!Int32.TryParse(portArg, out port) || port < 10002 || port > 65535)
                {
                    System.Console.WriteLine("Invalid port '" + portArg + "'. Enter a port in the range 10002-65535.");
                    return false;
                }
            } else
            {
                return false;
            }
            return true;
        }

        static void Main(string[] args)
        {
            if(!parseArgs(args))
            {
                printUsage();
                return;
            }

            TcpChannel channel = new TcpChannel(port);

            /* register our service */
            ChannelServices.RegisterChannel(channel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(OperatorServices), "op",
                WellKnownObjectMode.Singleton);

            System.Console.WriteLine("Press <enter> to terminate Operator...");
            System.Console.ReadLine();

            System.Console.WriteLine("Operator reached end of main().");
            Thread.Sleep(500);
        }
    }
}
