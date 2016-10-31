using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    public class ConnectionArgs : MarshalByRefObject
    {
        public string url { get; set; }
        public int port { get; set; }
        public ConnectionArgs(string url, int port)
        {
            this.url = url;
            this.port = port;
        }

    }

    public abstract class IRemoteOperator: MarshalByRefObject
    {
        //Receives a signal from the PuppetMaster
        public abstract void receiveSignal();

        /// <summary> Receives and enqueues a command to be executed by the Operator </summary>
        public abstract void execute(Command cmd);
        public abstract void receiveMessage(string message);// Might not be needed
    }

    public abstract class IRemotePuppetMaster : MarshalByRefObject
    {
        public abstract void registerClient(string name,string url,int port); 
    }

    public abstract class IRemoteProcessCreationService : MarshalByRefObject
    {
        public abstract void startOperator(/* TODO add args */);
        public abstract void receiveMessage(string message);
    }

}
