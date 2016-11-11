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
        void registerOutputOperator(String operatorURL); //keeping track of "child" operators to send tuples
        void receiveTuple(IList<string> tuple); //receive tuple from input operator

        /* *** command request by operatorService *** */
        void start();
        void interval(int milliseconds);
        void freeze();
        void unfreeze();
        void crash();
        void status();


    }
}
