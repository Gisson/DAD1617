using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace Operator.StreamOperators {
    class Custom : StreamOperator {
        private String Dll; //= "..\\..\\..\\InputFiles\\LibOperator\\bin\\Debug\\LibOperator.dll";
        private String ClassName; //= "OutputOperator";
        private String Method; //= "CustomOperation";

        public Custom(String dll, String className, String method) {
            Dll = dll;
            ClassName = className;
            Method = method;
        }

        /*does not work in this project*/

        public IList<IList<string>> processTuple(IList<string> inputTuple) {
            IList<IList<string>> outputTuples = new List<IList<string>>();
            byte[] code = File.ReadAllBytes(Dll);
            Assembly assembly = Assembly.Load(code);
            // Walk through each type in the assembly looking for our class
            foreach (Type type in assembly.GetTypes()) { //not a valid call, aparently
                if (type.IsClass == true && type.FullName.EndsWith("." + ClassName)) {
                    // Dynamically Invoke the method
                    outputTuples = (IList<IList<string>>)type.InvokeMember(Method,
                      BindingFlags.Default | BindingFlags.InvokeMethod,
                           null,
                           Activator.CreateInstance(type),
                           new object[] { inputTuple });
                }
            }
            return outputTuples;
        }
    }
}
