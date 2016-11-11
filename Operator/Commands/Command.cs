using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operator.Commands
{
    /// abstract command (command pattern)
    public abstract class Command
    {
        protected string name;
        protected ICommandableOperator op;
        protected ICommandableOperator Operator
        {
            get
            {
                return op;
            }
        }
        /* FIXME use a separate interface for the methods that a Command can call */
        public Command(string name, ICommandableOperator op)
        {
            this.op = op;
            this.name = name;
        }

        public abstract void execute();
    }

}
