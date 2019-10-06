using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            OpenDM.Gpgpu.State.Initialize();

            int batch = 5;
            int iw = 2, ow1 = 1000, ow2 = 2;

            var g1 = new Affine(iw, ow1).Initialize(Activator.Confirm(ActivationType.LReLU), Optimizer.Confirm(OptimizationType.Adam));
            var g2 = new Affine(ow1, ow2).Initialize(Activator.Confirm(ActivationType.LReLU), Optimizer.Confirm(OptimizationType.Adam));

            var t = new R1dArray(ow2, batch);
            t[0, 0] = 1;
            t[0, 2] = 1;
            t[0, 4] = 1;

            int gen = 0;
            while (true)
            {
                var input = new R1dArray(iw, batch);
                input[0, 1] = 1;
                input[1, 2] = 1;
                input[0, 3] = 1;
                input[1, 3] = 1;
                input[1, 4] = 2;
                input.Shuffle(0.25);
                R1dArray o, e, p;

                e = (o = (R1dArray)g2.Forward(g1.Forward(input))) - t;
                p = (R1dArray)g1.Back(g2.Back(e));

                string str = string.Format("gen : {0}, {1}\ni -> {2}\nt -> {3}\no -> {4}", ++gen,
                    Math.Round(e.Power, 4), input.ToString(2), t.ToString(2), o.ToString(2));
                Console.WriteLine(str);

                System.Threading.Thread.Sleep(1);
                GC.Collect();
            }
        }
    }
}
