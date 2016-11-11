using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operator {
    /// <summary>
    /// Abstraction layer to call internal Operator methods
    /// </summary>
    interface IOperator {
        void RegisterOutputOperator(String operatorURL); //keeping track of "child" operators to send tuples
        void ReceiveTuple(String tuple); //receive tuple from input operator

        /* *** command request by operatorService *** */
        void Start();
        void Interval(int milliseconds);
        void Freeze();
        void Unfreeze();
        void Crash();


    }
}
