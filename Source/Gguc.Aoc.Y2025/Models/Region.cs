namespace Gguc.Aoc.Y2025.Models;

internal record Region
{
    public Guid Id { get; } = Guid.NewGuid();

    public int Width { get; set; }

    public int Height { get; set; }

    public long Surface { get; set; }

    public List<int> Values { get; set; }

    public static Region Parse(string line)
    {
        var region = new Region();
        var parts = line.Split([' ', 'x', ':'], StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToInt()).ToList();
        
        region.Width = parts[0];
        region.Height = parts[1];
        region.Values = [];
        foreach (var p in parts[2..])
        {
            region.Values.Add(p);
        }

        region.Surface = region.Width * region.Height;

        return region;
    }
}