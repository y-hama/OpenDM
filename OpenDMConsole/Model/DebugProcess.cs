using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMConsole.Model
{


    class DebugProcess
    {
        int batch = 20;
        int iw = 2, ow = 3;
        double noize = 0.1;

        Random random = new Random();

        OpenDM.Unit.Process process { get; set; }

        public DebugProcess(OpenDM.Unit.Process.UpdateInstance genfunc, OpenDM.Unit.Process.UpdateInstance epochfunc)
        {
            OpenDM.Gpgpu.State.Initialize();

            int[] nodes = new int[] { iw, 800, 500, ow };

            process = new OpenDM.Unit.Process();
            process.GenerateUpdate += genfunc;
            process.EpochUpdate += epochfunc;

            process.BatchSize = batch;
            process.InputNoize = noize;

            for (int n = 0; n < nodes.Length - 2; n++)
            {
                process.Units.AddGrid(new OpenDM.Grid.Affine(nodes[n], nodes[n + 1], Activator.Confirm(ActivationType.LReLU), Optimizer.Confirm(OptimizationType.Adam, -1, 0.5)).Initialize());
            }
            process.Units.AddGrid(new OpenDM.Grid.Affine(nodes[nodes.Length - 2], nodes[nodes.Length - 1], Activator.Confirm(ActivationType.LReLU), Optimizer.Confirm(OptimizationType.Adam, -1, 0.5)).Initialize());

            for (int i = 0; i < 100; i++)
            {
                double x = (random.NextDouble() * 2 - 1), y = (random.NextDouble() * 2 - 1);
                double r, g, b;
                r = x > 0 ? 1 : 0; g = y > 0 ? 1 : 0; b = x * x + y * y < 0.5 * 0.5 ? 1 : 0;

                process.Store.Add(new OpenDM.Store.Item.SourceItem(new R1dArray(iw, 1, x, y), new R1dArray(ow, 1, r, g, b)));
            }

            process.Start();
        }

        public void AddData()
        {
            process.Abort();
            for (int i = 0; i < 100; i++)
            {
                double x = (random.NextDouble() * 2 - 1), y = (random.NextDouble() * 2 - 1);
                double r, g, b;
                r = y > 0 ? 1 : 0; g = x > 0 ? 1 : 0; b = x * y < 0.5 * 0.5 ? 1 : 0;

                process.Store.Add(new OpenDM.Store.Item.SourceItem(new R1dArray(iw, 1, x, y), new R1dArray(ow, 1, r, g, b)));
            }
            process.Start();
        }
    }
}
