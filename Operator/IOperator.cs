﻿using Operator.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operator {
    /// <summary>
    /// Abstraction layer to call internal Operator methods
    /// </summary>
    public interface IOperator {
        /// <summary>
        /// register operatorURL as an input for our output
        /// </summary>
        /// <param name="operatorURL">OP that will receive our tuples</param>
        void registerOutputOperator(string opId, string opURL, int replicaIndex); //keeping track of "child" operators to send tuples
        void receiveTuple(IList<string> tuple); //receive tuple from input operator

        /// <summary> enqueues a command to be run on the command processing thread of the Operator </summary>
        /// <param name="c"></param>
        void enqueue(Command c);
    }
    /// <summary>
    /// Operator methods that can be queued for execution by the Operator via Operator.Commands
    /// </summary>
    public interface ICommandableOperator
    {
        void start();
        void interval(int milliseconds);
        void freeze();
        void unfreeze();
        void crash();
        void status();

        void connectToPuppetMaster();


    }
}
