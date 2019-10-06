using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDM.Store
{
    static class GridItemStore
    {
        private static List<Item.GridItem> items = new List<Item.GridItem>();

        public static void Add(Item.GridItem item) { items.Add(item); }



    }
}
