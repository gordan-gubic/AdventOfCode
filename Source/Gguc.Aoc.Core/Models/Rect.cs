namespace Gguc.Aoc.Core.Models;

public readonly struct Rect
{
    public Rect(int x, int y, int size = 0)
    {
        X1 = x;
        X2 = x + size;
        Y1 = y;
        Y2 = y + size;
    }
    public Rect(int x1, int y1, int x2, int y2)
    {
        X1 = x1;
        X2 = x2;
        Y1 = y1;
        Y2 = y2;
    }

    public int X1 { get; }

    public int X2 { get; }

    public int Y1 { get; }

    public int Y2 { get; }

    public override string ToString() => $"Rect: {X1}, {Y1} - {X2}, {Y2}";

    /// <inheritdoc />
    public override bool Equals(object obj) => base.Equals(obj);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(X1, Y1, X2, Y2);

    public static bool operator ==(Rect left, Rect right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Rect left, Rect right)
    {
        return !left.Equals(right);
    }
}
