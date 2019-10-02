using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RNdArray
{
    private float[] data { get; set; }

    private Shape Shape { get; set; }
    public int Batch { get { return Shape.Batch; } }
    public int Width { get { return Shape.Width; } }
    public int Height { get { return Shape.Height; } }
    public int Channel { get { return Shape.Channel; } }
    public int Depth { get { return Shape.Depth; } }

    public float this[params int[] n]
    {
        get { var idx = Shape.IndexOf(n); if (idx >= 0) { return data[idx]; } { return 0; } }
        set { var idx = Shape.IndexOf(n); if (idx >= 0) { data[idx] = value; } }
    }

    public new string ToString()
    {
        string str = string.Format("{0} -> [", Shape.ToString());
        int bidx = 0;
        for (int i = 0; i < data.Length; i++)
        {
            str += string.Format("{0}", data[i]);
            bidx++;
            if (Shape.SegmentLength == bidx)
            {
                if (i == data.Length - 1) { str += "]"; }
                else { str += "]["; }
                bidx = 0;
            }
            else
            {
                str += ", ";
            }
        }
        return str;
    }

    public RNdArray(Shape shape)
    {
        Shape = shape;
        data = new float[shape.TotalLength];
    }

    public void Fill(double value)
    {
        for (int i = 0; i < Shape.TotalLength; i++)
        {
            data[i] = (float)value;
        }
    }

    public static RNdArray operator <<(RNdArray a1, int c)
    {
        Shape shape = a1.Shape << c;
        RNdArray item = new RNdArray(shape);
        item.Fill(1);

        var container = a1.Shape.Container();
        do
        {
            item[container.Structure] = a1[container.Structure];
        } while (a1.Shape.Indexer(ref container));

        return item;
    }
}

