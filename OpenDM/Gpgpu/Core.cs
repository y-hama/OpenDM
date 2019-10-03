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
        private const string METHOD_NAMESPACE1 = "Components.GPGPU.Function.Method";
        private const string METHOD_NAMESPACE2 = "Components.GPGPU.Function.Optimization.Method";
        private const string METHOD_BASETYPE = "Components.GPGPU.Function.FunctionBase";

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
                if (Processors.Count != 0 && value)
                {
                    OptionalUseGPU = true;
                }
                else
                {
                    OptionalUseGPU = false;
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
                //#region BuildProgram
                //Assembly asm = Assembly.GetExecutingAssembly();
                //List<Function.FunctionBase> fList = new List<Function.FunctionBase>();
                //foreach (var item in asm.GetTypes())
                //{
                //    if (item.Namespace.Contains(METHOD_NAMESPACE1) || item.Namespace.Contains(METHOD_NAMESPACE2))
                //    {
                //        fList.Add((Function.FunctionBase)Activator.CreateInstance(item));
                //    }
                //}

                //foreach (var item in fList)
                //{
                //    if (item.IsGpuProcess)
                //    {
                //        var sourceList = item.GetSourceList();
                //        foreach (var source in sourceList)
                //        {
                //            Build(source.Item1, source.Item2);
                //        }
                //    }
                //}
                //#endregion
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

        private GpuPlatform.ProgramKernel.OptionSet GetOption(string name)
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
