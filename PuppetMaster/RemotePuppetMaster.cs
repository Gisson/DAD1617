using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTypes;

namespace PuppetMaster
{
    class RemotePuppetMaster : IRemotePuppetMaster
    {
        private SortedDictionary<string,ConnectionArgs> operators= 
            new SortedDictionary<string, ConnectionArgs>();

        public override void registerClient(string name)
        {
            /*TODO*/
            throw new NotImplementedException();
        }

        public void readConfig()
        {
            /*TODO*/
        }
    }
}
