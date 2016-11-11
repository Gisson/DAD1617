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

        /// <summary>
        /// register operatorURL as an input for our output
        /// </summary>
        /// <param name="operatorURL">OP that will receive our tuples</param>
        void registerOutputOperator(string opId, string opURL, int replicaIndex);

        // perhaps there is a better name for this
        /// <summary> send tuple to downstream OP </summary>
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
