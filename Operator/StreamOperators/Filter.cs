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
        String Value;

        public Filter(int fieldNumber, String condition, String value)
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
            int fieldInt=0, valueInt=0;
            bool areInt = int.TryParse(field, out fieldInt) && int.TryParse(Value, out valueInt);
            int result;
            if(areInt)
            {
                // if field and value are ints, compare them as ints
                result = fieldInt - valueInt;
            } else
            {
                result = String.Compare(field, Value);
            }

            // XXX: this could be optimized for performance (e.g. use compare int vs string) but who cares?
            if ((Condition == ">" && result > 0) ||
                (Condition == "<" && result < 0) ||
                (Condition == "=" && result == 0)) {
                outputTuples.Add(inputTuple);
            }

            return outputTuples;
        }
    }
}
