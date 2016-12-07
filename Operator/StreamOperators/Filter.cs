using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operator.StreamOperators {

    class Filter : StreamOperator {
        String[] validConditions = { ">", "=", "<" };
        int FieldNumber;
        String Condition;
        int Value;

        public Filter(int fieldNumber, String condition, int value)
        {
            if(!validConditions.Contains(condition))
            {
                throw new ArgumentException("invalid Filter condition:" + condition);
            }
            Console.WriteLine("Filter field " + fieldNumber + condition + value);
            FieldNumber = fieldNumber;
            Condition = condition;
            Value = value;
        }

        public IList<IList<string>> processTuple(IList<string> inputTuple) {
            IList<IList<string>> outputTuples = new List<IList<string>>();
            String field = inputTuple.ElementAt(FieldNumber);
            int fieldInt = int.Parse(field);
            // XXX: this could be optimized for performance (e.g. use compare int vs string) but who cares?
            if ((Condition == ">" && fieldInt > Value) ||
                (Condition == "<" && fieldInt < Value) ||
                (Condition == "=" && fieldInt == Value)) {
                outputTuples.Add(inputTuple);
            }

            return outputTuples;
        }
    }
}
