using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDM.Store
{
    public class SourceStore
    {
        private static Random random = new Random();

        private List<Item.SourceItem> items = new List<Item.SourceItem>();

        private List<int> selectedList = new List<int>();

        public int Count { get { return items.Count; } }

        public int Generation { get; private set; }

        private Item.SourceItem ShapeSource { get; set; }

        public void Add(Item.SourceItem item)
        {
            if (items.Count == 0)
            {
                ShapeSource = item;
            }
            else
            {
                if (!RNdArray.ShapeCheck(ShapeSource.Input, item.Input) || !RNdArray.ShapeCheck(ShapeSource.Input, item.Input))
                {
                    return;
                }
            }

            items.Add(item);
        }

        public Item.SourceItem CreateBatch(int batch)
        {
            if (items.Count == 0) { return null; }
            List<RNdArray> s = new List<global::RNdArray>();
            List<RNdArray> t = new List<global::RNdArray>();
            bool epi = false;

            for (int i = 0; i < batch; i++)
            {
                int idx = random.Next(items.Count);
                while (selectedList.Contains(items[idx].ID))
                {
                    idx = random.Next(items.Count);
                }
                s.Add(items[idx].Input);
                t.Add(items[idx].Teacher);

                selectedList.Add(idx);
                if (selectedList.Count == items.Count)
                {
                    selectedList.Clear();
                    Generation++;
                    epi = true;
                }
            }
            return new Item.SourceItem(RNdArray.CombineBatch(s), RNdArray.CombineBatch(t), epi);
        }
    }
}
