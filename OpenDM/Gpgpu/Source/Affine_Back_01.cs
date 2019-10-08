using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDM.Gpgpu.Source
{
    class Affine_Back_01 : SourceCode
    {
        public override string Name
        {
            get { return @"Affine_Back_01"; }
        }

        protected override FunctionType FunctionLocale
        {
            get { return FunctionType.Global; }
        }

        protected override void ParameterConfigration()
        {
            AddParameter("sigma", ObjectType.Array, ElementType.FLOAT);
            AddParameter("u", ObjectType.Array, ElementType.FLOAT);
            AddParameter("s", ObjectType.Array, ElementType.FLOAT);

            AddParameter("width", ObjectType.Value, ElementType.INT);
        }

        protected override void CreateSource()
        {
            GlobalID(2);
            AddMethodBody(@"
int b = i0;
int i = i1;
int idx = IndexOf1D(b, i, width);
s[idx] = sigma[idx] * u[idx];
");
        }
    }
}
