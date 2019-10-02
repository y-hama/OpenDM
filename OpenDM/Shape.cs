using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Dimension
{
    D0,
    D1,
    D2,
    D3,
    D4,
}

public class Shape
{
    private int[] structure { get; set; }
    public int[] Structure { get { return structure; } }

    public int Batch { get; private set; } = 1;
    public int Width { get; private set; } = 1;
    public int Height { get; private set; } = 1;
    public int Channel { get; private set; } = 1;
    public int Depth { get; private set; } = 1;

    public Dimension Dimension { get; private set; } = Dimension.D0;

    public int TotalLength { get; private set; } = 1;
    public int SegmentLength { get; private set; } = 1;

    public new string ToString()
    {
        string str = string.Format("b:{0} w:{1} h:{2} c:{3} d:{4}", structure[0], structure[1], structure[2], structure[3], structure[4]);

        return str;
    }

    private int LongIndex(int[] n)
    {
        return n[0] * Depth * Channel * Height * Width + n[4] * Channel * Height * Width + n[3] * Height * Width + n[2] * Width + n[1];
    }
    public int IndexOf(params int[] n)
    {
        var l = new int[structure.Length];
        bool check = false;
        for (int i = 0; i < (int)Dimension + 1; i++)
        {
            if (structure[i] > n[i]) { l[i] = n[i]; }
            else { check = true; break; }
        }
        if (!check) { return LongIndex(l); }
        else { return -1; }
    }

    public bool Indexer(ref Shape container)
    {
        var str = container.structure;
        bool ret = _indexer((int)this.Dimension, this.structure, ref str);
        container.CopyList();
        return ret;
    }
    private bool _indexer(int p, int[] bs, ref int[] vs)
    {
        bool ret = true;
        vs[p]++;
        if (vs[p] == bs[p])
        {
            vs[p] = 0;
            if (p == 0)
            {
                ret = false;
            }
            else
            {
                ret = _indexer(p - 1, bs, ref vs);
            }
        }

        return ret;
    }

    private void CreateList()
    {
        structure = new int[] { Batch, Width, Height, Channel, Depth };
        TotalLength = SegmentLength = 1;
        for (int i = 0; i < structure.Length; i++)
        {
            TotalLength *= structure[i];
            if (i > 0)
            {
                SegmentLength *= structure[i];
            }
        }
    }
    private void CopyList()
    {
        Batch = structure[0];
        Width = structure[1];
        Height = structure[2];
        Channel = structure[3];
        Depth = structure[4];
        TotalLength = SegmentLength = 1;
        for (int i = 0; i < structure.Length; i++)
        {
            TotalLength *= structure[i];
            if (i > 0)
            {
                SegmentLength *= structure[i];
            }
        }
    }

    private Shape(int batch, int width, int height, int channel, int depth)
    {
        Batch = batch;
        Width = width;
        Height = height;
        Channel = channel;
        Depth = depth;
        CreateList();
    }

    public static Shape D0(int batch = 1)
    {
        return new Shape(batch, 1, 1, 1, 1) { Dimension = Dimension.D0 };
    }
    public static Shape D1(int width, int batch = 1)
    {
        return new Shape(batch, width, 1, 1, 1) { Dimension = Dimension.D1 };
    }
    public static Shape D2(int width, int height, int batch = 1)
    {
        return new Shape(batch, width, height, 1, 1) { Dimension = Dimension.D2 };
    }
    public static Shape D3(int channel, int width, int height, int batch = 1)
    {
        return new Shape(batch, width, height, channel, 1) { Dimension = Dimension.D3 };
    }
    public static Shape D4(int depth, int channel, int width, int height, int batch = 1)
    {
        return new Shape(batch, width, height, channel, depth) { Dimension = Dimension.D4 };
    }

    public Shape Clone()
    {
        return new Shape(Batch, Width, Height, Channel, Depth) { Dimension = this.Dimension };
    }
    public Shape Container()
    {
        return new Shape(0, 0, 0, 0, 0) { Dimension = this.Dimension };
    }

    public static bool Same(Shape x, Shape y)
    {
        bool res = false;
        if (x.Dimension == y.Dimension)
        {
            for (int i = 0; i < x.structure.Length; i++)
            {
                if (x.structure[i] != y.structure[i]) { break; }
            }
            res = true;
        }
        return res;
    }


    public static Shape operator <<(Shape a1, int c)
    {
        Shape item = a1.Clone();
        switch (a1.Dimension)
        {
            case Dimension.D0:
                break;
            case Dimension.D1:
                item.Width += c;
                break;
            case Dimension.D2:
                break;
            case Dimension.D3:
                break;
            case Dimension.D4:
                break;
            default:
                break;
        }
        item.CreateList();
        return item;
    }
    public static Shape operator >>(Shape a1, int c)
    {
        Shape item = a1.Clone();
        switch (a1.Dimension)
        {
            case Dimension.D0:
                break;
            case Dimension.D1:
                item.Width -= c;
                break;
            case Dimension.D2:
                break;
            case Dimension.D3:
                break;
            case Dimension.D4:
                break;
            default:
                break;
        }
        item.CreateList();
        return item;
    }
}