namespace Operator.Commands
{
    internal class ForceInterval : Command
    {
        private ICommandableOperator cmdOP;
        private int milliseconds;

        public ForceInterval(ICommandableOperator op, int milliseconds) : base("ForceInterval", op)
        {
            this.cmdOP = op;
            this.milliseconds = milliseconds;
        }
        public override void execute()
        {
            // op.interval() has a lock mechanism
            op.interval(milliseconds);
        }
    }
}