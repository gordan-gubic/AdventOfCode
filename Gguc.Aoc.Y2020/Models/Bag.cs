namespace Gguc.Aoc.Y2020.Models
{
    using System.Collections.Generic;

    public class Bag
    {
        public Bag(string name1, string name2)
        {
            Name1 = name1;
            Name2 = name2;
            Id = $"{name1}-{name2}";
        }

        public string Id { get; set; }

        public string Name1 { get; set; }

        public string Name2 { get; set; }

        public List<string> Raw { get; set; } // = new List<string>();

        public Dictionary<Bag, int> Items { get; set; } = new Dictionary<Bag, int>();

        public int Value { get; set; } = -1;

        /// <inheritdoc />
        public override string ToString() => $"{Id}: Items=[{Items.Count}]; Value=[{Value}];";
    }
}
