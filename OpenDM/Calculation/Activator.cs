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
                return LReLU;
            case ActivationType.ELU:
                return ELU;
            case ActivationType.Sigmoid:
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
                Parallel.For(0, u.TotalLength, i =>
                {
                    vt.Data[i] = u.Data[i] > 0 ? u.Data[i] : u.Data[i] * alpha;
                });
                break;
            case Direction.Deactivation:
                Parallel.For(0, u.TotalLength, i =>
                {
                    vt.Data[i] = u.Data[i] > 0 ? 1 : alpha;
                });
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
                Parallel.For(0, u.TotalLength, i =>
                {
                    vt.Data[i] = (float)(1.0 / (1 + Math.Exp(-u.Data[i])));
                });
                break;
            case Direction.Deactivation:
                Parallel.For(0, u.TotalLength, i =>
                {
                    var f = (float)(1.0 / (1 + Math.Exp(-u.Data[i])));
                    vt.Data[i] = (1 - f) * f;
                });
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