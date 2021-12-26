namespace Gguc.Aoc.Y2021.Models;

using Newtonsoft.Json;

public record Snail
{
    [JsonIgnore]
    public string Id { get; } = Guid.NewGuid().ToString("N");

    public object X { get; set; }

    public object Y { get; set; }

    [JsonIgnore]
    public Snail Parent { get; set; }

    [JsonIgnore]
    public long Value { get; set; }

    public int Level { get; set; }

    public bool IsLarge()
    {
        return (X is BoxInt b1 && b1.Value > 9) || (Y is BoxInt b2 && b2.Value > 9);
    }

    public long CalculateMagnitude()
    {
        // long x = 0L;
        // long y = 0L;

        var x1 = (X is Snail s1) ? s1.CalculateMagnitude() : (long)(((BoxInt)X).Value);
        var y1 = (Y is Snail s2) ? s2.CalculateMagnitude() : (long)(((BoxInt)Y).Value);

        Value = (x1 * 3) + (y1 * 2);
        return Value;
    }

    public override string ToString()
    {
        return $"[{X},{Y}]";
    }

    public static Snail Create(Snail s1, Snail s2)
    {
        var snail = new Snail
        {
            X = s1,
            Y = s2,
        };

        s1.Parent = snail;
        s2.Parent = snail;

        return snail;
    }

    public static Snail Create(Dictionary<string, Snail> dict, string text)
    {
        var parts = text.Split(new char[] { ' ', ',', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

        var x = parts[0];
        var y = parts[1];

        var s1 = (object)dict.GetValueOrDefault(x, null);
        var s2 = (object)dict.GetValueOrDefault(y, null);

        var i1 = (object)BoxInt.Create(x.ToInt());
        var i2 = (object)BoxInt.Create(y.ToInt());

        var v1 = s1 ?? i1;
        var v2 = s2 ?? i2;

        var snail = new Snail
        {
            X = v1,
            Y = v2,
        };

        if(v1 is Snail) ((Snail)v1).Parent = snail;
        if(v2 is Snail) ((Snail)v2).Parent = snail;

        dict[snail.Id] = snail;
        return snail;
    }
}