using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTypes;
using CommonTypes.RemoteInterfaces;
using System.Diagnostics;
using System.Collections.Generic;

namespace Operator
{
    /// <summary>
    /// Redirects remote calls received by IOperatorServices to IOperator
    /// </summary>
    class OperatorService : MarshalByRefObject, IOperatorService
    {

        private IOperator OP;
        private ICommandableOperator cmdOP;
        /* perhaps just one unified interface is needed :( 
         * I wanted to prevent commands from running methods that are intended for the service only */

        public OperatorService(IOperator op, ICommandableOperator cmdOP) {
            OP = op;
            this.cmdOP = cmdOP;
        }

        /* FIXME does this need to be here? */
        public void connectToPuppetMaster(String puppetMasterURL) { //for logging when needed
            Logger.errorWriteLine("connectToPuppetMaster is not implemented... ignoring");
        }

        //registers as a subscriber: ask to input operator to provide tuples when possible
        public void requestInput(String opID, String opURL, int replicaIndex) {
            OP.registerOutputOperator(opID, opURL, replicaIndex);
            //throw new NotImplementedException();
        }

        public void emitTuple(IList<string> tuple) {
            OP.receiveTuple(tuple);
            //throw new NotImplementedException();
        }

        /* *** commands by puppet master *** */
        public void forceStart() {
            OP.enqueue(new Commands.Start(cmdOP));
        }

        public void forceInterval(int milliseconds)
        {
            OP.enqueue(new Commands.ForceInterval(cmdOP, milliseconds));
        }

        public void forceFreeze()
        {
            OP.enqueue(new Commands.Freeze(cmdOP, true));
        }

        public void forceUnfreeze()
        {
            OP.enqueue(new Commands.Freeze(cmdOP, false));
        }

        public void forceCrash()
        {
            // TODO
            Logger.errorWriteLine("forceCrash is not implemented... ignoring");
        }

        public void getStatus()
        {
            // TODO
            Logger.errorWriteLine("getStatus is not implemented... ignoring");
        }

        public void registerOutputOperator(string opId, string opURL, int replicaIndex)
        {
            OP.registerOutputOperator(opId, opURL, replicaIndex);
        }
        public void ping()
        {
            // do nothing
            Logger.debugWriteLine("received ping");
        }
    }
}
