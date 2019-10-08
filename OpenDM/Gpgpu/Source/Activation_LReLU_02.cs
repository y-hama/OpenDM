using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDM.Gpgpu.Source
{
    class Activation_LReLU_02 : SourceCode
    {
        public override string Name
        {
            get { return @"Activation_LReLU_02"; }
        }

        protected override FunctionType FunctionLocale
        {
            get { return FunctionType.Global; }
        }

        protected override void ParameterConfigration()
        {
            AddParameter("u", ObjectType.Array, ElementType.FLOAT);
            AddParameter("v", ObjectType.Array, ElementType.FLOAT);

            AddParameter("batch", ObjectType.Value, ElementType.INT);
            AddParameter("width", ObjectType.Value, ElementType.INT);

            AddParameter("alpha", ObjectType.Value, ElementType.FLOAT);
        }

        protected override void CreateSource()
        {
            GlobalID(2);
            AddMethodBody(@"
int b = i0;
int i = i1;
int idx = IndexOf1D(b, i, width); 
v[idx] = u[idx] > 0 ? 1 : alpha;
");
        }
    }
}
