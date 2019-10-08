using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDM.Gpgpu.Source
{
    class Activation_Sigmoid_01 : SourceCode
    {
        public override string Name
        {
            get { return @"Activation_Sigmoid_01"; }
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
v[idx] = (1.0 / (1.0 + exp(-alpha * u[idx])));
");
        }
    }
}