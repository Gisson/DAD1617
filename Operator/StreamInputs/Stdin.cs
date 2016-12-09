using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTypes;

namespace Operator.StreamInputs
{
    class Stdin : StreamInput
    {
        public IList<string> getTuple()
        {
            string line = Console.ReadLine();
            Logger.debugWriteLine("StreamInput.Stdin: read "+ line);

            string[] words = line.Split(' ');
            List<string> tuple = new List<string>(words);
            return tuple;
        }
    }
}
