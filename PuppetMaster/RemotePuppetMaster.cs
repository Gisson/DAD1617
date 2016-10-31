using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTypes;

namespace PuppetMaster
{
    class RemotePuppetMaster : IRemotePuppetMaster
    {
        private SortedDictionary<string,ConnectionArgs> _operators= 
            new SortedDictionary<string, ConnectionArgs>();

        public override void registerClient(string name,string url,int port)
        {
            if (_operators.ContainsKey(name))
                throw new AlreadyRegisteredException();
            /* All of this prints should be in the log file FIXME */
            Console.WriteLine("Trying to connect to " + url + " at port " + port + "...");
            IRemoteProcessCreationService iro = (IRemoteProcessCreationService)
                Activator.GetObject(typeof(IRemoteProcessCreationService),
                url + ":" + port + "/" + name);
            if (iro == null)
                throw new UnaccessibleClientException(name);
            else
                iro.receiveMessage("Registered successful in puppetMaster!");
            _operators.Add(name, new ConnectionArgs(url, port));
            Console.WriteLine("Connected successfully to " + name); //This could be with an exception....maybe
        }

        public void readConfig()
        {
            /*TODO*/
        }
    }
}
