using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operator.StreamOperators {
    class Count : StreamOperator {
        int seenCounter;

        public Count() {
            seenCounter = 0;
        }

        public IList<IList<string>> processTuple(IList<string> tuple) {

            IList<IList<string>> outputTuples = new List<IList<string>>();
            ++seenCounter;
            IList<string> container = new List<string>();
            container.Add(seenCounter.ToString());
            outputTuples.Add(container);
            return outputTuples; //never returns null
        }
    }
}
