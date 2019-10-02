using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            int batch = 10;
            int iw = 30, ow = 4;

            var input = new RNdArray(Shape.D1(iw, batch));

            var w = new RNdWeight(iw, ow);

            RNdArray u, v;
            OpenDM.Convolution.D1(w, input, out u, out v);

            Console.WriteLine(v.ToString());

            Console.ReadLine();
        }
    }
}
