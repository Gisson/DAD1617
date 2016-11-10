using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operator.Routing
{
    interface RoutingPolicy
    {
        void outputTuple(IList<string> tuple);
    }
}
