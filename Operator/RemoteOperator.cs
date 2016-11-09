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


        public override void receiveMessage(string message)
        {
            throw new NotImplementedException();
        }
        

        public override void start()
        {
            throw new NotImplementedException();
        }

        public override void interval(int milliseconds)
        {
            throw new NotImplementedException();
        }

        public override void status()
        {
            throw new NotImplementedException();
        }

        public override void crash()
        {
            throw new NotImplementedException();
        }

        public override void freeze()
        {
            throw new NotImplementedException();
        }

        public override void unfreeze()
        {
            throw new NotImplementedException();
        }
    }
}
