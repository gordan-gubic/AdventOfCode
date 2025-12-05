namespace Gguc.Aoc.Core.Extensions;

using System.Numerics;

public static class RangeExtensions
{
    public static bool Intersect<T>(this Range<T> init, Range<T> range) where T : INumber<T>
    {
        return !(init.Lower > range.Upper || range.Lower > init.Upper);
    }

    public static (bool, Range<T>) Merge<T>(this Range<T> init, Range<T> range) where T : INumber<T>
    {
        if (!init.Intersect(range)) return (false, init);

        return (true, new Range<T>(T.Min(init.Lower, range.Lower), T.Max(init.Upper, range.Upper)));
    }
}
