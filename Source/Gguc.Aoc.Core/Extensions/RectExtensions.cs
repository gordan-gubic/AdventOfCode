namespace Gguc.Aoc.Core.Extensions;

public static class RectExtensions
{
    public static bool Contains(this Rect rect, Point point)
    {
        return !(point.X < rect.X1 || point.Y < rect.Y1 || point.X >= rect.X2 || point.Y >= rect.Y2);
    }
}
