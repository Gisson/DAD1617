using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operator.StreamOperators
{
    class Uniq : StreamOperator
    {
        List<string> seen;
        private int field;

        public Uniq(int field)
        {
            this.field = field;
            this.seen = new List<string>();
        }

        public IList<string> processTuple(IList<string> tuple)
        {
            string currentField = tuple[field];
            if(seen.Contains(currentField))
            {
                /* FIXME null is probably not a good idea */
                return null;
            } else
            {
                seen.Add(currentField);
                return tuple;
            }
        }
    }
}
