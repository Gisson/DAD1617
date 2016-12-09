using CommonTypes.RemoteInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommonTypes;

namespace PuppetMaster
{
    public class PCSManager
    {
        private const int PCS_PORT = 10001;
        private const String PCS_SERVICE_NAME = "pcs";


        private const String RGX_URI = Parser.RGX_URI;
        /// <summary>
        /// (replicaIP, PCS)
        /// </summary>
        private IDictionary<String, IProcessCreationService> services;
        private Regex rgxURI;

        public PCSManager()
        {
            services = new Dictionary<String, IProcessCreationService>();
            rgxURI = new Regex(RGX_URI, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="URI">e.g. "tcp://1.2.3.6:11000/op"</param>
        /// <returns></returns>
        public IProcessCreationService getPCSbyReplicaURI(string URI)
        {
            Match match = rgxURI.Match(URI);
            if (match.Success)
            {
                string host = match.Groups["host"].Value;
                Logger.debugWriteLine("PCSManager: extracted host " + host + " from URI " + URI);

                IProcessCreationService PCS;
                if (services.TryGetValue(host, out PCS))
                {
                    Logger.debugWriteLine("PCSManager: reusing proxy for " + host);
                    return PCS;
                } else {
                    string PCS_URL = "tcp://" + host + ":" + PCS_PORT + "/" + PCS_SERVICE_NAME;

                    Logger.debugWriteLine("PCSManager: get PCS using URI " + PCS_URL);

                    // get PCS proxy
                    PCS = (IProcessCreationService)Activator.GetObject(
                            typeof(IProcessCreationService),
                            PCS_URL);

                    services.Add(host, PCS);
                    return PCS;
                }
            } else
            {
                Logger.errorWriteLine("PCSManager: not an URI: " + URI);
                return null;
            }
        }
    }
}
