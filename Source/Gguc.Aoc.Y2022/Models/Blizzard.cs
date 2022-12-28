namespace Gguc.Aoc.Y2022.Models;

using Newtonsoft.Json;

internal class Blizzard
{
    [JsonIgnore]
    public Guid Id { get; set; } = Guid.NewGuid();

    public int Index { get; set; }

    public char Sign { get; set; }

    public int Direction { get; set; }

    public Point Location { get; set; }

    public int Row { get; set; } = -1;

    public int Column { get; set; } = -1;
    
    public int Size { get; set; }

    public override string ToString() => this.ToJson();

    public int GetRelative(int minute)
    {
        var init = (Row < 0) ? Location.Y : Location.X;

        var ix = (init + minute * Direction) % (Size);
        if (ix < 0) ix = Size + (ix % Size);

        return ix;
    }

    public Point GetPoint(int minute)
    {
        var relative = GetRelative(minute);
        var column = (Row < 0) ? relative : Column;
        var row = (Row < 0) ? relative : Row;

        return new Point(column, row);
    }
}