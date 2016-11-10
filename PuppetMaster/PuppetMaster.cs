using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTypes;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using CommandExceptions;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

/* ********************** HOW THIS WORKS ***************************
 * App.cs is the starting point.
 * It creates a form, and that form contains the
 * PuppetMaster within (can't be backwards because: threads).
 * As soon as we choose the config file from the UI by
 * pressing "Browse...", all text is copied to the form window.
 * Meanwhile, all commands are parsed and **STORED** in a queue - DON'T RUN ANYTHING YET.
 * If the "Run" button is pressed, all commands are executed asynchronously,
 * with the exception of the WAIT command.
 * If the "Step" button is pressed, only one thread runs while
 * all others are blocked until further orders
 */

namespace PuppetMaster
{
    class PuppetMaster
    {
        public ConcurrentQueue<ICommand> Commands;
        public Parser Parser;

        private const string psName="PuppetMaster";
        private const int port = 10000;

        public PuppetMaster() {
            Commands = new ConcurrentQueue<ICommand>();
            Parser = new Parser(this);

           /* RemotePuppetMaster rpm = startPuppetMaster();
            rpm.readConfig();
            string c;
            while (true) {
                c = Console.ReadLine();
                readCommand(c);
                //Need to send execution command
            }*/
        }

          //asynchronous command execution
        //stepHandle autoblocks any other threads untill the current one finishes
        static EventWaitHandle stepHandle = new AutoResetEvent(false);

        public void executeInstructions(bool step) {
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
        }


     /*   private RemotePuppetMaster startPuppetMaster()
        {
            //Should RemotePuppetMaster handle this instead?
            TcpChannel channel = new TcpChannel(port);
            ChannelServices.RegisterChannel(channel,true);
            RemotePuppetMaster rpm = new RemotePuppetMaster();
            RemotingServices.Marshal(rpm, psName, typeof(RemotePuppetMaster));
            return rpm;
        }
    */

        /*
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
        }*/
    }
}
