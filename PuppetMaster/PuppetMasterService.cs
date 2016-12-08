using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTypes;
using CommonTypes.Exceptions;
using CommonTypes.RemoteInterfaces;

namespace PuppetMaster {
    class PuppetMasterService : MarshalByRefObject, IPuppetMasterService {
        private SortedDictionary<string, ConnectionArgs> pcs =
            new SortedDictionary<string, ConnectionArgs>();

       /* public void registerClient(string name, string url, int port) {
            if (pcs.ContainsKey(name))
                throw new AlreadyRegisteredException();
            /* All of this prints should be in the log file FIXME 
            Console.WriteLine("Trying to connect to " + url + " at port " + port + "...");
              IRemoteProcessCreationService iro = (IRemoteProcessCreationService)
                  Activator.GetObject(typeof(IRemoteProcessCreationService),
                  url + ":" + port + "/" + name);
              if (iro == null) {
                  throw new UnaccessibleClientException(name);
              }
              else {
                  iro.receiveMessage("Registered successful in puppetMaster!");
              }
            pcs.Add(name, new ConnectionArgs(url, port));
            Console.WriteLine("Connected successfully to " + name); //This could be with an exception....maybe
        }*/



        public void writeIntoLog(string opID, string logMessage) {
            PuppetMaster.receiveLog(opID, logMessage);
        }

        void IPuppetMasterService.ping()
        {
            // do nothing
            throw new NotImplementedException();
        }
    }
}
