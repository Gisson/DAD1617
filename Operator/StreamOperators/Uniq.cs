using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTypes;

namespace Operator.StreamOperators
{
    class Uniq : StreamOperator
    {
        List<string> seen;
        int FieldNumber;

        public Uniq(int fieldNumber)
        {
            FieldNumber = fieldNumber;
            seen = new List<string>();
        }

        public IList<IList<string>> processTuple(IList<string> inputTuple) {            
            IList<IList<string>> outputTuples = new List<IList<string>>();
            String currentField = "";
            try
            {
                currentField = inputTuple[FieldNumber];
            }
            catch (ArgumentOutOfRangeException)
            {
                Logger.errorWriteLine("index " + FieldNumber + " is out of range for tuple " + inputTuple);
                return outputTuples;
            }
            if(!seen.Contains(currentField))
            {
                seen.Add(currentField);
                outputTuples.Add(inputTuple);
            }
            return outputTuples; //never returns null
        }
    }
}
