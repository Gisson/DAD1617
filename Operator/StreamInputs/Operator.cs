using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operator.StreamInputs
{
    /// <summary>
    /// InputStream that gets tuples from another operator
    /// </summary>
    public class Operator : StreamInput
    {
        /// <summary> thread-safe queue of received tuples. </summary>
        private Queue tupleQueue;

        public Operator()
        {
            Queue q = new Queue();
            tupleQueue = Queue.Synchronized(q);
        }
        
        public IList<string> getTuple()
        {
            if(tupleQueue.Count > 0)
            {
                return (IList<string>)tupleQueue.Dequeue();
            } else
            {
                /* FIXME should we block instead? */
                return null;
            }
        }

        /* FIXME currently I'm not distinguishing between different operators.
         * That is, everything that comes from OPx is received by this class. */

        /// <summary> called by OperatorServices when a new tuple is received </summary>
        public void putTuple(IList<string> tuples)
        {
            tupleQueue.Enqueue(tuples);
        }
    }
}
