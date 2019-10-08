using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDM.Gpgpu.Source
{
    class Optimizer_Adam_01 : SourceCode
    {
        public override string Name
        {
            get { return @"Optimizer_Adam_01"; }
        }

        protected override FunctionType FunctionLocale
        {
            get { return FunctionType.Global; }
        }

        protected override void ParameterConfigration()
        {
            AddParameter("w", ObjectType.Array, ElementType.FLOAT);
            AddParameter("dw", ObjectType.Array, ElementType.FLOAT);
            AddParameter("m", ObjectType.Array, ElementType.FLOAT);
            AddParameter("v", ObjectType.Array, ElementType.FLOAT);
            AddParameter("dpo", ObjectType.Array, ElementType.FLOAT);

            AddParameter("wwidth", ObjectType.Value, ElementType.INT);
            AddParameter("wheight", ObjectType.Value, ElementType.INT);

            AddParameter("rho", ObjectType.Value, ElementType.FLOAT);

            AddParameter("beta1", ObjectType.Value, ElementType.FLOAT);
            AddParameter("beta2", ObjectType.Value, ElementType.FLOAT);
            AddParameter("ep", ObjectType.Value, ElementType.FLOAT);

            AddParameter("t", ObjectType.Value, ElementType.FLOAT);
        }

        protected override void CreateSource()
        {
            GlobalID(2);
            AddMethodBody(@"
int i = i0;
int j = i1;
int iw = IndexOf2D(0, i, j, wwidth, wheight);

float grad = dw[iw];
m[iw] = beta1 * m[iw] + (1 - beta1) * grad;
v[iw] = beta2 * v[iw] + (1 - beta2) * grad * grad;
if(dpo[iw] > 0)
{
    float mhat = m[iw] / ( 1 - pow(beta1, t) );
    float vhat = v[iw] / ( 1 - pow(beta2, t) );
    w[iw] -= (rho * (mhat / (sqrt(vhat) + ep)));
}
");
        }
    }
}
