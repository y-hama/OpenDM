using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDM.Unit
{
    public class Process
    {
        public class UpdateInstanceArgs
        {
            public int Epoch { get; set; }
            public int Generation { get; set; }
            public RNdArray Input { get; set; }
            public RNdArray Output { get; set; }
            public RNdArray Teacher { get; set; }
            public RNdArray Propagator { get; set; }

            public double ProcessTime { get; set; }
            public double LearnTime { get; set; }
            public double UpdateTime { get; set; }

            public double Error { get; set; }
            public double EpochError { get; set; }
            public double EpochTime { get; set; }
        }
        public delegate void UpdateInstance(UpdateInstanceArgs e);

        public UpdateInstance _generateUpdate;
        public event UpdateInstance GenerateUpdate
        {
            add { _generateUpdate += value; }
            remove { _generateUpdate -= value; }
        }
        public UpdateInstance _epochUpdate;
        public event UpdateInstance EpochUpdate
        {
            add { _epochUpdate += value; }
            remove { _epochUpdate -= value; }
        }

        public Segment Units { get; set; } = new Segment();
        public Store.SourceStore Store { get; set; } = new Store.SourceStore();

        public double InputNoize { get; set; } = 0;
        public int BatchSize { get; set; } = 10;

        private double errorstack = 0, count = 0, rho = 0.01;

        private bool abort { get; set; } = false;
        private bool running { get; set; } = false;

        public Process()
        {
        }

        public void Start()
        {
            if (!running)
            {
                new Task(() =>
                {
                    double ttime = 0;
                    running = true;
                    abort = false;
                    while (!abort)
                    {
                        double ltime, utime;
                        DateTime start;

                        if (Store.Count == 0) { continue; }
                        RNdArray i, t, o, p;
                        var bitem = Store.CreateBatch(BatchSize);
                        i = bitem.Input.Shuffle(InputNoize);
                        t = bitem.Teacher;

                        start = DateTime.Now;
                        var error = Units.Learn(i, t, out o, out p, rho);
                        ltime = (DateTime.Now - start).TotalMilliseconds;
                        start = DateTime.Now;
                        Units.Update((float)error);
                        utime = (DateTime.Now - start).TotalMilliseconds;
                        _generateUpdate?.Invoke(new UpdateInstanceArgs()
                        {
                            Generation = Units.Generation,
                            Error = error,
                            Input = i,
                            Output = o,
                            Teacher = t,
                            Propagator = p,
                            LearnTime = ltime,
                            UpdateTime = utime,
                            ProcessTime = ltime + utime,
                        });

                        ttime += (ltime + utime);
                        errorstack += error;
                        count++;

                        if (bitem.EpochIteration)
                        {
                            errorstack /= count;
                            _epochUpdate?.Invoke(new UpdateInstanceArgs()
                            {
                                Epoch = Store.Generation,
                                EpochError = errorstack,
                                EpochTime = ttime,
                            });
                            rho = errorstack;
                            errorstack = count = ttime = 0;
                            GC.Collect();
                        }
                    }
                    running = false;
                }).Start();
            }
        }

        public void Abort()
        {
            abort = true;
            while (running)
            {
                System.Threading.Thread.Sleep(1);
            }
            Console.WriteLine("abort end");
        }
    }
}
