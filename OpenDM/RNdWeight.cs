using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RNdWeight : RNdArray
{
    public RNdWeight(int width, int height)
    : base(Shape.D2(width + 1, height))
    {

    }

}
