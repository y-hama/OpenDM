using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDM.Gpgpu.Function
{
    class gf01_IndexOf : SourceCode
    {
        public override string Name
        {
            get { return @"IndexOf"; }
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
            AddParameter("c", ObjectType.Value, ElementType.INT);
            AddParameter("d", ObjectType.Value, ElementType.INT);

            AddParameter("width", ObjectType.Value, ElementType.INT);
            AddParameter("height", ObjectType.Value, ElementType.INT);
            AddParameter("channel", ObjectType.Value, ElementType.INT);
            AddParameter("depth", ObjectType.Value, ElementType.INT);

        }

        protected override void CreateSource()
        {
            AddMethodBody(@"
return b * depth * channel * width * height + d * channel * width * height + c * width * height + w * height + h;
");
        }
    }
}