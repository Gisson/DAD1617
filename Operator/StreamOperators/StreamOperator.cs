using System.Collections.Generic;

namespace Operator.StreamOperators
{
    interface StreamOperator
    {
        IList<IList<string>> processTuple(IList<string> tuple);
    }
}
