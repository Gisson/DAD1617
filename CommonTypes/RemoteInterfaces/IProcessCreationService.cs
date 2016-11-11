using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes.RemoteInterfaces {
    interface IProcessCreationService {
        void startOperator(/* TODO add args */);
        void receiveMessage(string message);
    }
}
