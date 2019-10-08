using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDM.Gpgpu.Source
{
    class Affine_Back_02 : SourceCode
    {
        public override string Name
        {
            get { return @"Affine_Back_02"; }
        }

        protected override FunctionType FunctionLocale
        {
            get { return FunctionType.Global; }
        }

        protected override void ParameterConfigration()
        {
            AddParameter("ipt", ObjectType.Array, ElementType.FLOAT);
            AddParameter("s", ObjectType.Array, ElementType.FLOAT);
            AddParameter("dw", ObjectType.Array, ElementType.FLOAT);

            AddParameter("wwidth", ObjectType.Value, ElementType.INT);
            AddParameter("wheight", ObjectType.Value, ElementType.INT);

            AddParameter("batch", ObjectType.Value, ElementType.INT);
        }

        protected override void CreateSource()
        {
            GlobalID(2);
            AddMethodBody(@"
int i = i0;
int j = i1;
int idw = IndexOf2D(0, i, j, wwidth, wheight);
dw[idw] = 0;
for (int b = 0; b < batch; b++)
{
    dw[idw] += s[IndexOf1D(b, j, wheight)] * ipt[IndexOf1D(b, i, wwidth)];
}
");
        }
    }
}
