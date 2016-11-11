using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Operator.Routing;
using CommonTypes.RemoteInterfaces;

namespace Operator.Routing
{
    class Primary : RoutingPolicy
    {
        IDictionary<string, IOperatorService[]> ops;
        /* FIXME maybe ops can be in the superclass */

        public Primary(IDictionary<string, IOperatorService[]> ops)
        {
            this.ops = ops;
        }

        public void outputTuple(IList<string> tuple)
        {
            foreach(IOperatorService[] replicas in ops.Values)
            {
                // PRIMARY: send to the first replica
                replicas[0].emitTuple(tuple);
            }
        }
    }
}
