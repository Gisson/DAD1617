using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    public class ConnectionArgs : MarshalByRefObject
    {
        public string url { get; set; }
        public int port { get; set; }
        public ConnectionArgs(string url, int port)
        {
            this.url = url;
            this.port = port;
        }

    }
}
