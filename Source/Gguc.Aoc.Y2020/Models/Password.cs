namespace Gguc.Aoc.Y2020.Models
{
    public class Password
    {
        public int Min { get; set; }

        public int Max { get; set; }

        public char Letter { get; set; }

        public string Word { get; set; }

        /// <inheritdoc />
        public override string ToString() => $"Min=[{Min}]; Max=[{Max}]; Letter=[{Letter}]; Word=[{Word}]";
    }
}
