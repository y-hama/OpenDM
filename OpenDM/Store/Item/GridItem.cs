using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDM.Store.Item
{
    class GridItem
    {
        private static int _seed = 0;
        private int id;
        public int ID { get { return id; } }

        public GridItem() { id = _seed++; }
    }
}
