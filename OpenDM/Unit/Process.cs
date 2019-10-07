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

            public double Error { get; set; }
            public double EpochError { get; set; }
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

        public Segment Units { get; set; }
        private Store.SourceStore store { get; set; } = new Store.SourceStore();

        public double InputNoize { get; set; } = 0;
        public int BatchSize { get; set; } = 10;

        private double errorstack = 0, count = 0, rho = 0.01;

        private bool abort { get; set; } = false;
        private bool running { get; set; } = false;

        public Process()
        {
            Units = new Segment();
        }

        public void AddDataStore(Store.Item.SourceItem item)
        {
            store.Add(item);
        }

        public void Start()
        {
            if (!running)
            {
                new Task(() =>
                {
                    running = true;
                    while (!abort)
                    {
                        if (store.Count == 0) { continue; }
                        RNdArray i, t, o, p;
                        var bitem = store.CreateBatch(BatchSize);
                        i = bitem.Input.Shuffle(InputNoize);
                        t = bitem.Teacher;

                        var error = Units.Learn(i, t, out o, out p, rho);
                        _generateUpdate?.Invoke(new UpdateInstanceArgs()
                        {
                            Generation = Units.Generation,
                            Error = error,
                            Input = i,
                            Output = o,
                            Teacher = t,
                            Propagator = p,
                        });

                        errorstack += error;
                        count++;

                        if (bitem.EpochIteration)
                        {
                            errorstack /= count;
                            _epochUpdate?.Invoke(new UpdateInstanceArgs()
                            {
                                Epoch = store.Generation,
                                EpochError = errorstack,
                            });
                            rho = errorstack;
                            errorstack = count = 0;
                        }
                        GC.Collect();
                    }
                    running = false;
                }).Start();
            }
        }

        public void Abort()
        {
            abort = true;
        }
    }
}
