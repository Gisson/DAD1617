using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operator.StreamInputs
{
    class Stdin : StreamInput
    {
        public IList<string> getTuple()
        {
            string line = Console.ReadLine();
            string[] words = line.Split(' ');
            List<string> tuple = new List<string>(words);
            return tuple;
        }
    }
}
