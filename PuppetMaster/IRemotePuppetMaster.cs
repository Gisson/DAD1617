using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuppetMaster {
    interface IRemotePuppetMaster {
        void registerClient(string name, string url, int port);
        void receiveLog(string clientName, string logMessage);
    }
}
