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

        public GridBase NextGrid { get; private set; }
        public GridBase PrevGrid { get; private set; }

        protected double[] Options { get; private set; }

        protected abstract void InitOption(RNdArray initWeight);
        protected abstract void Confirm();
        protected abstract RNdArray ForwardProcess(RNdArray input, params RNdArray[] rNdArrays);
        protected abstract RNdArray BackProcess(RNdArray sigma, params RNdArray[] rNdArrays);

        protected abstract RNdArray BackThroughProcess(RNdArray sigma, params RNdArray[] rNdArrays);
        protected abstract void UpdateProcess(params object[] param);

        public GridBase Initialize(RNdArray initWeight = null)
        {
            Confirm();
            InitOption(initWeight);
            return this;
        }

        public void GridConnection(GridBase next)
        {
            NextGrid = next;
            next.PrevGrid = this;
        }

        public void SetOption(params double[] options)
        {
            Options = options;
        }

        public RNdArray Forward(RNdArray input, params RNdArray[] rNdArrays)
        {
            return ForwardProcess(input, rNdArrays);
        }

        public RNdArray Back(RNdArray sigma, params RNdArray[] rNdArrays)
        {
            return BackProcess(sigma, rNdArrays);
        }

        public void Update(float rho)
        {
            UpdateProcess(rho);
        }
    }
}
