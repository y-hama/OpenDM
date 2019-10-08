using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDM.Grid
{
    public class Affine : OpenDM.Grid.GridBase
    {

        public Affine(int innode, int outnode, Activator act, Optimizer opt)
        {
            InNode = innode;
            OutNode = outnode;
            activator = act;
            optimizer = opt;
        }

        private int InNode { get; set; }
        private int OutNode { get; set; }

        private Activator activator { get; set; }
        private Optimizer optimizer { get; set; }

        protected override void Confirm()
        {
            w = new R2dArray(InNode + 1, OutNode);
            w = (R2dArray)w.Shuffle();
        }

        protected override void InitOption(RNdArray initWeight)
        {
            if (initWeight != null && initWeight.Dimension == Dimension.D2)
            {
                w = (R2dArray)initWeight;
            }
        }

        private Calculation.Affine Calculation { get; set; } = new Grid.Calculation.Affine();

        private R1dArray i;
        private R1dArray u;
        private R1dArray o;

        private R1dArray s;
        private R1dArray p;

        private R2dArray w;

        protected override RNdArray ForwardProcess(RNdArray input, params RNdArray[] rNdArrays)
        {
            i = (R1dArray)input;
            Calculation.Forwerd(i, w, out u, out o, activator);
            return o;
        }


        protected override RNdArray BackProcess(RNdArray sigma, params RNdArray[] rNdArrays)
        {
            s = (R1dArray)sigma;
            Calculation.Back(s, i, u, ref w, out p, activator);
            return p;
        }

        protected override RNdArray BackThroughProcess(RNdArray sigma, params RNdArray[] rNdArrays)
        {
            s = (R1dArray)sigma;
            Calculation.Back(s, i, u, ref w, out p, activator);
            return p;
        }

        protected override void UpdateProcess(params object[] param)
        {
            float rho = 0.01f;
            if (param.Length > 0) { rho = Convert.ToSingle(param[0]); }
            RNdArray _w = (RNdArray)w;
            Calculation.Update(ref _w, rho, optimizer);
        }
    }
}