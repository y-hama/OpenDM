using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDM.Gpgpu.Source
{
    class Affine_Back_03 : SourceCode
    {
        public override string Name
        {
            get { return @"Affine_Back_03"; }
        }

        protected override FunctionType FunctionLocale
        {
            get { return FunctionType.Global; }
        }

        protected override void ParameterConfigration()
        {
            AddParameter("s", ObjectType.Array, ElementType.FLOAT);
            AddParameter("w", ObjectType.Array, ElementType.FLOAT);
            AddParameter("p", ObjectType.Array, ElementType.FLOAT);

            AddParameter("wwidth", ObjectType.Value, ElementType.INT);
            AddParameter("wheight", ObjectType.Value, ElementType.INT);
        }

        protected override void CreateSource()
        {
            GlobalID(2);
            AddMethodBody(@"
int i = i0;
int b = i1;
int ip = IndexOf1D(b, i, wwidth);
p[ip] = 0;
for (int j = 0; j < wheight; j++)
{
    p[ip] += w[IndexOf2D(0, i, j, wwidth, wheight)] * s[IndexOf1D(b, j, wheight)];
}
");
        }
    }
}
