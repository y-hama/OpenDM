using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Direction
{
    Forwerd,
    Back,
}

namespace OpenDM.Grid
{
    public abstract class GridBase
    {

        protected abstract void InitOption(params object[] initialParameter);
        protected abstract void Confirm();
        protected abstract RNdArray ForwardProcess(RNdArray input, params RNdArray[] rNdArrays);
        protected abstract RNdArray BackProcess(RNdArray sigma, params RNdArray[] rNdArrays);

        public GridBase Initialize(params object[] initialParameter)
        {
            InitOption(initialParameter != null ? initialParameter : new object[] { });
            Confirm();
            return this;
        }


        public RNdArray Forward(RNdArray input, params RNdArray[] rNdArrays)
        {
            return ForwardProcess(input, rNdArrays);
        }

        public RNdArray Back(RNdArray sigma, params RNdArray[] rNdArrays)
        {
            return BackProcess(sigma, rNdArrays);
        }
    }
}
