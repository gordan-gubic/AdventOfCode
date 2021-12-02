namespace Gguc.Aoc.Y2020.Models
{
    public class TicketRule
    {
        public string Name { get; set; }
     
        public long ValueA1 { get; set; }
   
        public long ValueA2 { get; set; }
   
        public long ValueB1 { get; set; }
   
        public long ValueB2 { get; set; }

        public int Index { get; set; } = -1;

        /// <inheritdoc />
        public override string ToString() => $"Name=[{Name}, {Index}]; RangeA=[{ValueA1}-{ValueA2}]; RangeB=[{ValueB1}-{ValueB2}]";
    }
}
