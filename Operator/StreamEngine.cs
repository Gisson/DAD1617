using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Operator.StreamInputs;
using Operator.StreamOperators;
using Operator.Routing;
using CommonTypes;


namespace Operator {


    class StreamEngine {
        private IList<StreamInput> inputs;
        private StreamOperator op;
        private RoutingPolicy route;

        //status attributes
        private bool freezed = false;
        private bool started = false;

        private Object intervalLock = new Object();
        private int interval = 100;

        private Thread processingThread;

        public StreamEngine(IList<StreamInput> i, StreamOperator o, RoutingPolicy r) {
            inputs = i;
            op = o;
            route = r;
        }

        public int Interval {
            get {
                int i;
                lock (intervalLock) {
                    i = interval;
                }
                return i;
            }
            set {
                lock (intervalLock) {
                    interval = value;
                }
            }
        }

        public void freeze() {
            freezed = true;
        }

        public void unfreeze() {
            freezed = false;
        }

        public void start() {
            if (!started) {
                Logger.debugWriteLine("StreamEngine starting...");
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
        private void process() {
            while (started) {
                if (!freezed) {
                    // round-robin a tuple from each input
                    foreach (StreamInput i in inputs) {
                        IList<string> inTuple = i.getTuple();
                        if(inTuple != null)
                        {
                            Logger.debugWriteLine("StreamEngine: input[0]: " + inTuple.ElementAt(0));
                            foreach (IList<string> tuple in op.processTuple(inTuple))
                            {
                                Logger.debugWriteLine("StreamEngine: output[0] " + tuple.ElementAt(0));
                                /* TODO log the output tuple to the PM */
                                route.outputTuple(tuple);
                            }
                            Thread.Sleep(Interval);
                        }
                    }
                }
                else {
                    Thread.Sleep(Interval);
                }
            }
        }
    }
}
