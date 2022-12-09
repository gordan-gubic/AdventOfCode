namespace Gguc.Aoc.Core.Models;

public class MovePoint
{
    public MovePoint(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }

    public int Y { get; set; }

    public override string ToString() => $"Point: {X}, {Y}";

    /// <inheritdoc />
    public override bool Equals(object obj) => base.Equals(obj);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(X, Y);

    public static bool operator ==(MovePoint left, MovePoint right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(MovePoint left, MovePoint right)
    {
        return !left.Equals(right);
    }
}
