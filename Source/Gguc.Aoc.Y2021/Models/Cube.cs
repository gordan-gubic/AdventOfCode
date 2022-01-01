namespace Gguc.Aoc.Y2021.Models;

public class Cube
{
    public bool On { get; set; }

    public int X1 { get; set; }

    public int X2 { get; set; }

    public int Y1 { get; set; }

    public int Y2 { get; set; }

    public int Z1 { get; set; }

    public int Z2 { get; set; }

    public override bool Equals(object obj)
    {
        if (!(obj is Cube other)) return false;

        return X1 == other.X1 && X2 == other.X2 && Y1 == other.Y1 && Y2 == other.Y2 && Z1 == other.Z1 && Z2 == other.Z2;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X1, X2, Y1, Y2, Z1, Z2, On);
    }

    public override string ToString()
    {
        return this.ToJson();
    }
}
