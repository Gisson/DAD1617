using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTypes;
using Operator.Commands;
using System.Diagnostics;
using System.Collections.Generic;

namespace Operator
{
    /// <summary>
    /// Implements the services that the Operator exports via IRemoteOperator
    /// </summary>
    class OperatorServices : IRemoteOperator
    {
        private StreamEngine engine;

        public OperatorServices()
        {

            // FIXME just for testing
            List<StreamInputs.StreamInput> inputs = new List<StreamInputs.StreamInput>();
            inputs.Add(new StreamInputs.Stdin());
            engine = new StreamEngine(inputs, new StreamOperators.Uniq(0), new Routing.Stdout());

            // FIXME just testing
            engine.start();
        }

        
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
