using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;

namespace PCS {
    class ProcessCreationApp {
        private const int PORT = 10001;
        private const String SERVICE_NAME = "pcs";

        [STAThread]
        static void Main() {
            TcpChannel channel = new TcpChannel(PORT);
            ChannelServices.RegisterChannel(channel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(ProcessCreationService),
                SERVICE_NAME,
                WellKnownObjectMode.Singleton);

            Console.WriteLine("press any key to exit");
            System.Console.ReadLine();
        }
    }
}
