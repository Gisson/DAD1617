using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operator {
    /// <summary>
    /// Abstraction layer to call internal Operator methods
    /// </summary>
    public interface IOperator {
        /// <summary>
        /// register operatorURL as an input for our output
        /// </summary>
        /// <param name="operatorURL">OP that will receive our tuples</param>
        void registerOutputOperator(string opId, string opURL, int replicaIndex); //keeping track of "child" operators to send tuples
        void receiveTuple(IList<string> tuple); //receive tuple from input operator

        /* *** command request by operatorService *** */
        void start();
        void interval(int milliseconds);
        void freeze();
        void unfreeze();
        void crash();
        void status();

        void connectToPuppetMaster();


    }
}
