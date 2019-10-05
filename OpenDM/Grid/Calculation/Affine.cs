using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDM.Calculation
{
    public static class Affine
    {
        public static void Forwerd(R1dArray input, R2dArray w, out R1dArray u, out R1dArray o, Activator act)
        {
            u = new R1dArray(w.Height, input.Batch);
            o = new R1dArray(w.Height, input.Batch);

            var ipt = input << 1;
            var _u = u;
            var _o = (RNdArray)o;

            Parallel.For(0, ipt.Batch, b =>
            {
                Parallel.For(0, w.Height, j =>
                {
                    for (int i = 0; i < w.Width; i++)
                    {
                        _u[j, b] += w[i, j] * ipt[i, b];
                    }
                });
            });

            if (act != null)
            {
                act.Activation(u, ref _o);
            }
            else
            {
                Parallel.For(0, u.TotalLength, i =>
                {
                    _o.Data[i] = _u.Data[i];
                });
            }
        }

        public static double Back(R1dArray sigma, R1dArray input, R1dArray u, ref R2dArray w, out R1dArray p, Activator act, Optimizer opt = null)
        {
            p = new R1dArray(input.Width, input.Batch);

            var ipt = input << 1;

            var du = (RNdArray)(new R1dArray(u.Width, u.Batch));
            var s = new R1dArray(sigma.Width, sigma.Batch);
            var _p = p << 1;

            if (act != null)
            {
                act.DeActivation(u, ref du);
                u = (R1dArray)du;
            }
            else
            {
                u.Fill(1);
            }
            Parallel.For(0, sigma.Batch, b =>
            {
                Parallel.For(0, sigma.Width, i =>
                {
                    s[i, b] = sigma[i, b] * u[i, b];
                });
            });

            var dw = new R2dArray(w.Width, w.Height);
            Parallel.For(0, dw.Width, i =>
            {
                Parallel.For(0, dw.Height, j =>
                {
                    for (int b = 0; b < s.Batch; b++)
                    {
                        dw[i, j] += s[j, b] * ipt[i, b];
                    }
                });
            });

            var _w = w;
            Parallel.For(0, w.Width, i =>
            {
                Parallel.For(0, _p.Batch, b =>
                {
                    for (int j = 0; j < _w.Height; j++)
                    {
                        _p[i, b] += _w[i, j] * s[j, b];
                    }
                });
            });
            p = _p >> 1;

            var __w = (RNdArray)w;
            opt.Update(dw, ref __w, sigma.Power);
            return sigma.Power;
        }
    }
}