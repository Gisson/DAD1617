using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes.RemoteInterfaces {
    /*Methods provided to the exterior classes to communicate with OperatorService*/
    public interface IOperatorService {

        void ConnectToPuppetMaster(String puppetMasterURL); //for logging when needed
        void RequestInput(String operatorURL); //registers as a subscriber: ask to input operator to provide tuples when possible
        void EmitTuple(String operatorURL); //request for child operator to receive tuples

        /* *** commands by puppet master *** */
        void ForceStart();
        void ForceInterval(int milliseconds);
        void ForceFreeze();
        void ForceUnfreeze();
        void ForceCrash();
        void GetStatus();
        //note: the wait command is not remote
    }
}
