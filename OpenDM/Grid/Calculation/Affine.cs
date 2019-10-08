using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDM.Grid.Calculation
{
    class Affine
    {
        Gpgpu.ProgramOption Option_F_01 { get; set; }
        Gpgpu.ProgramOption Option_B_01 { get; set; }
        Gpgpu.ProgramOption Option_B_02 { get; set; }
        Gpgpu.ProgramOption Option_B_03 { get; set; }

        RNdArray dw;

        public Affine()
        {
            Option_F_01 = new Gpgpu.ProgramOption(typeof(Gpgpu.Source.Affine_Forward_01).Name);
            Option_B_01 = new Gpgpu.ProgramOption(typeof(Gpgpu.Source.Affine_Back_01).Name);
            Option_B_02 = new Gpgpu.ProgramOption(typeof(Gpgpu.Source.Affine_Back_02).Name);
            Option_B_03 = new Gpgpu.ProgramOption(typeof(Gpgpu.Source.Affine_Back_03).Name);
        }

        public void Forwerd(R1dArray input, R2dArray w, out R1dArray u, out R1dArray o, Activator act)
        {
            u = new R1dArray(w.Height, input.Batch);
            o = new R1dArray(w.Height, input.Batch);

            var ipt = input << 1;
            var _u = u;
            var _o = (RNdArray)o;

            if (Option_F_01 != null)
            {
                Option_F_01.Startup();
                using (Cloo.ComputeBuffer<float> __ipt = Option_F_01.ConvertBuffer(Cloo.ComputeMemoryFlags.ReadOnly, ipt.Data))
                using (Cloo.ComputeBuffer<float> __w = Option_F_01.ConvertBuffer(Cloo.ComputeMemoryFlags.ReadOnly, w.Data))
                using (Cloo.ComputeBuffer<float> __u = Option_F_01.ConvertBuffer(Cloo.ComputeMemoryFlags.WriteOnly, u.Data))
                {
                    Option_F_01.SetParameter(__ipt);
                    Option_F_01.SetParameter(__u);
                    Option_F_01.SetParameter(__w);

                    Option_F_01.SetParameter(w.Width, Gpgpu.ProgramOption.ValueMode.INT);
                    Option_F_01.SetParameter(w.Height, Gpgpu.ProgramOption.ValueMode.INT);

                    Option_F_01.Execute(ipt.Batch, w.Height);
                    Option_F_01.ReadBuffer(__u, ref u.Data);
                }
            }
            else
            {
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
            }
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

        public double Back(R1dArray sigma, R1dArray input, R1dArray u, ref R2dArray w, out R1dArray p, Activator act)
        {
            p = new R1dArray(input.Width, input.Batch);

            var ipt = input << 1;

            var du = (RNdArray)(new R1dArray(u.Width, u.Batch));
            var s = new R1dArray(sigma.Width, sigma.Batch);
            var _p = p << 1;
            var _w = w;

            if (act != null)
            {
                act.DeActivation(u, ref du);
                u = (R1dArray)du;
            }
            else
            {
                u.Fill(1);
            }
            if (Option_B_01 != null)
            {
                Option_B_01.Startup();
                using (Cloo.ComputeBuffer<float> __sigma = Option_B_01.ConvertBuffer(Cloo.ComputeMemoryFlags.ReadOnly, sigma.Data))
                using (Cloo.ComputeBuffer<float> __u = Option_B_01.ConvertBuffer(Cloo.ComputeMemoryFlags.ReadOnly, u.Data))
                using (Cloo.ComputeBuffer<float> __s = Option_B_01.ConvertBuffer(Cloo.ComputeMemoryFlags.WriteOnly, s.Data))
                {
                    Option_B_01.SetParameter(__sigma);
                    Option_B_01.SetParameter(__u);
                    Option_B_01.SetParameter(__s);

                    Option_B_01.SetParameter(sigma.Width, Gpgpu.ProgramOption.ValueMode.INT);

                    Option_B_01.Execute(sigma.Batch, sigma.Width);
                    Option_B_01.ReadBuffer(__s, ref s.Data);
                }
            }
            else
            {
                Parallel.For(0, sigma.Batch, b =>
                {
                    Parallel.For(0, sigma.Width, i =>
                    {
                        s[i, b] = sigma[i, b] * u[i, b];
                    });
                });
            }

            if (dw == null)
            {
                dw = w.Clone();
                dw.Fill(0);
            }
            var _dw = (R2dArray)dw;
            if (Option_B_02 != null)
            {
                Option_B_02.Startup();
                using (Cloo.ComputeBuffer<float> __ipt = Option_B_02.ConvertBuffer(Cloo.ComputeMemoryFlags.ReadOnly, ipt.Data))
                using (Cloo.ComputeBuffer<float> __s = Option_B_02.ConvertBuffer(Cloo.ComputeMemoryFlags.ReadOnly, s.Data))
                using (Cloo.ComputeBuffer<float> __dw = Option_B_02.ConvertBuffer(Cloo.ComputeMemoryFlags.WriteOnly, _dw.Data))
                {
                    Option_B_02.SetParameter(__ipt);
                    Option_B_02.SetParameter(__s);
                    Option_B_02.SetParameter(__dw);

                    Option_B_02.SetParameter(dw.Width, Gpgpu.ProgramOption.ValueMode.INT);
                    Option_B_02.SetParameter(dw.Height, Gpgpu.ProgramOption.ValueMode.INT);

                    Option_B_02.SetParameter(sigma.Batch, Gpgpu.ProgramOption.ValueMode.INT);

                    Option_B_02.Execute(dw.Width, dw.Height);
                    Option_B_02.ReadBuffer(__dw, ref _dw.Data);
                }
            }
            else
            {
                Parallel.For(0, dw.Width, i =>
                {
                    Parallel.For(0, dw.Height, j =>
                    {
                        for (int b = 0; b < s.Batch; b++)
                        {
                            _dw[i, j] += s[j, b] * ipt[i, b];
                        }
                    });
                });
            }

            if (Option_B_03 != null)
            {
                Option_B_03.Startup();
                using (Cloo.ComputeBuffer<float> __s = Option_B_03.ConvertBuffer(Cloo.ComputeMemoryFlags.ReadOnly, s.Data))
                using (Cloo.ComputeBuffer<float> __w = Option_B_03.ConvertBuffer(Cloo.ComputeMemoryFlags.ReadOnly, _w.Data))
                using (Cloo.ComputeBuffer<float> __p = Option_B_03.ConvertBuffer(Cloo.ComputeMemoryFlags.WriteOnly, _p.Data))
                {
                    Option_B_03.SetParameter(__s);
                    Option_B_03.SetParameter(__w);
                    Option_B_03.SetParameter(__p);

                    Option_B_03.SetParameter(w.Width, Gpgpu.ProgramOption.ValueMode.INT);
                    Option_B_03.SetParameter(w.Height, Gpgpu.ProgramOption.ValueMode.INT);

                    Option_B_03.Execute(dw.Width, _p.Batch);
                    Option_B_03.ReadBuffer(__p, ref _p.Data);
                }
            }
            else
            {
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
            }
            p = _p >> 1;

            return sigma.Power;
        }

        public void Update(ref RNdArray w, float rho, Optimizer opt)
        {
            var w__w = (RNdArray)w;
            opt.Update(dw, ref w__w, rho);
        }
    }
}