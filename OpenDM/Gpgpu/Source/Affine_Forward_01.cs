using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDM.Gpgpu.Source
{
    class Affine_Forward_01 : SourceCode
    {
        public override string Name
        {
            get { return @"Affine_Forward_01"; }
        }

        protected override FunctionType FunctionLocale
        {
            get { return FunctionType.Global; }
        }

        protected override void ParameterConfigration()
        {
            AddParameter("ipt", ObjectType.Array, ElementType.FLOAT);
            AddParameter("u", ObjectType.Array, ElementType.FLOAT);
            AddParameter("w", ObjectType.Array, ElementType.FLOAT);

            AddParameter("wwidth", ObjectType.Value, ElementType.INT);
            AddParameter("wheight", ObjectType.Value, ElementType.INT);
        }

        protected override void CreateSource()
        {
            GlobalID(2);
            AddMethodBody(@"
int b = i0;
int j = i1;
int iu = IndexOf1D(b, j, wheight);
u[iu] = 0;
for (int i = 0; i < wwidth; i++)
{
    u[iu] += w[IndexOf2D(0, i, j, wwidth, wheight)] * ipt[IndexOf1D(b, i, wwidth)];
}
");
        }
    }
}
