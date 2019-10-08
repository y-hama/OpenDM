using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace OpenDM.Gpgpu
{
    abstract class SourceCode
    {
        protected enum ObjectType
        {
            Value,
            Array,
        }
        protected enum ElementType
        {
            VOID,
            INT,
            FLOAT
        }

        protected enum FunctionType
        {
            Global,
            Local,
        }

        private class ParameterSet
        {
            public string Name { get; set; }
            public ObjectType ObjectType { get; set; }
            public ElementType ElementType { get; set; }

            private const string ArgumentFormat = @"{0} {1}";
            public string Argument
            {
                get
                {
                    string cd = string.Empty;
                    string tp = string.Empty;
                    switch (ElementType)
                    {
                        case ElementType.INT:
                            tp = "int";
                            break;
                        case ElementType.FLOAT:
                            tp = "float";
                            break;
                        default:
                            break;
                    }
                    switch (ObjectType)
                    {
                        case ObjectType.Array:
                            tp = "__global " + tp + "*";
                            break;
                        case ObjectType.Value:
                            break;
                        default:
                            break;
                    }
                    cd = string.Format(ArgumentFormat, tp, Name);
                    return cd;
                }
            }
        }

        #region Property
        private const string METHOD_NAMESPACE1 = "OpenDM.Gpgpu.Function";

        private List<ParameterSet> pList = new List<ParameterSet>();
        private const string GlobalHeaderFormat = @"__{0} {1} {2}({3})";
        private const string LocalHeaderFormat = @"{0} {1}({2})";
        protected const string GET_GLOBAL_ID_FORMAT = @"int i{0} = get_global_id({1});";
        private const int MAX_GLOBAL_ID_COUNT = 3;
        private string argumentstring { get; set; }
        private string bodystring { get; set; }
        private string sourcestring { get; set; }

        public string Source
        {
            get
            {
                if (bodystring == null) { CreateSourceInterface(); }
                return sourcestring;
            }
        }
        #endregion

        public SourceCode()
        {
            ParameterConfigration();
        }

        private void CreateSourceInterface()
        {
            CreateSource();
            CreateArguments();
            switch (FunctionLocale)
            {
                case FunctionType.Local:
                    {
                        sourcestring = string.Format(LocalHeaderFormat,
                                      ReturnType.ToString().ToLower(),
                                      Name,
                                      argumentstring)
                                      + "{\n" +
                                      bodystring
                                      + "}\n";
                    }
                    break;
                case FunctionType.Global:
                default:
                    {
                        sourcestring = string.Format(GlobalHeaderFormat,
                                      "kernel",
                                      ReturnType.ToString().ToLower(),
                                      Name,
                                      argumentstring)
                                      + "{\n" +
                                      bodystring
                                      + "}\n";

                        Assembly asm = Assembly.GetExecutingAssembly();
                        List<SourceCode> fList = new List<SourceCode>();
                        var asmtypes = asm.GetTypes();
                        foreach (var item in asmtypes)
                        {
                            if (item.Namespace != null)
                            {
                                if (item.Namespace.Contains(METHOD_NAMESPACE1))
                                {
                                    fList.Add((SourceCode)System.Activator.CreateInstance(item));
                                }
                            }
                        }
                        fList.Reverse();
                        foreach (var item in fList)
                        {
                            sourcestring = item.Source + sourcestring;
                        }
                    }
                    break;
            }
        }

        private void CreateArguments()
        {
            argumentstring = string.Empty;
            for (int i = 0; i < pList.Count; i++)
            {
                argumentstring += pList[i].Argument;
                if (i != pList.Count - 1)
                {
                    argumentstring += ",";
                }
            }
        }

        #region Abstruct/Virtual
        public abstract string Name { get; }
        protected abstract void ParameterConfigration();
        protected abstract void CreateSource();
        protected abstract FunctionType FunctionLocale { get; }
        protected virtual ElementType ReturnType { get { return ElementType.VOID; } }
        #endregion

        #region ProtectedMethod
        protected void GlobalID(int count)
        {
            count = Math.Min(count, MAX_GLOBAL_ID_COUNT);
            string temporary = string.Empty;
            for (int i = 0; i < count; i++)
            {
                temporary += string.Format(GET_GLOBAL_ID_FORMAT + "\n", i, i);
            }
            bodystring = temporary + bodystring;
        }

        protected void AddParameter(string name, ObjectType otype, ElementType etype)
        {
            pList.Add(new ParameterSet() { Name = name, ObjectType = otype, ElementType = etype });
        }

        protected void AddMethodBody(string lines)
        {
            bodystring += lines;
        }
        #endregion
    }
}
