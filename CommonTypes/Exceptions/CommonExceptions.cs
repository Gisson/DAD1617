using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes.Exceptions {
    [Serializable]
    public class AlreadyRegisteredException : Exception {
        public AlreadyRegisteredException()
            : base("Client already registered") {

        }
    }
}