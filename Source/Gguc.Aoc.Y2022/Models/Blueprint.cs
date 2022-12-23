namespace Gguc.Aoc.Y2022.Models;

public class Blueprint
{
    public int Id { get; set; }

    public int OreCost1 { get; set; }

    public int ClayCost1 { get; set; }

    public int ObsidianCost1 { get; set; }

    public int ObsidianCost2 { get; set; }

    public int GeodeCost1 { get; set; }

    public int GeodeCost2 { get; set; }

    public int MaxOre { get; set; }

    public void Init()
    {
        MaxOre = new[] { OreCost1, ClayCost1, ObsidianCost1, ObsidianCost1 }.Max();
    }
}