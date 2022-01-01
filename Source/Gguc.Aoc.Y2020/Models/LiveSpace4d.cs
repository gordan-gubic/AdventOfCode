namespace Gguc.Aoc.Y2020.Models
{
    using Gguc.Aoc.Core.Models;

    public class LiveSpace4d : Space4d<bool>, ILiveSpace
    {
        public LiveSpace4d(int d0, int d1, int d2, int d3) : base(d0, d1, d2, d3)
        {
        }

        public void Init(bool[,] source, int cycles = 0)
        {
            var l0 = source.GetLength(0);
            var l1 = source.GetLength(1);

            for (var y = 0; y < l0; y++)
            {
                for (var x = 0; x < l1; x++)
                {
                    Values[x + cycles, y + cycles, cycles, cycles] = source[x, y];
                }
            }
        }

        public void ActivateSpaceForCycles(in int cycles)
        {
            for (var i = 0; i < cycles; i++)
            {
                ActivateSpace();
            }
        }

        private void ActivateSpace()
        {
            var newSpace = CopyValue();

            for (var q = 0; q < Length3; q++)
            {
                for (var z = 0; z < Length2; z++)
                {
                    for (var y = 0; y < Length1; y++)
                    {
                        for (var x = 0; x < Length0; x++)
                        {
                            newSpace[x, y, z, q] = ActivateCube(x, y, z, q);
                        }
                    }
                }
            }

            Values = newSpace;
        }

        private bool ActivateCube(in int x, in int y, in int z, in int q)
        {
            var cube = GetValue(x, y, z, q);
            var count = CountNeighbours(x, y, z, q);

            if (cube && (count == 2 || count == 3)) return true;
            if (!cube && count == 3) return true;
            return false;
        }

        private int CountNeighbours(in int x0, in int y0, in int z0, in int q0)
        {
            var count = 0;

            for (var q = -1; q <= 1; q++)
            {
                for (var z = -1; z <= 1; z++)
                {
                    for (var y = -1; y <= 1; y++)
                    {
                        for (var x = -1; x <= 1; x++)
                        {
                            if (x == 0 && y == 0 && z == 0 && q == 0) continue;

                            var isActive = GetValue(x0 + x, y0 + y, z0 + z, q0 + q);
                            if (isActive) count++;
                        }
                    }
                }
            }

            return count;
        }
    }
}
