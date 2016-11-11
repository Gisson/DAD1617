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

        private StreamEngine engine;
        private Operator OP;

        public OperatorService(Operator op) {
            OP = op;
            //had to comment or else he'd be stuck in the command line
            // FIXME just for testing
            List<StreamInputs.StreamInput> inputs = new List<StreamInputs.StreamInput>();
            inputs.Add(new StreamInputs.Stdin());
            engine = new StreamEngine(inputs, new StreamOperators.Uniq(0), new Routing.Stdout());

            //FIXME just testing
            //engine.start();
            //Console.ReadLine();
        }

        public void connectToPuppetMaster(String puppetMasterURL) { //for logging when needed
            throw new NotImplementedException();
        }

        //registers as a subscriber: ask to input operator to provide tuples when possible
        public void requestInput(String opID, String opURL, int replicaIndex) { 
            throw new NotImplementedException();
        }

        public void emitTuple(String operatorURL) { //request for child operator to receive tuples
            throw new NotImplementedException();
        }

        /* *** commands by puppet master *** */
        public void forceStart() {
            OP.start();
            //throw new NotImplementedException();
        }

        public void forceInterval(int milliseconds) {
            OP.interval(milliseconds);
            //throw new NotImplementedException();
        }

        public void forceFreeze() {
            OP.freeze();
            //throw new NotImplementedException();
        }

        public void forceUnfreeze() {
            OP.unfreeze();
            //throw new NotImplementedException();
        }

        public void forceCrash() {
            OP.crash();
            //throw new NotImplementedException();
        }

        public void getStatus() {
            OP.status();
           // throw new NotImplementedException();
        }
    }
}
