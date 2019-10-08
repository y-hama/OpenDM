using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDM.Gpgpu.Function
{
    class f01_IndexOf1D : SourceCode
    {
        public override string Name
        {
            get { return @"IndexOf1D"; }
        }

        protected override FunctionType FunctionLocale
        {
            get { return FunctionType.Local; }
        }
        protected override ElementType ReturnType
        {
            get { return ElementType.INT; }
        }

        protected override void ParameterConfigration()
        {
            AddParameter("b", ObjectType.Value, ElementType.INT);
            AddParameter("w", ObjectType.Value, ElementType.INT);

            AddParameter("width", ObjectType.Value, ElementType.INT);
        }

        protected override void CreateSource()
        {
            AddMethodBody(@"
return b * width + w;
");
        }
    }
}
