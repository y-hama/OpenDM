using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum ActivationType
{
    LReLU,
    ELU,
    Sigmoid,
}
public class Activator
{
    #region
    public static Activator Confirm(ActivationType type, params object[] param)
    {
        return new Activator(type, param);
    }

    private enum Direction
    {
        Activation,
        Deactivation,
    }

    private delegate void ActivationFunction(RNdArray u, ref RNdArray v, Direction dir, params object[] param);
    private ActivationFunction Function { get; set; }

    private OpenDM.Gpgpu.ProgramOption Program01 { get; set; }
    private OpenDM.Gpgpu.ProgramOption Program02 { get; set; }

    private ActivationType Type { get; set; }
    private object[] Parameter { get; set; }

    private Activator(ActivationType type, params object[] param)
    {
        Type = type;
        Parameter = (param != null) ? param : new object[] { };
        Function = SelectFunction(type);
    }

    public void Activation(RNdArray u, ref RNdArray v)
    {
        Function(u, ref v, Direction.Activation, Parameter);
    }

    public void DeActivation(RNdArray u, ref RNdArray v)
    {
        Function(u, ref v, Direction.Deactivation, Parameter);
    }
    #endregion

    private ActivationFunction SelectFunction(ActivationType type)
    {
        switch (type)
        {
            case ActivationType.LReLU:
                Program01 = new OpenDM.Gpgpu.ProgramOption(typeof(OpenDM.Gpgpu.Source.Activation_LReLU_01).Name);
                Program02 = new OpenDM.Gpgpu.ProgramOption(typeof(OpenDM.Gpgpu.Source.Activation_LReLU_02).Name);
                return LReLU;
            case ActivationType.ELU:
                return ELU;
            case ActivationType.Sigmoid:
                Program01 = new OpenDM.Gpgpu.ProgramOption(typeof(OpenDM.Gpgpu.Source.Activation_Sigmoid_01).Name);
                Program02 = new OpenDM.Gpgpu.ProgramOption(typeof(OpenDM.Gpgpu.Source.Activation_Sigmoid_02).Name);
                return Sigmoid;
            default:
                return null;
        }
    }

