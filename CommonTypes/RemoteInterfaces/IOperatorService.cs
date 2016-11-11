using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes.RemoteInterfaces {
    /*Methods provided to the exterior classes to communicate with OperatorService*/
    public interface IOperatorService {
        //for logging when needed
        void connectToPuppetMaster(String puppetMasterURL);

        //registers as a subscriber: ask to input operator to provide tuples when possible
        void requestInput(String opID, String opURL, int replicaIndex);

        //send tuple to downstream OP
        // perhaps there is a better name for this
        void emitTuple(IList<string> tuple);

        /* *** commands by puppet master *** */
        void forceStart();
        void forceInterval(int milliseconds);
        void forceFreeze();
        void forceUnfreeze();
        void forceCrash();
        void getStatus();
        //note: the wait command is not remote
    }
}
