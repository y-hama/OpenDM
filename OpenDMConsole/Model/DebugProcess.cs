using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMConsole.Model
{


    class DebugProcess
    {


        public DebugProcess(OpenDM.Unit.Process.UpdateInstance genfunc, OpenDM.Unit.Process.UpdateInstance epochfunc)
        {
            OpenDM.Gpgpu.State.Initialize();

            int batch = 5;
            int iw = 2, ow = 3;
            double noize = 0.01;
            int[] nodes = new int[] { iw, 128, 128, 64, 64, 32, 16, 16, ow };

            OpenDM.Unit.Process process = new OpenDM.Unit.Process();
            process.GenerateUpdate += genfunc;
            process.EpochUpdate += epochfunc;

            process.BatchSize = batch;
            process.InputNoize = noize;

            for (int n = 0; n < nodes.Length - 2; n++)
            {
                process.Segment.AddGrid(new OpenDM.Grid.Affine(nodes[n], nodes[n + 1], Activator.Confirm(ActivationType.LReLU), Optimizer.Confirm(OptimizationType.Adam, -1, 0.5)).Initialize());
            }
            process.Segment.AddGrid(new OpenDM.Grid.Affine(nodes[nodes.Length - 2], nodes[nodes.Length - 1], Activator.Confirm(ActivationType.LReLU), Optimizer.Confirm(OptimizationType.Adam, -1, 0.5)).Initialize());

            var random = new Random();
            for (int i = 0; i < 100; i++)
            {
                double x = (random.NextDouble() * 2 - 1), y = (random.NextDouble() * 2 - 1);
                double r, g, b;
                r = x > 0 ? 1 : 0; g = y > 0 ? 1 : 0; b = x * x + y * y < 0.5 * 0.5 ? 1 : 0;

                process.AddDataStore(new OpenDM.Store.Item.SourceItem(new R1dArray(iw, 1, x, y), new R1dArray(ow, 1, r, g, b)));
            }

            process.Start();
        }
    }
}
