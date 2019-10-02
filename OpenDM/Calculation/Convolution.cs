using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDM
{
    public static class Convolution
    {
        public static void D1(RNdWeight w, RNdArray input, out RNdArray u, out RNdArray v)
        {
            var shape = Shape.D1(w.Height, input.Height);
            u = new RNdArray(shape);
            v = new RNdArray(shape);

            var inx = input << 1;
            var outx = inx >> 1;
        }
    }
}
