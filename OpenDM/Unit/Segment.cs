using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDM.Unit
{
    public class Segment
    {
        public int Generation { get; protected set; }

        private List<Grid.GridBase> GridList = new List<Grid.GridBase>();

        public void AddGrid(Grid.GridBase grid)
        {
            if (GridList.Count > 0)
            {
                GridList[GridList.Count - 1].GridConnection(grid);
            }
            GridList.Add(grid);
        }


        public RNdArray Forward(RNdArray input)
        {
            RNdArray i = input;

            Grid.GridBase tg = GridList[0];
            Grid.GridBase pg = tg.PrevGrid;
            Grid.GridBase ng = tg.NextGrid;

            while (tg != null)
            {
                i = tg.Forward(i);
                pg = tg;
                tg = tg.NextGrid;
            }
            return i;
        }

        public RNdArray Back(RNdArray error, params double[] options)
        {
            RNdArray e = error;

            Grid.GridBase tg = GridList[GridList.Count - 1];
            Grid.GridBase pg = tg.PrevGrid;
            Grid.GridBase ng = tg.NextGrid;
            while (tg != null)
            {
                tg.SetOption(options);
                e = tg.Back(e);
                tg = tg.PrevGrid;
            }
            Generation++;
            return e;
        }

        public double Learn(RNdArray input, RNdArray teacher, out RNdArray output, out RNdArray propagator, params double[] options)
        {
            output = Forward(input);
            var e = output - teacher;
            propagator = Back(e, options);
            return e.Power;
        }
    }
}
