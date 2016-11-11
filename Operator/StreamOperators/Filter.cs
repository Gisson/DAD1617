using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operator.StreamOperators {

    class Filter : StreamOperator {
        int FieldNumber;
        String Condition;
        String Value;

        public Filter(int fieldNumber, String condition, String value) {
            FieldNumber = fieldNumber;
            Condition = condition;
            Value = value;
        }

        public IList<IList<string>> processTuple(IList<string> inputTuple) {
            IList<IList<string>> outputTuples = new List<IList<string>>();
            String field = inputTuple.ElementAt(FieldNumber);
            if ((Condition == ">" && field.Length > Value.Length) ||
                (Condition == "<" && field.Length < Value.Length) ||
                (Condition == "=" && field.Equals(Value))) {
                outputTuples.Add(inputTuple);
            }

            return outputTuples;
        }
    }
}
