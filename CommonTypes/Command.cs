using System;

namespace CommonTypes
{
    [Serializable] /* Marshal by Value; Note: attributes are no inherited */
    public abstract class Command
    {
        private string _name;
        public Command(string name) { _name = name; }
    }

    namespace Commands {
 
        [Serializable]
        public class Start : Command
        {
            private string _operatoId;
            public Start(string opID) : base("Start")
            {
                _operatoId = opID;
            }
        }

        /* TODO add more commands */

    }
}
