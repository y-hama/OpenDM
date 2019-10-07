using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Dimension
{
    D1,
    D2,
    D3,
    D4,
}

public abstract class RNdArray
{
    private static Random random = new Random();

    protected abstract object[] InnerArgunents { get; }

    public Dimension Dimension { get; protected set; }
    public int Batch { get; protected set; } = 1;
    public int Width { get; protected set; } = 1;
    public int Height { get; protected set; } = 1;
    public int Channel { get; protected set; } = 1;
    public int Depth { get; protected set; } = 1;

    public int TotalLength { get; private set; } = 1;
    public int AreaLength { get; private set; } = 1;
    public int ZoneLength { get; private set; } = 1;
    public int LocalLength { get; private set; } = 1;

    public double Power
    {
        get
        {
            double res = 0;
            foreach (var item in Data)
            {
                res += item * item;
            }
            return res / AreaLength;
        }
    }

    public float[] Data { get; private set; }

    protected void SetDataArray(float[] v_a)
    {
        if (v_a.Length <= TotalLength)
        {
            for (int i = 0; i < Math.Min(TotalLength, v_a.Length); i++)
            {
                Data[i] = v_a[i];
            }
        }
    }

    protected int IndexOf(int b, int w, int h, int c, int d)
    {
        return b * AreaLength + d * ZoneLength + c * LocalLength + w * Height + h;
    }

    public new string ToString()
    {
        return this.ToString(-1);
    }

    public string ToString(int dig = -1)
    {
        string str = string.Format("b:{0}, d:{1}, c:{2}, w:{3}, h:{4}->[", Batch, Depth, Channel, Width, Height);
        int wtx = 0;
        for (int i = 0; i < TotalLength; i++)
        {
            string segstr = string.Empty;
            if (dig >= 0)
            {
                double seg = Math.Round(Math.Abs(Data[i]), dig);
                segstr = seg.ToString();
                if (dig > 0)
                {
                    if (!segstr.Contains(".")) { segstr += "."; }
                    if (segstr.Length < dig + 2)
                    {
                        while (segstr.Length < dig + 2)
                        {
                            segstr = segstr + "0";
                        }
                    }
                    else if (segstr.Length > dig + 2)
                    {
                        while (segstr.Length > dig + 2)
                        {
                            segstr = segstr.Substring(0, segstr.Length - 1);
                        }
                    }
                }
                string sign = Data[i] >= 0 ? " " : "-";
                segstr = sign + segstr;
            }
            else
            {
                segstr = (string.Format("{0} ", Data[i]));
            }
            str += (string.Format("{0}", segstr));

            wtx++;
            if (wtx == AreaLength)
            {
                if (i == TotalLength - 1)
                {
                    str += "]";
                }
                else
                {
                    str += "][";
                }
                wtx = 0;
            }
            else
            {
                str += ", ";
            }
        }
        return str;
    }

    protected void ConfirmProperty()
    {
        TotalLength = Batch * Width * Height * Channel * Depth;
        AreaLength = Width * Height * Channel * Depth;
        ZoneLength = Channel * Width * Height;
        LocalLength = Width * Height;

        Data = new float[TotalLength];
    }

    public RNdArray Shuffle(double amplify = -1)
    {
        if (amplify < 0)
        {
            amplify = 1;
            switch (Dimension)
            {
                case Dimension.D1:
                    break;
                case Dimension.D2:
                    amplify = 1.0 / (Width);
                    break;
                case Dimension.D3:
                    break;
                case Dimension.D4:
                    break;
                default:
                    break;
            }
        }
        var item = (RNdArray)System.Activator.CreateInstance(this.GetType(), this.InnerArgunents);
        for (int i = 0; i < TotalLength; i++)
        {
            item.Data[i] = this.Data[i] + (float)((random.NextDouble() * 2 - 1) * amplify);
        }
        return item;
    }

    public void Fill(double v)
    {
        float vx = (float)v;
        for (int i = 0; i < TotalLength; i++)
        {
            Data[i] = vx;
        }
    }

    public static bool ShapeCheck(RNdArray a1, RNdArray a2)
    {
        bool ret = true;
        if (a1.Batch != a2.Batch ||
            a1.Width != a2.Width ||
            a1.Height != a2.Height ||
            a1.Channel != a2.Channel ||
            a1.Depth != a2.Depth)
        {
            ret = false;
        }
        return ret;
    }

    public static RNdArray CombineBatch(List<RNdArray> list)
    {
        if (list != null && list.Count > 0)
        {
            var arg = list[0].InnerArgunents;
            arg[arg.Length - 1] = list.Count;
            var item = (RNdArray)System.Activator.CreateInstance(list[0].GetType(), arg);
            for (int b = 0; b < list.Count; b++)
            {
                for (int i = 0; i < item.AreaLength; i++)
                {
                    item.Data[b * item.AreaLength + i] = list[b].Data[i];
                }
            }
            return item;
        }
        throw new Exception();
    }

