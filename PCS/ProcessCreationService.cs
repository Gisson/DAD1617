using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using CommonTypes.RemoteInterfaces;

namespace PCS {
    class ProcessCreationService : MarshalByRefObject, IProcessCreationService {
        private const String OPERATOR = "Operator.exe";
        
        //op main syntax: OpID opURL replicaIndex inputFiles inputOpURLs Routing OpSpec OpParams;
        public void createOperator(String args) {
            System.Console.WriteLine(args);
            Process process = Process.Start(OPERATOR, args);
        }
    }
}