    private void LReLU(RNdArray u, ref RNdArray v, Direction dir, params object[] param)
    {
        float alpha = param.Length > 0 ? Convert.ToSingle(param[0]) : 0.01f;
        var vt = v;
        switch (dir)
        {
            case Direction.Activation:
                if (Program01 == null)
                {
                    Parallel.For(0, u.TotalLength, i =>
                    {
                        vt.Data[i] = u.Data[i] > 0 ? u.Data[i] : u.Data[i] * alpha;
                    });
                }
                else
                {
                    Program01.Startup();
                    using (Cloo.ComputeBuffer<float> __u = Program01.ConvertBuffer(Cloo.ComputeMemoryFlags.ReadOnly, u.Data))
                    using (Cloo.ComputeBuffer<float> __vt = Program01.ConvertBuffer(Cloo.ComputeMemoryFlags.WriteOnly, vt.Data))
                    {
                        Program01.SetParameter(__u);
                        Program01.SetParameter(__vt);

                        Program01.SetParameter(u.Batch, OpenDM.Gpgpu.ProgramOption.ValueMode.INT);
                        Program01.SetParameter(u.Width, OpenDM.Gpgpu.ProgramOption.ValueMode.INT);

                        Program01.SetParameter(alpha, OpenDM.Gpgpu.ProgramOption.ValueMode.FLOAT);

                        Program01.Execute(u.Batch, u.Width);
                        Program01.ReadBuffer(__vt, ref vt.Data);
                    }
                }
                break;
            case Direction.Deactivation:
                if (Program02 == null)
                {
                    Parallel.For(0, u.TotalLength, i =>
                    {
                        vt.Data[i] = u.Data[i] > 0 ? 1 : alpha;
                    });
                }
                else
                {
                    Program02.Startup();
                    using (Cloo.ComputeBuffer<float> __u = Program02.ConvertBuffer(Cloo.ComputeMemoryFlags.ReadOnly, u.Data))
                    using (Cloo.ComputeBuffer<float> __vt = Program02.ConvertBuffer(Cloo.ComputeMemoryFlags.WriteOnly, vt.Data))
                    {
                        Program02.SetParameter(__u);
                        Program02.SetParameter(__vt);

                        Program02.SetParameter(u.Batch, OpenDM.Gpgpu.ProgramOption.ValueMode.INT);
                        Program02.SetParameter(u.Width, OpenDM.Gpgpu.ProgramOption.ValueMode.INT);

                        Program02.SetParameter(alpha, OpenDM.Gpgpu.ProgramOption.ValueMode.FLOAT);

                        Program02.Execute(u.Batch, u.Width);
                        Program02.ReadBuffer(__vt, ref vt.Data);
                    }
                }
                break;
            default:
                break;
        }
    }
    private void Sigmoid(RNdArray u, ref RNdArray v, Direction dir, params object[] param)
    {
        float alpha = param.Length > 0 ? Convert.ToSingle(param[0]) : 1;
        var vt = v;
        switch (dir)
        {
            case Direction.Activation:
                if (Program02 == null)
                {
                    Parallel.For(0, u.TotalLength, i =>
                {
                    vt.Data[i] = (float)(1.0 / (1 + Math.Exp(-alpha * u.Data[i])));
                });
                }
                else
                {
                    Program01.Startup();
                    using (Cloo.ComputeBuffer<float> __u = Program01.ConvertBuffer(Cloo.ComputeMemoryFlags.ReadOnly, u.Data))
                    using (Cloo.ComputeBuffer<float> __vt = Program01.ConvertBuffer(Cloo.ComputeMemoryFlags.WriteOnly, vt.Data))
                    {
                        Program01.SetParameter(__u);
                        Program01.SetParameter(__vt);

                        Program01.SetParameter(u.Batch, OpenDM.Gpgpu.ProgramOption.ValueMode.INT);
                        Program01.SetParameter(u.Width, OpenDM.Gpgpu.ProgramOption.ValueMode.INT);

                        Program01.SetParameter(alpha, OpenDM.Gpgpu.ProgramOption.ValueMode.FLOAT);

                        Program01.Execute(u.Batch, u.Width);
                        Program01.ReadBuffer(__vt, ref vt.Data);
                    }
                }
                break;
            case Direction.Deactivation:
                if (Program02 == null)
                {
                    Parallel.For(0, u.TotalLength, i =>
                {
                    var f = (float)(1.0 / (1 + Math.Exp(-alpha * u.Data[i])));
                    vt.Data[i] = (1 - f) * f;
                });
                }
                else
                {
                    Program02.Startup();
                    using (Cloo.ComputeBuffer<float> __u = Program02.ConvertBuffer(Cloo.ComputeMemoryFlags.ReadOnly, u.Data))
                    using (Cloo.ComputeBuffer<float> __vt = Program02.ConvertBuffer(Cloo.ComputeMemoryFlags.WriteOnly, vt.Data))
                    {
                        Program02.SetParameter(__u);
                        Program02.SetParameter(__vt);

                        Program02.SetParameter(u.Batch, OpenDM.Gpgpu.ProgramOption.ValueMode.INT);
                        Program02.SetParameter(u.Width, OpenDM.Gpgpu.ProgramOption.ValueMode.INT);

                        Program02.SetParameter(alpha, OpenDM.Gpgpu.ProgramOption.ValueMode.FLOAT);

                        Program02.Execute(u.Batch, u.Width);
                        Program02.ReadBuffer(__vt, ref vt.Data);
                    }
                }
                break;
            default:
                break;
        }
    }
    private void ELU(RNdArray u, ref RNdArray v, Direction dir, params object[] param)
    {
        float alpha = param.Length > 0 ? Convert.ToSingle(param[0]) : 1;
        var vt = v;
        switch (dir)
        {
            case Direction.Activation:
                Parallel.For(0, u.TotalLength, i =>
                {
                    vt.Data[i] = u.Data[i] > 0 ? u.Data[i] : (float)(Math.Exp(alpha * u.Data[i]) - 1);
                });
                break;
            case Direction.Deactivation:
                Parallel.For(0, u.TotalLength, i =>
                {
                    vt.Data[i] = u.Data[i] > 0 ? 1 : (float)(alpha * Math.Exp(alpha * u.Data[i]));
                });
                break;
            default:
                break;
        }
    }
}