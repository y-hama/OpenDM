using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Affine : OpenDM.Grid.GridBase
{

    public Affine(int inNode, int outNode)
    {
        InNode = inNode;
        OutNode = outNode;
    }

    private int InNode { get; set; }
    private int OutNode { get; set; }

    private Activator act { get; set; }
    private Optimizer opt { get; set; }

    protected override void InitOption(params object[] initialParameter)
    {
        if (initialParameter.Length > 0)
        {
            act = (Activator)initialParameter[0];
        }
        if (initialParameter.Length > 1)
        {
            opt = (Optimizer)initialParameter[1];
        }
    }

    protected override void Confirm()
    {
        w = new R2dArray(InNode + 1, OutNode);
        w.Shuffle();
    }

    private R1dArray i;
    private R1dArray u;
    private R1dArray o;

    private R1dArray s;
    private R1dArray p;

    private R2dArray w;

    protected override RNdArray ForwardProcess(RNdArray input, params RNdArray[] rNdArrays)
    {
        i = (R1dArray)input;
        OpenDM.Grid.Calculation.Affine.Forwerd(i, w, out u, out o, act);
        return o;
    }


    protected override RNdArray BackProcess(RNdArray sigma, params RNdArray[] rNdArrays)
    {
        s = (R1dArray)sigma;
        OpenDM.Grid.Calculation.Affine.Back(s, i, u, ref w, out p, act, opt);
        return p;
    }
}
