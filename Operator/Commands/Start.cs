using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operator.Commands
{
    class Start : Command
    {
        public Start(ICommandableOperator op) : base("Start", op)
        {
        }

        public override void execute()
        {
            op.start();
        }
    }
}
