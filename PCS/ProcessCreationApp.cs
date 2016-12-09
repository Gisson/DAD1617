using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using CommonTypes;

namespace PCS {
    class ProcessCreationApp {
        private const int PORT = 10001;
        private const String SERVICE_NAME = "pcs";


        [STAThread]
        static void Main(string[] args) {
            TcpChannel channel = new TcpChannel(PORT);
            ChannelServices.RegisterChannel(channel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(ProcessCreationService),
                SERVICE_NAME,
                WellKnownObjectMode.Singleton);

            if(args.Contains("-d") || args.Contains("--debug"))
            {
                Logger.debug = true;
                Console.WriteLine("debug activated");
            }


            Console.Title = "PCS";

            Console.WriteLine("press any key to exit");
            Console.ReadLine();
        }
    }
}
