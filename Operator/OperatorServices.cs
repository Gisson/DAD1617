using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTypes;
using System.Diagnostics;

namespace Operator
{
    /// <summary>
    /// Implements the services that the Operator exports via IRemoteOperator
    /// </summary>
    class OperatorServices : IRemoteOperator
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
            Process.GetCurrentProcess().Kill();
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
