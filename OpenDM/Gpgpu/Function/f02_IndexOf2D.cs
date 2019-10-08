using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDM.Gpgpu.Function
{
    class f02_IndexOf2D : SourceCode
    {
        public override string Name
        {
            get { return @"IndexOf2D"; }
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
            AddParameter("h", ObjectType.Value, ElementType.INT);

            AddParameter("width", ObjectType.Value, ElementType.INT);
            AddParameter("height", ObjectType.Value, ElementType.INT);
        }

        protected override void CreateSource()
        {
            AddMethodBody(@"
return b * width * height + w * height + h;
");
        }
    }
}
