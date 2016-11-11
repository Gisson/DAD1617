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
        protected Operator op;
        /* FIXME use a separate interface for the methods that a Command can call */
        public Command(string name, Operator op)
        {
            this.op = op;
            this.name = name;
        }

        public abstract void execute();
    }

}
