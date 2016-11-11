using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operator.StreamOperators {
    class Dup : StreamOperator {

        public Dup() { }

        public IList<IList<string>> processTuple(IList<string> inputTuple) {
            IList<IList<string>> outputTuples = new List<IList<string>>();
            outputTuples.Add(inputTuple);

            return outputTuples;
        }
    }
}
