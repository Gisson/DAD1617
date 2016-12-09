using CommonTypes;
using CommonTypes.RemoteInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operator.Routing
{
    class Random : RoutingPolicy
    {
        private System.Random rand;
        IDictionary<string, IList<IOperatorService>> ops;
        /* FIXME maybe ops can be in the superclass */

        public Random(IDictionary<string, IList<IOperatorService>> ops)
        {
            this.ops = ops;
            Logger.debugWriteLine("RoutingPolicy: Random (" + ops.Count + " subscribers)");
            rand = new System.Random();
        }

        public void outputTuple(IList<string> tuple)
        {
            Logger.debugWriteLine("Random outputTuple to " + ops.Count + " subscribers: " + tuple.ElementAt(0));
            foreach (IList<IOperatorService> replicas in ops.Values)
            {
                // random: send to the a random replica
                int randomReplica = rand.Next(0, replicas.Count);
                Logger.debugWriteLine("Random replica " + randomReplica);
                replicas.ElementAt(randomReplica).emitTuple(tuple);
            }
        }
    }
}
