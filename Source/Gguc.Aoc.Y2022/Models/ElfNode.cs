namespace Gguc.Aoc.Y2022.Models;

public class ElfNode
{
    public ElfNode(int x = 0, int y = 0)
    {
        Location = new Point(x, y);
    }

    public ElfNode(Point location)
    {
        Location = location;
    }

    public Point Location { get; set; }

    public Point Destination { get; set; }

    public bool PlanToStay { get; set; }

    public bool PlanToMove { get; set; }

    public bool TryToMove { get; set; }
}