using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum OptimizationType
{
    SDG,
    Adam,
}

public class Optimizer
{
    #region

    private static Random random = new Random();
    public static Optimizer Confirm(OptimizationType type, params object[] param)
    {
        return new Optimizer(type, param);
    }

    private delegate void OptimizationFunction(RNdArray dw, ref RNdArray w, params object[] param);
    private OptimizationFunction Function { get; set; }

    private OptimizationType Type { get; set; }
    private object[] Parameter { get; set; }
    private float[] Option { get; set; }

    private Optimizer(OptimizationType type, params object[] param)
    {
        Type = type;
        Parameter = (param != null) ? param : new object[] { };
        Function = SelectFunction(type);
    }

    public void Update(RNdArray dw, ref RNdArray w, params object[] temporaryParameter)
    {
        if (temporaryParameter == null)
        {
            temporaryParameter = new object[] { };
        }

        object[] tp = new object[Math.Max(temporaryParameter.Length, Parameter.Length)];
        for (int i = 0; i < tp.Length; i++)
        {
            if (temporaryParameter.Length > i)
            {
                tp[i] = temporaryParameter[i];
            }
            else if (Parameter.Length > i)
            {
                tp[i] = Parameter[i];
            }
        }

        Function(dw, ref w, tp);
    }

    private bool[] DropOutFlag(int length, double probability)
    {
        bool[] dpo = new bool[length];
        for (int i = 0; i < length; i++)
        {
            if (random.NextDouble() <= probability)
            {
                dpo[i] = true;
            }
        }
        return dpo;
    }

    #endregion
    private OptimizationFunction SelectFunction(OptimizationType type)
    {
        switch (type)
        {
            case OptimizationType.SDG:
                return SDG;
            case OptimizationType.Adam:
                return Adam;
            default:
                return null;
        }
    }

    private void SDG(RNdArray dw, ref RNdArray w, params object[] param)
    {
        float rho = param.Length >= 1 ? Convert.ToSingle(param[0]) / 1000 : 0.01f;
        float dropoput = param.Length >= 2 ? Convert.ToSingle(param[1]) : 1.0f;
        var c = w;
        var dpo = DropOutFlag(w.TotalLength, dropoput);
        Parallel.For(0, w.TotalLength, i =>
        {
            if (dpo[i])
            {
                c.Data[i] -= rho * dw.Data[i];
            }
        });
    }

    private int adam_t = 0;
    private float rho = 0;
    private const float adam_alpha = 0.1f;
    private const float adam_beta1 = 0.9f;
    private const float adam_beta2 = 0.999f;
    private const float adam_ep = 10E-8f;
    private RNdArray adam_m, adam_v;
    private void Adam(RNdArray dw, ref RNdArray w, params object[] param)
    {
        if (adam_t == 0)
        {
            adam_m = new R2dArray(w.Width, w.Height, w.Batch);
            adam_v = new R2dArray(w.Width, w.Height, w.Batch);
            rho = adam_alpha;
        }

        adam_t++;
        var bt1 = (1 - Math.Pow(adam_beta1, adam_t));
        var bt2 = (1 - Math.Pow(adam_beta2, adam_t));

        rho = (param.Length >= 1 && Convert.ToSingle(param[0]) >= 0) ? Convert.ToSingle(param[0]) / 1000 : adam_alpha * (float)(Math.Sqrt(bt2) / bt1);
        if (rho > adam_alpha) { rho = adam_alpha; }
        float dropoput = param.Length >= 2 ? Convert.ToSingle(param[1]) : 1.0f;

        var c = w;
        var m = adam_m;
        var v = adam_v;
        var dpo = DropOutFlag(w.TotalLength, dropoput);
        Parallel.For(0, w.TotalLength, i =>
        {
            var grad = dw.Data[i];
            m.Data[i] = adam_beta1 * m.Data[i] + (1 - adam_beta1) * grad;
            v.Data[i] = adam_beta2 * v.Data[i] + (1 - adam_beta2) * grad * grad;
            if (dpo[i])
            {
                var mhat = m.Data[i] / bt1;
                var vhat = v.Data[i] / bt2;
                c.Data[i] -= (float)(rho * (mhat / (Math.Sqrt(vhat) + adam_ep)));
            }
        });
    }
}
