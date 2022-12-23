namespace Gguc.Aoc.Y2022.Models;

using Newtonsoft.Json;

public class FactoryNode
{
    public int Time { get; set; }

    public int OreMiner { get; set; }

    public int ClayMiner { get; set; }

    public int ObsidianMiner { get; set; }

    public int GeodeMiner { get; set; }

    public long Ore { get; set; }

    public long Clay { get; set; }

    public long Obsidian { get; set; }

    public long Geode { get; set; }
}