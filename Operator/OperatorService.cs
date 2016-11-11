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

        public OperatorService() {
            // FIXME just for testing
            List<StreamInputs.StreamInput> inputs = new List<StreamInputs.StreamInput>();
            inputs.Add(new StreamInputs.Stdin());
            engine = new StreamEngine(inputs, new StreamOperators.Uniq(0), new Routing.Stdout());

            // FIXME just testing
            engine.start();
            Console.ReadLine();
        }

        public void ConnectToPuppetMaster(String puppetMasterURL) { //for logging when needed
            throw new NotImplementedException();
        }

        public void RequestInput(String operatorURL) { //registers as a subscriber: ask to input operator to provide tuples when possible
            throw new NotImplementedException();
        }

        public void EmitTuple(String operatorURL) { //request for child operator to receive tuples
            throw new NotImplementedException();
        }

        /* *** commands by puppet master *** */
        public void ForceStart() {
            throw new NotImplementedException();
        }

        public void ForceInterval(int milliseconds) {
            throw new NotImplementedException();
        }

        public void ForceFreeze() {
            throw new NotImplementedException();
        }

        public void ForceUnfreeze() {
            throw new NotImplementedException();
        }

        public void ForceCrash() {
            throw new NotImplementedException();
        }

        public void GetStatus() {
            throw new NotImplementedException();
        }
    }
}
