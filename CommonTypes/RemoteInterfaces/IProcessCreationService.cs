using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes.RemoteInterfaces {
    public interface IProcessCreationService {
        void createOperator(String args);
        void ping();
    }
}
