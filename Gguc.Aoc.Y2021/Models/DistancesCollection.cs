namespace Gguc.Aoc.Y2021.Models;

public class DistancesCollection
{
    public DistancesCollection()
    {
        XPoints = new Dictionary<string, List<int>>();
        YPoints = new Dictionary<string, List<int>>();
        ZPoints = new Dictionary<string, List<int>>();

        XDist = new Dictionary<string, List<int>>();
        YDist = new Dictionary<string, List<int>>();
        ZDist = new Dictionary<string, List<int>>();
    }

    public Dictionary<string, List<int>> XPoints { get; set; }

    public Dictionary<string, List<int>> YPoints { get; set; }

    public Dictionary<string, List<int>> ZPoints { get; set; }

    public Dictionary<string, List<int>> XDist { get; set; }

    public Dictionary<string, List<int>> YDist { get; set; }

    public Dictionary<string, List<int>> ZDist { get; set; }
}

