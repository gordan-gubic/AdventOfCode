namespace Gguc.Aoc.Y2020.Models
{
    using System;

    public interface ILiveSpace
    {
        void Init(bool[,] source, int cycles = 0);

        void ActivateSpaceForCycles(in int cycles);

        int CountValues(bool value = true);
    }
}