using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    public static class Logger
    {
        public static bool debug = false;
        private static readonly object ConsoleWriterLock = new object();

        public static void debugWriteLine(string line)
        {
            if (debug)
            {
                lock (ConsoleWriterLock)
                {
                    ConsoleColor old = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("debug: " + line);
                    Console.ForegroundColor = old;
                }
            }
        }

        public static void errorWriteLine(string line)
        {
            lock (ConsoleWriterLock)
            {
                ConsoleColor old = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("error: " + line);
                Console.ForegroundColor = old;
            }
        }
        
        public static void errorWriteLine()
        {
            errorWriteLine("");
        }

        public static void debugWriteLine()
        {
            debugWriteLine("");
        }
    }
}
