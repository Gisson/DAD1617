using System.Collections.Generic;

namespace Operator.StreamInputs
{
    /// <summary>
    /// interface for a tuple input
    /// </summary>
    interface StreamInput
    {
        IList<string> getTuple();
    }
}
