#define LOG

namespace Gguc.Aoc.Y2020.Models
{
    using System;
    using System.Collections.Generic;
    using Gguc.Aoc.Core.Extensions;
    using Gguc.Aoc.Core.Models;

    public class Tile20
    {
        public Tile20()
        {
            Sides = new List<int>();
        }

        public int Index { get; set; }
        
        public Map<bool> Map { get; set; }

        // public List<List<int>> Sides { get; set; }
        public List<int> Sides { get; set; }

        public int MatchSides = 0;

        public int MatchUp = -1;

        public int MatchLeft = -1;

        public bool FlipVer = false;

        public bool FlipHor = false;

        public void Print()
        {
            Console.WriteLine(Index);
            // Map.Values.DumpMap();
            Console.WriteLine(Sides.ToJson());
        }

        /// <inheritdoc />
        public override string ToString() => $"Tile: [{Index}]({MatchSides}) {Sides.ToJson()} MatchLeft: [{MatchLeft}] MatchUp: [{MatchUp}]";
    }
}
