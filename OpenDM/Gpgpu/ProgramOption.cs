using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cloo;

namespace OpenDM.Gpgpu
{
    class ProgramOption
    {
        #region InnerClass
        public enum ParamMode
        {
            Memory,
            Value,
        }
        public enum ValueMode
        {
            INT,
            FLOAT,
        }
        protected class GpuParamSet
        {
            public object Instance { get; set; }
            public ParamMode ParamMode { get; private set; }
            public ValueMode ValueMode { get; private set; }
            public GpuParamSet(ComputeBuffer<float> instance, ParamMode mode = ParamMode.Memory)
            {
                ParamMode = mode;
                Instance = instance;
            }
            public GpuParamSet(object instance, ValueMode vmode, ParamMode mode = ParamMode.Value)
            {
                ParamMode = mode;
                Instance = instance;
                ValueMode = vmode;
            }
        }
        #endregion

        Gpgpu.GpuPlatform.ProgramKernel.OptionSet Option { get; set; }

        private List<GpuParamSet> GpuParameter { get; set; } = new List<GpuParamSet>();

        public ProgramOption(string name)
        {
            Option = Gpgpu.Core.Instance.GetOption(name);
            if (Option == null) { throw new Exception(); }
        }

        public void Startup()
        {
            GpuParameter.Clear();
        }

        public ComputeBuffer<float> ConvertBuffer(ComputeMemoryFlags flag, float[] data)
        {
            switch (flag)
            {
                case ComputeMemoryFlags.ReadOnly:
                    flag = ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer;
                    break;
                case ComputeMemoryFlags.WriteOnly:
                    flag = ComputeMemoryFlags.WriteOnly | ComputeMemoryFlags.CopyHostPointer;
                    break;
                default:
                    break;
            }
            return new ComputeBuffer<float>(Option.Context, flag, data);
        }

        public void SetParameter(ComputeBuffer<float> instance)
        {
            GpuParameter.Add(new GpuParamSet(instance, ParamMode.Memory));
        }
        public void SetParameter(object instance, ValueMode vmode, ParamMode mode = ParamMode.Value)
        {
            GpuParameter.Add(new GpuParamSet(instance, vmode, mode));
        }

        public void Execute(params long[] globalworksize)
        {
            for (int i = 0; i < GpuParameter.Count; i++)
            {
                switch (GpuParameter[i].ParamMode)
                {
                    case ParamMode.Memory:
                        Option.Kernel.SetMemoryArgument(i, (ComputeBuffer<float>)GpuParameter[i].Instance);
                        break;
                    case ParamMode.Value:
                        switch (GpuParameter[i].ValueMode)
                        {
                            case ValueMode.INT:
                                Option.Kernel.SetValueArgument(i, Convert.ToInt32(GpuParameter[i].Instance));
                                break;
                            case ValueMode.FLOAT:
                                Option.Kernel.SetValueArgument(i, Convert.ToSingle(GpuParameter[i].Instance));
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            Option.Queue.Execute(Option.Kernel, null, globalworksize, null, null);
            Option.Queue.Finish();
        }

        public void ReadBuffer(ComputeBuffer<float> mem, ref float[] buffer)
        {
            Option.Queue.ReadFromBuffer(mem, ref buffer, true, null);
        }
    }
}
