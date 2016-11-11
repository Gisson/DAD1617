using DADStorm;
using System.Collections.Generic;

namespace LibCustomOperator {
    public class OutputOperator : IOperator {
        public IList<IList<string>> CustomOperation(IList<string> l) {
            string outputFile = @".\output.txt";

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(outputFile, true))
            {
                foreach (string line in l)
                {
                    // If the line doesn't contain the word 'Second', write the line to the file.
                    file.WriteLine(line);
                }
            }
            IList<IList<string>> result = new List<IList<string>>();
            result.Add(l);
            return result;
        }
    }
}
