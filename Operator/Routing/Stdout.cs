using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operator.Routing
{
    class Stdout : RoutingPolicy
    {
        public void outputTuple(IList<string> tuple)
        {
            string line = "TUPLE OUT: ";
            foreach(string s in tuple)
            {
                line += " " + s;
            }
            Console.WriteLine(line);
        }
    }
}
