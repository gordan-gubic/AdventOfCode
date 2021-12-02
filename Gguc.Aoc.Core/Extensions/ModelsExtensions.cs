namespace Gguc.Aoc.Core.Extensions;

public static class ModelsExtensions
{
    public static long ManhattanDistance(this in Point dot)
    {
        return Math.Abs(dot.X) + Math.Abs(dot.Y);
    }
}
