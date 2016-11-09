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
        /* *** commands *** */
        public abstract void start();
        public abstract void interval(int milliseconds);
        public abstract void status();
        public abstract void crash();
        public abstract void freeze();
        public abstract void unfreeze();
        //note: the wait command is not remote


        public abstract void receiveMessage(string message);// Might not be needed
    }

    public abstract class IRemotePuppetMaster : MarshalByRefObject
    {
        public abstract void registerClient(string name,string url,int port);
        public abstract void receiveLog(string clientName, string logMessage);
    }

    public abstract class IRemoteProcessCreationService : MarshalByRefObject
    {
        public abstract void startOperator(/* TODO add args */);
        public abstract void receiveMessage(string message);
    }

}
