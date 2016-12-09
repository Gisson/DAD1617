using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using CommonTypes;

namespace PuppetMaster {

    /*fancy way to detect bad syntax in the config file*/
    public enum LineSyntax { VALID, INVALID, COMMENT };


    public sealed class Parser {

        private static readonly Parser instance = new Parser();
        static Parser() { }

        private Parser() { }

        //Regex patterns for all commands: i'll clean this up sometime
        private const String RGX_COMMENT = @"^%.*$";
        private const String RGX_START = @"^START \w+$";
        private const String RGX_INTERVAL = @"^INTERVAL \w+ \d+$";
        private const String RGX_STATUS = @"^STATUS$";
        private const String RGX_CRASH = @"^CRASH \w+ \d+$";
        private const String RGX_FREEZE = @"^FREEZE \w+ \d+$";
        private const String RGX_UNFREEZE = @"^UNFREEZE \w+ \d+$";
        private const String RGX_WAIT = @"^WAIT \d+$";
        private const String RGX_LOG = @"^LOGGINGLEVEL (full|light)$";
        private const String RGX_SEMANTICS = @"^SEMANTICS (at-most-once|at-least-once|exactly-once)$";

        private const String RGX_INPUT = @"^\w+ INPUT_OPS (\w|\.)+(,(\w|\.)+)* ";
        private const String RGX_REP = @"REP_FACT \d+ ROUTING (primary|hashing\(\d+\)|random) ";
        /// <summary>
        /// the host token is used by PCSManager to extract the host part
        /// </summary>
        public const String RGX_URI = @"tcp://(?<host>(\w|\.)+):\d{4,5}\w+/\w+";
        private const String RGX_ADDRESS = @"ADDRESS ((" +RGX_URI+ @")(, ?)?)+ ";
        private const String RGX_STRING = @"((\w|\W)*)"; // matches a"www.tecnico.ulisboa.pt"aa, with(out) quotes
        private const String RGX_INT = @"\d+";
        private const String RGX_SPEC_FILTER = @"filter \d+,(>|<|=),((" + RGX_STRING + @")|(" + RGX_INT + @"))";
        private const String RGX_SPEC_CUSTOM = @"custom \w+\.\w+,\w+,\w+";
        private const String RGX_SPEC = @"OPERATOR_SPEC ((uniq \d+)|(count)|(dup)|("+ RGX_SPEC_FILTER + ")|("+ RGX_SPEC_CUSTOM + "))( )*$";
        private const String RGX_CONF = RGX_INPUT + RGX_REP + RGX_ADDRESS + RGX_SPEC;


        //Parsing method
        /* receives a delegate as an argument
         * so the class is allowed to update the visual form
         * while parsing the file
         */
        public static void execute(String pathToFile, Action<String, LineSyntax> PreviewTextBox_Update) {

            StreamReader file = new StreamReader(pathToFile);
            String line;


            //read each line from file
            while ((line = file.ReadLine()) != null)
            {
                PreviewTextBox_Update(line, executeLine(line, true));
            }
            file.Close();
        }

        /// <summary>
        /// parses a single line and queues/executes it
        /// </summary>
        /// <param name="line"></param>
        /// <param name="queue">true if the command is to be enqueue. False mean it should be ran immediately</param>
        /// <returns></returns>
        public static LineSyntax executeLine(string line, bool queue)
        {
            //regex init
            Regex rgxComment = new Regex(RGX_COMMENT, RegexOptions.IgnoreCase);
            Regex rgxStart = new Regex(RGX_START, RegexOptions.IgnoreCase);
            Regex rgxInterval = new Regex(RGX_INTERVAL, RegexOptions.IgnoreCase);
            Regex rgxStatus = new Regex(RGX_STATUS, RegexOptions.IgnoreCase);
            Regex rgxCrash = new Regex(RGX_CRASH, RegexOptions.IgnoreCase);
            Regex rgxFreeze = new Regex(RGX_FREEZE, RegexOptions.IgnoreCase);
            Regex rgxUnfreeze = new Regex(RGX_UNFREEZE, RegexOptions.IgnoreCase);
            Regex rgxWait = new Regex(RGX_WAIT, RegexOptions.IgnoreCase);
            Regex rgxLog = new Regex(RGX_LOG, RegexOptions.IgnoreCase);
            Regex rgxSemantics = new Regex(RGX_SEMANTICS, RegexOptions.IgnoreCase);
            Regex rgxConf = new Regex(RGX_CONF, RegexOptions.IgnoreCase);

            //comment
            if (rgxComment.IsMatch(line))
            {
                Logger.debugWriteLine("Parser: return match COMMENT");
                return LineSyntax.COMMENT;
            }
            //hack just in case the config file has more spaces than it should, such as the sample one
            line = line.Replace(", ", ",");
            String[] fields = line.Split(' ');
            ICommand command = null;
            //start
            if (rgxStart.IsMatch(line))
            {
                String opID = fields[1];
                command = new Start(opID);
            } //interval
            else if (rgxInterval.IsMatch(line))
            {
                String opID = fields[1];
                int millisec = Int32.Parse(fields[2]);
                command = new Interval(opID, millisec);
            } //status
            else if (rgxStatus.IsMatch(line))
            {
                command = new Status();
            } //crash
            else if (rgxCrash.IsMatch(line))
            {
                String opID = fields[1];
                int rep = Int32.Parse(fields[2]);
                command = new Crash(opID, rep);
            } //unfreeze
            else if (rgxUnfreeze.IsMatch(line))
            {
                String opID = fields[1];
                int rep = Int32.Parse(fields[2]);
                command = new Unfreeze(opID, rep);
            } //freeze
            else if (rgxFreeze.IsMatch(line))
            {
                String opID = fields[1];
                int rep = Int32.Parse(fields[2]);
                command = new Freeze(opID, rep);
            } //wait
            else if (rgxWait.IsMatch(line))
            {
                int millisec = Int32.Parse(fields[1]);
                command = new Wait(millisec);
            }
            else if (rgxLog.IsMatch(line))
            {
                String loggingLevel = fields[1];
                command = new Log(loggingLevel);
            }
            else if (rgxSemantics.IsMatch(line))
            {
                String semantics = fields[1];
                command = new SetSemantics(semantics);
            }
            //TODO MISSING: LOG & SEMANTICS

            //configuration of operator
            else if (rgxConf.IsMatch(line))
            {
                String opID = fields[0];
                String[] inputOps = fields[2].Split(',');
                int repFact = Int32.Parse(fields[4]);
                String routing = fields[6];
                String[] addresses = fields[8].Split(',');
                String[] opSpec = fields.Skip(10).ToArray();
                command = new ConfigureOperator(opID, inputOps, repFact, routing, addresses, opSpec);
            } //invalid command
            else
            {
                Logger.debugWriteLine("Parser: return match INVALID");
                return LineSyntax.INVALID;
            }
            //store commands
            if (command != null) {
                if(queue)
                {
                    Logger.debugWriteLine("Parser: Enqueue");
                    PuppetMaster.Commands.Enqueue(command);
                }
                else
                {
                    Logger.debugWriteLine("Parser: execute");
                    command.execute();
                }
            } // FIXME
            else
            {
                Logger.debugWriteLine("Parser: return match INVALID");
                return LineSyntax.INVALID;
            }
            Logger.debugWriteLine("Parser: return match VALID");
            return LineSyntax.VALID;
            
        }
    }

}

