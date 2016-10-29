﻿using System;
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

    }

    public abstract class IRemoteOperator: MarshalByRefObject
    {
        //Receives a signal from the PuppetMaster
        public abstract void receiveSignal();

    }

    public abstract class IRemotePuppetMaster : MarshalByRefObject
    {
        public abstract void registerClient(string name);
    }

    public abstract class IRemoteProcessCreationService : MarshalByRefObject
    {
        public abstract void startOperator(/* TODO add args */);
    }

}
