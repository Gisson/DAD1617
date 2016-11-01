using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTypes;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using CommonTypes.Commands;
using CommandExceptions;

namespace PuppetMaster
{
    class PuppetMaster
    {
        private const string psName="PuppetMaster";
        private const int port = 10000;

        static void Main(string[] args)
        {
            RemotePuppetMaster rpm = startPuppetMaster();
            rpm.readConfig();
            string c;
            while (true)
            {
                c = Console.ReadLine();
                readCommand(c);
                //Need to send execution command
            }
        }

        static RemotePuppetMaster startPuppetMaster()
        {
            //Should RemotePuppetMaster handle this instead?
            TcpChannel channel = new TcpChannel(port);
            ChannelServices.RegisterChannel(channel,true);
            RemotePuppetMaster rpm = new RemotePuppetMaster();
            RemotingServices.Marshal(rpm, psName, typeof(RemotePuppetMaster));
            return rpm;
        }

        static Command readCommand(string input)
        {
            string[] splitted = input.Split(' ');
            switch (splitted[0])
            {
                case "Start":
                    if (splitted.Length != 2)
                        throw new WrongCommandFormat(splitted[0]);
                    return (new Start(splitted[1]));
                case "Interval":
                    if (splitted.Length != 3)
                        throw new WrongCommandFormat(splitted[0]);
                    return (new Interval(splitted[1], Int32.Parse(splitted[1])));
                case "Status":
                    return (new Status());
                case "Crash":
                    if (splitted.Length != 2)
                        throw new WrongCommandFormat(splitted[0]);
                    return (new Crash(splitted[1]));
                case "Freeze":
                    if (splitted.Length != 2)
                        throw new WrongCommandFormat(splitted[0]);
                    return (new Freeze(splitted[1]));
                case "Unfreeze":
                    if (splitted.Length != 2)
                        throw new WrongCommandFormat(splitted[0]);
                    return (new UnFreeze(splitted[1]));
                case "Wait":
                    if (splitted.Length != 2)
                        throw new WrongCommandFormat(splitted[0]);
                    return (new Wait(Int32.Parse( splitted[1])));
                default:
                    throw new UnexistentCommand(splitted[0]);
            }
        }
    }
}
