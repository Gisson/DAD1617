using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTypes;

namespace Operator
{
    class RemoteOperator : IRemoteOperator
    {


        public override void receiveSignal()
        {
            /*TODO*/
            throw new NotImplementedException();
        }
        public override void receiveMessage(string message)
        {
            throw new NotImplementedException();
        }

        public override void execute(Command cmd)
        {
            throw new NotImplementedException();
        }
    }
}
