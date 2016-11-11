using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PCS
{
    class ProcessCreationService : IProcessCreationService {
        IDictionary<String, IList<String>> OperatorTable; /*table with key=op-name|value=all-replica-addresses*/

        [STAThread]
        public static void Main() {
            String arguments = "tcp://1.2.3.8:11000/op";
            Process process = Process.Start("Operator.exe", arguments);
            Console.ReadLine();

        }
    }
}
