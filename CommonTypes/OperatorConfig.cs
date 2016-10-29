using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    [Serializable]
    class OperatorConfig
    {
        // 0 is "at-most-once" 1 is "at-least-once" 2 is "exactly-once"
        public int semantic { get; set; }
        public List<string> inputs { get; set; }
    }
}
