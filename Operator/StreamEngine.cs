﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Operator.StreamInputs;
using Operator.StreamOperators;
using Operator.Routing;

namespace Operator
{
    /// <summary>
    /// Ihis is the "core" class of the project
    /// It gets tuples from StreamInputs, processes them using a StreamOperator
    /// and outputs them using a RoutingPolicy.
    /// </summary>
    class StreamEngine
    {
        private IList<StreamInput> inputs;
        private StreamOperator op;
        private RoutingPolicy route;

        private bool freezed = false;
        private bool started = false;

        private Object intervalLock = new Object();
        private int interval = 100;

        private Thread processingThread;

        public StreamEngine(IList<StreamInput> i, StreamOperator o, RoutingPolicy r)
        {
            inputs = i;
            op = o;
            route = r;
        }

        public int Interval
        {
            get
            {
                int i;
                lock(intervalLock)
                {
                    i = interval;
                }
                return i;
            }
            set
            {
                lock (intervalLock)
                {
                    interval = value;
                }
            }
        }

        public void freeze()
        {
            freezed = true;
        }

        public void unfreeze()
        {
            freezed = false;
        }
         
        public void start()
        {
            if(!started)
            {
                Console.WriteLine("StreamEngine starting...");
                started = true;
                ThreadStart ts = new ThreadStart(this.process);
                processingThread = new Thread(ts);
                processingThread.Start();
                // TODO: check if the thread was actually created
            }
        }
        
        /// <summary>
        /// thread that processes tuples
        /// </summary>
        private void process()
        {
            while (started)
            {
                if (!freezed)
                {
                    // round-robin a tuple from each input
                    foreach (StreamInput i in inputs)
                    {
                        IList<string> tuple = i.getTuple();
                        if (tuple != null)
                        {
                            tuple = op.processTuple(tuple);
                            if (tuple != null)
                            {
                                route.outputTuple(tuple);
                            }else
                            {
                            }
                            Thread.Sleep(Interval);
                        }
                    }
                }else
                {
                    Thread.Sleep(Interval);
                }
            }
        }
    }
}