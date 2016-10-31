using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    public class AlreadyRegisteredException : Exception
    {
        public AlreadyRegisteredException() : base("Client already registered")
        {

        }


    }

    public class UnaccessibleClientException : Exception
    {
        public UnaccessibleClientException(string clientName) : base("Client " + clientName + " is unaccessible!")
        {

        }
    }
}
