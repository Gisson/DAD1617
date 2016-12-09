using CommonTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Operator.StreamInputs
{
    /// <summary>
    /// InputStream that gets tuples from a given file
    /// </summary>
    class File : StreamInput
    {
        private const String RGX_COMMENT = @"^%.*$";
        private const String RGX_STRING = @"((\w|\W)*)"; // matches a"www.tecnico.ulisboa.pt"aa, with(out) quotes
        private const String RGX_FIELD_SEPARATOR = @", *";
        private const String RGX_TUPLE = @"(?<field>" + RGX_STRING + ")("+ RGX_FIELD_SEPARATOR + "(?<field>" + RGX_STRING + "))*";

        Regex rgxComment = new Regex(RGX_COMMENT);
        Regex rgxtuple = new Regex(RGX_TUPLE);

        /// <summary>
        /// parse a lineList<string> from an input file (.dat)
        /// </summary>
        /// <param name="line">the line to parse</param>
        /// <returns>a list of tuple fields or null if the line is empty/comment/invalid</returns>
        private List<string> parse(string line)
        {
            if (!rgxComment.IsMatch(line))
            {
                String[] fields = Regex.Split(line, RGX_FIELD_SEPARATOR);
                if (fields.Length > 0)
                {
                    List<string> l = new List<string>();
                    foreach(string f in fields)
                    {
                        l.Add(removeQuotes(f));
                    }
                    return l;
                }
            }
            return null;
        }


        string path;
        StreamReader file = null;
        private Boolean _isOpen = false;
        public Boolean isOpen
        {
            get
            {
                return _isOpen;
            }
            private set
            {
                _isOpen = value;
            }
        }
        public File(string path)
        {
            this.path = path;
            try
            {
                file = new StreamReader(path);
                _isOpen = true;
                // FIXME TESTING \/
                Console.WriteLine(string.Join("|", parse("a, b, sdfsdfsdf,2342343")));
            } catch (System.IO.IOException)
            {
                Logger.errorWriteLine("Could not open input file " + path);
                file = null;
                _isOpen = false;
            }
        }

        public IList<string> getTuple()
        {
            if(file != null)
            {
                List<string> tuple = null;
                while (file.Peek() != -1 && tuple == null)
                {
                    string line = file.ReadLine();

                    tuple = parse(line);
                }
                return tuple;
            }
            return null;
        }




        ~File()
        {
            if(file != null)
                file.Close();
        }

        public static string removeQuotes(string s)
        {
            if (s.StartsWith("\"") && s.EndsWith("\""))
            {
                Regex rgx = new Regex("^\"");
                s = rgx.Replace(s, "");
                rgx = new Regex("\"$");
                s = rgx.Replace(s, "");
                return s;
            }
            else
            {
                return s;
            }
        }
    }
}
