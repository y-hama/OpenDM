using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDM.Store.Item
{
    public abstract class StoreItem
    {
        private static int _seed = 0;
        private int id;
        public int ID { get { return id; } }

        public StoreItem() { id = _seed++; }
    }
}
