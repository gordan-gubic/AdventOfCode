namespace Gguc.Aoc.Y2022.Models;

using Newtonsoft.Json;

public class GeodeFactory
{
    public int Id { get; set; }

    [JsonIgnore]
    public Blueprint Blueprint { get; set; }

    public int OreMiner { get; set; }

    public int ClayMiner { get; set; }

    public int ObsidianMiner { get; set; }

    public int GeodeMiner { get; set; }

    public int NewOreMiner { get; set; }

    public int NewClayMiner { get; set; }

    public int NewObsidianMiner { get; set; }

    public int NewGeodeMiner { get; set; }

    public int MaxOreMiner { get; set; } = int.MaxValue;

    public int MaxClayMiner { get; set; } = int.MaxValue;

    public int MaxObsidianMiner { get; set; } = int.MaxValue;

    public int MaxGeodeMiner { get; set; } = int.MaxValue;

    public long OreResource { get; set; }

    public long ClayResource { get; set; }

    public long ObsidianResource { get; set; }

    public long GeodeResource { get; set; }

    private bool CanBuyOre => MaxOreMiner > OreMiner + NewOreMiner && OreResource >= Blueprint.OreCost1;

    private bool CanBuyClay => MaxClayMiner > ClayMiner + NewClayMiner && OreResource >= Blueprint.ClayCost1;

    private bool CanBuyObsidian => MaxObsidianMiner > ObsidianMiner + NewObsidianMiner && OreResource >= Blueprint.ObsidianCost1 && ClayResource >= Blueprint.ObsidianCost2;

    private bool CanBuyGeode => OreResource >= Blueprint.GeodeCost1 && ObsidianResource >= Blueprint.GeodeCost2;

    public void ProduceNewMiners()
    {
        OreMiner += NewOreMiner;
        ClayMiner += NewClayMiner;
        ObsidianMiner += NewObsidianMiner;
        GeodeMiner += NewGeodeMiner;

        NewOreMiner = 0;
        NewClayMiner = 0;
        NewObsidianMiner = 0;
        NewGeodeMiner = 0;
    }

    public void BuyNewMiners()
    {
        if (CanBuyGeode) BuyGeode();
        else if (CanBuyObsidian) BuyObsidian();
        else if (CanBuyClay) BuyClay();
        else if (CanBuyOre) BuyOre();
    }

    public void CollectResources()
    {
        OreResource += OreMiner;
        ClayResource += ClayMiner;
        ObsidianResource += ObsidianMiner;
        GeodeResource += GeodeMiner;
    }

    private void BuyOre()
    {
        NewOreMiner += 1;
        OreResource -= Blueprint.OreCost1;
    }

    private void BuyClay()
    {
        NewClayMiner += 1;
        OreResource -= Blueprint.ClayCost1;
    }

    private void BuyObsidian()
    {
        NewObsidianMiner += 1;
        OreResource -= Blueprint.ObsidianCost1;
        ClayResource -= Blueprint.ObsidianCost2;
    }

    private void BuyGeode()
    {
        NewGeodeMiner += 1;
        OreResource -= Blueprint.GeodeCost1;
        ObsidianResource -= Blueprint.GeodeCost2;
    }
}