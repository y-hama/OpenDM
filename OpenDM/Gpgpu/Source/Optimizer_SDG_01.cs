using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDM.Gpgpu.Source
{
    class Optimizer_SDG_01 : SourceCode
    {
        public override string Name
        {
            get { return @"Optimizer_SDG_01"; }
        }

        protected override FunctionType FunctionLocale
        {
            get { return FunctionType.Global; }
        }

        protected override void ParameterConfigration()
        {
            AddParameter("w", ObjectType.Array, ElementType.FLOAT);
            AddParameter("dw", ObjectType.Array, ElementType.FLOAT);
            AddParameter("dpo", ObjectType.Array, ElementType.FLOAT);

            AddParameter("wwidth", ObjectType.Value, ElementType.INT);
            AddParameter("wheight", ObjectType.Value, ElementType.INT);

            AddParameter("rho", ObjectType.Value, ElementType.FLOAT);
        }

        protected override void CreateSource()
        {
            GlobalID(2);
            AddMethodBody(@"
int i = i0;
int j = i1;
int iw = IndexOf2D(0, i, j, wwidth, wheight);
w[iw] -= rho * dw[iw];
");
        }
    }
}
