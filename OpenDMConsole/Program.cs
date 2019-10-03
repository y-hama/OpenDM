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
            int iw = 10, ow1 = 1000, ow2 = 2;
            var input = new R1dArray(iw, batch);
            input[0, 1] = 1;
            input[1, 2] = 1;
            input[0, 3] = 1;
            input[1, 3] = 1;
            input[1, 4] = 2;

            var w1 = new R2dArray(iw + 1, ow1);
            w1.Shuffle();
            var w2 = new R2dArray(ow1 + 1, ow2);
            w2.Shuffle();

            Activator act1 = Activator.Confirm(ActivationType.ELU, 0.1);
            Optimizer opt1 = Optimizer.Confirm(OptimizationType.Adam, 0.01);
            Activator act2 = Activator.Confirm(ActivationType.ReLU, 0.01);
            Optimizer opt2 = Optimizer.Confirm(OptimizationType.Adam, 0.01);

            var t = new R1dArray(ow2, batch);
            t[0, 0] = 1;
            t[0, 2] = 1;
            t[0, 4] = 2;

            int gen = 0;
            while (true)
            {
                R1dArray u1, o1, p1, u2, o2, p2, e;

                Affine.Forwerd(input, w1, out u1, out o1, act1);
                Affine.Forwerd(o1, w2, out u2, out o2, act2);
                e = o2 - t;

                Affine.Back(e, o1, u2, ref w2, out p2, act2, opt2);
                Affine.Back(p2, input, u1, ref w1, out p1, act1, opt1);

                string str = string.Format("gen : {0}, {1}\nt -> {2}\no -> {3}", ++gen, e.Power, t.ToString(3), o2.ToString(3));
                Console.WriteLine(str);

                System.Threading.Thread.Sleep(1);
            }

        }
    }
}
