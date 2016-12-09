namespace Operator.Commands
{
    internal class Freeze : Command
    {
        private ICommandableOperator cmdOP;
        private bool freeze;

        public Freeze(ICommandableOperator cmdOP, bool freeze) : base(freeze?"Freeze": "Unfreeze", cmdOP)
        {
            this.cmdOP = cmdOP;
            this.freeze = freeze;
        }
        public override void execute()
        {
            if(freeze)
            {
                cmdOP.freeze();
            } else
            {
                cmdOP.unfreeze();
            }
        }
    }
}