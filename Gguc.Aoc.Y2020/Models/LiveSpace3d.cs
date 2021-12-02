namespace Gguc.Aoc.Y2020.Models
{
    using Gguc.Aoc.Core.Models;

    public class LiveSpace3d : Space3d<bool>, ILiveSpace
    {
        public LiveSpace3d(int d0, int d1, int d2) : base(d0, d1, d2)
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
                    Values[x + cycles, y + cycles, cycles] = source[x, y];
                }
            }
        }

        public void ActivateSpaceForCycles(in int cycles)
        {
            for (int i = 0; i < cycles; i++)
            {
                ActivateSpace();
            }
        }

        private void ActivateSpace()
        {
            var newSpace = CopyValue();

            for (var z = 0; z < Length2; z++)
            {
                for (var y = 0; y < Length1; y++)
                {
                    for (var x = 0; x < Length0; x++)
                    {
                        newSpace[x, y, z] = ActivateCube(x, y, z);
                    }
                }
            }

            Values = newSpace;
        }

        private bool ActivateCube(in int x, in int y, in int z)
        {
            var cube = GetValue(x, y, z);
            var count = CountNeighbours(x, y, z);

            if (cube && (count == 2 || count == 3)) return true;
            if (!cube && count == 3) return true;
            return false;
        }

        private int CountNeighbours(in int x0, in int y0, in int z0)
        {
            var count = 0;

            for (var z = -1; z <= 1; z++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    for (var x = -1; x <= 1; x++)
                    {
                        if (x == 0 && y == 0 && z == 0) continue;

                        var isActive = GetValue(x0 + x, y0 + y, z0 + z);
                        if (isActive) count++;
                    }
                }
            }

            return count;
        }
    }
}
