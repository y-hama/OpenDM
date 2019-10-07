using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDM.Store.Item
{
    public class SourceItem : StoreItem
    {
        public RNdArray Input { get; private set; }
        public RNdArray Teacher { get; private set; }

        public bool EpochIteration { get; private set; }

        public SourceItem(RNdArray input, RNdArray teacher, bool epochIteration = false)
        {
            Input = input; Teacher = teacher;
            EpochIteration = epochIteration;
        }
    }
}
