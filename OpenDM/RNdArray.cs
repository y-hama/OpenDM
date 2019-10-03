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

    protected int IndexOf(int b, int w, int h, int c, int d)
    {
        return b * AreaLength + d * ZoneLength + c * LocalLength + w * Height + h;
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
                    while (segstr.Length < dig + 2)
                    {
                        segstr = segstr + "0";
                    }
                }
            }
            else
            {
                segstr = (string.Format("{0} ", Data[i]));
            }
            string sign = Data[i] >= 0 ? " " : "-";
            str += (string.Format("{0}{1}", sign, segstr));

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

    public void Shuffle(double amplify = -1)
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
        for (int i = 0; i < TotalLength; i++)
        {
            Data[i] = (float)((random.NextDouble() * 2 - 1) * amplify);
        }
    }

    public void Fill(double v)
    {
        float vx = (float)v;
        for (int i = 0; i < TotalLength; i++)
        {
            Data[i] = vx;
        }
    }
}


public class R1dArray : RNdArray
{
    public float Offset { get; private set; } = 1;

    public float this[int i, int b = 0]
    {
        get { return Data[IndexOf(b, i, 0, 0, 0)]; }
        set { Data[IndexOf(b, i, 0, 0, 0)] = value; }
    }

    public R1dArray(int width, int batch = 1)
    {
        Dimension = Dimension.D1;
        Batch = batch;
        Width = width;
        ConfirmProperty();
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
    public static R1dArray operator +(R1dArray a1, R1dArray a2)
    {
        R1dArray item = new R1dArray(a1.Width, a1.Batch);

        for (int b = 0; b < a1.Batch; b++)
        {
            for (int i = 0; i < a1.Width; i++)
            {
                item[i, b] = a1[i, b] + a2[i, b];
            }
        }
        return item;
    }
    public static R1dArray operator -(R1dArray a1, R1dArray a2)
    {
        R1dArray item = new R1dArray(a1.Width, a1.Batch);

        for (int b = 0; b < a1.Batch; b++)
        {
            for (int i = 0; i < a1.Width; i++)
            {
                item[i, b] = a1[i, b] - a2[i, b];
            }
        }
        return item;
    }
    public static R1dArray operator *(R1dArray a1, R1dArray a2)
    {
        R1dArray item = new R1dArray(a1.Width, a1.Batch);

        for (int b = 0; b < a1.Batch; b++)
        {
            for (int i = 0; i < a1.Width; i++)
            {
                item[i, b] = a1[i, b] * a2[i, b];
            }
        }
        return item;
    }
    public static R1dArray operator /(R1dArray a1, R1dArray a2)
    {
        R1dArray item = new R1dArray(a1.Width, a1.Batch);

        for (int b = 0; b < a1.Batch; b++)
        {
            for (int i = 0; i < a1.Width; i++)
            {
                item[i, b] = a1[i, b] / a2[i, b];
            }
        }
        return item;
    }
}

public class R2dArray : RNdArray
{
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
