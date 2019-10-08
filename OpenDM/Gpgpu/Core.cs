using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Cloo;

namespace OpenDM.Gpgpu
{
    class Core
    {
        private const string METHOD_NAMESPACE1 = "OpenDM.Gpgpu.Function";
        private const string METHOD_NAMESPACE2 = "OpenDM.Gpgpu.Source";
        private const string METHOD_BASETYPE = "Empty";

        private static Core _instance = new Core();
        public static Core Instance { get { return _instance; } }
        private Core()
        {

        }

        #region Property
        private bool OptionalUseGPU = true;
        public bool UseGPU
        {
            get
            {
                return ((Processors != null && Processors.Count > 0) ? true : false) && OptionalUseGPU;
            }
            set
            {
                if (value)
                {
                    PlatformConfirm();
                }
                else
                {
                    if (Processors != null)
                    {
                        if (Processors.Count != 0)
                        {
                            OptionalUseGPU = true;
                        }
                        else
                        {
                            OptionalUseGPU = false;
                        }
                    }
                    else
                    {
                        OptionalUseGPU = value;
                    }
                }
            }
        }

        public string ProcesserStatus
        {
            get
            {
                PlatformConfirm();
                string status = string.Empty;
                if (OptionalUseGPU)
                {
                    foreach (var item in Processors)
                    {
                        status += "===============v===============\n";
                        status += item.Status;
                        status += "===============^===============\n";
                    }
                }
                else
                {
                    status = "Do not use GPU\n";
                }
                return status;
            }
        }

        private bool PlatformInitialized { get; set; }
        private List<GpuPlatform> Processors { get; set; }
        #endregion

        #region PrivateMethod
        private void PlatformConfirm()
        {
            if (!PlatformInitialized)
            {
                Processors = new List<GpuPlatform>();
                foreach (var platform in ComputePlatform.Platforms)
                {
                    GpuPlatform item;
                    if ((item = GpuPlatform.IsGpuProcesser(platform)) != null)
                    {
                        Processors.Add(item);
                    }
                }

                PlatformInitialized = true;
            }
        }

        public void BuildAllMethod()
        {
            if (PlatformInitialized)
            {
                #region BuildProgram
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
                foreach (var source in fList)
                {
                    Build(source.Name, source.Source);
                }

                fList.Clear();
                foreach (var item in asmtypes)
                {
                    if (item.Namespace != null)
                    {
                        if (item.Namespace.Contains(METHOD_NAMESPACE2))
                        {
                            fList.Add((SourceCode)System.Activator.CreateInstance(item));
                        }
                    }
                }
                foreach (var source in fList)
                {
                    Build(source.Name, source.Source);
                }
                #endregion
            }
        }

        private bool Build(string name, string source)
        {
            int index = -1, count = int.MaxValue;
            for (int i = 0; i < Processors.Count; i++)
            {
                if (Processors[i].Exists(name)) { return true; }
                if (Processors[i].ProgramCount < count)
                {
                    count = Processors[i].ProgramCount;
                    index = i;
                }
            }
            if (index < 0) { return false; }
            return Processors[index].CreateProgram(name, source);
        }

        #endregion

        #region PublicMethod
        public void PlatformClose()
        {
            foreach (var item in Processors)
            {
                item.Trush();
            }
            Processors.Clear();
            Processors = null;
            PlatformInitialized = false;
            OptionalUseGPU = false;
        }

        public GpuPlatform.ProgramKernel.OptionSet GetOption(string name)
        {
            foreach (var item in Processors)
            {
                if (item.Exists(name))
                {
                    return item.CreateKernel(name);
                }
            }
            return null;
        }

        //public List<GpuPlatform.ProgramKernel.OptionSet> GetOption(List<Function.SourceCode> function)
        //{
        //    List<GpuPlatform.ProgramKernel.OptionSet> list = new List<GpuPlatform.ProgramKernel.OptionSet>();
        //    foreach (var item in function)
        //    {
        //        var option = GetOption(item.Name);
        //        if (option == null) { throw new Exception(); }
        //        else { list.Add(option); }
        //    }
        //    return list;
        //}
        #endregion
    }
}