    public static RNdArray operator +(RNdArray a1, RNdArray a2)
    {
        if (a1.GetType() == a2.GetType())
        {
            if (ShapeCheck(a1, a2))
            {
                var item = (RNdArray)System.Activator.CreateInstance(a1.GetType(), a1.InnerArgunents);
                for (int i = 0; i < item.TotalLength; i++)
                {
                    item.Data[i] = a1.Data[i] + a2.Data[i];
                }
                return item;
            }
        }
        throw new Exception();
    }
    public static RNdArray operator -(RNdArray a1, RNdArray a2)
    {
        if (a1.GetType() == a2.GetType())
        {
            if (ShapeCheck(a1, a2))
            {
                var item = (RNdArray)System.Activator.CreateInstance(a1.GetType(), a1.InnerArgunents);
                for (int i = 0; i < item.TotalLength; i++)
                {
                    item.Data[i] = a1.Data[i] - a2.Data[i];
                }
                return item;
            }
        }
        throw new Exception();
    }
    public static RNdArray operator *(RNdArray a1, RNdArray a2)
    {
        if (a1.GetType() == a2.GetType())
        {
            if (ShapeCheck(a1, a2))
            {
                var item = (RNdArray)System.Activator.CreateInstance(a1.GetType(), a1.InnerArgunents);
                for (int i = 0; i < item.TotalLength; i++)
                {
                    item.Data[i] = a1.Data[i] * a2.Data[i];
                }
                return item;
            }
        }
        throw new Exception();
    }
    public static RNdArray operator /(RNdArray a1, RNdArray a2)
    {
        if (a1.GetType() == a2.GetType())
        {
            if (ShapeCheck(a1, a2))
            {
                var item = (RNdArray)System.Activator.CreateInstance(a1.GetType(), a1.InnerArgunents);
                for (int i = 0; i < item.TotalLength; i++)
                {
                    item.Data[i] = a1.Data[i] / a2.Data[i];
                }
                return item;
            }
        }
        throw new Exception();
    }
}


public class R1dArray : RNdArray
{
    protected override object[] InnerArgunents
    {
        get
        {
            return new object[] { Width, Batch };
        }
    }

    public float Offset { get; private set; } = 1;

    public float this[int i, int b = 0]
    {
        get { return Data[IndexOf(b, i, 0, 0, 0)]; }
        set { Data[IndexOf(b, i, 0, 0, 0)] = value; }
    }

    public R1dArray(int width, int batch = 1, params double[] v_a)
    {
        Dimension = Dimension.D1;
        Batch = batch;
        Width = width;
        ConfirmProperty();
        if (v_a != null && v_a.Length <= TotalLength)
        {
            SetDataArray(v_a.Select(x => (float)x).ToArray());
        }
    }

    public static R1dArray operator <<(R1dArray a, int c)
    {
        R1dArray item = new R1dArray(a.Width + c, a.Batch);
        item.Fill(a.Offset);

        for (int b = 0; b < a.Batch; b++)
        {
            for (int i = 0; i < a.Width; i++)
            {
                item[i, b] = a[i, b];
            }
        }
        return item;
    }
    public static R1dArray operator >>(R1dArray a, int c)
    {
        R1dArray item = new R1dArray(a.Width - c, a.Batch);
        item.Fill(a.Offset);

        for (int b = 0; b < a.Batch; b++)
        {
            for (int i = 0; i < item.Width; i++)
            {
                item[i, b] = a[i, b];
            }
        }
        return item;
    }
}

public class R2dArray : RNdArray
{
    protected override object[] InnerArgunents
    {
        get
        {
            return new object[] { Width, Height, Batch };
        }
    }

    public float this[int i, int j, int b = 0]
    {
        get { return Data[IndexOf(b, i, j, 0, 0)]; }
        set { Data[IndexOf(b, i, j, 0, 0)] = value; }
    }
    public R2dArray(int width, int height, int batch = 1)
    {
        Dimension = Dimension.D2;
        Batch = batch;
        Width = width;
        Height = height;
        ConfirmProperty();
    }
}

public class R3dArray : RNdArray
{
    protected override object[] InnerArgunents
    {
        get
        {
            return new object[] { Width, Height, Channel, Batch };
        }
    }

    public R3dArray(int width, int height, int channel, int batch = 1)
    {
        Dimension = Dimension.D3;
        Batch = batch;
        Width = width;
        Height = height;
        Channel = channel;
        ConfirmProperty();
    }
}

public class R4dArray : RNdArray
{
    protected override object[] InnerArgunents
    {
        get
        {
            return new object[] { Width, Height, Channel, Depth, Batch };
        }
    }

    public R2dArray Bias { get; private set; }

    public R4dArray(int width, int height, int channel, int depth, int batch = 1)
    {
        Dimension = Dimension.D4;
        Batch = batch;
        Width = width;
        Height = height;
        Channel = channel;
        Depth = depth;
        ConfirmProperty();
        Bias = new R2dArray(channel, depth, batch);
    }
}
