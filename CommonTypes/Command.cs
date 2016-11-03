using System;

namespace CommonTypes
{

    public interface IRemoteCommands
    {
        void start(String operatorID);
        void interval(String operatorID, int milliseconds);
        void status();
        void crash(String operatorID, int replicaID);
        void freeze(String operatorID, int replicaID);
        void unfreeze(String operatorID, int replicaID);
        /* the wait command is not remote */
    }

}
